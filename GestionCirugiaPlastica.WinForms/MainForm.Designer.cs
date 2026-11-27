namespace GestionCirugiaPlastica.WinForms
{
    partial class MainForm
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            _root = new Panel();
            _loginCard = new Panel();
            _btnPublicNews = new Button();
            _btnPublicBooking = new Button();
            _btnRegister = new Button();
            _btnLogin = new Button();
            _loginMessage = new Label();
            _loginShowPassword = new CheckBox();
            _loginPassword = new TextBox();
            _loginUser = new TextBox();
            _loginSubtitle = new Label();
            _loginTitle = new Label();
            _loginBrand = new Label();
            _loginLogo = new PictureBox();
            _workspacePanel = new Panel();
            _content = new Panel();
            _pageContent = new Panel();
            _mainGrid = new DataGridView();
            _dynamicHost = new Panel();
            _statusBar = new Panel();
            _status = new Label();
            _sectionToolbar = new Panel();
            _btnToolbarAdd = new Button();
            _btnToolbarAddProcedure = new Button();
            _btnToolbarRefresh = new Button();
            _surgeryStatusFilterCombo = new ComboBox();
            _sectionSearch = new TextBox();
            _sectionHeader = new Panel();
            _btnHeaderNewSurgery = new Button();
            _sectionSubtitle = new Label();
            _sectionTitle = new Label();
            _topbar = new Panel();
            _topbarCaption = new Label();
            _topbarWelcome = new Label();
            _rightPanel = new Panel();
            _dayAppointments = new FlowLayoutPanel();
            _appointmentHeaderLabel = new Label();
            _calendarTable = new TableLayoutPanel();
            _monthNavPanel = new Panel();
            _btnNextMonth = new Button();
            _btnPrevMonth = new Button();
            _calendarTitle = new Label();
            _rightProfilePanel = new Panel();
            _rightProfile = new Label();
            _btnRightLogout = new Button();
            _btnRightProfile = new Button();
            _sidebar = new Panel();
            _menuFlow = new FlowLayoutPanel();
            _navResumen = new Button();
            _navAgenda = new Button();
            _navPacientes = new Button();
            _navPagos = new Button();
            _navCirugias = new Button();
            _navRecetas = new Button();
            _navAseguradoras = new Button();
            _navConfiguracion = new Button();
            _navExpedientes = new Button();
            _navInicio = new Button();
            _navMiPortal = new Button();
            _navNotificaciones = new Button();
            _navNoticias = new Button();
            _sidebarFooter = new Panel();
            _btnSidebarLogout = new Button();
            _footerRoleLabel = new Label();
            _footerUserLabel = new Label();
            _menuHeaderLabel = new Label();
            _brandPanel = new Panel();
            _brandSubtitle = new Label();
            _brandTitle = new Label();
            _brandIcon = new Label();
            _toolTip = new ToolTip(components);
            _loginErrorProvider = new ErrorProvider(components);
            _root.SuspendLayout();
            _loginCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_loginLogo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_loginErrorProvider).BeginInit();
            _workspacePanel.SuspendLayout();
            _content.SuspendLayout();
            _pageContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_mainGrid).BeginInit();
            _statusBar.SuspendLayout();
            _sectionToolbar.SuspendLayout();
            _sectionHeader.SuspendLayout();
            _topbar.SuspendLayout();
            _rightPanel.SuspendLayout();
            _monthNavPanel.SuspendLayout();
            _rightProfilePanel.SuspendLayout();
            _sidebar.SuspendLayout();
            _menuFlow.SuspendLayout();
            _sidebarFooter.SuspendLayout();
            _brandPanel.SuspendLayout();
            SuspendLayout();
            // 
            // _root
            // 
            _root.BackColor = Color.FromArgb(244, 245, 247);
            _root.Controls.Add(_loginCard);
            _root.Controls.Add(_workspacePanel);
            _root.Dock = DockStyle.Fill;
            _root.Location = new Point(0, 0);
            _root.Name = "_root";
            _root.Size = new Size(1344, 811);
            _root.TabIndex = 0;
            // 
            // _loginCard
            // 
            _loginCard.BackColor = Color.White;
            _loginCard.Controls.Add(_btnPublicNews);
            _loginCard.Controls.Add(_btnPublicBooking);
            _loginCard.Controls.Add(_btnRegister);
            _loginCard.Controls.Add(_btnLogin);
            _loginCard.Controls.Add(_loginMessage);
            _loginCard.Controls.Add(_loginShowPassword);
            _loginCard.Controls.Add(_loginPassword);
            _loginCard.Controls.Add(_loginUser);
            _loginCard.Controls.Add(_loginSubtitle);
            _loginCard.Controls.Add(_loginTitle);
            _loginCard.Controls.Add(_loginBrand);
            _loginCard.Controls.Add(_loginLogo);
            _loginCard.Location = new Point(442, 130);
            _loginCard.Name = "_loginCard";
            _loginCard.Padding = new Padding(42, 24, 42, 32);
            _loginCard.Size = new Size(460, 530);
            _loginCard.TabIndex = 0;
            // 
            // _btnPublicNews
            // 
            _btnPublicNews.BackColor = Color.White;
            _btnPublicNews.Cursor = Cursors.Hand;
            _btnPublicNews.FlatAppearance.BorderColor = Color.FromArgb(231, 221, 224);
            _btnPublicNews.FlatStyle = FlatStyle.Flat;
            _btnPublicNews.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _btnPublicNews.ForeColor = Color.FromArgb(158, 110, 121);
            _btnPublicNews.Location = new Point(236, 448);
            _btnPublicNews.Name = "_btnPublicNews";
            _btnPublicNews.Size = new Size(182, 38);
            _btnPublicNews.TabIndex = 6;
            _btnPublicNews.Text = "Noticias";
            _btnPublicNews.UseVisualStyleBackColor = false;
            // 
            // _btnPublicBooking
            // 
            _btnPublicBooking.BackColor = Color.White;
            _btnPublicBooking.Cursor = Cursors.Hand;
            _btnPublicBooking.FlatAppearance.BorderColor = Color.FromArgb(231, 221, 224);
            _btnPublicBooking.FlatStyle = FlatStyle.Flat;
            _btnPublicBooking.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _btnPublicBooking.ForeColor = Color.FromArgb(158, 110, 121);
            _btnPublicBooking.Location = new Point(42, 448);
            _btnPublicBooking.Name = "_btnPublicBooking";
            _btnPublicBooking.Size = new Size(182, 38);
            _btnPublicBooking.TabIndex = 5;
            _btnPublicBooking.Text = "Agendar cita";
            _btnPublicBooking.UseVisualStyleBackColor = false;
            // 
            // _btnRegister
            // 
            _btnRegister.BackColor = Color.White;
            _btnRegister.Cursor = Cursors.Hand;
            _btnRegister.FlatAppearance.BorderColor = Color.FromArgb(231, 221, 224);
            _btnRegister.FlatStyle = FlatStyle.Flat;
            _btnRegister.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _btnRegister.ForeColor = Color.FromArgb(158, 110, 121);
            _btnRegister.Location = new Point(42, 396);
            _btnRegister.Name = "_btnRegister";
            _btnRegister.Size = new Size(376, 38);
            _btnRegister.TabIndex = 4;
            _btnRegister.Text = "Crear cuenta de paciente";
            _btnRegister.UseVisualStyleBackColor = false;
            // 
            // _btnLogin
            // 
            _btnLogin.BackColor = Color.FromArgb(158, 110, 121);
            _btnLogin.Cursor = Cursors.Hand;
            _btnLogin.FlatStyle = FlatStyle.Flat;
            _btnLogin.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _btnLogin.ForeColor = Color.White;
            _btnLogin.Location = new Point(42, 336);
            _btnLogin.Name = "_btnLogin";
            _btnLogin.Size = new Size(376, 46);
            _btnLogin.TabIndex = 3;
            _btnLogin.Text = "Iniciar sesión";
            _btnLogin.UseVisualStyleBackColor = false;
            // 
            // _loginMessage
            // 
            _loginMessage.ForeColor = Color.Firebrick;
            _loginMessage.Location = new Point(42, 282);
            _loginMessage.Name = "_loginMessage";
            _loginMessage.Size = new Size(376, 44);
            _loginMessage.TabIndex = 11;
            _loginMessage.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // _loginShowPassword
            // 
            _loginShowPassword.BackColor = Color.Transparent;
            _loginShowPassword.Cursor = Cursors.Hand;
            _loginShowPassword.ForeColor = Color.FromArgb(158, 154, 155);
            _loginShowPassword.Location = new Point(338, 245);
            _loginShowPassword.Name = "_loginShowPassword";
            _loginShowPassword.Size = new Size(80, 26);
            _loginShowPassword.TabIndex = 2;
            _loginShowPassword.Text = "Mostrar";
            _loginShowPassword.UseVisualStyleBackColor = false;
            // 
            // _loginPassword
            // 
            _loginPassword.BackColor = Color.White;
            _loginPassword.Font = new Font("Segoe UI", 11F);
            _loginPassword.ForeColor = Color.FromArgb(54, 56, 58);
            _loginPassword.Location = new Point(42, 244);
            _loginPassword.Name = "_loginPassword";
            _loginPassword.PlaceholderText = "Contraseña";
            _loginPassword.Size = new Size(288, 27);
            _loginPassword.TabIndex = 1;
            _loginPassword.UseSystemPasswordChar = true;
            // 
            // _loginUser
            // 
            _loginUser.BackColor = Color.White;
            _loginUser.Font = new Font("Segoe UI", 11F);
            _loginUser.ForeColor = Color.FromArgb(54, 56, 58);
            _loginUser.Location = new Point(42, 200);
            _loginUser.Name = "_loginUser";
            _loginUser.PlaceholderText = "Usuario, correo, cédula o teléfono";
            _loginUser.Size = new Size(376, 27);
            _loginUser.TabIndex = 0;
            // 
            // _loginSubtitle
            // 
            _loginSubtitle.ForeColor = Color.FromArgb(158, 154, 155);
            _loginSubtitle.Location = new Point(0, 158);
            _loginSubtitle.Name = "_loginSubtitle";
            _loginSubtitle.Size = new Size(460, 24);
            _loginSubtitle.TabIndex = 10;
            _loginSubtitle.Text = "Accede al sistema de gestión clínica";
            _loginSubtitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // _loginTitle
            // 
            _loginTitle.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            _loginTitle.ForeColor = Color.FromArgb(54, 56, 58);
            _loginTitle.Location = new Point(0, 112);
            _loginTitle.Name = "_loginTitle";
            _loginTitle.Size = new Size(460, 44);
            _loginTitle.TabIndex = 9;
            _loginTitle.Text = "Bienvenido";
            _loginTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // _loginBrand
            // 
            _loginBrand.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            _loginBrand.ForeColor = Color.FromArgb(158, 110, 121);
            _loginBrand.Location = new Point(0, 84);
            _loginBrand.Name = "_loginBrand";
            _loginBrand.Size = new Size(460, 26);
            _loginBrand.TabIndex = 8;
            _loginBrand.Text = "TANGER CLINIC";
            _loginBrand.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // _loginLogo
            // 
            _loginLogo.BackColor = Color.FromArgb(158, 110, 121);
            _loginLogo.Location = new Point(204, 24);
            _loginLogo.Name = "_loginLogo";
            _loginLogo.Size = new Size(52, 52);
            _loginLogo.SizeMode = PictureBoxSizeMode.Zoom;
            _loginLogo.TabIndex = 7;
            _loginLogo.TabStop = false;
            // 
            // _workspacePanel
            // 
            _workspacePanel.BackColor = Color.FromArgb(244, 245, 247);
            _workspacePanel.Controls.Add(_content);
            _workspacePanel.Controls.Add(_rightPanel);
            _workspacePanel.Controls.Add(_sidebar);
            _workspacePanel.Dock = DockStyle.Fill;
            _workspacePanel.Location = new Point(0, 0);
            _workspacePanel.Name = "_workspacePanel";
            _workspacePanel.Size = new Size(1344, 811);
            _workspacePanel.TabIndex = 1;
            // 
            // _content
            // 
            _content.BackColor = Color.FromArgb(244, 245, 247);
            _content.Controls.Add(_pageContent);
            _content.Controls.Add(_topbar);
            _content.Dock = DockStyle.Fill;
            _content.Location = new Point(245, 0);
            _content.Name = "_content";
            _content.Padding = new Padding(30);
            _content.Size = new Size(799, 811);
            _content.TabIndex = 0;
            // 
            // _pageContent
            // 
            _pageContent.BackColor = Color.FromArgb(244, 245, 247);
            _pageContent.Controls.Add(_mainGrid);
            _pageContent.Controls.Add(_dynamicHost);
            _pageContent.Controls.Add(_statusBar);
            _pageContent.Controls.Add(_sectionToolbar);
            _pageContent.Controls.Add(_sectionHeader);
            _pageContent.Dock = DockStyle.Fill;
            _pageContent.Location = new Point(30, 114);
            _pageContent.Name = "_pageContent";
            _pageContent.Size = new Size(739, 667);
            _pageContent.TabIndex = 0;
            // 
            // _mainGrid
            // 
            _mainGrid.AllowUserToAddRows = false;
            _mainGrid.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 233, 237);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(54, 56, 58);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(158, 110, 121);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            _mainGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            _mainGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _mainGrid.BackgroundColor = Color.White;
            _mainGrid.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(158, 110, 121);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(158, 110, 121);
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            _mainGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            _mainGrid.ColumnHeadersHeight = 44;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(54, 56, 58);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(158, 110, 121);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            _mainGrid.DefaultCellStyle = dataGridViewCellStyle3;
            _mainGrid.Dock = DockStyle.Fill;
            _mainGrid.EnableHeadersVisualStyles = false;
            _mainGrid.GridColor = Color.FromArgb(235, 231, 230);
            _mainGrid.Location = new Point(0, 150);
            _mainGrid.MultiSelect = false;
            _mainGrid.Name = "_mainGrid";
            _mainGrid.ReadOnly = true;
            _mainGrid.RowHeadersVisible = false;
            _mainGrid.RowTemplate.Height = 42;
            _mainGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _mainGrid.Size = new Size(739, 481);
            _mainGrid.TabIndex = 2;
            // 
            // _dynamicHost
            // 
            _dynamicHost.BackColor = Color.FromArgb(244, 245, 247);
            _dynamicHost.Dock = DockStyle.Fill;
            _dynamicHost.Location = new Point(0, 150);
            _dynamicHost.Name = "_dynamicHost";
            _dynamicHost.Size = new Size(739, 481);
            _dynamicHost.TabIndex = 3;
            _dynamicHost.Visible = false;
            // 
            // _statusBar
            // 
            _statusBar.Controls.Add(_status);
            _statusBar.Dock = DockStyle.Bottom;
            _statusBar.Location = new Point(0, 631);
            _statusBar.Name = "_statusBar";
            _statusBar.Size = new Size(739, 36);
            _statusBar.TabIndex = 4;
            // 
            // _status
            // 
            _status.AutoSize = true;
            _status.Font = new Font("Segoe UI", 9F);
            _status.ForeColor = Color.FromArgb(158, 110, 121);
            _status.Location = new Point(0, 9);
            _status.Name = "_status";
            _status.Size = new Size(32, 15);
            _status.TabIndex = 0;
            _status.Text = "Listo";
            // 
            // _sectionToolbar
            // 
            _sectionToolbar.Controls.Add(_btnToolbarAdd);
            _sectionToolbar.Controls.Add(_btnToolbarAddProcedure);
            _sectionToolbar.Controls.Add(_btnToolbarRefresh);
            _sectionToolbar.Controls.Add(_surgeryStatusFilterCombo);
            _sectionToolbar.Controls.Add(_sectionSearch);
            _sectionToolbar.Dock = DockStyle.Top;
            _sectionToolbar.Location = new Point(0, 86);
            _sectionToolbar.Name = "_sectionToolbar";
            _sectionToolbar.Padding = new Padding(0, 8, 0, 10);
            _sectionToolbar.Size = new Size(739, 64);
            _sectionToolbar.TabIndex = 1;
            // 
            // _btnToolbarAdd
            // 
            _btnToolbarAdd.BackColor = Color.FromArgb(158, 110, 121);
            _btnToolbarAdd.Cursor = Cursors.Hand;
            _btnToolbarAdd.Dock = DockStyle.Right;
            _btnToolbarAdd.FlatStyle = FlatStyle.Flat;
            _btnToolbarAdd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _btnToolbarAdd.ForeColor = Color.White;
            _btnToolbarAdd.Location = new Point(279, 8);
            _btnToolbarAdd.Name = "_btnToolbarAdd";
            _btnToolbarAdd.Size = new Size(175, 46);
            _btnToolbarAdd.TabIndex = 2;
            _btnToolbarAdd.Text = "+  Nuevo paciente";
            _toolTip.SetToolTip(_btnToolbarAdd, "Crear un nuevo registro en la sección actual");
            _btnToolbarAdd.UseVisualStyleBackColor = false;
            // 
            // _btnToolbarAddProcedure
            // 
            _btnToolbarAddProcedure.BackColor = Color.White;
            _btnToolbarAddProcedure.Cursor = Cursors.Hand;
            _btnToolbarAddProcedure.Dock = DockStyle.Right;
            _btnToolbarAddProcedure.FlatStyle = FlatStyle.Flat;
            _btnToolbarAddProcedure.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _btnToolbarAddProcedure.ForeColor = Color.FromArgb(158, 110, 121);
            _btnToolbarAddProcedure.Location = new Point(454, 8);
            _btnToolbarAddProcedure.Name = "_btnToolbarAddProcedure";
            _btnToolbarAddProcedure.Size = new Size(165, 46);
            _btnToolbarAddProcedure.TabIndex = 3;
            _btnToolbarAddProcedure.Text = "+  Procedimiento";
            _toolTip.SetToolTip(_btnToolbarAddProcedure, "Agregar un nuevo tipo de procedimiento al catálogo");
            _btnToolbarAddProcedure.UseVisualStyleBackColor = false;
            _btnToolbarAddProcedure.Visible = false;
            // 
            // _btnToolbarRefresh
            // 
            _btnToolbarRefresh.BackColor = Color.White;
            _btnToolbarRefresh.Cursor = Cursors.Hand;
            _btnToolbarRefresh.Dock = DockStyle.Right;
            _btnToolbarRefresh.FlatStyle = FlatStyle.Flat;
            _btnToolbarRefresh.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _btnToolbarRefresh.ForeColor = Color.FromArgb(158, 110, 121);
            _btnToolbarRefresh.Location = new Point(619, 8);
            _btnToolbarRefresh.Name = "_btnToolbarRefresh";
            _btnToolbarRefresh.Size = new Size(120, 46);
            _btnToolbarRefresh.TabIndex = 4;
            _btnToolbarRefresh.Text = "Actualizar";
            _toolTip.SetToolTip(_btnToolbarRefresh, "Recargar los datos de la sección actual");
            _btnToolbarRefresh.UseVisualStyleBackColor = false;
            // 
            // _surgeryStatusFilterCombo
            // 
            _surgeryStatusFilterCombo.BackColor = Color.White;
            _surgeryStatusFilterCombo.Dock = DockStyle.Left;
            _surgeryStatusFilterCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            _surgeryStatusFilterCombo.ForeColor = Color.FromArgb(54, 56, 58);
            _surgeryStatusFilterCombo.FormattingEnabled = true;
            _surgeryStatusFilterCombo.Items.AddRange(new object[] { "Todos los estados", "Estudios preoperatorios", "Programada", "En proceso", "Postoperatorio / Recuperación", "Completada" });
            _surgeryStatusFilterCombo.Location = new Point(310, 8);
            _surgeryStatusFilterCombo.Name = "_surgeryStatusFilterCombo";
            _surgeryStatusFilterCombo.Size = new Size(235, 25);
            _surgeryStatusFilterCombo.TabIndex = 1;
            _toolTip.SetToolTip(_surgeryStatusFilterCombo, "Filtrar cirugías por estado clínico");
            _surgeryStatusFilterCombo.Visible = false;
            // 
            // _sectionSearch
            // 
            _sectionSearch.BackColor = Color.White;
            _sectionSearch.Dock = DockStyle.Left;
            _sectionSearch.Font = new Font("Segoe UI", 10F);
            _sectionSearch.ForeColor = Color.FromArgb(54, 56, 58);
            _sectionSearch.Location = new Point(0, 8);
            _sectionSearch.Name = "_sectionSearch";
            _sectionSearch.PlaceholderText = "Buscar en la lista…";
            _sectionSearch.Size = new Size(310, 25);
            _sectionSearch.TabIndex = 0;
            _toolTip.SetToolTip(_sectionSearch, "Filtrar registros visibles en la lista actual");
            // 
            // _sectionHeader
            // 
            _sectionHeader.Controls.Add(_btnHeaderNewSurgery);
            _sectionHeader.Controls.Add(_sectionSubtitle);
            _sectionHeader.Controls.Add(_sectionTitle);
            _sectionHeader.Dock = DockStyle.Top;
            _sectionHeader.Location = new Point(0, 0);
            _sectionHeader.Name = "_sectionHeader";
            _sectionHeader.Size = new Size(739, 86);
            _sectionHeader.TabIndex = 0;
            // 
            // _btnHeaderNewSurgery
            // 
            _btnHeaderNewSurgery.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _btnHeaderNewSurgery.BackColor = Color.FromArgb(158, 110, 121);
            _btnHeaderNewSurgery.Cursor = Cursors.Hand;
            _btnHeaderNewSurgery.FlatStyle = FlatStyle.Flat;
            _btnHeaderNewSurgery.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _btnHeaderNewSurgery.ForeColor = Color.White;
            _btnHeaderNewSurgery.Location = new Point(589, 4);
            _btnHeaderNewSurgery.Name = "_btnHeaderNewSurgery";
            _btnHeaderNewSurgery.Size = new Size(150, 40);
            _btnHeaderNewSurgery.TabIndex = 2;
            _btnHeaderNewSurgery.Text = "Nueva cirugía";
            _toolTip.SetToolTip(_btnHeaderNewSurgery, "Programar una nueva cirugía");
            _btnHeaderNewSurgery.UseVisualStyleBackColor = false;
            _btnHeaderNewSurgery.Visible = false;
            // 
            // _sectionSubtitle
            // 
            _sectionSubtitle.Dock = DockStyle.Bottom;
            _sectionSubtitle.ForeColor = Color.FromArgb(158, 154, 155);
            _sectionSubtitle.Location = new Point(0, 61);
            _sectionSubtitle.Name = "_sectionSubtitle";
            _sectionSubtitle.Size = new Size(739, 25);
            _sectionSubtitle.TabIndex = 1;
            _sectionSubtitle.Text = "Consulta la actividad y el rendimiento de tu consultorio";
            // 
            // _sectionTitle
            // 
            _sectionTitle.Dock = DockStyle.Top;
            _sectionTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            _sectionTitle.ForeColor = Color.FromArgb(54, 56, 58);
            _sectionTitle.Location = new Point(0, 0);
            _sectionTitle.Name = "_sectionTitle";
            _sectionTitle.Size = new Size(739, 47);
            _sectionTitle.TabIndex = 0;
            _sectionTitle.Text = "Resumen";
            // 
            // _topbar
            // 
            _topbar.BackColor = Color.White;
            _topbar.Controls.Add(_topbarCaption);
            _topbar.Controls.Add(_topbarWelcome);
            _topbar.Dock = DockStyle.Top;
            _topbar.Location = new Point(30, 30);
            _topbar.Name = "_topbar";
            _topbar.Padding = new Padding(28, 15, 28, 12);
            _topbar.Size = new Size(739, 84);
            _topbar.TabIndex = 1;
            // 
            // _topbarCaption
            // 
            _topbarCaption.Dock = DockStyle.Top;
            _topbarCaption.Font = new Font("Segoe UI", 9F);
            _topbarCaption.ForeColor = Color.FromArgb(158, 154, 155);
            _topbarCaption.Location = new Point(28, 43);
            _topbarCaption.Name = "_topbarCaption";
            _topbarCaption.Size = new Size(683, 25);
            _topbarCaption.TabIndex = 1;
            _topbarCaption.Text = "Panel de control del consultorio";
            // 
            // _topbarWelcome
            // 
            _topbarWelcome.Dock = DockStyle.Top;
            _topbarWelcome.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            _topbarWelcome.ForeColor = Color.FromArgb(54, 56, 58);
            _topbarWelcome.Location = new Point(28, 15);
            _topbarWelcome.Name = "_topbarWelcome";
            _topbarWelcome.Size = new Size(683, 28);
            _topbarWelcome.TabIndex = 0;
            _topbarWelcome.Text = "Bienvenido";
            _topbarWelcome.Click += _topbarWelcome_Click;
            // 
            // _rightPanel
            // 
            _rightPanel.BackColor = Color.White;
            _rightPanel.Controls.Add(_dayAppointments);
            _rightPanel.Controls.Add(_appointmentHeaderLabel);
            _rightPanel.Controls.Add(_calendarTable);
            _rightPanel.Controls.Add(_monthNavPanel);
            _rightPanel.Controls.Add(_calendarTitle);
            _rightPanel.Controls.Add(_rightProfilePanel);
            _rightPanel.Dock = DockStyle.Right;
            _rightPanel.Location = new Point(1044, 0);
            _rightPanel.Name = "_rightPanel";
            _rightPanel.Padding = new Padding(18);
            _rightPanel.Size = new Size(300, 811);
            _rightPanel.TabIndex = 1;
            // 
            // _dayAppointments
            // 
            _dayAppointments.AutoScroll = true;
            _dayAppointments.BackColor = Color.White;
            _dayAppointments.Dock = DockStyle.Fill;
            _dayAppointments.FlowDirection = FlowDirection.TopDown;
            _dayAppointments.Location = new Point(18, 460);
            _dayAppointments.Name = "_dayAppointments";
            _dayAppointments.Padding = new Padding(0, 8, 0, 0);
            _dayAppointments.Size = new Size(264, 333);
            _dayAppointments.TabIndex = 5;
            _dayAppointments.WrapContents = false;
            // 
            // _appointmentHeaderLabel
            // 
            _appointmentHeaderLabel.Dock = DockStyle.Top;
            _appointmentHeaderLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            _appointmentHeaderLabel.ForeColor = Color.FromArgb(54, 56, 58);
            _appointmentHeaderLabel.Location = new Point(18, 416);
            _appointmentHeaderLabel.Name = "_appointmentHeaderLabel";
            _appointmentHeaderLabel.Size = new Size(264, 44);
            _appointmentHeaderLabel.TabIndex = 4;
            _appointmentHeaderLabel.Text = "Citas del día";
            _appointmentHeaderLabel.TextAlign = ContentAlignment.BottomLeft;
            // 
            // _calendarTable
            // 
            _calendarTable.BackColor = Color.White;
            _calendarTable.ColumnCount = 7;
            _calendarTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.285F));
            _calendarTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.285F));
            _calendarTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.285F));
            _calendarTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.285F));
            _calendarTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.285F));
            _calendarTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.285F));
            _calendarTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.285F));
            _calendarTable.Dock = DockStyle.Top;
            _calendarTable.Location = new Point(18, 194);
            _calendarTable.Margin = new Padding(0, 8, 0, 16);
            _calendarTable.Name = "_calendarTable";
            _calendarTable.RowCount = 7;
            _calendarTable.RowStyles.Add(new RowStyle(SizeType.Percent, 14.285F));
            _calendarTable.RowStyles.Add(new RowStyle(SizeType.Percent, 14.285F));
            _calendarTable.RowStyles.Add(new RowStyle(SizeType.Percent, 14.285F));
            _calendarTable.RowStyles.Add(new RowStyle(SizeType.Percent, 14.285F));
            _calendarTable.RowStyles.Add(new RowStyle(SizeType.Percent, 14.285F));
            _calendarTable.RowStyles.Add(new RowStyle(SizeType.Percent, 14.285F));
            _calendarTable.RowStyles.Add(new RowStyle(SizeType.Percent, 14.285F));
            _calendarTable.Size = new Size(264, 222);
            _calendarTable.TabIndex = 3;
            // 
            // _monthNavPanel
            // 
            _monthNavPanel.Controls.Add(_btnNextMonth);
            _monthNavPanel.Controls.Add(_btnPrevMonth);
            _monthNavPanel.Dock = DockStyle.Top;
            _monthNavPanel.Location = new Point(18, 156);
            _monthNavPanel.Name = "_monthNavPanel";
            _monthNavPanel.Size = new Size(264, 38);
            _monthNavPanel.TabIndex = 2;
            // 
            // _btnNextMonth
            // 
            _btnNextMonth.BackColor = Color.White;
            _btnNextMonth.Cursor = Cursors.Hand;
            _btnNextMonth.Dock = DockStyle.Right;
            _btnNextMonth.FlatAppearance.BorderSize = 0;
            _btnNextMonth.FlatStyle = FlatStyle.Flat;
            _btnNextMonth.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _btnNextMonth.ForeColor = Color.FromArgb(158, 110, 121);
            _btnNextMonth.Location = new Point(222, 0);
            _btnNextMonth.Name = "_btnNextMonth";
            _btnNextMonth.Size = new Size(42, 38);
            _btnNextMonth.TabIndex = 1;
            _btnNextMonth.Text = "›";
            _toolTip.SetToolTip(_btnNextMonth, "Mes siguiente en el calendario");
            _btnNextMonth.UseVisualStyleBackColor = false;
            // 
            // _btnPrevMonth
            // 
            _btnPrevMonth.BackColor = Color.White;
            _btnPrevMonth.Cursor = Cursors.Hand;
            _btnPrevMonth.Dock = DockStyle.Left;
            _btnPrevMonth.FlatAppearance.BorderSize = 0;
            _btnPrevMonth.FlatStyle = FlatStyle.Flat;
            _btnPrevMonth.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _btnPrevMonth.ForeColor = Color.FromArgb(158, 110, 121);
            _btnPrevMonth.Location = new Point(0, 0);
            _btnPrevMonth.Name = "_btnPrevMonth";
            _btnPrevMonth.Size = new Size(42, 38);
            _btnPrevMonth.TabIndex = 0;
            _btnPrevMonth.Text = "‹";
            _toolTip.SetToolTip(_btnPrevMonth, "Mes anterior en el calendario");
            _btnPrevMonth.UseVisualStyleBackColor = false;
            // 
            // _calendarTitle
            // 
            _calendarTitle.Dock = DockStyle.Top;
            _calendarTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            _calendarTitle.ForeColor = Color.FromArgb(54, 56, 58);
            _calendarTitle.Location = new Point(18, 126);
            _calendarTitle.Name = "_calendarTitle";
            _calendarTitle.Size = new Size(264, 30);
            _calendarTitle.TabIndex = 1;
            _calendarTitle.Text = "Calendario";
            _calendarTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // _rightProfilePanel
            // 
            _rightProfilePanel.BackColor = Color.White;
            _rightProfilePanel.Controls.Add(_rightProfile);
            _rightProfilePanel.Controls.Add(_btnRightLogout);
            _rightProfilePanel.Controls.Add(_btnRightProfile);
            _rightProfilePanel.Dock = DockStyle.Top;
            _rightProfilePanel.Location = new Point(18, 18);
            _rightProfilePanel.Name = "_rightProfilePanel";
            _rightProfilePanel.Padding = new Padding(0, 6, 0, 12);
            _rightProfilePanel.Size = new Size(264, 108);
            _rightProfilePanel.TabIndex = 0;
            // 
            // _rightProfile
            // 
            _rightProfile.Dock = DockStyle.Top;
            _rightProfile.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _rightProfile.ForeColor = Color.FromArgb(54, 56, 58);
            _rightProfile.Location = new Point(104, 6);
            _rightProfile.Name = "_rightProfile";
            _rightProfile.Size = new Size(70, 48);
            _rightProfile.TabIndex = 2;
            _rightProfile.Text = "Usuario\r\nRol";
            // 
            // _btnRightLogout
            // 
            _btnRightLogout.BackColor = Color.White;
            _btnRightLogout.Cursor = Cursors.Hand;
            _btnRightLogout.Dock = DockStyle.Right;
            _btnRightLogout.FlatStyle = FlatStyle.Flat;
            _btnRightLogout.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _btnRightLogout.ForeColor = Color.FromArgb(158, 110, 121);
            _btnRightLogout.Location = new Point(174, 6);
            _btnRightLogout.Name = "_btnRightLogout";
            _btnRightLogout.Size = new Size(90, 90);
            _btnRightLogout.TabIndex = 1;
            _btnRightLogout.Text = "Salir";
            _toolTip.SetToolTip(_btnRightLogout, "Cerrar sesión y volver al inicio");
            _btnRightLogout.UseVisualStyleBackColor = false;
            // 
            // _btnRightProfile
            // 
            _btnRightProfile.BackColor = Color.White;
            _btnRightProfile.Cursor = Cursors.Hand;
            _btnRightProfile.Dock = DockStyle.Left;
            _btnRightProfile.FlatStyle = FlatStyle.Flat;
            _btnRightProfile.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _btnRightProfile.ForeColor = Color.FromArgb(158, 110, 121);
            _btnRightProfile.Location = new Point(0, 6);
            _btnRightProfile.Name = "_btnRightProfile";
            _btnRightProfile.Size = new Size(104, 90);
            _btnRightProfile.TabIndex = 0;
            _btnRightProfile.Text = "Mi perfil";
            _toolTip.SetToolTip(_btnRightProfile, "Consultar y editar mi perfil de usuario");
            _btnRightProfile.UseVisualStyleBackColor = false;
            _btnRightProfile.Click += _btnRightProfile_Click;
            // 
            // _sidebar
            // 
            _sidebar.BackColor = Color.White;
            _sidebar.Controls.Add(_menuFlow);
            _sidebar.Controls.Add(_sidebarFooter);
            _sidebar.Controls.Add(_menuHeaderLabel);
            _sidebar.Controls.Add(_brandPanel);
            _sidebar.Dock = DockStyle.Left;
            _sidebar.Location = new Point(0, 0);
            _sidebar.Name = "_sidebar";
            _sidebar.Padding = new Padding(18, 24, 18, 18);
            _sidebar.Size = new Size(245, 811);
            _sidebar.TabIndex = 2;
            // 
            // _menuFlow
            // 
            _menuFlow.AutoScroll = true;
            _menuFlow.BackColor = Color.White;
            _menuFlow.Controls.Add(_navResumen);
            _menuFlow.Controls.Add(_navAgenda);
            _menuFlow.Controls.Add(_navPacientes);
            _menuFlow.Controls.Add(_navPagos);
            _menuFlow.Controls.Add(_navCirugias);
            _menuFlow.Controls.Add(_navRecetas);
            _menuFlow.Controls.Add(_navAseguradoras);
            _menuFlow.Controls.Add(_navConfiguracion);
            _menuFlow.Controls.Add(_navExpedientes);
            _menuFlow.Controls.Add(_navInicio);
            _menuFlow.Controls.Add(_navMiPortal);
            _menuFlow.Controls.Add(_navNotificaciones);
            _menuFlow.Controls.Add(_navNoticias);
            _menuFlow.Dock = DockStyle.Fill;
            _menuFlow.FlowDirection = FlowDirection.TopDown;
            _menuFlow.Location = new Point(18, 136);
            _menuFlow.Name = "_menuFlow";
            _menuFlow.Padding = new Padding(0, 10, 0, 0);
            _menuFlow.Size = new Size(209, 545);
            _menuFlow.TabIndex = 2;
            _menuFlow.WrapContents = false;
            // 
            // _navResumen
            // 
            _navResumen.BackColor = Color.FromArgb(248, 233, 237);
            _navResumen.Cursor = Cursors.Hand;
            _navResumen.FlatAppearance.BorderSize = 0;
            _navResumen.FlatAppearance.MouseOverBackColor = Color.FromArgb(248, 233, 237);
            _navResumen.FlatStyle = FlatStyle.Flat;
            _navResumen.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _navResumen.ForeColor = Color.FromArgb(158, 110, 121);
            _navResumen.Location = new Point(3, 13);
            _navResumen.Name = "_navResumen";
            _navResumen.Size = new Size(207, 44);
            _navResumen.TabIndex = 0;
            _navResumen.Tag = "Resumen";
            _navResumen.Text = "◈   Resumen";
            _navResumen.TextAlign = ContentAlignment.MiddleLeft;
            _toolTip.SetToolTip(_navResumen, "Panel general de actividad y métricas");
            _navResumen.UseVisualStyleBackColor = false;
            // 
            // _navAgenda
            // 
            _navAgenda.BackColor = Color.White;
            _navAgenda.Cursor = Cursors.Hand;
            _navAgenda.FlatAppearance.BorderSize = 0;
            _navAgenda.FlatAppearance.MouseOverBackColor = Color.FromArgb(248, 233, 237);
            _navAgenda.FlatStyle = FlatStyle.Flat;
            _navAgenda.Font = new Font("Segoe UI", 10F);
            _navAgenda.ForeColor = Color.FromArgb(111, 105, 107);
            _navAgenda.Location = new Point(3, 63);
            _navAgenda.Name = "_navAgenda";
            _navAgenda.Size = new Size(207, 44);
            _navAgenda.TabIndex = 1;
            _navAgenda.Tag = "Agenda";
            _navAgenda.Text = "▦   Agenda";
            _navAgenda.TextAlign = ContentAlignment.MiddleLeft;
            _toolTip.SetToolTip(_navAgenda, "Gestionar citas y agenda clínica");
            _navAgenda.UseVisualStyleBackColor = false;
            _navAgenda.Click += _navAgenda_Click;
            // 
            // _navPacientes
            // 
            _navPacientes.BackColor = Color.White;
            _navPacientes.Cursor = Cursors.Hand;
            _navPacientes.FlatAppearance.BorderSize = 0;
            _navPacientes.FlatAppearance.MouseOverBackColor = Color.FromArgb(248, 233, 237);
            _navPacientes.FlatStyle = FlatStyle.Flat;
            _navPacientes.Font = new Font("Segoe UI", 10F);
            _navPacientes.ForeColor = Color.FromArgb(111, 105, 107);
            _navPacientes.Location = new Point(3, 113);
            _navPacientes.Name = "_navPacientes";
            _navPacientes.Size = new Size(207, 44);
            _navPacientes.TabIndex = 2;
            _navPacientes.Tag = "Pacientes";
            _navPacientes.Text = "♙   Pacientes";
            _navPacientes.TextAlign = ContentAlignment.MiddleLeft;
            _toolTip.SetToolTip(_navPacientes, "Directorio y gestión de pacientes");
            _navPacientes.UseVisualStyleBackColor = false;
            // 
            // _navPagos
            // 
            _navPagos.BackColor = Color.White;
            _navPagos.Cursor = Cursors.Hand;
            _navPagos.FlatAppearance.BorderSize = 0;
            _navPagos.FlatAppearance.MouseOverBackColor = Color.FromArgb(248, 233, 237);
            _navPagos.FlatStyle = FlatStyle.Flat;
            _navPagos.Font = new Font("Segoe UI", 10F);
            _navPagos.ForeColor = Color.FromArgb(111, 105, 107);
            _navPagos.Location = new Point(3, 163);
            _navPagos.Name = "_navPagos";
            _navPagos.Size = new Size(207, 44);
            _navPagos.TabIndex = 3;
            _navPagos.Tag = "Pagos";
            _navPagos.Text = "$   Pagos";
            _navPagos.TextAlign = ContentAlignment.MiddleLeft;
            _toolTip.SetToolTip(_navPagos, "Registro y control de pagos");
            _navPagos.UseVisualStyleBackColor = false;
            // 
            // _navCirugias
            // 
            _navCirugias.BackColor = Color.White;
            _navCirugias.Cursor = Cursors.Hand;
            _navCirugias.FlatAppearance.BorderSize = 0;
            _navCirugias.FlatAppearance.MouseOverBackColor = Color.FromArgb(248, 233, 237);
            _navCirugias.FlatStyle = FlatStyle.Flat;
            _navCirugias.Font = new Font("Segoe UI", 10F);
            _navCirugias.ForeColor = Color.FromArgb(111, 105, 107);
            _navCirugias.Location = new Point(3, 213);
            _navCirugias.Name = "_navCirugias";
            _navCirugias.Size = new Size(207, 44);
            _navCirugias.TabIndex = 4;
            _navCirugias.Tag = "Cirugías";
            _navCirugias.Text = "✚   Cirugías";
            _navCirugias.TextAlign = ContentAlignment.MiddleLeft;
            _toolTip.SetToolTip(_navCirugias, "Control y seguimiento de cirugías");
            _navCirugias.UseVisualStyleBackColor = false;
            // 
            // _navRecetas
            // 
            _navRecetas.BackColor = Color.White;
            _navRecetas.Cursor = Cursors.Hand;
            _navRecetas.FlatAppearance.BorderSize = 0;
            _navRecetas.FlatAppearance.MouseOverBackColor = Color.FromArgb(248, 233, 237);
            _navRecetas.FlatStyle = FlatStyle.Flat;
            _navRecetas.Font = new Font("Segoe UI", 10F);
            _navRecetas.ForeColor = Color.FromArgb(111, 105, 107);
            _navRecetas.Location = new Point(3, 263);
            _navRecetas.Name = "_navRecetas";
            _navRecetas.Size = new Size(207, 44);
            _navRecetas.TabIndex = 5;
            _navRecetas.Tag = "Recetas";
            _navRecetas.Text = "Rx   Recetas";
            _navRecetas.TextAlign = ContentAlignment.MiddleLeft;
            _toolTip.SetToolTip(_navRecetas, "Emitir y consultar recetas médicas");
            _navRecetas.UseVisualStyleBackColor = false;
            // 
            // _navAseguradoras
            // 
            _navAseguradoras.BackColor = Color.White;
            _navAseguradoras.Cursor = Cursors.Hand;
            _navAseguradoras.FlatAppearance.BorderSize = 0;
            _navAseguradoras.FlatAppearance.MouseOverBackColor = Color.FromArgb(248, 233, 237);
            _navAseguradoras.FlatStyle = FlatStyle.Flat;
            _navAseguradoras.Font = new Font("Segoe UI", 10F);
            _navAseguradoras.ForeColor = Color.FromArgb(111, 105, 107);
            _navAseguradoras.Location = new Point(3, 313);
            _navAseguradoras.Name = "_navAseguradoras";
            _navAseguradoras.Size = new Size(207, 44);
            _navAseguradoras.TabIndex = 6;
            _navAseguradoras.Tag = "Aseguradoras";
            _navAseguradoras.Text = "♡   Aseguradoras";
            _navAseguradoras.TextAlign = ContentAlignment.MiddleLeft;
            _toolTip.SetToolTip(_navAseguradoras, "Catálogo de aseguradoras médicas");
            _navAseguradoras.UseVisualStyleBackColor = false;
            // 
            // _navConfiguracion
            // 
            _navConfiguracion.BackColor = Color.White;
            _navConfiguracion.Cursor = Cursors.Hand;
            _navConfiguracion.FlatAppearance.BorderSize = 0;
            _navConfiguracion.FlatAppearance.MouseOverBackColor = Color.FromArgb(248, 233, 237);
            _navConfiguracion.FlatStyle = FlatStyle.Flat;
            _navConfiguracion.Font = new Font("Segoe UI", 10F);
            _navConfiguracion.ForeColor = Color.FromArgb(111, 105, 107);
            _navConfiguracion.Location = new Point(3, 363);
            _navConfiguracion.Name = "_navConfiguracion";
            _navConfiguracion.Size = new Size(207, 44);
            _navConfiguracion.TabIndex = 7;
            _navConfiguracion.Tag = "Configuración";
            _navConfiguracion.Text = "⚙   Configuración";
            _navConfiguracion.TextAlign = ContentAlignment.MiddleLeft;
            _toolTip.SetToolTip(_navConfiguracion, "Configuración de usuarios, roles y gastos");
            _navConfiguracion.UseVisualStyleBackColor = false;
            // 
            // _navExpedientes
            // 
            _navExpedientes.BackColor = Color.White;
            _navExpedientes.Cursor = Cursors.Hand;
            _navExpedientes.FlatAppearance.BorderSize = 0;
            _navExpedientes.FlatAppearance.MouseOverBackColor = Color.FromArgb(248, 233, 237);
            _navExpedientes.FlatStyle = FlatStyle.Flat;
            _navExpedientes.Font = new Font("Segoe UI", 10F);
            _navExpedientes.ForeColor = Color.FromArgb(111, 105, 107);
            _navExpedientes.Location = new Point(3, 413);
            _navExpedientes.Name = "_navExpedientes";
            _navExpedientes.Size = new Size(207, 44);
            _navExpedientes.TabIndex = 8;
            _navExpedientes.Tag = "Expedientes";
            _navExpedientes.Text = "▤   Expedientes";
            _navExpedientes.TextAlign = ContentAlignment.MiddleLeft;
            _toolTip.SetToolTip(_navExpedientes, "Consultar expedientes clínicos de pacientes");
            _navExpedientes.UseVisualStyleBackColor = false;
            // 
            // _navInicio
            // 
            _navInicio.BackColor = Color.White;
            _navInicio.Cursor = Cursors.Hand;
            _navInicio.FlatAppearance.BorderSize = 0;
            _navInicio.FlatAppearance.MouseOverBackColor = Color.FromArgb(248, 233, 237);
            _navInicio.FlatStyle = FlatStyle.Flat;
            _navInicio.Font = new Font("Segoe UI", 10F);
            _navInicio.ForeColor = Color.FromArgb(111, 105, 107);
            _navInicio.Location = new Point(3, 463);
            _navInicio.Name = "_navInicio";
            _navInicio.Size = new Size(207, 44);
            _navInicio.TabIndex = 9;
            _navInicio.Tag = "Inicio";
            _navInicio.Text = "◈   Inicio";
            _navInicio.TextAlign = ContentAlignment.MiddleLeft;
            _toolTip.SetToolTip(_navInicio, "Resumen de inicio para paciente");
            _navInicio.UseVisualStyleBackColor = false;
            // 
            // _navMiPortal
            // 
            _navMiPortal.BackColor = Color.White;
            _navMiPortal.Cursor = Cursors.Hand;
            _navMiPortal.FlatAppearance.BorderSize = 0;
            _navMiPortal.FlatAppearance.MouseOverBackColor = Color.FromArgb(248, 233, 237);
            _navMiPortal.FlatStyle = FlatStyle.Flat;
            _navMiPortal.Font = new Font("Segoe UI", 10F);
            _navMiPortal.ForeColor = Color.FromArgb(111, 105, 107);
            _navMiPortal.Location = new Point(3, 513);
            _navMiPortal.Name = "_navMiPortal";
            _navMiPortal.Size = new Size(207, 44);
            _navMiPortal.TabIndex = 10;
            _navMiPortal.Tag = "Mi perfil, citas y recetas";
            _navMiPortal.Text = "○   Mi perfil, citas y recetas";
            _navMiPortal.TextAlign = ContentAlignment.MiddleLeft;
            _toolTip.SetToolTip(_navMiPortal, "Ver mi perfil, citas y recetas");
            _navMiPortal.UseVisualStyleBackColor = false;
            // 
            // _navNotificaciones
            // 
            _navNotificaciones.BackColor = Color.White;
            _navNotificaciones.Cursor = Cursors.Hand;
            _navNotificaciones.FlatAppearance.BorderSize = 0;
            _navNotificaciones.FlatAppearance.MouseOverBackColor = Color.FromArgb(248, 233, 237);
            _navNotificaciones.FlatStyle = FlatStyle.Flat;
            _navNotificaciones.Font = new Font("Segoe UI", 10F);
            _navNotificaciones.ForeColor = Color.FromArgb(111, 105, 107);
            _navNotificaciones.Location = new Point(3, 563);
            _navNotificaciones.Name = "_navNotificaciones";
            _navNotificaciones.Size = new Size(207, 44);
            _navNotificaciones.TabIndex = 11;
            _navNotificaciones.Tag = "Notificaciones";
            _navNotificaciones.Text = "♧   Notificaciones";
            _navNotificaciones.TextAlign = ContentAlignment.MiddleLeft;
            _toolTip.SetToolTip(_navNotificaciones, "Ver notificaciones del sistema");
            _navNotificaciones.UseVisualStyleBackColor = false;
            // 
            // _navNoticias
            // 
            _navNoticias.BackColor = Color.White;
            _navNoticias.Cursor = Cursors.Hand;
            _navNoticias.FlatAppearance.BorderSize = 0;
            _navNoticias.FlatAppearance.MouseOverBackColor = Color.FromArgb(248, 233, 237);
            _navNoticias.FlatStyle = FlatStyle.Flat;
            _navNoticias.Font = new Font("Segoe UI", 10F);
            _navNoticias.ForeColor = Color.FromArgb(111, 105, 107);
            _navNoticias.Location = new Point(3, 613);
            _navNoticias.Name = "_navNoticias";
            _navNoticias.Size = new Size(207, 44);
            _navNoticias.TabIndex = 12;
            _navNoticias.Tag = "Noticias";
            _navNoticias.Text = "N   Noticias";
            _navNoticias.TextAlign = ContentAlignment.MiddleLeft;
            _toolTip.SetToolTip(_navNoticias, "Ver noticias y novedades de la clínica");
            _navNoticias.UseVisualStyleBackColor = false;
            // 
            // _sidebarFooter
            // 
            _sidebarFooter.BackColor = Color.White;
            _sidebarFooter.Controls.Add(_btnSidebarLogout);
            _sidebarFooter.Controls.Add(_footerRoleLabel);
            _sidebarFooter.Controls.Add(_footerUserLabel);
            _sidebarFooter.Dock = DockStyle.Bottom;
            _sidebarFooter.Location = new Point(18, 681);
            _sidebarFooter.Name = "_sidebarFooter";
            _sidebarFooter.Padding = new Padding(0, 8, 0, 0);
            _sidebarFooter.Size = new Size(209, 112);
            _sidebarFooter.TabIndex = 3;
            // 
            // _btnSidebarLogout
            // 
            _btnSidebarLogout.BackColor = Color.White;
            _btnSidebarLogout.Cursor = Cursors.Hand;
            _btnSidebarLogout.Dock = DockStyle.Bottom;
            _btnSidebarLogout.FlatAppearance.BorderSize = 0;
            _btnSidebarLogout.FlatAppearance.MouseOverBackColor = Color.FromArgb(248, 233, 237);
            _btnSidebarLogout.FlatStyle = FlatStyle.Flat;
            _btnSidebarLogout.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _btnSidebarLogout.ForeColor = Color.FromArgb(158, 110, 121);
            _btnSidebarLogout.Location = new Point(0, 72);
            _btnSidebarLogout.Name = "_btnSidebarLogout";
            _btnSidebarLogout.Size = new Size(209, 40);
            _btnSidebarLogout.TabIndex = 2;
            _btnSidebarLogout.Text = "↪   Cerrar sesión";
            _btnSidebarLogout.TextAlign = ContentAlignment.MiddleLeft;
            _toolTip.SetToolTip(_btnSidebarLogout, "Cerrar sesión actual");
            _btnSidebarLogout.UseVisualStyleBackColor = false;
            // 
            // _footerRoleLabel
            // 
            _footerRoleLabel.Dock = DockStyle.Top;
            _footerRoleLabel.Font = new Font("Segoe UI", 9F);
            _footerRoleLabel.ForeColor = Color.FromArgb(158, 154, 155);
            _footerRoleLabel.Location = new Point(0, 34);
            _footerRoleLabel.Name = "_footerRoleLabel";
            _footerRoleLabel.Size = new Size(209, 24);
            _footerRoleLabel.TabIndex = 1;
            _footerRoleLabel.Text = "Rol";
            _footerRoleLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _footerUserLabel
            // 
            _footerUserLabel.Dock = DockStyle.Top;
            _footerUserLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _footerUserLabel.ForeColor = Color.FromArgb(54, 56, 58);
            _footerUserLabel.Location = new Point(0, 8);
            _footerUserLabel.Name = "_footerUserLabel";
            _footerUserLabel.Size = new Size(209, 26);
            _footerUserLabel.TabIndex = 0;
            _footerUserLabel.Text = "Usuario";
            _footerUserLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _menuHeaderLabel
            // 
            _menuHeaderLabel.Dock = DockStyle.Top;
            _menuHeaderLabel.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            _menuHeaderLabel.ForeColor = Color.FromArgb(180, 175, 176);
            _menuHeaderLabel.Location = new Point(18, 98);
            _menuHeaderLabel.Name = "_menuHeaderLabel";
            _menuHeaderLabel.Padding = new Padding(7, 0, 0, 8);
            _menuHeaderLabel.Size = new Size(209, 38);
            _menuHeaderLabel.TabIndex = 1;
            _menuHeaderLabel.Text = "MENÚ PRINCIPAL";
            _menuHeaderLabel.TextAlign = ContentAlignment.BottomLeft;
            // 
            // _brandPanel
            // 
            _brandPanel.BackColor = Color.White;
            _brandPanel.Controls.Add(_brandSubtitle);
            _brandPanel.Controls.Add(_brandTitle);
            _brandPanel.Controls.Add(_brandIcon);
            _brandPanel.Dock = DockStyle.Top;
            _brandPanel.Location = new Point(18, 24);
            _brandPanel.Name = "_brandPanel";
            _brandPanel.Size = new Size(209, 74);
            _brandPanel.TabIndex = 0;
            // 
            // _brandSubtitle
            // 
            _brandSubtitle.Font = new Font("Segoe UI", 7F);
            _brandSubtitle.ForeColor = Color.FromArgb(158, 154, 155);
            _brandSubtitle.Location = new Point(56, 31);
            _brandSubtitle.Name = "_brandSubtitle";
            _brandSubtitle.Size = new Size(165, 20);
            _brandSubtitle.TabIndex = 2;
            _brandSubtitle.Text = "P L A S T I C   S U R G E R Y";
            // 
            // _brandTitle
            // 
            _brandTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            _brandTitle.ForeColor = Color.FromArgb(54, 56, 58);
            _brandTitle.Location = new Point(56, 7);
            _brandTitle.Name = "_brandTitle";
            _brandTitle.Size = new Size(155, 24);
            _brandTitle.TabIndex = 1;
            _brandTitle.Text = "TANGER CLINIC";
            // 
            // _brandIcon
            // 
            _brandIcon.BackColor = Color.FromArgb(158, 110, 121);
            _brandIcon.Font = new Font("Segoe UI Symbol", 19F, FontStyle.Bold);
            _brandIcon.ForeColor = Color.White;
            _brandIcon.Location = new Point(0, 4);
            _brandIcon.Name = "_brandIcon";
            _brandIcon.Size = new Size(44, 44);
            _brandIcon.TabIndex = 0;
            _brandIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            _loginErrorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            _loginErrorProvider.ContainerControl = this;
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1344, 811);
            Controls.Add(_root);
            Font = new Font("Segoe UI", 10F);
            MinimumSize = new Size(1024, 680);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Tanger Clinic | Gestión clínica";
            _root.ResumeLayout(false);
            _loginCard.ResumeLayout(false);
            _loginCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_loginLogo).EndInit();
            ((System.ComponentModel.ISupportInitialize)_loginErrorProvider).EndInit();
            _workspacePanel.ResumeLayout(false);
            _content.ResumeLayout(false);
            _pageContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_mainGrid).EndInit();
            _statusBar.ResumeLayout(false);
            _statusBar.PerformLayout();
            _sectionToolbar.ResumeLayout(false);
            _sectionToolbar.PerformLayout();
            _sectionHeader.ResumeLayout(false);
            _topbar.ResumeLayout(false);
            _rightPanel.ResumeLayout(false);
            _monthNavPanel.ResumeLayout(false);
            _rightProfilePanel.ResumeLayout(false);
            _sidebar.ResumeLayout(false);
            _menuFlow.ResumeLayout(false);
            _sidebarFooter.ResumeLayout(false);
            _brandPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel _root = null!;
        private Panel _loginCard = null!;
        private PictureBox _loginLogo = null!;
        private Label _loginBrand = null!;
        private Label _loginTitle = null!;
        private Label _loginSubtitle = null!;
        private TextBox _loginUser = null!;
        private TextBox _loginPassword = null!;
        private CheckBox _loginShowPassword = null!;
        private Label _loginMessage = null!;
        private Button _btnLogin = null!;
        private Button _btnRegister = null!;
        private Button _btnPublicBooking = null!;
        private Button _btnPublicNews = null!;
        private Panel _workspacePanel = null!;
        private Panel _sidebar = null!;
        private Panel _brandPanel = null!;
        private Label _brandIcon = null!;
        private Label _brandTitle = null!;
        private Label _brandSubtitle = null!;
        private Label _menuHeaderLabel = null!;
        private FlowLayoutPanel _menuFlow = null!;
        private Button _navResumen = null!;
        private Button _navAgenda = null!;
        private Button _navPacientes = null!;
        private Button _navPagos = null!;
        private Button _navCirugias = null!;
        private Button _navRecetas = null!;
        private Button _navAseguradoras = null!;
        private Button _navConfiguracion = null!;
        private Button _navExpedientes = null!;
        private Button _navInicio = null!;
        private Button _navMiPortal = null!;
        private Button _navNotificaciones = null!;
        private Button _navNoticias = null!;
        private Panel _sidebarFooter = null!;
        private Label _footerUserLabel = null!;
        private Label _footerRoleLabel = null!;
        private Button _btnSidebarLogout = null!;
        private Panel _rightPanel = null!;
        private Panel _rightProfilePanel = null!;
        private Label _rightProfile = null!;
        private Button _btnRightProfile = null!;
        private Button _btnRightLogout = null!;
        private Label _calendarTitle = null!;
        private Panel _monthNavPanel = null!;
        private Button _btnPrevMonth = null!;
        private Button _btnNextMonth = null!;
        private TableLayoutPanel _calendarTable = null!;
        private Label _appointmentHeaderLabel = null!;
        private FlowLayoutPanel _dayAppointments = null!;
        private Panel _content = null!;
        private Panel _topbar = null!;
        private Label _topbarWelcome = null!;
        private Label _topbarCaption = null!;
        private Panel _pageContent = null!;
        private Panel _sectionHeader = null!;
        private Label _sectionTitle = null!;
        private Label _sectionSubtitle = null!;
        private Button _btnHeaderNewSurgery = null!;
        private Panel _sectionToolbar = null!;
        private TextBox _sectionSearch = null!;
        private ComboBox _surgeryStatusFilterCombo = null!;
        private Button _btnToolbarRefresh = null!;
        private Button _btnToolbarAddProcedure = null!;
        private Button _btnToolbarAdd = null!;
        private Panel _statusBar = null!;
        private Label _status = null!;
        private Panel _dynamicHost = null!;
        private DataGridView _mainGrid = null!;
        private ToolTip _toolTip = null!;
        private ErrorProvider _loginErrorProvider = null!;
    }
}
