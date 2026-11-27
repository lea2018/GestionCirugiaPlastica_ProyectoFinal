namespace GestionCirugiaPlastica.WinForms.Forms
{
    partial class PublicAppointmentForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null!;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            _errorProvider = new ErrorProvider(components);
            _headerPanel = new Panel();
            _headerTitleLabel = new Label();
            _headerSubtitleLabel = new Label();
            _tabs = new TabControl();
            _bookingTab = new TabPage();
            _bookingContent = new Panel();
            _bookingMessage = new Label();
            _bookActions = new FlowLayoutPanel();
            _bookButton = new Button();
            _bookingFields = new TableLayoutPanel();
            _pnlName = new Panel();
            _name = new TextBox();
            _lblName = new Label();
            _pnlSurname = new Panel();
            _surname = new TextBox();
            _lblSurname = new Label();
            _pnlId = new Panel();
            _id = new TextBox();
            _lblId = new Label();
            _pnlPhone = new Panel();
            _phone = new TextBox();
            _lblPhone = new Label();
            _pnlEmail = new Panel();
            _email = new TextBox();
            _lblEmail = new Label();
            _pnlDate = new Panel();
            _date = new DateTimePicker();
            _lblDate = new Label();
            _pnlReason = new Panel();
            _reason = new ComboBox();
            _lblReason = new Label();
            _pnlNotes = new Panel();
            _notes = new TextBox();
            _lblNotes = new Label();
            _cancelTab = new TabPage();
            _cancelContent = new Panel();
            _cancelMessage = new Label();
            _cancelActions = new FlowLayoutPanel();
            _cancelButton = new Button();
            _cancelFields = new TableLayoutPanel();
            _pnlCode = new Panel();
            _code = new TextBox();
            _lblCode = new Label();
            _lblCancelHint = new Label();
            _pnlCancelId = new Panel();
            _cancelId = new TextBox();
            _lblCancelId = new Label();
            _pnlCancelPhone = new Panel();
            _cancelPhone = new TextBox();
            _lblCancelPhone = new Label();
            ((System.ComponentModel.ISupportInitialize)_errorProvider).BeginInit();
            _headerPanel.SuspendLayout();
            _tabs.SuspendLayout();
            _bookingTab.SuspendLayout();
            _bookingContent.SuspendLayout();
            _bookActions.SuspendLayout();
            _bookingFields.SuspendLayout();
            _pnlName.SuspendLayout();
            _pnlSurname.SuspendLayout();
            _pnlId.SuspendLayout();
            _pnlPhone.SuspendLayout();
            _pnlEmail.SuspendLayout();
            _pnlDate.SuspendLayout();
            _pnlReason.SuspendLayout();
            _pnlNotes.SuspendLayout();
            _cancelTab.SuspendLayout();
            _cancelContent.SuspendLayout();
            _cancelActions.SuspendLayout();
            _cancelFields.SuspendLayout();
            _pnlCode.SuspendLayout();
            _pnlCancelId.SuspendLayout();
            _pnlCancelPhone.SuspendLayout();
            SuspendLayout();
            // 
            // _headerPanel
            // 
            _headerPanel.BackColor = Color.White;
            _headerPanel.Controls.Add(_headerTitleLabel);
            _headerPanel.Controls.Add(_headerSubtitleLabel);
            _headerPanel.Dock = DockStyle.Top;
            _headerPanel.Location = new Point(0, 0);
            _headerPanel.Name = "_headerPanel";
            _headerPanel.Padding = new Padding(26, 16, 26, 10);
            _headerPanel.Size = new Size(804, 100);
            _headerPanel.TabIndex = 1;
            // 
            // _headerTitleLabel
            // 
            _headerTitleLabel.Dock = DockStyle.Top;
            _headerTitleLabel.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            _headerTitleLabel.ForeColor = Color.FromArgb(54, 56, 58);
            _headerTitleLabel.Location = new Point(26, 43);
            _headerTitleLabel.Name = "_headerTitleLabel";
            _headerTitleLabel.Size = new Size(752, 39);
            _headerTitleLabel.TabIndex = 0;
            _headerTitleLabel.Text = "Solicita o cancela una cita";
            // 
            // _headerSubtitleLabel
            // 
            _headerSubtitleLabel.Dock = DockStyle.Top;
            _headerSubtitleLabel.ForeColor = Color.FromArgb(158, 154, 155);
            _headerSubtitleLabel.Location = new Point(26, 16);
            _headerSubtitleLabel.Name = "_headerSubtitleLabel";
            _headerSubtitleLabel.Size = new Size(752, 27);
            _headerSubtitleLabel.TabIndex = 1;
            _headerSubtitleLabel.Text = "Tanger Clinic · Plastic Surgery";
            // 
            // _tabs
            // 
            _tabs.Controls.Add(_bookingTab);
            _tabs.Controls.Add(_cancelTab);
            _tabs.Dock = DockStyle.Fill;
            _tabs.Location = new Point(0, 100);
            _tabs.Name = "_tabs";
            _tabs.Padding = new Point(16, 8);
            _tabs.SelectedIndex = 0;
            _tabs.Size = new Size(804, 541);
            _tabs.TabIndex = 0;
            // 
            // _bookingTab
            // 
            _bookingTab.BackColor = Color.White;
            _bookingTab.Controls.Add(_bookingContent);
            _bookingTab.Location = new Point(4, 36);
            _bookingTab.Name = "_bookingTab";
            _bookingTab.Padding = new Padding(18);
            _bookingTab.Size = new Size(796, 501);
            _bookingTab.TabIndex = 0;
            _bookingTab.Text = "Solicitar cita";
            // 
            // _bookingContent
            // 
            _bookingContent.AutoScroll = true;
            _bookingContent.BackColor = Color.White;
            _bookingContent.Controls.Add(_bookingMessage);
            _bookingContent.Controls.Add(_bookActions);
            _bookingContent.Controls.Add(_bookingFields);
            _bookingContent.Dock = DockStyle.Fill;
            _bookingContent.Location = new Point(18, 18);
            _bookingContent.Name = "_bookingContent";
            _bookingContent.Size = new Size(760, 465);
            _bookingContent.TabIndex = 0;
            // 
            // _bookingMessage
            // 
            _bookingMessage.Dock = DockStyle.Top;
            _bookingMessage.ForeColor = Color.FromArgb(158, 110, 121);
            _bookingMessage.Location = new Point(0, 489);
            _bookingMessage.Name = "_bookingMessage";
            _bookingMessage.Padding = new Padding(4);
            _bookingMessage.Size = new Size(743, 72);
            _bookingMessage.TabIndex = 2;
            // 
            // _bookActions
            // 
            _bookActions.Controls.Add(_bookButton);
            _bookActions.Dock = DockStyle.Top;
            _bookActions.Location = new Point(0, 439);
            _bookActions.Name = "_bookActions";
            _bookActions.Padding = new Padding(0, 8, 0, 0);
            _bookActions.Size = new Size(743, 50);
            _bookActions.TabIndex = 1;
            // 
            // _bookButton
            // 
            _bookButton.AutoSize = true;
            _bookButton.BackColor = Color.FromArgb(158, 110, 121);
            _bookButton.Cursor = Cursors.Hand;
            _bookButton.FlatStyle = FlatStyle.Flat;
            _bookButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _bookButton.ForeColor = Color.White;
            _bookButton.Location = new Point(3, 11);
            _bookButton.Name = "_bookButton";
            _bookButton.Padding = new Padding(12, 4, 12, 4);
            _bookButton.Size = new Size(134, 40);
            _bookButton.TabIndex = 0;
            _bookButton.Text = "Solicitar cita";
            _bookButton.UseVisualStyleBackColor = false;
            // 
            // _bookingFields
            // 
            _bookingFields.AutoSize = true;
            _bookingFields.BackColor = Color.White;
            _bookingFields.ColumnCount = 2;
            _bookingFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            _bookingFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            _bookingFields.Controls.Add(_pnlName, 0, 0);
            _bookingFields.Controls.Add(_pnlSurname, 1, 0);
            _bookingFields.Controls.Add(_pnlId, 0, 1);
            _bookingFields.Controls.Add(_pnlPhone, 1, 1);
            _bookingFields.Controls.Add(_pnlEmail, 0, 2);
            _bookingFields.Controls.Add(_pnlDate, 1, 2);
            _bookingFields.Controls.Add(_pnlReason, 0, 3);
            _bookingFields.Controls.Add(_pnlNotes, 0, 4);
            _bookingFields.Dock = DockStyle.Top;
            _bookingFields.Location = new Point(0, 0);
            _bookingFields.Name = "_bookingFields";
            _bookingFields.RowCount = 5;
            _bookingFields.RowStyles.Add(new RowStyle());
            _bookingFields.RowStyles.Add(new RowStyle());
            _bookingFields.RowStyles.Add(new RowStyle());
            _bookingFields.RowStyles.Add(new RowStyle());
            _bookingFields.RowStyles.Add(new RowStyle());
            _bookingFields.Size = new Size(743, 439);
            _bookingFields.TabIndex = 0;
            // 
            // _pnlName
            // 
            _pnlName.Controls.Add(_name);
            _pnlName.Controls.Add(_lblName);
            _pnlName.Dock = DockStyle.Fill;
            _pnlName.Location = new Point(2, 2);
            _pnlName.Margin = new Padding(2);
            _pnlName.Name = "_pnlName";
            _pnlName.Padding = new Padding(5, 4, 12, 4);
            _pnlName.Size = new Size(367, 76);
            _pnlName.TabIndex = 0;
            // 
            // _name
            // 
            _name.BackColor = Color.White;
            _name.Dock = DockStyle.Top;
            _name.Font = new Font("Segoe UI", 10F);
            _name.ForeColor = Color.FromArgb(54, 56, 58);
            _name.Location = new Point(5, 28);
            _name.Margin = new Padding(0, 3, 0, 0);
            _name.Name = "_name";
            _name.PlaceholderText = "Nombre *";
            _name.Size = new Size(350, 25);
            _name.TabIndex = 1;
            // 
            // _lblName
            // 
            _lblName.Dock = DockStyle.Top;
            _lblName.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _lblName.ForeColor = Color.FromArgb(54, 56, 58);
            _lblName.Location = new Point(5, 4);
            _lblName.Name = "_lblName";
            _lblName.Size = new Size(350, 24);
            _lblName.TabIndex = 0;
            _lblName.Text = "Nombre";
            // 
            // _pnlSurname
            // 
            _pnlSurname.Controls.Add(_surname);
            _pnlSurname.Controls.Add(_lblSurname);
            _pnlSurname.Dock = DockStyle.Fill;
            _pnlSurname.Location = new Point(373, 2);
            _pnlSurname.Margin = new Padding(2);
            _pnlSurname.Name = "_pnlSurname";
            _pnlSurname.Padding = new Padding(5, 4, 12, 4);
            _pnlSurname.Size = new Size(368, 76);
            _pnlSurname.TabIndex = 1;
            // 
            // _surname
            // 
            _surname.BackColor = Color.White;
            _surname.Dock = DockStyle.Top;
            _surname.Font = new Font("Segoe UI", 10F);
            _surname.ForeColor = Color.FromArgb(54, 56, 58);
            _surname.Location = new Point(5, 28);
            _surname.Margin = new Padding(0, 3, 0, 0);
            _surname.Name = "_surname";
            _surname.PlaceholderText = "Apellido *";
            _surname.Size = new Size(351, 25);
            _surname.TabIndex = 1;
            // 
            // _lblSurname
            // 
            _lblSurname.Dock = DockStyle.Top;
            _lblSurname.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _lblSurname.ForeColor = Color.FromArgb(54, 56, 58);
            _lblSurname.Location = new Point(5, 4);
            _lblSurname.Name = "_lblSurname";
            _lblSurname.Size = new Size(351, 24);
            _lblSurname.TabIndex = 0;
            _lblSurname.Text = "Apellido";
            // 
            // _pnlId
            // 
            _pnlId.Controls.Add(_id);
            _pnlId.Controls.Add(_lblId);
            _pnlId.Dock = DockStyle.Fill;
            _pnlId.Location = new Point(2, 82);
            _pnlId.Margin = new Padding(2);
            _pnlId.Name = "_pnlId";
            _pnlId.Padding = new Padding(5, 4, 12, 4);
            _pnlId.Size = new Size(367, 76);
            _pnlId.TabIndex = 2;
            // 
            // _id
            // 
            _id.BackColor = Color.White;
            _id.Dock = DockStyle.Top;
            _id.Font = new Font("Segoe UI", 10F);
            _id.ForeColor = Color.FromArgb(54, 56, 58);
            _id.Location = new Point(5, 28);
            _id.Margin = new Padding(0, 3, 0, 0);
            _id.Name = "_id";
            _id.PlaceholderText = "Cédula (opcional)";
            _id.Size = new Size(350, 25);
            _id.TabIndex = 1;
            // 
            // _lblId
            // 
            _lblId.Dock = DockStyle.Top;
            _lblId.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _lblId.ForeColor = Color.FromArgb(54, 56, 58);
            _lblId.Location = new Point(5, 4);
            _lblId.Name = "_lblId";
            _lblId.Size = new Size(350, 24);
            _lblId.TabIndex = 0;
            _lblId.Text = "Cédula (opcional)";
            // 
            // _pnlPhone
            // 
            _pnlPhone.Controls.Add(_phone);
            _pnlPhone.Controls.Add(_lblPhone);
            _pnlPhone.Dock = DockStyle.Fill;
            _pnlPhone.Location = new Point(373, 82);
            _pnlPhone.Margin = new Padding(2);
            _pnlPhone.Name = "_pnlPhone";
            _pnlPhone.Padding = new Padding(5, 4, 12, 4);
            _pnlPhone.Size = new Size(368, 76);
            _pnlPhone.TabIndex = 3;
            // 
            // _phone
            // 
            _phone.BackColor = Color.White;
            _phone.Dock = DockStyle.Top;
            _phone.Font = new Font("Segoe UI", 10F);
            _phone.ForeColor = Color.FromArgb(54, 56, 58);
            _phone.Location = new Point(5, 28);
            _phone.Margin = new Padding(0, 3, 0, 0);
            _phone.Name = "_phone";
            _phone.PlaceholderText = "Teléfono (opcional)";
            _phone.Size = new Size(351, 25);
            _phone.TabIndex = 1;
            // 
            // _lblPhone
            // 
            _lblPhone.Dock = DockStyle.Top;
            _lblPhone.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _lblPhone.ForeColor = Color.FromArgb(54, 56, 58);
            _lblPhone.Location = new Point(5, 4);
            _lblPhone.Name = "_lblPhone";
            _lblPhone.Size = new Size(351, 24);
            _lblPhone.TabIndex = 0;
            _lblPhone.Text = "Teléfono (opcional)";
            // 
            // _pnlEmail
            // 
            _pnlEmail.Controls.Add(_email);
            _pnlEmail.Controls.Add(_lblEmail);
            _pnlEmail.Dock = DockStyle.Fill;
            _pnlEmail.Location = new Point(2, 162);
            _pnlEmail.Margin = new Padding(2);
            _pnlEmail.Name = "_pnlEmail";
            _pnlEmail.Padding = new Padding(5, 4, 12, 4);
            _pnlEmail.Size = new Size(367, 76);
            _pnlEmail.TabIndex = 4;
            // 
            // _email
            // 
            _email.BackColor = Color.White;
            _email.Dock = DockStyle.Top;
            _email.Font = new Font("Segoe UI", 10F);
            _email.ForeColor = Color.FromArgb(54, 56, 58);
            _email.Location = new Point(5, 28);
            _email.Margin = new Padding(0, 3, 0, 0);
            _email.Name = "_email";
            _email.PlaceholderText = "Correo electrónico (opcional)";
            _email.Size = new Size(350, 25);
            _email.TabIndex = 1;
            // 
            // _lblEmail
            // 
            _lblEmail.Dock = DockStyle.Top;
            _lblEmail.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _lblEmail.ForeColor = Color.FromArgb(54, 56, 58);
            _lblEmail.Location = new Point(5, 4);
            _lblEmail.Name = "_lblEmail";
            _lblEmail.Size = new Size(350, 24);
            _lblEmail.TabIndex = 0;
            _lblEmail.Text = "Correo electrónico (opcional)";
            // 
            // _pnlDate
            // 
            _pnlDate.Controls.Add(_date);
            _pnlDate.Controls.Add(_lblDate);
            _pnlDate.Dock = DockStyle.Fill;
            _pnlDate.Location = new Point(373, 162);
            _pnlDate.Margin = new Padding(2);
            _pnlDate.Name = "_pnlDate";
            _pnlDate.Padding = new Padding(5, 4, 12, 4);
            _pnlDate.Size = new Size(368, 76);
            _pnlDate.TabIndex = 5;
            // 
            // _date
            // 
            _date.CustomFormat = "dd/MM/yyyy hh:mm tt";
            _date.Dock = DockStyle.Top;
            _date.Font = new Font("Segoe UI", 10F);
            _date.ForeColor = Color.FromArgb(54, 56, 58);
            _date.Format = DateTimePickerFormat.Custom;
            _date.Location = new Point(5, 28);
            _date.Margin = new Padding(0, 3, 0, 0);
            _date.Name = "_date";
            _date.Size = new Size(351, 25);
            _date.TabIndex = 1;
            // 
            // _lblDate
            // 
            _lblDate.Dock = DockStyle.Top;
            _lblDate.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _lblDate.ForeColor = Color.FromArgb(54, 56, 58);
            _lblDate.Location = new Point(5, 4);
            _lblDate.Name = "_lblDate";
            _lblDate.Size = new Size(351, 24);
            _lblDate.TabIndex = 0;
            _lblDate.Text = "Fecha y hora";
            // 
            // _pnlReason
            // 
            _pnlReason.Controls.Add(_reason);
            _pnlReason.Controls.Add(_lblReason);
            _pnlReason.Dock = DockStyle.Fill;
            _pnlReason.Location = new Point(2, 242);
            _pnlReason.Margin = new Padding(2);
            _pnlReason.Name = "_pnlReason";
            _pnlReason.Padding = new Padding(5, 4, 12, 4);
            _pnlReason.Size = new Size(367, 76);
            _pnlReason.TabIndex = 6;
            // 
            // _reason
            // 
            _reason.Dock = DockStyle.Top;
            _reason.DropDownStyle = ComboBoxStyle.DropDownList;
            _reason.Font = new Font("Segoe UI", 10F);
            _reason.ForeColor = Color.FromArgb(54, 56, 58);
            _reason.FormattingEnabled = true;
            _reason.Items.AddRange(new object[] { "Consulta", "Evaluación", "Seguimiento", "Postoperatorio" });
            _reason.Location = new Point(5, 28);
            _reason.Margin = new Padding(0, 3, 0, 0);
            _reason.Name = "_reason";
            _reason.Size = new Size(350, 25);
            _reason.TabIndex = 1;
            // 
            // _lblReason
            // 
            _lblReason.Dock = DockStyle.Top;
            _lblReason.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _lblReason.ForeColor = Color.FromArgb(54, 56, 58);
            _lblReason.Location = new Point(5, 4);
            _lblReason.Name = "_lblReason";
            _lblReason.Size = new Size(350, 24);
            _lblReason.TabIndex = 0;
            _lblReason.Text = "Motivo";
            // 
            // _pnlNotes
            // 
            _bookingFields.SetColumnSpan(_pnlNotes, 2);
            _pnlNotes.Controls.Add(_notes);
            _pnlNotes.Controls.Add(_lblNotes);
            _pnlNotes.Dock = DockStyle.Fill;
            _pnlNotes.Location = new Point(2, 322);
            _pnlNotes.Margin = new Padding(2);
            _pnlNotes.Name = "_pnlNotes";
            _pnlNotes.Padding = new Padding(5, 4, 12, 4);
            _pnlNotes.Size = new Size(739, 115);
            _pnlNotes.TabIndex = 7;
            // 
            // _notes
            // 
            _notes.BackColor = Color.White;
            _notes.Dock = DockStyle.Top;
            _notes.Font = new Font("Segoe UI", 10F);
            _notes.ForeColor = Color.FromArgb(54, 56, 58);
            _notes.Location = new Point(5, 28);
            _notes.Margin = new Padding(0, 3, 0, 0);
            _notes.Multiline = true;
            _notes.Name = "_notes";
            _notes.PlaceholderText = "Información adicional";
            _notes.Size = new Size(722, 75);
            _notes.TabIndex = 1;
            // 
            // _lblNotes
            // 
            _lblNotes.Dock = DockStyle.Top;
            _lblNotes.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _lblNotes.ForeColor = Color.FromArgb(54, 56, 58);
            _lblNotes.Location = new Point(5, 4);
            _lblNotes.Name = "_lblNotes";
            _lblNotes.Size = new Size(722, 24);
            _lblNotes.TabIndex = 0;
            _lblNotes.Text = "Información adicional";
            // 
            // _cancelTab
            // 
            _cancelTab.BackColor = Color.White;
            _cancelTab.Controls.Add(_cancelContent);
            _cancelTab.Location = new Point(4, 36);
            _cancelTab.Name = "_cancelTab";
            _cancelTab.Padding = new Padding(18);
            _cancelTab.Size = new Size(796, 501);
            _cancelTab.TabIndex = 1;
            _cancelTab.Text = "Cancelar cita";
            // 
            // _cancelContent
            // 
            _cancelContent.AutoScroll = true;
            _cancelContent.BackColor = Color.White;
            _cancelContent.Controls.Add(_cancelMessage);
            _cancelContent.Controls.Add(_cancelActions);
            _cancelContent.Controls.Add(_cancelFields);
            _cancelContent.Dock = DockStyle.Fill;
            _cancelContent.Location = new Point(18, 18);
            _cancelContent.Name = "_cancelContent";
            _cancelContent.Size = new Size(760, 465);
            _cancelContent.TabIndex = 0;
            _cancelContent.Paint += _cancelContent_Paint;
            //
            // _cancelMessage
            //
            _cancelMessage.Dock = DockStyle.Top;
            _cancelMessage.ForeColor = Color.FromArgb(158, 110, 121);
            _cancelMessage.Location = new Point(0, 338);
            _cancelMessage.Name = "_cancelMessage";
            _cancelMessage.Padding = new Padding(4);
            _cancelMessage.Size = new Size(760, 72);
            _cancelMessage.TabIndex = 2;
            // 
            // _cancelActions
            // 
            _cancelActions.Controls.Add(_cancelButton);
            _cancelActions.Dock = DockStyle.Top;
            _cancelActions.Location = new Point(0, 283);
            _cancelActions.Name = "_cancelActions";
            _cancelActions.Padding = new Padding(0, 8, 0, 0);
            _cancelActions.Size = new Size(760, 55);
            _cancelActions.TabIndex = 1;
            // 
            // _cancelButton
            // 
            _cancelButton.AutoSize = true;
            _cancelButton.BackColor = Color.White;
            _cancelButton.Cursor = Cursors.Hand;
            _cancelButton.FlatStyle = FlatStyle.Flat;
            _cancelButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _cancelButton.ForeColor = Color.FromArgb(158, 110, 121);
            _cancelButton.Location = new Point(3, 11);
            _cancelButton.Name = "_cancelButton";
            _cancelButton.Padding = new Padding(12, 4, 12, 4);
            _cancelButton.Size = new Size(134, 40);
            _cancelButton.TabIndex = 0;
            _cancelButton.Text = "Cancelar cita";
            _cancelButton.UseVisualStyleBackColor = false;
            // 
            // _cancelFields
            // 
            _cancelFields.AutoSize = true;
            _cancelFields.BackColor = Color.White;
            _cancelFields.ColumnCount = 1;
            _cancelFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _cancelFields.Controls.Add(_pnlCode, 0, 0);
            _cancelFields.Controls.Add(_lblCancelHint, 0, 1);
            _cancelFields.Controls.Add(_pnlCancelId, 0, 2);
            _cancelFields.Controls.Add(_pnlCancelPhone, 0, 3);
            _cancelFields.Dock = DockStyle.Top;
            _cancelFields.Location = new Point(0, 0);
            _cancelFields.Name = "_cancelFields";
            _cancelFields.RowCount = 4;
            _cancelFields.RowStyles.Add(new RowStyle());
            _cancelFields.RowStyles.Add(new RowStyle());
            _cancelFields.RowStyles.Add(new RowStyle());
            _cancelFields.RowStyles.Add(new RowStyle());
            _cancelFields.Size = new Size(760, 283);
            _cancelFields.TabIndex = 0;
            // 
            // _pnlCode
            // 
            _pnlCode.Controls.Add(_code);
            _pnlCode.Controls.Add(_lblCode);
            _pnlCode.Dock = DockStyle.Fill;
            _pnlCode.Location = new Point(2, 2);
            _pnlCode.Margin = new Padding(2);
            _pnlCode.Name = "_pnlCode";
            _pnlCode.Padding = new Padding(5, 4, 12, 4);
            _pnlCode.Size = new Size(756, 76);
            _pnlCode.TabIndex = 0;
            // 
            // _code
            // 
            _code.BackColor = Color.White;
            _code.Dock = DockStyle.Top;
            _code.Font = new Font("Segoe UI", 10F);
            _code.ForeColor = Color.FromArgb(54, 56, 58);
            _code.Location = new Point(5, 28);
            _code.Margin = new Padding(0, 3, 0, 0);
            _code.Name = "_code";
            _code.PlaceholderText = "Código de gestión (preferible)";
            _code.Size = new Size(739, 25);
            _code.TabIndex = 1;
            // 
            // _lblCode
            // 
            _lblCode.Dock = DockStyle.Top;
            _lblCode.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _lblCode.ForeColor = Color.FromArgb(54, 56, 58);
            _lblCode.Location = new Point(5, 4);
            _lblCode.Name = "_lblCode";
            _lblCode.Size = new Size(739, 24);
            _lblCode.TabIndex = 0;
            _lblCode.Text = "Código de gestión (preferible)";
            // 
            // _lblCancelHint
            // 
            _lblCancelHint.AutoSize = true;
            _lblCancelHint.ForeColor = Color.FromArgb(158, 154, 155);
            _lblCancelHint.Location = new Point(3, 90);
            _lblCancelHint.Margin = new Padding(3, 10, 3, 14);
            _lblCancelHint.Name = "_lblCancelHint";
            _lblCancelHint.Size = new Size(286, 19);
            _lblCancelHint.TabIndex = 1;
            _lblCancelHint.Text = "o bien, verifica tu identidad con ambos datos:";
            // 
            // _pnlCancelId
            // 
            _pnlCancelId.Controls.Add(_cancelId);
            _pnlCancelId.Controls.Add(_lblCancelId);
            _pnlCancelId.Dock = DockStyle.Fill;
            _pnlCancelId.Location = new Point(2, 125);
            _pnlCancelId.Margin = new Padding(2);
            _pnlCancelId.Name = "_pnlCancelId";
            _pnlCancelId.Padding = new Padding(5, 4, 12, 4);
            _pnlCancelId.Size = new Size(756, 76);
            _pnlCancelId.TabIndex = 2;
            // 
            // _cancelId
            // 
            _cancelId.BackColor = Color.White;
            _cancelId.Dock = DockStyle.Top;
            _cancelId.Font = new Font("Segoe UI", 10F);
            _cancelId.ForeColor = Color.FromArgb(54, 56, 58);
            _cancelId.Location = new Point(5, 28);
            _cancelId.Margin = new Padding(0, 3, 0, 0);
            _cancelId.Name = "_cancelId";
            _cancelId.PlaceholderText = "Cédula";
            _cancelId.Size = new Size(739, 25);
            _cancelId.TabIndex = 1;
            // 
            // _lblCancelId
            // 
            _lblCancelId.Dock = DockStyle.Top;
            _lblCancelId.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _lblCancelId.ForeColor = Color.FromArgb(54, 56, 58);
            _lblCancelId.Location = new Point(5, 4);
            _lblCancelId.Name = "_lblCancelId";
            _lblCancelId.Size = new Size(739, 24);
            _lblCancelId.TabIndex = 0;
            _lblCancelId.Text = "Cédula";
            // 
            // _pnlCancelPhone
            // 
            _pnlCancelPhone.Controls.Add(_cancelPhone);
            _pnlCancelPhone.Controls.Add(_lblCancelPhone);
            _pnlCancelPhone.Dock = DockStyle.Fill;
            _pnlCancelPhone.Location = new Point(2, 205);
            _pnlCancelPhone.Margin = new Padding(2);
            _pnlCancelPhone.Name = "_pnlCancelPhone";
            _pnlCancelPhone.Padding = new Padding(5, 4, 12, 4);
            _pnlCancelPhone.Size = new Size(756, 76);
            _pnlCancelPhone.TabIndex = 3;
            // 
            // _cancelPhone
            // 
            _cancelPhone.BackColor = Color.White;
            _cancelPhone.Dock = DockStyle.Top;
            _cancelPhone.Font = new Font("Segoe UI", 10F);
            _cancelPhone.ForeColor = Color.FromArgb(54, 56, 58);
            _cancelPhone.Location = new Point(5, 28);
            _cancelPhone.Margin = new Padding(0, 3, 0, 0);
            _cancelPhone.Name = "_cancelPhone";
            _cancelPhone.PlaceholderText = "Teléfono";
            _cancelPhone.Size = new Size(739, 25);
            _cancelPhone.TabIndex = 1;
            // 
            // _lblCancelPhone
            // 
            _lblCancelPhone.Dock = DockStyle.Top;
            _lblCancelPhone.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _lblCancelPhone.ForeColor = Color.FromArgb(54, 56, 58);
            _lblCancelPhone.Location = new Point(5, 4);
            _lblCancelPhone.Name = "_lblCancelPhone";
            _lblCancelPhone.Size = new Size(739, 24);
            _lblCancelPhone.TabIndex = 0;
            _lblCancelPhone.Text = "Teléfono";
            // 
            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            _errorProvider.ContainerControl = this;
            // PublicAppointmentForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(246, 244, 243);
            ClientSize = new Size(804, 641);
            Controls.Add(_tabs);
            Controls.Add(_headerPanel);
            Font = new Font("Segoe UI", 10F);
            MinimumSize = new Size(720, 680);
            Name = "PublicAppointmentForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Agendar una cita | Tanger Clinic";
            _headerPanel.ResumeLayout(false);
            _tabs.ResumeLayout(false);
            _bookingTab.ResumeLayout(false);
            _bookingContent.ResumeLayout(false);
            _bookingContent.PerformLayout();
            _bookActions.ResumeLayout(false);
            _bookActions.PerformLayout();
            _bookingFields.ResumeLayout(false);
            _pnlName.ResumeLayout(false);
            _pnlName.PerformLayout();
            _pnlSurname.ResumeLayout(false);
            _pnlSurname.PerformLayout();
            _pnlId.ResumeLayout(false);
            _pnlId.PerformLayout();
            _pnlPhone.ResumeLayout(false);
            _pnlPhone.PerformLayout();
            _pnlEmail.ResumeLayout(false);
            _pnlEmail.PerformLayout();
            _pnlDate.ResumeLayout(false);
            _pnlReason.ResumeLayout(false);
            _pnlNotes.ResumeLayout(false);
            _pnlNotes.PerformLayout();
            _cancelTab.ResumeLayout(false);
            _cancelContent.ResumeLayout(false);
            _cancelContent.PerformLayout();
            _cancelActions.ResumeLayout(false);
            _cancelActions.PerformLayout();
            _cancelFields.ResumeLayout(false);
            _cancelFields.PerformLayout();
            _pnlCode.ResumeLayout(false);
            _pnlCode.PerformLayout();
            _pnlCancelId.ResumeLayout(false);
            _pnlCancelId.PerformLayout();
            _pnlCancelPhone.ResumeLayout(false);
            _pnlCancelPhone.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_errorProvider).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel _headerPanel = null!;
        private Label _headerTitleLabel = null!;
        private Label _headerSubtitleLabel = null!;
        private TabControl _tabs = null!;
        private TabPage _bookingTab = null!;
        private Panel _bookingContent = null!;
        private TableLayoutPanel _bookingFields = null!;
        private Panel _pnlName = null!;
        private Label _lblName = null!;
        private TextBox _name = null!;
        private Panel _pnlSurname = null!;
        private Label _lblSurname = null!;
        private TextBox _surname = null!;
        private Panel _pnlId = null!;
        private Label _lblId = null!;
        private TextBox _id = null!;
        private Panel _pnlPhone = null!;
        private Label _lblPhone = null!;
        private TextBox _phone = null!;
        private Panel _pnlEmail = null!;
        private Label _lblEmail = null!;
        private TextBox _email = null!;
        private Panel _pnlDate = null!;
        private Label _lblDate = null!;
        private DateTimePicker _date = null!;
        private Panel _pnlReason = null!;
        private Label _lblReason = null!;
        private ComboBox _reason = null!;
        private Panel _pnlNotes = null!;
        private Label _lblNotes = null!;
        private TextBox _notes = null!;
        private FlowLayoutPanel _bookActions = null!;
        private Button _bookButton = null!;
        private Label _bookingMessage = null!;
        private TabPage _cancelTab = null!;
        private Panel _cancelContent = null!;
        private TableLayoutPanel _cancelFields = null!;
        private Panel _pnlCode = null!;
        private Label _lblCode = null!;
        private TextBox _code = null!;
        private Label _lblCancelHint = null!;
        private Panel _pnlCancelId = null!;
        private Label _lblCancelId = null!;
        private TextBox _cancelId = null!;
        private Panel _pnlCancelPhone = null!;
        private Label _lblCancelPhone = null!;
        private TextBox _cancelPhone = null!;
        private FlowLayoutPanel _cancelActions = null!;
        private Button _cancelButton = null!;
        private Label _cancelMessage = null!;
        private ErrorProvider _errorProvider = null!;
    }
}
