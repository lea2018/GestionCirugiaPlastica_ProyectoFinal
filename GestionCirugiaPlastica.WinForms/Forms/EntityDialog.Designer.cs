namespace GestionCirugiaPlastica.WinForms
{
    partial class EntityDialog
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
            _fields = new FlowLayoutPanel();
            _buttonsPanel = new Panel();
            _btnSave = new Button();
            _btnCancel = new Button();
            _buttonsPanel.SuspendLayout();
            SuspendLayout();
            // 
            // _fields
            // 
            _fields.AutoScroll = true;
            _fields.BackColor = Color.White;
            _fields.Dock = DockStyle.Fill;
            _fields.FlowDirection = FlowDirection.TopDown;
            _fields.Location = new Point(0, 0);
            _fields.Name = "_fields";
            _fields.Padding = new Padding(24);
            _fields.Size = new Size(544, 587);
            _fields.TabIndex = 0;
            _fields.WrapContents = false;
            // 
            // _buttonsPanel
            // 
            _buttonsPanel.BackColor = Color.FromArgb(246, 244, 243);
            _buttonsPanel.Controls.Add(_btnSave);
            _buttonsPanel.Controls.Add(_btnCancel);
            _buttonsPanel.Dock = DockStyle.Bottom;
            _buttonsPanel.Location = new Point(0, 587);
            _buttonsPanel.Name = "_buttonsPanel";
            _buttonsPanel.Padding = new Padding(20, 10, 20, 10);
            _buttonsPanel.Size = new Size(544, 64);
            _buttonsPanel.TabIndex = 1;
            // 
            // _btnSave
            // 
            _btnSave.BackColor = Color.FromArgb(158, 110, 121);
            _btnSave.DialogResult = DialogResult.OK;
            _btnSave.Dock = DockStyle.Right;
            _btnSave.FlatStyle = FlatStyle.Flat;
            _btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _btnSave.ForeColor = Color.White;
            _btnSave.Location = new Point(273, 10);
            _btnSave.Name = "_btnSave";
            _btnSave.Size = new Size(128, 44);
            _btnSave.TabIndex = 0;
            _btnSave.Text = "Guardar";
            _btnSave.UseVisualStyleBackColor = false;
            // 
            // _btnCancel
            // 
            _btnCancel.DialogResult = DialogResult.Cancel;
            _btnCancel.Dock = DockStyle.Right;
            _btnCancel.FlatStyle = FlatStyle.Flat;
            _btnCancel.Font = new Font("Segoe UI", 10F);
            _btnCancel.Location = new Point(401, 10);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.Size = new Size(123, 44);
            _btnCancel.TabIndex = 1;
            _btnCancel.Text = "Cancelar";
            // 
            // EntityDialog
            // 
            AcceptButton = _btnSave;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            CancelButton = _btnCancel;
            ClientSize = new Size(544, 651);
            Controls.Add(_fields);
            Controls.Add(_buttonsPanel);
            Font = new Font("Segoe UI", 10F);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(520, 480);
            Name = "EntityDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Detalle";
            _buttonsPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        protected FlowLayoutPanel _fields = null!;
        protected Panel _buttonsPanel = null!;
        protected Button _btnSave = null!;
        protected Button _btnCancel = null!;
    }
}
