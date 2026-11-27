using System.Drawing;
using System.Globalization;
using System.Text.Json;
using GestionCirugiaPlastica.WinForms.Controls;
using GestionCirugiaPlastica.WinForms.Forms;

namespace GestionCirugiaPlastica.WinForms;

internal partial class MainForm : Form
{
    private static readonly Color Charcoal = ClinicTheme.Charcoal;
    private static readonly Color Taupe = ClinicTheme.Taupe;
    private static readonly Color Mauve = ClinicTheme.Mauve;
    private static readonly Color Rose = ClinicTheme.Rose;
    private readonly ClinicApiClient _api = null!;
    private string _section = "Inicio";
    private string _surgeryStatusFilter = "Todos los estados";
    private DataGridView? _grid;
    private DataGridView? _actionGrid;
    private readonly Dictionary<string, Func<DataGridViewRow, Task>> _rowActions = [];
    private List<ApiPaciente> _patients = [];
    private DateTime _calendarMonth = DateTime.Today;
    private DateTime _calendarDay = DateTime.Today;
    private List<ApiCita> _calendarAppointments = [];

    public MainForm()
    {
        InitializeComponent();
    }

    public MainForm(ClinicApiClient api) : this()
    {
        _api = api;
        WireStaticEvents();
        ShowLogin();
    }

    private void WireStaticEvents()
    {
        WireLoginEvents();
        WireWorkspaceEvents();
        WireRightPanelEvents();
    }

    private sealed class ApiDashboardExpense { public DateTime Fecha { get; set; } public decimal Monto { get; set; } }

    protected override void OnFormClosed(FormClosedEventArgs e) { _api?.Dispose(); base.OnFormClosed(e); }

    private void _topbarWelcome_Click(object sender, EventArgs e)
    {

    }

    private void _btnRightProfile_Click(object sender, EventArgs e)
    {

    }

    private void _navAgenda_Click(object sender, EventArgs e)
    {

    }
}
