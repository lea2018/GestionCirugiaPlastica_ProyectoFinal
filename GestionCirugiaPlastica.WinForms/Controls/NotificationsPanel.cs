namespace GestionCirugiaPlastica.WinForms.Controls;

internal partial class NotificationsPanel : UserControl
{
    private readonly ClinicApiClient _api = null!;
    private List<NotificationItem> _items = [];

    public NotificationsPanel()
    {
        InitializeComponent();
    }

    public NotificationsPanel(ClinicApiClient api) : this()
    {
        _api = api;
        _btnRefresh.Click += async (_, _) => await LoadAsync();
        _btnMarkAllRead.Click += async (_, _) =>
        {
            SetBusy(true, "Marcando todas como leídas...");
            try { await _api.PatchAsync("api/Notificaciones/leer-todas"); await LoadAsync(); }
            catch (Exception ex) { _status.Text = ex.Message; }
            finally { SetBusy(false); }
        };
        _btnMarkSelectedRead.Click += async (_, _) =>
        {
            if (_grid.CurrentRow?.Index is not int index || index < 0 || index >= _items.Count) return;
            SetBusy(true, "Guardando estado de notificación...");
            try { await _api.PatchAsync($"api/Notificaciones/{_items[index].NotificacionId}/leer"); await LoadAsync(); }
            catch (Exception ex) { _status.Text = ex.Message; }
            finally { SetBusy(false); }
        };
        _ = LoadAsync();
    }

    private void SetBusy(bool busy, string? statusMessage = null)
    {
        _btnRefresh.Enabled = !busy;
        _btnMarkAllRead.Enabled = !busy;
        _btnMarkSelectedRead.Enabled = !busy;
        if (statusMessage is not null)
            _status.Text = statusMessage;
    }

    private async Task LoadAsync()
    {
        SetBusy(true, "Cargando notificaciones...");
        try
        {
            _items = await _api.GetAsync<List<NotificationItem>>("api/Notificaciones");
            _grid.DataSource = _items.Select(x => new { x.NotificacionId, x.Titulo, x.Mensaje, Fecha = x.FechaCreacion.ToString("dd/MM/yyyy HH:mm"), Estado = x.Leida ? "Leída" : "Pendiente", x.CitaId }).ToList();
            _status.Text = $"{_items.Count(x => !x.Leida)} notificaciones sin leer de {_items.Count}.";
        }
        catch (Exception ex) { _status.Text = ex.Message; }
        finally { SetBusy(false); }
    }

    private sealed class NotificationItem
    {
        public int NotificacionId { get; set; }
        public string Tipo { get; set; } = "";
        public string Titulo { get; set; } = "";
        public string Mensaje { get; set; } = "";
        public int? CitaId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Leida { get; set; }
    }
}
