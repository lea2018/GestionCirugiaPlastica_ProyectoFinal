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

        _btnLogin.Click += async (_, _) =>
        {
            var identifier = _loginUser.Text.Trim();

            if (string.IsNullOrWhiteSpace(identifier) ||
                string.IsNullOrWhiteSpace(_loginPassword.Text))
            {
                _loginMessage.ForeColor = Color.Firebrick;
                _loginMessage.Text = "Ingresa tu usuario (o correo) y contraseña.";
                return;
            }

            _btnLogin.Enabled = false;
            _loginMessage.Text = "Conectando con la API…";
            _loginMessage.ForeColor = Taupe;

            try
            {
                await _api.LoginAsync(identifier, _loginPassword.Text);
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

    private void ShowLogin()
    {
        _workspacePanel.Visible = false;
        _loginMessage.Text = "";
        _loginPassword.Text = "";
        _loginShowPassword.Checked = false;
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
