using System.Drawing;
using GestionCirugiaPlastica.WinForms.Controls;

namespace GestionCirugiaPlastica.WinForms;

internal partial class MainForm
{
    private void WireWorkspaceEvents()
    {
        _sidebar.Paint += (_, e) =>
        {
            using var pen = new Pen(Color.FromArgb(240, 236, 238));
            e.Graphics.DrawLine(pen, _sidebar.Width - 1, 0, _sidebar.Width - 1, _sidebar.Height);
        };
        _topbar.Paint += (_, e) =>
        {
            using var pen = new Pen(Color.FromArgb(235, 231, 230));
            e.Graphics.DrawLine(pen, 0, _topbar.Height - 1, _topbar.Width, _topbar.Height - 1);
        };
        _rightPanel.Paint += (_, e) =>
        {
            using var pen = new Pen(Color.FromArgb(240, 236, 238));
            e.Graphics.DrawLine(pen, 0, 0, 0, _rightPanel.Height);
        };
        _brandIcon.Paint += (_, e) =>
        {
            using var brush = new System.Drawing.Drawing2D.LinearGradientBrush(_brandIcon.ClientRectangle, Mauve, Rose, 35f);
            e.Graphics.FillRectangle(brush, _brandIcon.ClientRectangle);
            TextRenderer.DrawText(e.Graphics, "✚", _brandIcon.Font, _brandIcon.ClientRectangle, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        };
        _sidebarFooter.Paint += (_, e) =>
        {
            using var pen = new Pen(Color.FromArgb(240, 236, 238));
            e.Graphics.DrawLine(pen, 0, 0, _sidebarFooter.Width, 0);
        };

        _btnSidebarLogout.Click += (_, _) => { _api.Logout(); ShowLogin(); };

        foreach (Button navButton in _menuFlow.Controls)
        {
            navButton.Click += async (_, _) =>
            {
                SelectNavButton(navButton);
                var name = navButton.Tag as string ?? navButton.Text.Trim();
                await ShowSection(name == "Agenda" ? "Citas" : name);
            };
        }

        _btnHeaderNewSurgery.Click += async (_, _) => await EditSurgery(null, () => LoadSection("Cirugías"));
        _sectionSearch.TextChanged += (_, _) => ApplySearch(_sectionSearch.Text);
        _surgeryStatusFilterCombo.SelectedValueChanged += async (_, _) =>
        {
            if (_surgeryStatusFilterCombo.SelectedItem is string selected && selected != _surgeryStatusFilter)
            {
                _surgeryStatusFilter = selected;
                if (_section == "Cirugías")
                    await LoadSection("Cirugías");
            }
        };
        _btnToolbarRefresh.Click += async (_, _) => await LoadSection(_section);
        _btnToolbarAddProcedure.Click += async (_, _) =>
        {
            if (await CreateProcedure())
            {
                await LoadSection("Cirugías");
                await EditSurgery(null);
            }
        };
        _btnToolbarAdd.Click += async (_, _) =>
        {
            if (_section == "Pacientes") await EditPatient(null);
            else if (_section == "Citas") await EditAppointment(null);
            else if (_section == "Cirugías") await EditSurgery(null);
            else await EditPayment(null);
        };
    }

    private void SelectNavButton(Button selectedButton)
    {
        foreach (Button item in _menuFlow.Controls)
        {
            var selected = ReferenceEquals(item, selectedButton);
            item.BackColor = selected ? Color.FromArgb(248, 233, 237) : Color.White;
            item.ForeColor = selected ? Mauve : Color.FromArgb(111, 105, 107);
            item.Font = new Font("Segoe UI", 10, selected ? FontStyle.Bold : FontStyle.Regular);
        }
    }

    private async Task BuildWorkspace()
    {
        _loginCard.Visible = false;
        AcceptButton = null;
        _workspacePanel.Visible = true;
        _workspacePanel.BringToFront();

        _topbarWelcome.Text = _api.IsAssistant ? "Área clínica" : _api.IsPatient ? "Mi portal de paciente" : "Bienvenido";
        _topbarCaption.Text = _api.IsAssistant ? "Expedientes y seguimiento médico" : _api.IsPatient ? "Consulta tu perfil, citas y recetas" : "Panel de control del consultorio";

        _footerUserLabel.Text = _api.UserName ?? "Usuario";
        _footerRoleLabel.Text = _api.Role ?? "";

        var isClinicalOrAdmin = _api.IsDoctor || _api.IsSecretary;
        _navResumen.Visible = isClinicalOrAdmin || _api.IsAssistant;
        _navAgenda.Visible = isClinicalOrAdmin;
        _navPacientes.Visible = isClinicalOrAdmin;
        _navPagos.Visible = isClinicalOrAdmin;
        _navCirugias.Visible = _api.IsDoctor || _api.IsAssistant;
        _navRecetas.Visible = _api.IsDoctor || _api.IsAssistant;
        _navAseguradoras.Visible = _api.IsDoctor;
        _navConfiguracion.Visible = _api.IsDoctor;
        _navExpedientes.Visible = _api.IsAssistant;
        _navInicio.Visible = _api.IsPatient;
        _navMiPortal.Visible = _api.IsPatient;
        _navNotificaciones.Visible = _api.IsDoctor;
        _navNoticias.Visible = true;

        await BuildRightPanel();

        var firstSection = _api.IsDoctor || _api.IsSecretary || _api.IsAssistant ? "Resumen" : "Inicio";
        SelectNavButton(firstSection == "Resumen" ? _navResumen : _navInicio);
        await ShowSection(firstSection);
    }

    private async Task ShowSection(string name, int? patientId = null)
    {
        _section = name;
        _dynamicHost.Controls.Clear();
        _dynamicHost.Visible = false;
        _sectionToolbar.Visible = false;
        _mainGrid.Visible = false;
        _statusBar.Visible = false;
        _grid = null;

        var pageName = name == "Expedientes" ? "Expediente clínico" : name == "Mi perfil, citas y recetas" ? "Mi perfil, citas y recetas" : name;
        _sectionTitle.Text = pageName;
        _sectionSubtitle.Text = name switch
        {
            "Resumen" => "Consulta la actividad y el rendimiento de tu consultorio",
            "Cirugías" => "Control clínico y seguimiento de operaciones",
            "Recetas" => "Medicamentos e indicaciones para tus pacientes",
            "Expedientes" => "Consulta y seguimiento del historial clínico",
            _ => "Gestión segura y centralizada de Tanger Clinic"
        };
        _btnHeaderNewSurgery.Visible = name == "Cirugías";

        if (name == "Resumen") { _dynamicHost.Visible = true; await ShowDashboard(); return; }
        if (name == "Inicio") { _dynamicHost.Visible = true; await ShowPatientHome(); return; }
        if (name is "Mi portal" or "Mi perfil, citas y recetas") { _dynamicHost.Visible = true; await ShowPatientPortal(); return; }
        if (name is "Expediente" or "Expedientes") { _dynamicHost.Visible = true; ShowRecordSearch(patientId); return; }
        if (name == "Recetas") { _dynamicHost.Visible = true; await ShowRecipes(); return; }
        if (name == "Aseguradoras") { _dynamicHost.Visible = true; await ShowInsurers(); return; }
        if (name == "Configuración") { _dynamicHost.Visible = true; await ShowConfiguration(); return; }
        if (name == "Mi perfil") { _dynamicHost.Visible = true; await ShowMyProfile(); return; }
        if (name == "Noticias") { _dynamicHost.Visible = true; ShowModule(new NewsPanel(_api, _api.IsDoctor || _api.IsSecretary)); return; }
        if (name == "Notificaciones") { _dynamicHost.Visible = true; ShowModule(new NotificationsPanel(_api)); return; }

        _sectionSearch.Text = "";
        _surgeryStatusFilterCombo.Visible = name == "Cirugías";
        if (name == "Cirugías")
            _surgeryStatusFilterCombo.SelectedItem = _surgeryStatusFilter;

        var isListSection = name is "Pacientes" or "Citas" or "Cirugías" or "Pagos";
        _btnToolbarRefresh.Visible = isListSection;
        _btnToolbarAddProcedure.Visible = name == "Cirugías";
        _btnToolbarAdd.Visible = isListSection;
        if (isListSection)
        {
            _btnToolbarAdd.Text = name == "Pacientes" ? "+  Nuevo paciente"
                : name == "Citas" ? "+  Nueva cita"
                : name == "Cirugías" ? "+  Nueva cirugía"
                : "+  Nuevo pago";
        }

        _sectionToolbar.Visible = true;
        _mainGrid.Visible = true;
        _statusBar.Visible = true;
        _status.Text = "";
        _grid = _mainGrid;

        await LoadSection(name);
    }
}
