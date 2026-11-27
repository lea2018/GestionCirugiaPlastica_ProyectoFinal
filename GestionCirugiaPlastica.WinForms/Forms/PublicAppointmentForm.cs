using System.Drawing;
using System.Net.Http;
using System.Net.Mail;
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
        WireValidation();
        _bookButton.Click += async (_, _) => await BookAsync();
        _cancelButton.Click += async (_, _) => await CancelAsync();
    }

    private void WireValidation()
    {
        foreach (var control in new Control[] { _name, _surname, _id, _phone, _email, _notes, _code, _cancelId, _cancelPhone })
            _errorProvider.SetIconPadding(control, -20);

        _name.TextChanged += (_, _) => ValidateRequiredText(_name, "El nombre es obligatorio.", 250);
        _name.Leave += (_, _) => ValidateRequiredText(_name, "El nombre es obligatorio.", 250);
        _surname.TextChanged += (_, _) => ValidateRequiredText(_surname, "El apellido es obligatorio.", 250);
        _surname.Leave += (_, _) => ValidateRequiredText(_surname, "El apellido es obligatorio.", 250);
        _id.TextChanged += (_, _) => ValidateOptionalId(_id);
        _id.Leave += (_, _) => ValidateOptionalId(_id);
        _phone.TextChanged += (_, _) => ValidateOptionalPhone(_phone);
        _phone.Leave += (_, _) => ValidateOptionalPhone(_phone);
        _email.TextChanged += (_, _) => ValidateOptionalEmail(_email);
        _email.Leave += (_, _) => ValidateOptionalEmail(_email);
        _notes.TextChanged += (_, _) => ValidateMaxLength(_notes, 1000, "Máximo 1000 caracteres.");
        _date.ValueChanged += (_, _) => ValidateFutureDate();

        _code.TextChanged += (_, _) => ValidateCancelInputs(onlyWhenNonEmpty: true);
        _cancelId.TextChanged += (_, _) => ValidateCancelInputs(onlyWhenNonEmpty: true);
        _cancelId.Leave += (_, _) => ValidateCancelInputs(onlyWhenNonEmpty: true);
        _cancelPhone.TextChanged += (_, _) => ValidateCancelInputs(onlyWhenNonEmpty: true);
        _cancelPhone.Leave += (_, _) => ValidateCancelInputs(onlyWhenNonEmpty: true);
    }

    private bool ValidateBooking()
    {
        var valid = true;
        valid &= ValidateRequiredText(_name, "El nombre es obligatorio.", 250);
        valid &= ValidateRequiredText(_surname, "El apellido es obligatorio.", 250);
        valid &= ValidateOptionalId(_id);
        valid &= ValidateOptionalPhone(_phone);
        valid &= ValidateOptionalEmail(_email);
        valid &= ValidateMaxLength(_notes, 1000, "Máximo 1000 caracteres.");
        valid &= ValidateFutureDate();
        return valid;
    }

    private bool ValidateCancelInputs(bool onlyWhenNonEmpty = false)
    {
        var hasCode = !string.IsNullOrWhiteSpace(_code.Text);
        var idOk = ValidateOptionalId(_cancelId);
        var phoneOk = ValidateOptionalPhone(_cancelPhone);

        if (onlyWhenNonEmpty || hasCode)
        {
            _errorProvider.SetError(_code, "");
            if (hasCode)
                return idOk && phoneOk;
            return idOk && phoneOk;
        }

        var missingId = string.IsNullOrWhiteSpace(_cancelId.Text);
        var missingPhone = string.IsNullOrWhiteSpace(_cancelPhone.Text);
        if (missingId)
            _errorProvider.SetError(_cancelId, "Indica la cédula o el código de gestión.");
        if (missingPhone)
            _errorProvider.SetError(_cancelPhone, "Indica el teléfono o el código de gestión.");
        return !missingId && !missingPhone && idOk && phoneOk;
    }

    private bool ValidateRequiredText(TextBox field, string requiredMessage, int maxLength)
    {
        var text = field.Text.Trim();
        if (text.Length == 0)
        {
            _errorProvider.SetError(field, requiredMessage);
            return false;
        }
        if (text.Length > maxLength)
        {
            _errorProvider.SetError(field, $"Máximo {maxLength} caracteres.");
            return false;
        }
        _errorProvider.SetError(field, "");
        return true;
    }

    private bool ValidateMaxLength(TextBox field, int maxLength, string message)
    {
        if (field.Text.Length > maxLength)
        {
            _errorProvider.SetError(field, message);
            return false;
        }
        _errorProvider.SetError(field, "");
        return true;
    }

    private bool ValidateOptionalId(TextBox field)
    {
        var value = field.Text.Trim();
        if (value.Length == 0)
        {
            _errorProvider.SetError(field, "");
            return true;
        }
        if (value.Length > 20 || !IsValidCedula(value))
        {
            _errorProvider.SetError(field, "Formato de cédula inválido (solo dígitos o guiones, mínimo 6 dígitos).");
            return false;
        }
        _errorProvider.SetError(field, "");
        return true;
    }

    private bool ValidateOptionalPhone(TextBox field)
    {
        var value = field.Text.Trim();
        if (value.Length == 0)
        {
            _errorProvider.SetError(field, "");
            return true;
        }
        if (value.Length > 250 || !IsValidPhone(value))
        {
            _errorProvider.SetError(field, "Teléfono inválido (mínimo 7 dígitos; permite +, -, espacios y paréntesis).");
            return false;
        }
        _errorProvider.SetError(field, "");
        return true;
    }

    private bool ValidateOptionalEmail(TextBox field)
    {
        var value = field.Text.Trim();
        if (value.Length == 0)
        {
            _errorProvider.SetError(field, "");
            return true;
        }
        if (value.Length > 250 || !IsValidEmail(value))
        {
            _errorProvider.SetError(field, "El correo electrónico no tiene un formato válido.");
            return false;
        }
        _errorProvider.SetError(field, "");
        return true;
    }

    private bool ValidateFutureDate()
    {
        if (_date.Value <= DateTime.Now)
        {
            _errorProvider.SetError(_date, "Selecciona una fecha y hora futuras.");
            return false;
        }
        _errorProvider.SetError(_date, "");
        return true;
    }

    private static bool IsValidCedula(string value)
    {
        if (!value.All(c => char.IsAsciiDigit(c) || c == '-')) return false;
        var digits = value.Count(char.IsAsciiDigit);
        return digits is >= 6 and <= 20;
    }

    private static bool IsValidPhone(string value)
    {
        if (!value.All(c => char.IsAsciiDigit(c) || c is '+' or '-' or '(' or ')' || char.IsWhiteSpace(c))) return false;
        var digits = value.Count(char.IsAsciiDigit);
        return digits is >= 7 and <= 20;
    }

    private static bool IsValidEmail(string value)
    {
        if (!MailAddress.TryCreate(value, out var parsed)) return false;
        var atIndex = parsed.Address.IndexOf('@');
        return atIndex > 0 && parsed.Address.IndexOf('.', atIndex) > atIndex + 1;
    }

    private async Task BookAsync()
    {
        if (!ValidateBooking())
        {
            ShowMessage(_bookingMessage, "Corrige los campos marcados antes de continuar.", false);
            return;
        }

        var email = _email.Text.Trim();
        _bookButton.Enabled = false;
        ShowMessage(_bookingMessage, "Enviando solicitud…", true);
        _bookingMessage.ForeColor = ClinicTheme.Taupe;
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
            _errorProvider.Clear();
        }
        catch (Exception ex) { ShowMessage(_bookingMessage, ex.Message, false); }
        finally { _bookButton.Enabled = true; }
    }

    private async Task CancelAsync()
    {
        if (!ValidateCancelInputs())
        {
            ShowMessage(_cancelMessage, "Indica el código o completa tanto la cédula como el teléfono con formato válido.", false);
            return;
        }
        _cancelButton.Enabled = false;
        ShowMessage(_cancelMessage, "Procesando cancelación…", true);
        _cancelMessage.ForeColor = ClinicTheme.Taupe;
        try
        {
            await _api.SendAsync(HttpMethod.Post, "api/Citas/publica/cancelar", new { CodigoGestion = EmptyNull(_code.Text), Cedula = EmptyNull(_cancelId.Text), Telefono = EmptyNull(_cancelPhone.Text) });
            ShowMessage(_cancelMessage, "La cita fue cancelada.", true);
            _code.Clear(); _cancelId.Clear(); _cancelPhone.Clear();
            _errorProvider.Clear();
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
