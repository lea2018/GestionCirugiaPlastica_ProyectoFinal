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
            try { await _api.PatchAsync("api/Notificaciones/leer-todas"); await LoadAsync(); }
            catch (Exception ex) { _status.Text = ex.Message; }
        };
        _btnMarkSelectedRead.Click += async (_, _) =>
        {
            if (_grid.CurrentRow?.Index is not int index || index < 0 || index >= _items.Count) return;
            try { await _api.PatchAsync($"api/Notificaciones/{_items[index].NotificacionId}/leer"); await LoadAsync(); }
            catch (Exception ex) { _status.Text = ex.Message; }
        };
        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        try
        {
            _items = await _api.GetAsync<List<NotificationItem>>("api/Notificaciones");
            _grid.DataSource = _items.Select(x => new { x.NotificacionId, x.Titulo, x.Mensaje, Fecha = x.FechaCreacion.ToString("dd/MM/yyyy HH:mm"), Estado = x.Leida ? "Leída" : "Pendiente", x.CitaId }).ToList();
            _status.Text = $"{_items.Count(x => !x.Leida)} notificaciones sin leer de {_items.Count}.";
        }
        catch (Exception ex) { _status.Text = ex.Message; }
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
