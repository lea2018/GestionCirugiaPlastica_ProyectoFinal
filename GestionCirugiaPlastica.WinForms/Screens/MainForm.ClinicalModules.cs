using System.Drawing;
using System.Globalization;
using System.Text.Json;

namespace GestionCirugiaPlastica.WinForms;

internal partial class MainForm
{
    private async Task ShowMyProfile()
    {
        AttachStatusBar();
        var panel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(24) };
        var message = new Label { Dock = DockStyle.Bottom, Height = 40, ForeColor = Mauve };
        panel.Controls.Add(message); _dynamicHost.Controls.Add(panel);
        try
        {
            var profile = await _api.GetAsync<JsonElement>("api/Perfil");
            var name = panelAdd("Nombre completo", ReadText(profile, "nombreCompleto"));
            var username = panelAdd("Nombre de usuario", ReadText(profile, "nombreUsuario"), true);
            var email = panelAdd("Correo electrónico", ReadText(profile, "email"), true);
            var role = panelAdd("Rol", ReadText(profile, "rol"), true);
            var currentPassword = panelAdd("Contraseña actual (opcional)", null, false, true);
            var newPassword = panelAdd("Nueva contraseña (mínimo 12 caracteres)", null, false, true);
            var save = Button("Guardar cambios", Mauve, Color.White); save.Dock = DockStyle.Top; save.Height = 42; save.Click += async (_, _) =>
            {
                if (string.IsNullOrWhiteSpace(name.Text)) { message.Text = "El nombre completo es obligatorio."; return; }
                if (!string.IsNullOrWhiteSpace(newPassword.Text) && (newPassword.Text.Length < 12 || string.IsNullOrWhiteSpace(currentPassword.Text))) { message.Text = "Para cambiar la contraseña debes introducir la actual y una nueva de al menos 12 caracteres."; return; }
                await Run(async () =>
                {
                    var result = await _api.SendForJsonAsync<JsonElement>(HttpMethod.Put, "api/Perfil", new { NombreCompleto = name.Text.Trim(), PasswordActual = EmptyNull(currentPassword.Text), PasswordNueva = EmptyNull(newPassword.Text) });
                    var token = ReadText(result, "accessToken"); if (!string.IsNullOrWhiteSpace(token)) _api.SetAccessToken(token);
                    _api.SetDisplayName(ReadText(result, "nombreCompleto") ?? name.Text.Trim());
                    if (_rightProfile is not null) _rightProfile.Text = $"{ReadText(result, "nombreCompleto")}\r\n{ReadText(result, "rol")}";
                    currentPassword.Clear(); newPassword.Clear(); message.Text = "Perfil actualizado.";
                });
            };
            var photo = Button("Seleccionar foto de perfil", Color.White, Mauve); photo.Dock = DockStyle.Top; photo.Height = 40; photo.Click += async (_, _) =>
            {
                using var picker = new OpenFileDialog { Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.webp" };
                if (picker.ShowDialog(this) != DialogResult.OK) return;
                var info = new FileInfo(picker.FileName); if (info.Length > 5 * 1024 * 1024) { message.Text = "La imagen no puede superar 5 MB."; return; }
                await Run(async () => { await _api.SendFileAsync("api/Perfil/foto", picker.FileName); message.Text = "Foto de perfil actualizada."; });
            };
            panel.Controls.Add(photo); panel.Controls.Add(save);
            TextBox panelAdd(string label, string? value, bool readOnly = false, bool password = false)
            {
                var caption = new Label { Text = label, Dock = DockStyle.Top, Height = 23, ForeColor = Charcoal, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
                var input = new TextBox { Text = value ?? "", Dock = DockStyle.Top, Height = 34, ReadOnly = readOnly, UseSystemPasswordChar = password, ForeColor = Charcoal, BackColor = readOnly ? Color.FromArgb(248, 247, 247) : Color.White, Margin = new Padding(0, 0, 0, 10) };
                panel.Controls.Add(input); panel.Controls.Add(caption); return input;
            }
        }
        catch (Exception ex) { message.Text = UserMessage(ex); }
    }

    private async Task ShowPatientHome()
    {
        var panel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(24) };
        var greeting = new Label { Text = "Cargando información de tu portal…", Dock = DockStyle.Top, Height = 80, ForeColor = Charcoal, Font = new Font("Segoe UI", 17, FontStyle.Bold) };
        var details = new Label { Dock = DockStyle.Top, Height = 100, ForeColor = Taupe, Font = new Font("Segoe UI", 10) };
        var openPortal = Button("Ver mi perfil, citas y recetas", Mauve, Color.White); openPortal.Dock = DockStyle.Top; openPortal.Height = 42; openPortal.Click += async (_, _) => await ShowSection("Mi perfil, citas y recetas");
        panel.Controls.Add(openPortal); panel.Controls.Add(details); panel.Controls.Add(greeting); _dynamicHost.Controls.Add(panel);
        try
        {
            var profile = await _api.GetAsync<JsonElement>("api/PortalPaciente/perfil");
            var appointments = await _api.GetAsync<List<JsonElement>>("api/PortalPaciente/citas");
            greeting.Text = $"Hola, {ReadText(profile, "nombre")} {ReadText(profile, "apellido")}";
            details.Text = $"Tienes {appointments.Count(c => FindProperty(c, "fecha").ValueKind == JsonValueKind.String && DateTime.TryParse(ReadText(c, "fecha"), out var date) && date >= DateTime.Today)} cita(s) desde hoy.\r\nDesde tu portal puedes consultar citas, recetas, información personal y solicitar una cita.";
        }
        catch (Exception ex) { greeting.Text = "No se pudo cargar tu inicio"; details.Text = UserMessage(ex); }
    }

    private void ShowRecordSearch(int? preselectedPatientId = null)
    {
        var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(14), BackColor = Color.White };
        var toolbar = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 112, WrapContents = true, FlowDirection = FlowDirection.LeftToRight, BackColor = Color.White, Padding = new Padding(0, 4, 0, 8) };
        var patientPicker = new ComboBox { Width = 225, Height = 36, DropDownStyle = ComboBoxStyle.DropDownList, ForeColor = Charcoal, BackColor = Color.White, Margin = new Padding(0, 4, 8, 4) };
        var search = new TextBox { PlaceholderText = "Nombre, cédula o ID", Width = 205, Height = 36, ForeColor = Charcoal, BackColor = Color.White, Margin = new Padding(0, 4, 8, 4) };
        var button = Button("Consultar expediente", Mauve, Color.White); button.Width = 165; button.Height = 38; button.Margin = new Padding(0, 4, 8, 4);
        var result = new TextBox { Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, Dock = DockStyle.Fill, BorderStyle = BorderStyle.None, BackColor = Color.White, ForeColor = Charcoal, Font = new Font("Segoe UI", 10) };
        JsonElement? loadedRecord = null;
        button.Click += async (_, _) =>
        {
            var selected = patientPicker.SelectedItem as ChoiceItem;
            var criterion = selected is not null ? selected.Id.ToString() : search.Text.Trim();
            if (string.IsNullOrWhiteSpace(criterion)) { MessageBox.Show(this, "Selecciona un paciente o escribe un criterio de búsqueda."); return; }
            try { var data = await _api.GetAsync<JsonElement>($"api/Pacientes/expediente/{Uri.EscapeDataString(criterion)}"); loadedRecord = data; result.Text = FormatRecord(data); }
            catch (Exception ex) { result.Text = UserMessage(ex); }
        };
        var edit = Button("Editar antecedentes clínicos", Color.White, Mauve); edit.Width = 190; edit.Height = 38; edit.Margin = new Padding(0, 4, 8, 4); edit.Visible = _api.IsDoctor;
        edit.Click += async (_, _) => { if (loadedRecord is { } record) await EditClinicalHistory(record, result); else MessageBox.Show(this, "Consulta primero el expediente del paciente."); };
        var addConsultation = Button("Nueva consulta", Color.White, Mauve); addConsultation.Width = 150; addConsultation.Height = 38; addConsultation.Margin = new Padding(0, 4, 8, 4); addConsultation.Visible = _api.IsClinicalStaff;
        addConsultation.Click += async (_, _) => { if (loadedRecord is { } record) await AddConsultation(record, result); else MessageBox.Show(this, "Consulta primero el expediente del paciente."); };
        search.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) button.PerformClick(); };
        toolbar.Controls.AddRange([patientPicker, search, button, edit, addConsultation]);
        panel.Controls.Add(result); panel.Controls.Add(toolbar); _dynamicHost.Controls.Add(panel);
        _ = LoadExpedientePatients(patientPicker, result, preselectedPatientId);
    }

    private async Task LoadExpedientePatients(ComboBox picker, TextBox result, int? preselectedPatientId = null)
    {
        try
        {
            var patients = await _api.GetAsync<List<ApiPaciente>>("api/Pacientes/expediente/selector");
            var choices = patients.Select(p => new ChoiceItem(p.PacienteId, $"{p.Nombre} {p.Apellido} · #{p.PacienteId}")).ToList();
            picker.DataSource = choices;
            if (preselectedPatientId.HasValue) picker.SelectedItem = choices.FirstOrDefault(choice => choice.Id == preselectedPatientId.Value);
            if (patients.Count == 0) result.Text = "No hay pacientes disponibles para consultar.";
        }
        catch (Exception ex)
        {
            result.Text = UserMessage(ex);
            MessageBox.Show(this, UserMessage(ex), "No se pudo cargar el selector de pacientes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private async Task EditClinicalHistory(JsonElement record, TextBox result)
    {
        var patient = FindProperty(record, "paciente"); var history = FindProperty(record, "historial"); var patientId = ReadInt(patient, "pacienteId");
        if (patientId <= 0) { MessageBox.Show(this, "El expediente no contiene un paciente identificable."); return; }
        using var form = new EntityDialog("Antecedentes clínicos");
        var personal = form.AddText("Antecedentes personales", ReadText(history, "antecedentesPersonales") ?? ReadText(history, "antecedentes"), true);
        var family = form.AddText("Antecedentes familiares", ReadText(history, "antecedentesFamiliares"), true); var allergies = form.AddText("Alergias", ReadText(history, "alergias"), true);
        var medicine = form.AddText("Medicamentos actuales", ReadText(history, "medicamentos"), true); var notes = form.AddText("Notas clínicas", ReadText(history, "notasMedicas"), true);
        if (form.ShowDialog(this) != DialogResult.OK) return;
        await Run(async () =>
        {
            await _api.SendAsync(HttpMethod.Post, "api/HistorialClinico", new { HistoriaId = ReadInt(history, "historiaId"), PacienteId = patientId, FechaActualizacion = DateTime.UtcNow, Antecedentes = EmptyNull(personal.Text), AntecedentesPersonales = EmptyNull(personal.Text), AntecedentesFamiliares = EmptyNull(family.Text), Alergias = EmptyNull(allergies.Text), Medicamentos = EmptyNull(medicine.Text), NotasMedicas = EmptyNull(notes.Text) });
            MessageBox.Show(this, "Antecedentes clínicos guardados.", "Tanger Clinic"); var fresh = await _api.GetAsync<JsonElement>($"api/Pacientes/expediente/{patientId}"); result.Text = FormatRecord(fresh);
        });
    }

    private async Task AddConsultation(JsonElement record, TextBox result)
    {
        var patientId = ReadInt(FindProperty(record, "paciente"), "pacienteId");
        if (patientId <= 0) { MessageBox.Show(this, "El expediente no contiene un paciente identificable."); return; }
        using var form = new EntityDialog("Consulta / diagnóstico"); var date = form.AddDateTime("Fecha de consulta", DateTime.Now);
        var reason = form.AddText("Motivo de consulta"); var diagnosis = form.AddText("Diagnóstico", multiline: true); var notes = form.AddText("Observaciones", multiline: true);
        if (form.ShowDialog(this) != DialogResult.OK) return;
        if (string.IsNullOrWhiteSpace(reason.Text) && string.IsNullOrWhiteSpace(diagnosis.Text)) { MessageBox.Show(this, "Indica un motivo o diagnóstico.", "Validación"); return; }
        await Run(async () =>
        {
            await _api.SendAsync(HttpMethod.Post, "api/Consultas", new { ConsultaId = 0, PacienteId = patientId, CitaId = (int?)null, Fecha = date.Value, Motivo = EmptyNull(reason.Text), Diagnostico = EmptyNull(diagnosis.Text), Observaciones = EmptyNull(notes.Text) });
            MessageBox.Show(this, "Consulta registrada.", "Tanger Clinic"); var fresh = await _api.GetAsync<JsonElement>($"api/Pacientes/expediente/{patientId}"); result.Text = FormatRecord(fresh);
        });
    }

    private async Task ShowPatientPortal()
    {
        var tabs = new TabControl { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10) };
        var profilePage = new TabPage("Perfil"); var appointmentsPage = new TabPage("Citas"); var insurancePage = new TabPage("Seguro"); var recipesPage = new TabPage("Recetas");
        var profile = new Label { Dock = DockStyle.Fill, ForeColor = Charcoal, Font = new Font("Segoe UI", 11), Padding = new Padding(22), AutoEllipsis = true };
        profilePage.Controls.Add(profile);
        var schedule = Button("Solicitar cita", Mauve, Color.White); schedule.Dock = DockStyle.Left; schedule.Width = 150; schedule.Height = 40;
        var reschedule = Button("Reprogramar seleccionada", Color.White, Mauve); reschedule.Dock = DockStyle.Left; reschedule.Width = 205; reschedule.Height = 40;
        var cancelAppointment = Button("Cancelar seleccionada", Color.White, Mauve); cancelAppointment.Dock = DockStyle.Left; cancelAppointment.Width = 190; cancelAppointment.Height = 40;
        var appointmentToolbar = new Panel { Dock = DockStyle.Top, Height = 46, BackColor = Color.White };
        appointmentToolbar.Controls.Add(cancelAppointment); appointmentToolbar.Controls.Add(reschedule); appointmentToolbar.Controls.Add(schedule);
        var appointmentGrid = CreateGrid(); appointmentGrid.Dock = DockStyle.Fill;
        appointmentsPage.Controls.Add(appointmentGrid); appointmentsPage.Controls.Add(appointmentToolbar);
        var insuranceGrid = CreateGrid(); insuranceGrid.Dock = DockStyle.Fill; insurancePage.Controls.Add(insuranceGrid);
        var recipeGrid = CreateGrid(); recipeGrid.Dock = DockStyle.Fill; recipesPage.Controls.Add(recipeGrid);
        tabs.TabPages.AddRange([profilePage, appointmentsPage, insurancePage, recipesPage]);
        var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 12, 0, 0), BackColor = Color.White }; panel.Controls.Add(tabs); _dynamicHost.Controls.Add(panel);
        List<JsonElement> appointments = [];
        try
        {
            var me = await _api.GetAsync<JsonElement>("api/PortalPaciente/perfil");
            if (FindProperty(me, "vinculado").ValueKind == JsonValueKind.False)
            {
                profile.Text = "Tu cuenta todavía no está vinculada a una ficha de paciente. Contacta a recepción para asociar tu perfil clínico.";
                schedule.Enabled = reschedule.Enabled = cancelAppointment.Enabled = false;
            }
            else profile.Text = $"Hola, {ReadText(me, "nombre")} {ReadText(me, "apellido")}\r\n\r\nCédula: {ReadText(me, "cedula") ?? "—"}\r\nTeléfono: {ReadText(me, "telefono") ?? "—"}\r\nCorreo: {ReadText(me, "email") ?? "—"}\r\nEstado de seguro: {(FindProperty(me, "tieneSeguro").ValueKind == JsonValueKind.True ? "Tiene seguro médico" : "Paciente privado · Sin seguro")}";
            appointments = await _api.GetAsync<List<JsonElement>>("api/PortalPaciente/citas");
            appointmentGrid.DataSource = appointments.OrderBy(c => ReadDate(c, "fecha")).Select(c => new { Cita = ReadInt(c, "citaId"), Fecha = FormatDateTime12(ReadDate(c, "fecha")), Tipo = ReadText(c, "tipoCita"), Estado = ReadText(c, "estado") }).ToList();
            var insurance = await _api.GetAsync<List<JsonElement>>("api/PortalPaciente/seguro");
            insuranceGrid.DataSource = insurance.Select(p => new { Aseguradora = ReadText(p, "seguro"), Poliza = ReadText(p, "numeroPoliza"), Vence = ReadText(p, "fechaVencimiento"), Observaciones = ReadText(p, "observaciones") }).ToList();
            var recipes = await _api.GetAsync<List<JsonElement>>("api/PortalPaciente/recetas");
            recipeGrid.DataSource = recipes.Select(r => new { Receta = ReadInt(r, "recetaId"), Fecha = ReadDate(r, "fecha").ToString("dd/MM/yyyy"), Indicaciones = ReadText(r, "indicaciones"), Medicamentos = FindProperty(r, "medicamentos").ValueKind == JsonValueKind.Array ? string.Join("\r\n", FindProperty(r, "medicamentos").EnumerateArray().Select(m => $"{ReadText(m, "medicamento")} · {ReadText(m, "dosis")} · {ReadText(m, "frecuencia")} · {ReadText(m, "duracion")} · {ReadText(m, "instrucciones")}")) : "" }).ToList();
        }
        catch (Exception ex) { profile.Text = UserMessage(ex); }
        schedule.Click += async (_, _) =>
        {
            using var form = new EntityDialog("Solicitar una cita"); var date = form.AddDateTime("Fecha y hora", DateTime.Now.AddDays(1)); var type = form.AddText("Tipo de cita", "Consulta"); var notes = form.AddText("Observaciones", multiline: true);
            if (form.ShowDialog(this) != DialogResult.OK) return;
            await Run(async () => { await _api.SendAsync(HttpMethod.Post, "api/PortalPaciente/citas", new { Fecha = date.Value, TipoCita = EmptyNull(type.Text), Observaciones = EmptyNull(notes.Text) }); MessageBox.Show(this, "Solicitud de cita enviada.", "Tanger Clinic", MessageBoxButtons.OK, MessageBoxIcon.Information); await ShowSection("Mi perfil, citas y recetas"); });
        };
        reschedule.Click += async (_, _) =>
        {
            var id = appointmentGrid.CurrentRow?.Cells["Cita"].Value;
            if (id is null || Convert.ToInt32(id) <= 0) { MessageBox.Show(this, "Selecciona una cita próxima para reprogramar.", "Tanger Clinic", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            var appointment = appointments.FirstOrDefault(c => ReadInt(c, "citaId") == Convert.ToInt32(id));
            var currentDate = ReadDate(appointment, "fecha");
            using var form = new EntityDialog("Reprogramar cita");
            var date = form.AddDateTime("Nueva fecha y hora", currentDate > DateTime.Now ? currentDate : DateTime.Now.AddDays(1));
            if (form.ShowDialog(this) != DialogResult.OK || date.Value <= DateTime.Now) return;
            await Run(async () => { await _api.SendAsync(HttpMethod.Put, $"api/PortalPaciente/citas/{Convert.ToInt32(id)}", new { Fecha = date.Value }); await ShowSection("Mi perfil, citas y recetas"); });
        };
        cancelAppointment.Click += async (_, _) =>
        {
            var id = appointmentGrid.CurrentRow?.Cells["Cita"].Value;
            if (id is null || Convert.ToInt32(id) <= 0) { MessageBox.Show(this, "Selecciona una cita próxima para cancelar.", "Tanger Clinic", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            if (MessageBox.Show(this, "¿Deseas cancelar esta cita?", "Confirmar cancelación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            await Run(async () => { await _api.DeleteAsync($"api/PortalPaciente/citas/{Convert.ToInt32(id)}"); await ShowSection("Mi perfil, citas y recetas"); });
        };
    }
}
