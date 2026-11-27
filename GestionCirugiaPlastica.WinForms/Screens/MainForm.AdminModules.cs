using System.Drawing;
using System.Globalization;
using System.Text.Json;

namespace GestionCirugiaPlastica.WinForms;

internal partial class MainForm
{
    private async Task ShowRecipes()
    {
        AttachStatusBar();
        var tools = new Panel { Dock = DockStyle.Top, Height = 52, Padding = new Padding(0, 4, 0, 8) };
        var search = new TextBox { PlaceholderText = "Filtrar por paciente o medicamento", Dock = DockStyle.Left, Width = 320, ForeColor = Charcoal, BackColor = Color.White };
        var add = Button("Redactar receta", Mauve, Color.White); add.Dock = DockStyle.Right; add.Width = 160;
        var refresh = Button("Actualizar", Color.White, Mauve); refresh.Dock = DockStyle.Right; refresh.Width = 120;
        tools.Controls.Add(search); tools.Controls.Add(refresh); tools.Controls.Add(add);
        var grid = CreateGrid(); grid.Dock = DockStyle.Fill; _grid = grid;
        _dynamicHost.Controls.Add(grid); _dynamicHost.Controls.Add(tools);
        List<ApiReceta> recipes = [];
        async Task LoadRecipes()
        {
            try
            {
                var patients = await _api.GetAsync<List<ApiPaciente>>("api/Pacientes/expediente/selector");
                recipes = await _api.GetAsync<List<ApiReceta>>("api/Recetas");
                var names = patients.ToDictionary(p => p.PacienteId, p => $"{p.Nombre} {p.Apellido}");
                grid.Columns.Clear(); grid.DataSource = recipes.OrderByDescending(x => x.Fecha).Select(r => new { r.RecetaId, Paciente = names.GetValueOrDefault(r.PacienteId, $"Paciente #{r.PacienteId}"), Fecha = r.Fecha.ToString("dd/MM/yyyy"), Medicamentos = string.Join(", ", r.DetallesReceta.Select(d => d.Medicamento)), r.Indicaciones }).ToList();
                AddRowAction("Editar", async row => await EditRecipe(recipes.First(x => x.RecetaId == Convert.ToInt32(row.Cells["RecetaId"].Value)), LoadRecipes));
                _status.ForeColor = Mauve; _status.Text = $"{recipes.Count} recetas";
            }
            catch (Exception ex) { grid.DataSource = null; _status.ForeColor = Color.Firebrick; _status.Text = UserMessage(ex); MessageBox.Show(this, UserMessage(ex), "No se pudieron cargar recetas", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }
        add.Click += async (_, _) => await EditRecipe(null, LoadRecipes);
        refresh.Click += async (_, _) => await LoadRecipes();
        search.TextChanged += (_, _) => ApplySearch(search.Text);
        await LoadRecipes();
    }

    private async Task EditRecipe(ApiReceta? current, Func<Task> refresh)
    {
        try
        {
            var patients = await _api.GetAsync<List<ApiPaciente>>("api/Pacientes/expediente/selector");
            using var form = new EntityDialog(current is null ? "Redactar receta" : "Editar receta");
            var patient = form.AddChoice("Paciente", patients.Select(p => new ChoiceItem(p.PacienteId, $"{p.Nombre} {p.Apellido}")).ToList(), current?.PacienteId);
            var date = form.AddDate("Fecha", current?.Fecha ?? DateTime.Today);
            var instructions = form.AddText("Indicaciones médicas / postoperatorias", current?.Indicaciones, true);
            var details = new List<(ApiDetalleReceta Source, TextBox Drug, TextBox Dose, TextBox Freq, TextBox Duration, TextBox Notes)>();
            void AddDetail(ApiDetalleReceta? source = null)
            {
                source ??= new ApiDetalleReceta();
                details.Add((source, form.AddText("Medicamento", source.Medicamento), form.AddText("Dosis", source.Dosis), form.AddText("Frecuencia", source.Frecuencia), form.AddText("Duración", source.Duracion), form.AddText("Instrucciones", source.Instrucciones)));
            }
            foreach (var detail in current?.DetallesReceta ?? []) AddDetail(detail);
            if (details.Count == 0) AddDetail();
            form.AddAction("Agregar medicamento", () => AddDetail());
            if (form.ShowDialog(this) != DialogResult.OK) return;
            if (patient.SelectedItem is not ChoiceItem selected || details.Any(d => string.IsNullOrWhiteSpace(d.Drug.Text))) throw new InvalidOperationException("Selecciona un paciente e ingresa el nombre de cada medicamento.");
            var payload = new { RecetaId = current?.RecetaId ?? 0, Fecha = date.Value, PacienteId = selected.Id, ConsultaId = current?.ConsultaId, CirugiaId = current?.CirugiaId, Indicaciones = EmptyNull(instructions.Text), DetallesReceta = details.Select(d => new { d.Source.DetalleRecetaId, Medicamento = d.Drug.Text.Trim(), Dosis = EmptyNull(d.Dose.Text), Frecuencia = EmptyNull(d.Freq.Text), Duracion = EmptyNull(d.Duration.Text), Instrucciones = EmptyNull(d.Notes.Text) }).ToList() };
            await Run(async () => { await _api.SendAsync(HttpMethod.Post, "api/Recetas", payload); MessageBox.Show(this, "Receta guardada.", "Tanger Clinic", MessageBoxButtons.OK, MessageBoxIcon.Information); await refresh(); });
        }
        catch (Exception ex) { MessageBox.Show(this, UserMessage(ex), "No se pudo abrir el formulario", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }

    private async Task ShowInsurers()
    {
        AttachStatusBar();
        var tools = new Panel { Dock = DockStyle.Top, Height = 52, Padding = new Padding(0, 4, 0, 8) };
        var add = Button("Agregar aseguradora", Mauve, Color.White); add.Dock = DockStyle.Right; add.Width = 180;
        var refresh = Button("Actualizar", Color.White, Mauve); refresh.Dock = DockStyle.Right; refresh.Width = 120; tools.Controls.Add(add); tools.Controls.Add(refresh);
        var grid = CreateGrid(); grid.Dock = DockStyle.Fill; _grid = grid; _dynamicHost.Controls.Add(grid); _dynamicHost.Controls.Add(tools);
        List<ApiSeguro> insurers = [];
        async Task LoadInsurers()
        {
            try
            {
                insurers = await _api.GetAsync<List<ApiSeguro>>("api/Seguros");
                grid.Columns.Clear(); grid.DataSource = insurers.Select(s => new { s.SeguroId, Aseguradora = s.Nombre, s.Telefono, s.Email, s.Direccion, Estado = s.Activo ? "Activa" : "Inactiva" }).ToList();
                AddRowAction("Editar", async row => await EditInsurer(insurers.First(s => s.SeguroId == Convert.ToInt32(row.Cells["SeguroId"].Value)), LoadInsurers));
                AddRowAction("Estado", async row =>
                {
                    var insurer = insurers.First(s => s.SeguroId == Convert.ToInt32(row.Cells["SeguroId"].Value));
                    var action = insurer.Activo ? "desactivar" : "activar";
                    await Run(async () => { await _api.PatchAsync($"api/Seguros/{insurer.SeguroId}/{action}"); await LoadInsurers(); });
                });
                _status.ForeColor = Mauve; _status.Text = $"{insurers.Count} aseguradoras";
            }
            catch (Exception ex) { grid.DataSource = null; _status.ForeColor = Color.Firebrick; _status.Text = UserMessage(ex); MessageBox.Show(this, UserMessage(ex), "No se pudieron cargar aseguradoras", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }
        add.Click += async (_, _) => await EditInsurer(null, LoadInsurers);
        refresh.Click += async (_, _) => await LoadInsurers();
        await LoadInsurers();
    }

    private async Task EditInsurer(ApiSeguro? current, Func<Task> refresh)
    {
        using var form = new EntityDialog(current is null ? "Agregar aseguradora" : "Editar aseguradora");
        var name = form.AddText("Nombre *", current?.Nombre); var phone = form.AddText("Teléfono", current?.Telefono); var email = form.AddText("Correo", current?.Email); var address = form.AddText("Dirección", current?.Direccion);
        if (form.ShowDialog(this) != DialogResult.OK) return;
        if (string.IsNullOrWhiteSpace(name.Text)) { MessageBox.Show(this, "El nombre es obligatorio.", "Validación"); return; }
        await Run(async () => { await _api.SendAsync(current is null ? HttpMethod.Post : HttpMethod.Put, current is null ? "api/Seguros" : $"api/Seguros/{current.SeguroId}", new { SeguroId = current?.SeguroId ?? 0, FechaRegistro = current?.FechaRegistro ?? DateTime.Now, Nombre = name.Text.Trim(), Telefono = EmptyNull(phone.Text), Email = EmptyNull(email.Text), Direccion = EmptyNull(address.Text), Activo = true }); MessageBox.Show(this, "Aseguradora guardada.", "Tanger Clinic"); await refresh(); });
    }

    private async Task ShowConfiguration()
    {
        AttachStatusBar();
        var tools = new Panel { Dock = DockStyle.Top, Height = 50 };
        var search = new TextBox { PlaceholderText = "Buscar por nombre, usuario o correo", Dock = DockStyle.Left, Width = 330, ForeColor = Charcoal, BackColor = Color.White }; tools.Controls.Add(search);
        var refresh = Button("Actualizar", Color.White, Mauve); refresh.Dock = DockStyle.Right; refresh.Width = 120; tools.Controls.Add(refresh);
        var grid = CreateGrid(); grid.Dock = DockStyle.Fill; _grid = grid; _dynamicHost.Controls.Add(grid); _dynamicHost.Controls.Add(tools);
        try
        {
            var users = await _api.GetAsync<List<ApiUsuario>>("api/Usuarios"); var roles = await _api.GetAsync<List<ApiRol>>("api/Roles");
            var allowed = new[] { "Doctora", "AsistenteClinico", "Secretaria", "Paciente" };
            roles = roles.Where(r => allowed.Contains(r.Nombre, StringComparer.OrdinalIgnoreCase)).ToList();
            grid.DataSource = users.Select(u => new { u.UsuarioId, Usuario = $"{u.Nombre} {u.Apellido} (@{u.NombreUsuario})", u.Email, Rol = roles.FirstOrDefault(r => r.RolId == u.RolId)?.Nombre ?? $"#{u.RolId}", Estado = u.Activo ? "Activo" : "Inactivo" }).ToList();
            AddRowAction("Rol", async row =>
            {
                var user = users.First(u => u.UsuarioId == Convert.ToInt32(row.Cells["UsuarioId"].Value));
                using var form = new EntityDialog($"Cambiar rol · {user.Nombre} {user.Apellido}");
                var choice = form.AddChoice("Rol del usuario", roles.Select(r => new ChoiceItem(r.RolId, r.Nombre)).ToList(), user.RolId);
                if (form.ShowDialog(this) != DialogResult.OK || choice.SelectedItem is not ChoiceItem selected || selected.Id == user.RolId) return;
                await Run(async () => { await _api.PatchAsync($"api/Usuarios/{user.UsuarioId}/rol?nuevoRolId={selected.Id}"); await ShowSection("Configuración"); });
            });
            AddRowAction("Estado", async row =>
            {
                var user = users.First(u => u.UsuarioId == Convert.ToInt32(row.Cells["UsuarioId"].Value));
                var action = user.Activo ? "desactivar" : "activar";
                if (MessageBox.Show(this, $"¿Deseas {action} la cuenta de {user.Nombre} {user.Apellido}?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                await Run(async () => { await _api.PatchAsync($"api/Usuarios/{user.UsuarioId}/{action}"); await ShowSection("Configuración"); });
            });
            _status.ForeColor = Mauve; _status.Text = $"{users.Count} usuarios";
            search.TextChanged += (_, _) => ApplySearch(search.Text);
            refresh.Click += async (_, _) => await ShowSection("Configuración");
        }
        catch (Exception ex) { grid.DataSource = null; _status.ForeColor = Color.Firebrick; _status.Text = UserMessage(ex); MessageBox.Show(this, UserMessage(ex), "No se pudo cargar la configuración", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }
}
