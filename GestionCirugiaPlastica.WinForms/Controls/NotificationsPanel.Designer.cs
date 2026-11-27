namespace GestionCirugiaPlastica.WinForms.Controls
{
    partial class NotificationsPanel
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

        #region Component Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _toolbar = new FlowLayoutPanel();
            _btnRefresh = new Button();
            _btnMarkAllRead = new Button();
            _btnMarkSelectedRead = new Button();
            _grid = new DataGridView();
            _status = new Label();
            _toolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
            SuspendLayout();
            // 
            // _toolbar
            // 
            _toolbar.Controls.Add(_btnRefresh);
            _toolbar.Controls.Add(_btnMarkAllRead);
            _toolbar.Controls.Add(_btnMarkSelectedRead);
            _toolbar.Dock = DockStyle.Top;
            _toolbar.Location = new Point(16, 16);
            _toolbar.Name = "_toolbar";
            _toolbar.Size = new Size(768, 44);
            _toolbar.TabIndex = 0;
            _toolbar.WrapContents = false;
            // 
            // _btnRefresh
            // 
            _btnRefresh.AutoSize = true;
            _btnRefresh.BackColor = Color.White;
            _btnRefresh.Cursor = Cursors.Hand;
            _btnRefresh.FlatStyle = FlatStyle.Flat;
            _btnRefresh.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _btnRefresh.ForeColor = Color.FromArgb(158, 110, 121);
            _btnRefresh.Location = new Point(0, 0);
            _btnRefresh.Margin = new Padding(0, 0, 8, 6);
            _btnRefresh.Name = "_btnRefresh";
            _btnRefresh.Size = new Size(86, 36);
            _btnRefresh.TabIndex = 0;
            _btnRefresh.Text = "Actualizar";
            _btnRefresh.UseVisualStyleBackColor = false;
            // 
            // _btnMarkAllRead
            // 
            _btnMarkAllRead.AutoSize = true;
            _btnMarkAllRead.BackColor = Color.FromArgb(158, 110, 121);
            _btnMarkAllRead.Cursor = Cursors.Hand;
            _btnMarkAllRead.FlatStyle = FlatStyle.Flat;
            _btnMarkAllRead.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _btnMarkAllRead.ForeColor = Color.White;
            _btnMarkAllRead.Location = new Point(94, 0);
            _btnMarkAllRead.Margin = new Padding(0, 0, 8, 6);
            _btnMarkAllRead.Name = "_btnMarkAllRead";
            _btnMarkAllRead.Size = new Size(193, 36);
            _btnMarkAllRead.TabIndex = 1;
            _btnMarkAllRead.Text = "Marcar todas como leídas";
            _btnMarkAllRead.UseVisualStyleBackColor = false;
            // 
            // _btnMarkSelectedRead
            // 
            _btnMarkSelectedRead.AutoSize = true;
            _btnMarkSelectedRead.BackColor = Color.White;
            _btnMarkSelectedRead.Cursor = Cursors.Hand;
            _btnMarkSelectedRead.FlatStyle = FlatStyle.Flat;
            _btnMarkSelectedRead.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _btnMarkSelectedRead.ForeColor = Color.FromArgb(158, 110, 121);
            _btnMarkSelectedRead.Location = new Point(295, 0);
            _btnMarkSelectedRead.Margin = new Padding(0, 0, 8, 6);
            _btnMarkSelectedRead.Name = "_btnMarkSelectedRead";
            _btnMarkSelectedRead.Size = new Size(216, 36);
            _btnMarkSelectedRead.TabIndex = 2;
            _btnMarkSelectedRead.Text = "Marcar seleccionada como leída";
            _btnMarkSelectedRead.UseVisualStyleBackColor = false;
            // 
            // _grid
            // 
            _grid.AllowUserToAddRows = false;
            _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _grid.BackgroundColor = Color.White;
            _grid.BorderStyle = BorderStyle.None;
            _grid.Dock = DockStyle.Fill;
            _grid.Location = new Point(16, 60);
            _grid.MultiSelect = false;
            _grid.Name = "_grid";
            _grid.ReadOnly = true;
            _grid.RowTemplate.Height = 42;
            _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grid.Size = new Size(768, 412);
            _grid.TabIndex = 1;
            // 
            // _status
            // 
            _status.Dock = DockStyle.Bottom;
            _status.ForeColor = Color.FromArgb(158, 110, 121);
            _status.Location = new Point(16, 472);
            _status.Name = "_status";
            _status.Size = new Size(768, 32);
            _status.TabIndex = 2;
            // 
            // NotificationsPanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(_grid);
            Controls.Add(_status);
            Controls.Add(_toolbar);
            Name = "NotificationsPanel";
            Padding = new Padding(16);
            Size = new Size(800, 520);
            _toolbar.ResumeLayout(false);
            _toolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel _toolbar = null!;
        private Button _btnRefresh = null!;
        private Button _btnMarkAllRead = null!;
        private Button _btnMarkSelectedRead = null!;
        private DataGridView _grid = null!;
        private Label _status = null!;
    }
}
