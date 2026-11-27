using System.Drawing;
using GestionCirugiaPlastica.WinForms.Forms;

namespace GestionCirugiaPlastica.WinForms;

internal partial class MainForm
{
    private void CenterLoginCard()
    {
        _loginCard.Left = Math.Max(0, (_root.ClientSize.Width - _loginCard.Width) / 2);
        _loginCard.Top = Math.Max(0, (_root.ClientSize.Height - _loginCard.Height) / 2);
    }

    private void WireLoginEvents()
    {
        _root.Resize += (_, _) =>
        {
            if (_loginCard.Visible)
                CenterLoginCard();
        };

        _loginCard.Paint += (_, e) =>
            ControlPaint.DrawBorder(e.Graphics, _loginCard.ClientRectangle, ClinicTheme.Border, ButtonBorderStyle.Solid);

        _loginLogo.Paint += (_, e) =>
        {
            using var brush = new System.Drawing.Drawing2D.LinearGradientBrush(_loginLogo.ClientRectangle, Mauve, Rose, 35f);
            e.Graphics.FillRectangle(brush, _loginLogo.ClientRectangle);
            using var logoFont = new Font("Segoe UI Symbol", 22F, FontStyle.Bold);
            TextRenderer.DrawText(e.Graphics, "✚", logoFont, _loginLogo.ClientRectangle, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        };

        _loginShowPassword.CheckedChanged += (_, _) =>
            _loginPassword.UseSystemPasswordChar = !_loginShowPassword.Checked;

        _loginErrorProvider.SetIconPadding(_loginUser, -20);
        _loginErrorProvider.SetIconPadding(_loginPassword, -20);

        _loginUser.TextChanged += (_, _) => ValidateLoginUser();
        _loginUser.Leave += (_, _) => ValidateLoginUser();
        _loginPassword.TextChanged += (_, _) => ValidateLoginPassword();
        _loginPassword.Leave += (_, _) => ValidateLoginPassword();

        _btnLogin.Click += async (_, _) =>
        {
            var identifier = _loginUser.Text.Trim();

            if (!ValidateLoginInputs())
            {
                _loginMessage.ForeColor = Color.Firebrick;
                _loginMessage.Text = "Corrige los campos marcados para iniciar sesión.";
                return;
            }

            _btnLogin.Enabled = false;
            _loginMessage.Text = "Conectando con la API…";
            _loginMessage.ForeColor = Taupe;

            try
            {
                await _api.LoginAsync(identifier, _loginPassword.Text);
                _loginErrorProvider.Clear();
                await BuildWorkspace();
            }
            catch (Exception ex)
            {
                _loginMessage.ForeColor = Color.Firebrick;
                _loginMessage.Text = UserMessage(ex);
            }
            finally
            {
                _btnLogin.Enabled = true;
            }
        };

        _btnRegister.Click += async (_, _) => await RegisterPatient();

        _btnPublicBooking.Click += (_, _) =>
        {
            using var form = new PublicAppointmentForm(_api);
            form.ShowDialog(this);
        };

        _btnPublicNews.Click += async (_, _) => await ShowPublicNews();

        _loginUser.KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                _loginPassword.Focus();
            }
        };

        _loginPassword.KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                _btnLogin.PerformClick();
            }
        };
    }

    private bool ValidateLoginInputs()
    {
        var userOk = ValidateLoginUser();
        var passOk = ValidateLoginPassword();
        return userOk && passOk;
    }

    private bool ValidateLoginUser()
    {
        var value = _loginUser.Text.Trim();
        if (value.Length == 0)
        {
            _loginErrorProvider.SetError(_loginUser, "Ingresa tu usuario, correo, cédula o teléfono.");
            return false;
        }
        if (value.Contains('@'))
        {
            if (!System.Net.Mail.MailAddress.TryCreate(value, out var parsed) ||
                parsed.Address.IndexOf('.', parsed.Address.IndexOf('@')) <= parsed.Address.IndexOf('@') + 1)
            {
                _loginErrorProvider.SetError(_loginUser, "El correo electrónico no tiene un formato válido.");
                return false;
            }
        }
        else if (value.All(c => char.IsAsciiDigit(c) || c is '+' or '-' or '(' or ')' || char.IsWhiteSpace(c)))
        {
            if (value.Count(char.IsAsciiDigit) < 6)
            {
                _loginErrorProvider.SetError(_loginUser, "La cédula o teléfono debe tener al menos 6 dígitos.");
                return false;
            }
        }

        _loginErrorProvider.SetError(_loginUser, "");
        return true;
    }

    private bool ValidateLoginPassword()
    {
        if (string.IsNullOrWhiteSpace(_loginPassword.Text))
        {
            _loginErrorProvider.SetError(_loginPassword, "Ingresa tu contraseña.");
            return false;
        }
        _loginErrorProvider.SetError(_loginPassword, "");
        return true;
    }

    private void ShowLogin()
    {
        _workspacePanel.Visible = false;
        _loginMessage.Text = "";
        _loginPassword.Text = "";
        _loginShowPassword.Checked = false;
        _loginErrorProvider.Clear();
        _loginCard.Visible = true;
        AcceptButton = _btnLogin;
        CenterLoginCard();
        _loginCard.BringToFront();
        _loginUser.Focus();
    }

    private async Task RegisterPatient()
    {
        using var form = new EntityDialog("Crear cuenta de paciente");

        var fullName = form.AddText("Nombre completo *");
        var username = form.AddText("Nombre de usuario *");
        var password = form.AddText("Contraseña (mínimo 12 caracteres) *");

        password.UseSystemPasswordChar = true;

        var email = form.AddText("Correo electrónico (opcional)");
        var cedula = form.AddText("Cédula (opcional)");
        var phone = form.AddText("Teléfono (opcional)");

        if (form.ShowDialog(this) != DialogResult.OK)
            return;

        if (string.IsNullOrWhiteSpace(fullName.Text) ||
            username.Text.Trim().Length is < 3 or > 100 ||
            password.Text.Length < 12)
        {
            MessageBox.Show(
                this,
                "Completa el nombre, un usuario de 3 a 100 caracteres y una contraseña de al menos 12 caracteres.",
                "Validación",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            return;
        }

        if (username.Text.Any(char.IsWhiteSpace) ||
            username.Text.Any(c =>
                !(char.IsLetterOrDigit(c) || c is '.' or '_' or '-')))
        {
            MessageBox.Show(
                this,
                "El nombre de usuario solo puede incluir letras, números, punto, guion y guion bajo.",
                "Validación",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            return;
        }

        try
        {
            await _api.SendAsync(
                HttpMethod.Post,
                "api/Auth/register",
                new
                {
                    NombreCompleto = fullName.Text.Trim(),
                    NombreUsuario = username.Text.Trim(),
                    Password = password.Text,
                    Email = EmptyNull(email.Text),
                    Cedula = EmptyNull(cedula.Text),
                    Telefono = EmptyNull(phone.Text)
                });

            MessageBox.Show(
                this,
                "La cuenta de paciente quedó registrada. Ya puedes iniciar sesión.",
                "Tanger Clinic",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                this,
                UserMessage(ex),
                "No se pudo crear la cuenta",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
    }
}
