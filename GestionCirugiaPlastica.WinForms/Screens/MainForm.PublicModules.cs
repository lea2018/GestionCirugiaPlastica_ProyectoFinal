using System.Drawing;
using GestionCirugiaPlastica.WinForms.Controls;
using GestionCirugiaPlastica.WinForms.Forms;

namespace GestionCirugiaPlastica.WinForms;

internal partial class MainForm
{
    private void ShowModule(Control module)
    {
        _grid = null;
        module.Dock = DockStyle.Fill;
        _dynamicHost.Controls.Add(module);
    }

    private async Task ShowPublicNews()
    {
        using var window = new Form
        {
            Text = "Noticias | Tanger Clinic",
            StartPosition = FormStartPosition.CenterParent,
            MinimumSize = new Size(760, 520),
            Size = new Size(900, 650),
            Font = new Font("Segoe UI", 10),
            BackColor = Color.FromArgb(246, 244, 243)
        };
        var header = new Label
        {
            Text = "Noticias de Tanger Clinic",
            Dock = DockStyle.Top,
            Height = 58,
            Padding = new Padding(20, 12, 10, 8),
            BackColor = Color.White,
            ForeColor = ColorTranslator.FromHtml("#36383A"),
            Font = new Font("Segoe UI", 18, FontStyle.Bold)
        };
        var panel = new NewsPanel(_api, false) { Dock = DockStyle.Fill };
        window.Controls.Add(panel);
        window.Controls.Add(header);
        await Task.Yield();
        window.ShowDialog(this);
    }
}
