using System.Drawing;
using System.Net.Http;
using System.Text.Json;

namespace GestionCirugiaPlastica.WinForms.Forms;

internal partial class PublicAppointmentForm : Form
{
    private readonly ClinicApiClient _api = null!;

    public PublicAppointmentForm()
    {
        InitializeComponent();
    }

    public PublicAppointmentForm(ClinicApiClient api) : this()
    {
        _api = api;
        _date.Value = DateTime.Today.AddDays(1).AddHours(9);
        _date.MinDate = DateTime.Now.AddMinutes(1);
        _reason.SelectedIndex = 0;
        _bookButton.Click += async (_, _) => await BookAsync();
        _cancelButton.Click += async (_, _) => await CancelAsync();
    }

    private async Task BookAsync()
    {
        if (string.IsNullOrWhiteSpace(_name.Text) || string.IsNullOrWhiteSpace(_surname.Text)) { ShowMessage(_bookingMessage, "Indica tu nombre y apellido.", false); return; }
        if (_date.Value <= DateTime.Now) { ShowMessage(_bookingMessage, "Selecciona una fecha y hora futuras.", false); return; }
        var email = _email.Text.Trim();
        if (_name.Text.Length > 250 || _surname.Text.Length > 250 || _id.Text.Length > 20 || _phone.Text.Length > 250 || email.Length > 250 || _notes.Text.Length > 1000)
        { ShowMessage(_bookingMessage, "Revisa la longitud de los datos ingresados.", false); return; }
        if (email.Length > 0 && !System.Net.Mail.MailAddress.TryCreate(email, out _)) { ShowMessage(_bookingMessage, "El correo electrónico no es válido.", false); return; }
        _bookButton.Enabled = false;
        try
        {
            var response = await _api.SendForJsonAsync<JsonElement>(HttpMethod.Post, "api/Citas/publica", new
            {
                Nombre = _name.Text.Trim(),
                Apellido = _surname.Text.Trim(),
                Cedula = EmptyNull(_id.Text),
                Telefono = EmptyNull(_phone.Text),
                Email = EmptyNull(email),
                Fecha = _date.Value,
                TipoCita = _reason.Text,
                Observaciones = EmptyNull(_notes.Text)
            });
            var code = ReadText(response, "CodigoGestion") ?? "";
            ShowMessage(_bookingMessage, $"Solicitud recibida. La cita queda pendiente de confirmación. Código de gestión: {code}", true);
            if (!string.IsNullOrWhiteSpace(code) && MessageBox.Show(this, $"Código de gestión:\r\n{code}\r\n\r\n¿Copiarlo al portapapeles?", "Solicitud recibida", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                Clipboard.SetText(code);
            _name.Clear(); _surname.Clear(); _id.Clear(); _phone.Clear(); _email.Clear(); _notes.Clear();
            _date.Value = DateTime.Today.AddDays(1).AddHours(9);
        }
        catch (Exception ex) { ShowMessage(_bookingMessage, ex.Message, false); }
        finally { _bookButton.Enabled = true; }
    }

    private async Task CancelAsync()
    {
        if (string.IsNullOrWhiteSpace(_code.Text) && (string.IsNullOrWhiteSpace(_cancelId.Text) || string.IsNullOrWhiteSpace(_cancelPhone.Text)))
        { ShowMessage(_cancelMessage, "Indica el código o completa tanto la cédula como el teléfono.", false); return; }
        _cancelButton.Enabled = false;
        try
        {
            await _api.SendAsync(HttpMethod.Post, "api/Citas/publica/cancelar", new { CodigoGestion = EmptyNull(_code.Text), Cedula = EmptyNull(_cancelId.Text), Telefono = EmptyNull(_cancelPhone.Text) });
            ShowMessage(_cancelMessage, "La cita fue cancelada.", true);
            _code.Clear(); _cancelId.Clear(); _cancelPhone.Clear();
        }
        catch (Exception ex) { ShowMessage(_cancelMessage, ex.Message, false); }
        finally { _cancelButton.Enabled = true; }
    }

    private static void ShowMessage(Label label, string text, bool success) { label.Text = text; label.ForeColor = success ? ColorTranslator.FromHtml("#507A60") : Color.Firebrick; }
    private static string? EmptyNull(string value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    private static string? ReadText(JsonElement element, string property)
    {
        if (element.ValueKind != JsonValueKind.Object) return null;
        foreach (var item in element.EnumerateObject())
            if (string.Equals(item.Name, property, StringComparison.OrdinalIgnoreCase) && item.Value.ValueKind == JsonValueKind.String)
                return item.Value.GetString();
        return null;
    }

    private void _cancelContent_Paint(object sender, PaintEventArgs e)
    {

    }
}
