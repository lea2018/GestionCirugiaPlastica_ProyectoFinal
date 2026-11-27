using System.Drawing;
using System.Globalization;
using System.Text.Json;

namespace GestionCirugiaPlastica.WinForms;

internal partial class MainForm
{
    private async Task ShowDashboard()
    {
        if (_api.IsAssistant)
        {
            await ShowClinicalDashboard();
            return;
        }

        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4, Padding = new Padding(0, 8, 0, 0), BackColor = Color.Transparent };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 125));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 86));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        var dashboardStatus = new Label { Text = "Consultando información real de la API…", Dock = DockStyle.Fill, ForeColor = Taupe, TextAlign = ContentAlignment.MiddleLeft };
        var metricFlow = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, AutoScroll = true, BackColor = Color.Transparent, Padding = new Padding(0, 0, 0, 8) };
        var patientsCard = DashboardMetric("Total de pacientes", "—", "Registros en el sistema");
        var newPatientsCard = DashboardMetric("Pacientes nuevos", "—", "Registrados este mes");
        var appointmentsCard = DashboardMetric("Citas próximas", "—", "En los próximos 7 días");
        var revenueCard = DashboardMetric("Ingresos del mes", "—", "Pagos recibidos");
        foreach (var card in new[] { patientsCard, newPatientsCard, appointmentsCard, revenueCard }) metricFlow.Controls.Add(card);

        var revenueDetails = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 2, BackColor = Color.White, Padding = new Padding(12, 8, 12, 6), Margin = new Padding(0, 0, 0, 8) };
        for (var i = 0; i < 4; i++) revenueDetails.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        revenueDetails.RowStyles.Add(new RowStyle(SizeType.Absolute, 25)); revenueDetails.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        string[] revenueNames = ["Otros cobros", "Citas", "Cirugías", "Gastos registrados"];
        var revenueValues = new Label[4];
        for (var i = 0; i < revenueNames.Length; i++)
        {
            revenueDetails.Controls.Add(new Label { Text = revenueNames[i], Dock = DockStyle.Fill, ForeColor = Taupe, Font = new Font("Segoe UI", 9) }, i, 0);
            revenueValues[i] = new Label { Text = "—", Dock = DockStyle.Fill, ForeColor = Charcoal, Font = new Font("Segoe UI", 13, FontStyle.Bold) };
            revenueDetails.Controls.Add(revenueValues[i], i, 1);
        }

        var recentCard = DashboardCard("Pacientes recientes", "Últimos pacientes registrados");
        var recentGrid = CreateGrid(); recentGrid.Dock = DockStyle.Fill; _grid = recentGrid;
        recentCard.Controls.Add(recentGrid);
        var activityCard = DashboardCard("Citas recientes", "Actividad de agenda reciente");
        var activityGrid = CreateGrid(); activityGrid.Dock = DockStyle.Fill;
        activityCard.Controls.Add(activityGrid);
        var lower = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, BackColor = Color.Transparent };
        lower.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55)); lower.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
        lower.Controls.Add(recentCard, 0, 0); lower.Controls.Add(activityCard, 1, 0);
        metricFlow.SizeChanged += (_, _) => { var width = Math.Max(145, (metricFlow.ClientSize.Width - 52) / 4); foreach (Control item in metricFlow.Controls) item.Width = width; };
        layout.Controls.Add(metricFlow, 0, 0); layout.Controls.Add(revenueDetails, 0, 1); layout.Controls.Add(lower, 0, 2); layout.Controls.Add(dashboardStatus, 0, 3);
        _dynamicHost.Controls.Add(layout);
        try
        {
            var patientTask = _api.GetAsync<List<ApiPaciente>>("api/Pacientes");
            var appointmentTask = _api.GetAsync<List<ApiCita>>("api/Citas");
            var paymentTask = _api.GetAsync<List<ApiEstadoCuentaPago>>("api/Pagos/estado-cuenta");
            var incomeTask = _api.GetAsync<List<ApiPago>>("api/Pagos");
            var expenseTask = _api.IsDoctor ? _api.GetAsync<List<ApiDashboardExpense>>("api/Gastos") : Task.FromResult(new List<ApiDashboardExpense>());
            await Task.WhenAll(patientTask, appointmentTask, paymentTask, incomeTask, expenseTask);
            var patients = await patientTask;
            var appointments = await appointmentTask;
            var payments = await paymentTask;
            var incomeItems = (await incomeTask).Where(p => string.Equals(p.Estado, "Pagado", StringComparison.OrdinalIgnoreCase) && p.Fecha.Year == DateTime.Today.Year && p.Fecha.Month == DateTime.Today.Month).ToList();
            var expenses = await expenseTask;
            ((Label)patientsCard.Tag!).Text = patients.Count.ToString(CultureInfo.CurrentCulture);
            ((Label)newPatientsCard.Tag!).Text = patients.Count(p => p.FechaRegistro >= new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)).ToString(CultureInfo.CurrentCulture);
            ((Label)appointmentsCard.Tag!).Text = appointments.Count(c => c.Fecha >= DateTime.Now && c.Fecha <= DateTime.Now.AddDays(7) && !string.Equals(c.Estado, "Cancelada", StringComparison.OrdinalIgnoreCase)).ToString(CultureInfo.CurrentCulture);
            ((Label)revenueCard.Tag!).Text = payments.Where(p => p.EsRecibido && p.Fecha.Month == DateTime.Today.Month && p.Fecha.Year == DateTime.Today.Year).Sum(p => p.Monto).ToString("C", CultureInfo.GetCultureInfo("es-DO"));
            revenueValues[0].Text = incomeItems.Where(p => !p.CitaId.HasValue && !p.CirugiaId.HasValue).Sum(p => p.Monto).ToString("C", CultureInfo.GetCultureInfo("es-DO"));
            revenueValues[1].Text = incomeItems.Where(p => p.CitaId.HasValue && !p.CirugiaId.HasValue).Sum(p => p.Monto).ToString("C", CultureInfo.GetCultureInfo("es-DO"));
            revenueValues[2].Text = incomeItems.Where(p => p.CirugiaId.HasValue).Sum(p => p.Monto).ToString("C", CultureInfo.GetCultureInfo("es-DO"));
            revenueValues[3].Text = expenses.Where(e => e.Fecha.Year == DateTime.Today.Year && e.Fecha.Month == DateTime.Today.Month).Sum(e => e.Monto).ToString("C", CultureInfo.GetCultureInfo("es-DO"));
            if (!_api.IsDoctor) revenueDetails.Controls[6].Visible = revenueDetails.Controls[7].Visible = false;
            recentGrid.DataSource = patients.OrderByDescending(p => p.FechaRegistro).Take(5).Select(p =>
            {
                var lastAppointment = appointments.Where(c => c.PacienteId == p.PacienteId).OrderByDescending(c => c.Fecha).FirstOrDefault();
                var age = p.FechaNacimiento is DateOnly birthday ? Math.Max(0, DateOnly.FromDateTime(DateTime.Today).Year - birthday.Year - (DateOnly.FromDateTime(DateTime.Today) < birthday.AddYears(DateOnly.FromDateTime(DateTime.Today).Year - birthday.Year) ? 1 : 0)) : (int?)null;
                return new { p.PacienteId, Paciente = $"{p.Nombre} {p.Apellido}", Edad = age?.ToString() ?? "—", UltimaActividad = lastAppointment?.Fecha.ToString("dd MMM yyyy HH:mm", CultureInfo.GetCultureInfo("es-DO")) ?? p.FechaRegistro.ToString("dd MMM yyyy", CultureInfo.GetCultureInfo("es-DO")), Motivo = lastAppointment?.TipoCita ?? "Paciente registrado", Contacto = p.Telefono ?? p.Email ?? "—" };
            }).ToList();
            activityGrid.DataSource = appointments.OrderByDescending(c => c.Fecha).Take(6).Select(c => new { Cita = c.CitaId, Paciente = c.Paciente is null ? $"#{c.PacienteId}" : $"{c.Paciente.Nombre} {c.Paciente.Apellido}", Fecha = FormatDateTime12(c.Fecha), Motivo = c.TipoCita, c.Estado }).ToList();
            _calendarAppointments = appointments;
            RefreshRightCalendar();
            dashboardStatus.Text = "Información actualizada desde GestionCirugiaPlastica.Api";
        }
        catch (Exception ex)
        {
            dashboardStatus.ForeColor = Color.Firebrick; dashboardStatus.Text = "No se pudo cargar el resumen desde la API. " + UserMessage(ex);
            ((Label)patientsCard.Tag!).Text = ((Label)newPatientsCard.Tag!).Text = ((Label)appointmentsCard.Tag!).Text = ((Label)revenueCard.Tag!).Text = "—";
        }
    }

    private async Task ShowClinicalDashboard()
    {
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(0, 8, 0, 0), BackColor = Color.Transparent };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 125));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        var status = new Label { Text = "Consultando información clínica…", Dock = DockStyle.Fill, ForeColor = Taupe, TextAlign = ContentAlignment.MiddleLeft };
        var metrics = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, AutoScroll = true, BackColor = Color.Transparent };
        var patientsCard = DashboardMetric("Pacientes accesibles", "—", "Selector clínico autorizado");
        var appointmentsCard = DashboardMetric("Citas próximas", "—", "En los próximos 7 días");
        var surgeriesCard = DashboardMetric("Cirugías programadas", "—", "Seguimiento clínico");
        var recipesCard = DashboardMetric("Recetas", "—", "Registros disponibles");
        foreach (var card in new[] { patientsCard, appointmentsCard, surgeriesCard, recipesCard }) metrics.Controls.Add(card);
        metrics.SizeChanged += (_, _) => { var width = Math.Max(145, (metrics.ClientSize.Width - 40) / 4); foreach (Control item in metrics.Controls) item.Width = width; };

        var surgeriesCardList = DashboardCard("Cirugías recientes", "Seguimiento de procedimientos");
        var surgeriesGrid = CreateGrid(); surgeriesGrid.Dock = DockStyle.Fill; surgeriesCardList.Controls.Add(surgeriesGrid);
        var appointmentsCardList = DashboardCard("Citas recientes", "Actividad clínica de agenda");
        var appointmentsGrid = CreateGrid(); appointmentsGrid.Dock = DockStyle.Fill; appointmentsCardList.Controls.Add(appointmentsGrid);
        var lists = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, BackColor = Color.Transparent };
        lists.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55)); lists.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
        lists.Controls.Add(surgeriesCardList, 0, 0); lists.Controls.Add(appointmentsCardList, 1, 0);
        layout.Controls.Add(metrics, 0, 0); layout.Controls.Add(lists, 0, 1); layout.Controls.Add(status, 0, 2);
        _dynamicHost.Controls.Add(layout);

        try
        {
            var patientsTask = _api.GetAsync<List<ApiPaciente>>("api/Pacientes/expediente/selector");
            var appointmentsTask = _api.GetAsync<List<ApiCita>>("api/Citas");
            var surgeriesTask = _api.GetAsync<List<ApiCirugia>>("api/Cirugias");
            var recipesTask = _api.GetAsync<List<ApiReceta>>("api/Recetas");
            await Task.WhenAll(patientsTask, appointmentsTask, surgeriesTask, recipesTask);
            var patients = await patientsTask;
            var appointments = await appointmentsTask;
            var surgeries = await surgeriesTask;
            var recipes = await recipesTask;

            ((Label)patientsCard.Tag!).Text = patients.Count.ToString(CultureInfo.CurrentCulture);
            ((Label)appointmentsCard.Tag!).Text = appointments.Count(c => c.Fecha >= DateTime.Now && c.Fecha <= DateTime.Now.AddDays(7) && !string.Equals(c.Estado, "Cancelada", StringComparison.OrdinalIgnoreCase)).ToString(CultureInfo.CurrentCulture);
            ((Label)surgeriesCard.Tag!).Text = surgeries.Count(s => string.Equals(s.Estado, "Programada", StringComparison.OrdinalIgnoreCase) && s.FechaCirugia >= DateTime.Today).ToString(CultureInfo.CurrentCulture);
            ((Label)recipesCard.Tag!).Text = recipes.Count.ToString(CultureInfo.CurrentCulture);
            surgeriesGrid.DataSource = surgeries.OrderByDescending(s => s.FechaCirugia).Take(6).Select(s => new { Cirugía = s.CirugiaId, Paciente = s.Paciente is null ? $"#{s.PacienteId}" : $"{s.Paciente.Nombre} {s.Paciente.Apellido}", Procedimiento = s.Procedimiento?.Nombre, Fecha = FormatDateTime12(s.FechaCirugia), s.Estado }).ToList();
            appointmentsGrid.DataSource = appointments.OrderByDescending(c => c.Fecha).Take(6).Select(c => new { Cita = c.CitaId, Paciente = c.Paciente is null ? $"#{c.PacienteId}" : $"{c.Paciente.Nombre} {c.Paciente.Apellido}", Fecha = FormatDateTime12(c.Fecha), Motivo = c.TipoCita, c.Estado }).ToList();
            _calendarAppointments = appointments;
            RefreshRightCalendar();
            status.Text = "Resumen clínico cargado desde los endpoints autorizados.";
        }
        catch (Exception ex)
        {
            status.ForeColor = Color.Firebrick; status.Text = "No se pudo cargar el resumen clínico. " + UserMessage(ex);
            ((Label)patientsCard.Tag!).Text = ((Label)appointmentsCard.Tag!).Text = ((Label)surgeriesCard.Tag!).Text = ((Label)recipesCard.Tag!).Text = "—";
        }
    }

    private static Panel DashboardMetric(string title, string value, string caption)
    {
        var card = DashboardCard(title, caption);
        var valueLabel = new Label { Text = value, Dock = DockStyle.Top, Height = 37, ForeColor = Charcoal, Font = new Font("Segoe UI", 20, FontStyle.Bold) };
        card.Tag = valueLabel; card.Dock = DockStyle.None; card.Width = 218; card.Height = 108; card.Margin = new Padding(0, 0, 12, 0); card.Padding = new Padding(14);
        card.Controls.Add(valueLabel);
        return card;
    }

    private static Panel DashboardCard(string title, string subtitle)
    {
        var card = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(18), Margin = new Padding(0, 0, 12, 12) };
        card.Paint += (_, e) => { using var pen = new Pen(Color.FromArgb(231, 221, 224)); e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1); };
        card.Controls.Add(new Label { Text = subtitle, Dock = DockStyle.Top, Height = 24, ForeColor = Taupe, Font = new Font("Segoe UI", 9) });
        card.Controls.Add(new Label { Text = title, Dock = DockStyle.Top, Height = 27, ForeColor = Charcoal, Font = new Font("Segoe UI", 12, FontStyle.Bold) });
        return card;
    }

    private async Task LoadSection(string name)
    {
        try
        {
            _status.Text = "Cargando…";
            _grid?.Columns.Clear();
            switch (name)
            {
                case "Pacientes":
                    _patients = await _api.GetAsync<List<ApiPaciente>>("api/Pacientes");
                    _grid!.DataSource = _patients.Select(p => new { p.PacienteId, Nombre = $"{p.Nombre} {p.Apellido}", p.Cedula, p.Telefono, p.Email, Seguro = p.TieneSeguro ? "Sí" : "No", Registro = p.FechaRegistro.ToString("dd/MM/yyyy") }).ToList();
                    AddRowAction("Editar", async row => { var p = _patients.First(x => x.PacienteId == Convert.ToInt32(row.Cells["PacienteId"].Value)); await EditPatient(p); });
                    if (_api.IsDoctor) AddRowAction("Expediente", async row => await ShowSection("Expedientes", Convert.ToInt32(row.Cells["PacienteId"].Value)));
                    AddRowAction("Eliminar", async row => await DeleteRecord("api/Pacientes/" + row.Cells["PacienteId"].Value, "¿Eliminar este paciente?")); break;
                case "Citas":
                    var citas = await _api.GetAsync<List<ApiCita>>("api/Citas");
                    _grid!.DataSource = citas.Select(c => new { c.CitaId, Paciente = c.Paciente is null ? $"#{c.PacienteId}" : $"{c.Paciente.Nombre} {c.Paciente.Apellido}", c.TipoCita, Fecha = FormatDateTime12(c.Fecha), c.Estado, c.EstadoPago, c.Costo }).ToList();
                    AddRowAction("Editar", async row => { var c = citas.First(x => x.CitaId == Convert.ToInt32(row.Cells["CitaId"].Value)); await EditAppointment(c); });
                    AddRowAction("Estado", async row => await ChangeAppointmentStatus(Convert.ToInt32(row.Cells["CitaId"].Value)));
                    AddRowAction("Eliminar", async row => await DeleteRecord("api/Citas/" + row.Cells["CitaId"].Value, "¿Eliminar esta cita?")); break;
                case "Cirugías":
                    var surgeries = await _api.GetAsync<List<ApiCirugia>>("api/Cirugias");
                    var visibleSurgeries = surgeries.Where(s => _surgeryStatusFilter switch
                    {
                        "Todos los estados" => true,
                        _ => string.Equals(s.Estado, _surgeryStatusFilter, StringComparison.OrdinalIgnoreCase)
                    }).OrderByDescending(s => s.FechaCirugia).ToList();
                    _grid!.DataSource = visibleSurgeries.Select(c => new { c.CirugiaId, Paciente = c.Paciente is null ? $"#{c.PacienteId}" : $"{c.Paciente.Nombre} {c.Paciente.Apellido}", Procedimiento = c.Procedimiento?.Nombre, Fecha = FormatDateTime12(c.FechaCirugia), c.Estado, Descripcion = c.Observaciones }).ToList();
                    AddRowAction("Editar", async row => await EditSurgery(visibleSurgeries.First(x => x.CirugiaId == Convert.ToInt32(row.Cells["CirugiaId"].Value))));
                    AddRowAction("Eliminar", async row => await DeleteRecord("api/Cirugias/" + row.Cells["CirugiaId"].Value, "¿Eliminar esta cirugía?")); break;
                case "Pagos":
                    var accountEntries = await _api.GetAsync<List<ApiEstadoCuentaPago>>("api/Pagos/estado-cuenta");
                    _grid!.DataSource = accountEntries.Select(p => new { p.PagoId, Fecha = FormatDateTime12(p.Fecha), p.Paciente, p.Destino, p.Concepto, p.MetodoPago, p.Estado, Tipo = p.EsRecibido ? "Recibido" : "Saldo pendiente", p.Monto }).ToList();
                    AddRowAction("Registrar / editar", async row =>
                    {
                        var idValue = row.Cells["PagoId"].Value;
                        if (idValue is not null && idValue is not DBNull && Convert.ToInt32(idValue) > 0)
                        {
                            var payment = await _api.GetAsync<ApiPago>($"api/Pagos/{Convert.ToInt32(idValue)}");
                            await EditPayment(payment);
                        }
                        else
                        {
                            var citaId = row.Cells["Destino"].Value?.ToString();
                            var pending = accountEntries.First(x => x.PagoId is null && x.Destino == citaId);
                            await EditPayment(null, pending);
                        }
                    });
                    AddRowAction("Eliminar", async row =>
                    {
                        var idValue = row.Cells["PagoId"].Value;
                        if (idValue is null || idValue is DBNull) { MessageBox.Show(this, "Este saldo es calculado a partir del costo de la cita; no es una transacción guardada que se pueda eliminar."); return; }
                        await DeleteRecord("api/Pagos/" + idValue, "¿Eliminar este pago recibido?");
                    }); break;
            }
            _status.ForeColor = Mauve; _status.Text = $"{_grid?.Rows.Count ?? 0} registros";
        }
        catch (Exception ex)
        {
            if (_grid is not null) _grid.DataSource = null;
            _status.ForeColor = Color.Firebrick; _status.Text = UserMessage(ex);
            MessageBox.Show(this, UserMessage(ex), $"No se pudo cargar {name}", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private DataGridView CreateGrid() => new()
    {
        BackgroundColor = Color.White, BorderStyle = BorderStyle.None, GridColor = Color.FromArgb(235, 231, 230),
        AllowUserToAddRows = false, AllowUserToDeleteRows = false, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        RowHeadersVisible = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false,
        ColumnHeadersHeight = 44, RowTemplate = { Height = 42 }, EnableHeadersVisualStyles = false
    };

    private void AddRowAction(string title, Func<DataGridViewRow, Task> action)
    {
        var column = new DataGridViewButtonColumn { Name = "Accion" + title, HeaderText = "", Text = title, UseColumnTextForButtonValue = true, FillWeight = 55, FlatStyle = FlatStyle.Flat };
        _grid!.Columns.Add(column);
        _rowActions[column.Name] = action;
        if (!ReferenceEquals(_actionGrid, _grid))
        {
            _grid.CellContentClick += async (sender, e) =>
            {
                if (sender is not DataGridView clickedGrid || e.RowIndex < 0 || e.ColumnIndex < 0) return;
                var columnName = clickedGrid.Columns[e.ColumnIndex].Name;
                if (_rowActions.TryGetValue(columnName, out var rowAction)) await Run(() => rowAction(clickedGrid.Rows[e.RowIndex]));
            };
            _actionGrid = _grid;
        }
    }

    private void ApplySearch(string text)
    {
        if (_grid is null) return;
        foreach (DataGridViewRow row in _grid.Rows)
            row.Visible = string.IsNullOrWhiteSpace(text) || row.Cells.Cast<DataGridViewCell>().Any(cell => cell.Value?.ToString()?.Contains(text, StringComparison.OrdinalIgnoreCase) == true);
    }
}
