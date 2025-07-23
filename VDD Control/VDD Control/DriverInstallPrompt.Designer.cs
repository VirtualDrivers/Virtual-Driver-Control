namespace VDD_Control
{
    partial class DriverInstallPrompt
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.themeForm = new ReaLTaiizor.Forms.ThemeForm();
            this.labelMessage = new Label();
            this.buttonInstall = new ReaLTaiizor.Controls.Button();
            this.buttonDontInstall = new ReaLTaiizor.Controls.Button();
            this.checkBoxDontShow = new CheckBox();
            this.pictureBoxIcon = new PictureBox();
            this.themeForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // themeForm
            // 
            this.themeForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(41)))), ((int)(((byte)(50)))));
            this.themeForm.Controls.Add(this.pictureBoxIcon);
            this.themeForm.Controls.Add(this.checkBoxDontShow);
            this.themeForm.Controls.Add(this.buttonDontInstall);
            this.themeForm.Controls.Add(this.buttonInstall);
            this.themeForm.Controls.Add(this.labelMessage);
            this.themeForm.Dock = DockStyle.Fill;
            this.themeForm.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.themeForm.Image = null;
            this.themeForm.Location = new Point(0, 0);
            this.themeForm.Name = "themeForm";
            this.themeForm.Padding = new Padding(10, 50, 10, 9);
            this.themeForm.RoundCorners = true;
            this.themeForm.Sizable = false;
            this.themeForm.Size = new Size(450, 200);
            this.themeForm.SmartBounds = true;
            this.themeForm.StartPosition = FormStartPosition.Manual;
            this.themeForm.TabIndex = 0;
            this.themeForm.Text = "Virtual Display Driver Not Found";
            // 
            // labelMessage
            // 
            this.labelMessage.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.labelMessage.BackColor = Color.Transparent;
            this.labelMessage.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            this.labelMessage.ForeColor = Color.White;
            this.labelMessage.Location = new Point(70, 65);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new Size(360, 60);
            this.labelMessage.TabIndex = 0;
            this.labelMessage.Text = "The Virtual Display Driver was not detected on your system. Would you like to install it now?";
            this.labelMessage.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // buttonInstall
            // 
            this.buttonInstall.BackColor = Color.Transparent;
            this.buttonInstall.BorderColor = Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(34)))), ((int)(((byte)(37)))));
            this.buttonInstall.Cursor = Cursors.Hand;
            this.buttonInstall.EnteredBorderColor = Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(37)))), ((int)(((byte)(37)))));
            this.buttonInstall.EnteredColor = Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(34)))), ((int)(((byte)(37)))));
            this.buttonInstall.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.buttonInstall.Image = null;
            this.buttonInstall.ImageAlign = ContentAlignment.MiddleLeft;
            this.buttonInstall.InactiveColor = Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(34)))), ((int)(((byte)(37)))));
            this.buttonInstall.Location = new Point(245, 140);
            this.buttonInstall.Name = "buttonInstall";
            this.buttonInstall.PressedBorderColor = Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(37)))), ((int)(((byte)(37)))));
            this.buttonInstall.PressedColor = Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(37)))), ((int)(((byte)(37)))));
            this.buttonInstall.Size = new Size(90, 30);
            this.buttonInstall.TabIndex = 1;
            this.buttonInstall.Text = "Install";
            this.buttonInstall.TextAlignment = StringAlignment.Center;
            this.buttonInstall.Click += new EventHandler(this.buttonInstall_Click);
            // 
            // buttonDontInstall
            // 
            this.buttonDontInstall.BackColor = Color.Transparent;
            this.buttonDontInstall.BorderColor = Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(34)))), ((int)(((byte)(37)))));
            this.buttonDontInstall.Cursor = Cursors.Hand;
            this.buttonDontInstall.EnteredBorderColor = Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(37)))), ((int)(((byte)(37)))));
            this.buttonDontInstall.EnteredColor = Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(34)))), ((int)(((byte)(37)))));
            this.buttonDontInstall.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.buttonDontInstall.Image = null;
            this.buttonDontInstall.ImageAlign = ContentAlignment.MiddleLeft;
            this.buttonDontInstall.InactiveColor = Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(34)))), ((int)(((byte)(37)))));
            this.buttonDontInstall.Location = new Point(345, 140);
            this.buttonDontInstall.Name = "buttonDontInstall";
            this.buttonDontInstall.PressedBorderColor = Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(37)))), ((int)(((byte)(37)))));
            this.buttonDontInstall.PressedColor = Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(37)))), ((int)(((byte)(37)))));
            this.buttonDontInstall.Size = new Size(90, 30);
            this.buttonDontInstall.TabIndex = 2;
            this.buttonDontInstall.Text = "Don't Install";
            this.buttonDontInstall.TextAlignment = StringAlignment.Center;
            this.buttonDontInstall.Click += new EventHandler(this.buttonDontInstall_Click);
            // 
            // checkBoxDontShow
            // 
            this.checkBoxDontShow.AutoSize = true;
            this.checkBoxDontShow.BackColor = Color.Transparent;
            this.checkBoxDontShow.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.checkBoxDontShow.ForeColor = Color.White;
            this.checkBoxDontShow.Location = new Point(25, 147);
            this.checkBoxDontShow.Name = "checkBoxDontShow";
            this.checkBoxDontShow.Size = new Size(154, 19);
            this.checkBoxDontShow.TabIndex = 3;
            this.checkBoxDontShow.Text = "Don't show this again";
            this.checkBoxDontShow.UseVisualStyleBackColor = false;
            // 
            // pictureBoxIcon
            // 
            this.pictureBoxIcon.BackColor = Color.Transparent;
            this.pictureBoxIcon.Image = SystemIcons.Question.ToBitmap();
            this.pictureBoxIcon.Location = new Point(25, 65);
            this.pictureBoxIcon.Name = "pictureBoxIcon";
            this.pictureBoxIcon.Size = new Size(32, 32);
            this.pictureBoxIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBoxIcon.TabIndex = 4;
            this.pictureBoxIcon.TabStop = false;
            // 
            // DriverInstallPrompt
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(450, 200);
            this.Controls.Add(this.themeForm);
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DriverInstallPrompt";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Virtual Display Driver Not Found";
            this.TopMost = true;
            this.themeForm.ResumeLayout(false);
            this.themeForm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private ReaLTaiizor.Forms.ThemeForm themeForm;
        private Label labelMessage;
        private ReaLTaiizor.Controls.Button buttonInstall;
        private ReaLTaiizor.Controls.Button buttonDontInstall;
        private CheckBox checkBoxDontShow;
        private PictureBox pictureBoxIcon;
    }
}