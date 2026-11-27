using System.Drawing;
using GestionCirugiaPlastica.WinForms;

namespace GestionCirugiaPlastica.WinForms.Controls;

internal partial class NewsPanel : UserControl
{
    private readonly ClinicApiClient _api = null!;
    private readonly bool _canManage;
    private List<NewsItem> _items = [];

    public NewsPanel()
    {
        InitializeComponent();
    }

    public NewsPanel(ClinicApiClient api, bool canManage) : this()
    {
        _api = api;
        _canManage = canManage;
        _btnAdd.Visible = _canManage;
        _btnRefresh.Click += async (_, _) => await LoadAsync();
        _btnAdd.Click += async (_, _) => await EditAsync(null);
        _grid.CellDoubleClick += async (_, e) =>
        {
            if (e.RowIndex < 0 || e.RowIndex >= _items.Count) return;
            if (_canManage) await EditAsync(_items[e.RowIndex]);
            else ShowArticle(_items[e.RowIndex]);
        };
        _ = LoadAsync();
    }

    private void SetBusy(bool busy, string? statusMessage = null)
    {
        _btnRefresh.Enabled = !busy;
        _btnAdd.Enabled = !busy;
        if (statusMessage is not null)
            _status.Text = statusMessage;
    }

    private async Task LoadAsync()
    {
        SetBusy(true, "Cargando noticias...");
        try
        {
            _items = (await _api.GetAsync<List<NewsItem>>(_canManage ? "api/Noticias/admin" : "api/Noticias"))
                .OrderByDescending(x => x.FechaPublicacion).ToList();
            _grid.DataSource = _items
                .Select(x => new { x.NoticiaId, x.Titulo, x.Resumen, Fecha = x.FechaPublicacion.ToString("dd/MM/yyyy"), Estado = x.Publicada ? "Publicada" : "Borrador" }).ToList();
            _status.Text = $"{_items.Count} noticias. Doble clic para editar o cambiar publicación.";
        }
        catch (Exception ex) { _status.Text = ex.Message; }
        finally { SetBusy(false); }
    }

    private void ShowArticle(NewsItem item)
    {
        using var window = new Form { Text = item.Titulo, StartPosition = FormStartPosition.CenterParent, Size = new Size(760, 620), MinimumSize = new Size(600, 480), Font = new Font("Segoe UI", 10), BackColor = Color.White };
        var title = new Label { Text = item.Titulo, Dock = DockStyle.Top, Height = 54, Padding = new Padding(18, 12, 12, 6), Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = ClinicTheme.Charcoal };
        var date = new Label { Text = item.FechaPublicacion.ToString("dd/MM/yyyy"), Dock = DockStyle.Top, Height = 30, Padding = new Padding(20, 4, 10, 4), ForeColor = ClinicTheme.Taupe };
        var body = new TextBox { Text = (string.IsNullOrWhiteSpace(item.Resumen) ? "" : item.Resumen + Environment.NewLine + Environment.NewLine) + item.Contenido, Dock = DockStyle.Fill, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, BorderStyle = BorderStyle.None, Padding = new Padding(20), BackColor = Color.White, ForeColor = ClinicTheme.Charcoal, Font = new Font("Segoe UI", 11) };
        window.Controls.Add(body); window.Controls.Add(date); window.Controls.Add(title);
        window.ShowDialog(FindForm());
    }

    private async Task EditAsync(NewsItem? item)
    {
        using var dialog = new EntityDialog(item is null ? "Nueva noticia" : "Editar noticia");
        var title = dialog.AddText("Título *", item?.Titulo);
        var summary = dialog.AddText("Resumen (máximo 500 caracteres)", item?.Resumen);
        var content = dialog.AddText("Contenido *", item?.Contenido, true);
        var published = dialog.AddCheck("Publicada", item?.Publicada ?? true);
        if (dialog.ShowDialog(FindForm()) != DialogResult.OK) return;
        if (string.IsNullOrWhiteSpace(title.Text) || string.IsNullOrWhiteSpace(content.Text) || title.Text.Length > 200 || summary.Text.Length > 500 || content.Text.Length > 10000)
        {
            MessageBox.Show(FindForm(), "Completa el título y el contenido, y respeta las longitudes máximas.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        SetBusy(true, "Guardando noticia...");
        try
        {
            var payload = new NewsRequest(title.Text.Trim(), string.IsNullOrWhiteSpace(summary.Text) ? null : summary.Text.Trim(), content.Text.Trim(), published.Checked);
            if (item is null) await _api.SendAsync(HttpMethod.Post, "api/Noticias", payload);
            else await _api.SendAsync(HttpMethod.Put, $"api/Noticias/{item.NoticiaId}", payload);
            await LoadAsync();
        }
        catch (Exception ex)
        {
            _status.Text = ex.Message;
            MessageBox.Show(FindForm(), ex.Message, "No se pudo guardar la noticia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally { SetBusy(false); }
    }

    private sealed class NewsItem
    {
        public int NoticiaId { get; set; }
        public string Titulo { get; set; } = "";
        public string? Resumen { get; set; }
        public string Contenido { get; set; } = "";
        public DateTime FechaPublicacion { get; set; }
        public bool Publicada { get; set; }
    }
    private sealed record NewsRequest(string Titulo, string? Resumen, string Contenido, bool Publicada);
}
