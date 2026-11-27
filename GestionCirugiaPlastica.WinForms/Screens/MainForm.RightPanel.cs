using System.Drawing;
using System.Globalization;
using System.Text.Json;

namespace GestionCirugiaPlastica.WinForms;

internal partial class MainForm
{
    private void WireRightPanelEvents()
    {
        _btnRightProfile.Click += async (_, _) => await ShowSection("Mi perfil");
        _btnRightLogout.Click += (_, _) => { _api.Logout(); ShowLogin(); };
        _btnPrevMonth.Click += (_, _) => { _calendarMonth = _calendarMonth.AddMonths(-1); RefreshRightCalendar(); };
        _btnNextMonth.Click += (_, _) => { _calendarMonth = _calendarMonth.AddMonths(1); RefreshRightCalendar(); };
    }

    private async Task BuildRightPanel()
    {
        _rightProfile.Text = $"{_api.UserName}\r\n{_api.Role}";
        RefreshRightCalendar();
        try
        {
            var profileData = await _api.GetAsync<JsonElement>("api/Perfil");
            var displayName = ReadText(profileData, "nombreCompleto") ?? _api.UserName ?? "Usuario";
            _rightProfile.Text = $"{displayName}\r\n{ReadText(profileData, "rol") ?? _api.Role}";
        }
        catch (Exception ex) { _rightProfile.Text = $"{_api.UserName}\r\n{_api.Role}\r\n{UserMessage(ex)}"; }
        if (_api.IsDoctor || _api.IsSecretary || _api.IsAssistant)
        {
            try { _calendarAppointments = await _api.GetAsync<List<ApiCita>>("api/Citas"); RefreshRightCalendar(); }
            catch (Exception ex) { _dayAppointments.Controls.Add(new Label { Text = UserMessage(ex), Width = 245, Height = 58, ForeColor = Color.Firebrick }); }
        }
        else if (_api.IsPatient)
        {
            try { _calendarAppointments = await _api.GetAsync<List<ApiCita>>("api/PortalPaciente/citas"); RefreshRightCalendar(); }
            catch (Exception ex) { _dayAppointments.Controls.Add(new Label { Text = UserMessage(ex), Width = 245, Height = 58, ForeColor = Color.Firebrick }); }
        }
    }

    private void AttachStatusBar()
    {
        _status.Text = "";
        _statusBar.Visible = true;
    }

    private void RefreshRightCalendar()
    {
        if (_calendarTitle is null || _calendarTable is null || _dayAppointments is null) return;
        _calendarTitle.Text = _calendarMonth.ToString("MMMM yyyy", CultureInfo.GetCultureInfo("es-DO"));
        _calendarTable.SuspendLayout(); _calendarTable.Controls.Clear();
        string[] weekdays = ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sá"];
        for (var i = 0; i < weekdays.Length; i++) _calendarTable.Controls.Add(new Label { Text = weekdays[i], Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, ForeColor = Taupe, Font = new Font("Segoe UI", 8, FontStyle.Bold) }, i, 0);
        var first = new DateTime(_calendarMonth.Year, _calendarMonth.Month, 1);
        var start = first.AddDays(-(int)first.DayOfWeek);
        for (var i = 0; i < 42; i++)
        {
            var date = start.AddDays(i);
            var hasAppointments = _calendarAppointments.Any(c => c.Fecha.Date == date.Date && !string.Equals(c.Estado, "Cancelada", StringComparison.OrdinalIgnoreCase));
            var day = new Button { Text = date.Day + (hasAppointments ? " •" : ""), Dock = DockStyle.Fill, Margin = new Padding(1), FlatStyle = FlatStyle.Flat, FlatAppearance = { BorderSize = 0 }, BackColor = date.Date == _calendarDay.Date ? Color.FromArgb(248, 233, 237) : Color.White, ForeColor = date.Month == _calendarMonth.Month ? Charcoal : Taupe, Font = new Font("Segoe UI", 8, date.Date == _calendarDay.Date ? FontStyle.Bold : FontStyle.Regular), Tag = date };
            day.Click += (_, _) => { _calendarDay = (DateTime)day.Tag!; RefreshRightCalendar(); };
            _calendarTable.Controls.Add(day, i % 7, (i / 7) + 1);
        }
        _calendarTable.ResumeLayout();
        _dayAppointments.Controls.Clear();
        var selectedAppointments = _calendarAppointments.Where(c => c.Fecha.Date == _calendarDay.Date && !string.Equals(c.Estado, "Cancelada", StringComparison.OrdinalIgnoreCase)).OrderBy(c => c.Fecha).ToList();
        if (selectedAppointments.Count == 0) _dayAppointments.Controls.Add(new Label { Text = _api.IsDoctor || _api.IsSecretary || _api.IsAssistant ? "No hay citas programadas para este día." : "La agenda está disponible para el equipo clínico.", Width = 245, Height = 46, ForeColor = Taupe });
        foreach (var appointment in selectedAppointments)
        {
            var patientName = appointment.Paciente is null ? $"Paciente #{appointment.PacienteId}" : $"{appointment.Paciente.Nombre} {appointment.Paciente.Apellido}";
            _dayAppointments.Controls.Add(new Label { Text = $"{appointment.Fecha:hh:mm tt}\r\n{appointment.TipoCita} · {patientName}\r\n{appointment.Estado}", Width = 250, Height = 65, Padding = new Padding(9), BackColor = Color.FromArgb(250, 247, 248), ForeColor = Charcoal, Font = new Font("Segoe UI", 9) });
        }
    }
}
