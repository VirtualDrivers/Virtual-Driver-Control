namespace VDD_Control
{
    partial class XMLEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XMLEditor));
            numericUpDown1 = new NumericUpDown();
            monitorCountLabel = new Label();
            label2 = new Label();
            listBox1 = new ListBox();
            label3 = new Label();
            dataGridView1 = new DataGridView();
            width = new DataGridViewTextBoxColumn();
            height = new DataGridViewTextBoxColumn();
            RefreshRate = new DataGridViewTextBoxColumn();
            label5 = new Label();
            checkedListBox1 = new CheckedListBox();
            label6 = new Label();
            comboBox1 = new ComboBox();
            themeForm1 = new ReaLTaiizor.Forms.ThemeForm();
            chatButtonRight5 = new ReaLTaiizor.Controls.ChatButtonRight();
            chatButtonRight6 = new ReaLTaiizor.Controls.ChatButtonRight();
            chatButtonRight3 = new ReaLTaiizor.Controls.ChatButtonRight();
            chatButtonRight4 = new ReaLTaiizor.Controls.ChatButtonRight();
            chatButtonRight2 = new ReaLTaiizor.Controls.ChatButtonRight();
            chatButtonRight1 = new ReaLTaiizor.Controls.ChatButtonRight();
            foreverClose1 = new ReaLTaiizor.Controls.ForeverClose();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            themeForm1.SuspendLayout();
            SuspendLayout();
            // 
            // numericUpDown1
            // 
            numericUpDown1.BackColor = Color.FromArgb(32, 32, 32);
            numericUpDown1.BorderStyle = BorderStyle.FixedSingle;
            numericUpDown1.ForeColor = SystemColors.Window;
            numericUpDown1.Location = new Point(132, 74);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(325, 21);
            numericUpDown1.TabIndex = 0;
            // 
            // monitorCountLabel
            // 
            monitorCountLabel.AutoSize = true;
            monitorCountLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            monitorCountLabel.ForeColor = SystemColors.Window;
            monitorCountLabel.Location = new Point(16, 74);
            monitorCountLabel.Name = "monitorCountLabel";
            monitorCountLabel.Size = new Size(110, 19);
            monitorCountLabel.TabIndex = 1;
            monitorCountLabel.Text = "Monitor Count:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            label2.ForeColor = SystemColors.Window;
            label2.Location = new Point(16, 108);
            label2.Name = "label2";
            label2.Size = new Size(138, 19);
            label2.TabIndex = 3;
            label2.Text = "Select Default GPU:";
            // 
            // listBox1
            // 
            listBox1.BackColor = Color.FromArgb(32, 32, 32);
            listBox1.ForeColor = SystemColors.Window;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(173, 137);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(284, 94);
            listBox1.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            label3.ForeColor = SystemColors.Window;
            label3.Location = new Point(16, 137);
            label3.Name = "label3";
            label3.Size = new Size(151, 19);
            label3.TabIndex = 5;
            label3.Text = "Global Refresh Rates:";
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { width, height, RefreshRate });
            dataGridView1.Location = new Point(111, 266);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(346, 137);
            dataGridView1.TabIndex = 9;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // width
            // 
            width.HeaderText = "Width";
            width.Name = "width";
            // 
            // height
            // 
            height.HeaderText = "Height";
            height.Name = "height";
            // 
            // RefreshRate
            // 
            RefreshRate.HeaderText = "Refresh Rate";
            RefreshRate.Name = "RefreshRate";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            label5.ForeColor = SystemColors.Window;
            label5.Location = new Point(16, 266);
            label5.Name = "label5";
            label5.Size = new Size(89, 19);
            label5.TabIndex = 11;
            label5.Text = "Resolutions:";
            // 
            // checkedListBox1
            // 
            checkedListBox1.BackColor = Color.FromArgb(32, 32, 32);
            checkedListBox1.ForeColor = SystemColors.Window;
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Items.AddRange(new object[] { "Custom EDID", "Prevent EDID Spoofing", "EDID CEA Override", "Hardware Cursor", "SDR 10 Bit", "HDR+", "User-Mode Logging", "Dev-Mode Logging" });
            checkedListBox1.Location = new Point(87, 438);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new Size(370, 148);
            checkedListBox1.TabIndex = 12;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            label6.ForeColor = SystemColors.Window;
            label6.Location = new Point(16, 438);
            label6.Name = "label6";
            label6.Size = new Size(65, 19);
            label6.TabIndex = 15;
            label6.Text = "Options:";
            // 
            // comboBox1
            // 
            comboBox1.BackColor = Color.FromArgb(32, 32, 32);
            comboBox1.FlatStyle = FlatStyle.Popup;
            comboBox1.ForeColor = SystemColors.Window;
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "(Automatic)" });
            comboBox1.Location = new Point(160, 107);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(297, 23);
            comboBox1.TabIndex = 18;
            // 
            // themeForm1
            // 
            themeForm1.BackColor = Color.FromArgb(32, 41, 50);
            themeForm1.Controls.Add(chatButtonRight5);
            themeForm1.Controls.Add(chatButtonRight6);
            themeForm1.Controls.Add(chatButtonRight3);
            themeForm1.Controls.Add(chatButtonRight4);
            themeForm1.Controls.Add(chatButtonRight2);
            themeForm1.Controls.Add(chatButtonRight1);
            themeForm1.Controls.Add(foreverClose1);
            themeForm1.Controls.Add(comboBox1);
            themeForm1.Controls.Add(listBox1);
            themeForm1.Controls.Add(numericUpDown1);
            themeForm1.Controls.Add(monitorCountLabel);
            themeForm1.Controls.Add(label6);
            themeForm1.Controls.Add(label2);
            themeForm1.Controls.Add(label3);
            themeForm1.Controls.Add(checkedListBox1);
            themeForm1.Controls.Add(label5);
            themeForm1.Controls.Add(dataGridView1);
            themeForm1.Dock = DockStyle.Fill;
            themeForm1.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point);
            themeForm1.Image = (Image)resources.GetObject("themeForm1.Image");
            themeForm1.Location = new Point(0, 0);
            themeForm1.Name = "themeForm1";
            themeForm1.Padding = new Padding(10, 70, 10, 9);
            themeForm1.RoundCorners = true;
            themeForm1.Sizable = true;
            themeForm1.Size = new Size(470, 620);
            themeForm1.SmartBounds = true;
            themeForm1.StartPosition = FormStartPosition.WindowsDefaultLocation;
            themeForm1.TabIndex = 19;
            themeForm1.Text = "Virtual Display Driver XML Editor";
            themeForm1.Click += themeForm1_Click;
            // 
            // chatButtonRight5
            // 
            chatButtonRight5.BackColor = Color.Transparent;
            chatButtonRight5.Font = new Font("Consolas", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            chatButtonRight5.ForeColor = Color.LightGray;
            chatButtonRight5.Image = null;
            chatButtonRight5.ImageAlign = ContentAlignment.MiddleLeft;
            chatButtonRight5.InactiveColorA = Color.FromArgb(0, 120, 0);
            chatButtonRight5.InactiveColorB = Color.FromArgb(0, 120, 0);
            chatButtonRight5.Location = new Point(370, 592);
            chatButtonRight5.Name = "chatButtonRight5";
            chatButtonRight5.PressedColorA = Color.FromArgb(0, 80, 0);
            chatButtonRight5.PressedColorB = Color.FromArgb(0, 80, 0);
            chatButtonRight5.PressedContourColorA = Color.FromArgb(0, 80, 0);
            chatButtonRight5.PressedContourColorB = Color.FromArgb(0, 80, 0);
            chatButtonRight5.Size = new Size(87, 22);
            chatButtonRight5.TabIndex = 40;
            chatButtonRight5.Text = "Save XML";
            chatButtonRight5.TextAlignment = StringAlignment.Center;
            // 
            // chatButtonRight6
            // 
            chatButtonRight6.BackColor = Color.Transparent;
            chatButtonRight6.Font = new Font("Consolas", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            chatButtonRight6.ForeColor = Color.LightGray;
            chatButtonRight6.Image = null;
            chatButtonRight6.ImageAlign = ContentAlignment.MiddleLeft;
            chatButtonRight6.InactiveColorA = Color.FromArgb(0, 120, 0);
            chatButtonRight6.InactiveColorB = Color.FromArgb(0, 120, 0);
            chatButtonRight6.Location = new Point(202, 592);
            chatButtonRight6.Name = "chatButtonRight6";
            chatButtonRight6.PressedColorA = Color.FromArgb(0, 80, 0);
            chatButtonRight6.PressedColorB = Color.FromArgb(0, 80, 0);
            chatButtonRight6.PressedContourColorA = Color.FromArgb(0, 80, 0);
            chatButtonRight6.PressedContourColorB = Color.FromArgb(0, 80, 0);
            chatButtonRight6.Size = new Size(162, 22);
            chatButtonRight6.TabIndex = 39;
            chatButtonRight6.Text = "Close Without Saving";
            chatButtonRight6.TextAlignment = StringAlignment.Center;
            // 
            // chatButtonRight3
            // 
            chatButtonRight3.BackColor = Color.Transparent;
            chatButtonRight3.Font = new Font("Consolas", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            chatButtonRight3.ForeColor = Color.LightGray;
            chatButtonRight3.Image = null;
            chatButtonRight3.ImageAlign = ContentAlignment.MiddleLeft;
            chatButtonRight3.InactiveColorA = Color.FromArgb(0, 120, 0);
            chatButtonRight3.InactiveColorB = Color.FromArgb(0, 120, 0);
            chatButtonRight3.Location = new Point(396, 410);
            chatButtonRight3.Name = "chatButtonRight3";
            chatButtonRight3.PressedColorA = Color.FromArgb(0, 80, 0);
            chatButtonRight3.PressedColorB = Color.FromArgb(0, 80, 0);
            chatButtonRight3.PressedContourColorA = Color.FromArgb(0, 80, 0);
            chatButtonRight3.PressedContourColorB = Color.FromArgb(0, 80, 0);
            chatButtonRight3.Size = new Size(61, 22);
            chatButtonRight3.TabIndex = 38;
            chatButtonRight3.Text = "Remove";
            chatButtonRight3.TextAlignment = StringAlignment.Center;
            // 
            // chatButtonRight4
            // 
            chatButtonRight4.BackColor = Color.Transparent;
            chatButtonRight4.Font = new Font("Consolas", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            chatButtonRight4.ForeColor = Color.LightGray;
            chatButtonRight4.Image = null;
            chatButtonRight4.ImageAlign = ContentAlignment.MiddleLeft;
            chatButtonRight4.InactiveColorA = Color.FromArgb(0, 120, 0);
            chatButtonRight4.InactiveColorB = Color.FromArgb(0, 120, 0);
            chatButtonRight4.Location = new Point(325, 410);
            chatButtonRight4.Name = "chatButtonRight4";
            chatButtonRight4.PressedColorA = Color.FromArgb(0, 80, 0);
            chatButtonRight4.PressedColorB = Color.FromArgb(0, 80, 0);
            chatButtonRight4.PressedContourColorA = Color.FromArgb(0, 80, 0);
            chatButtonRight4.PressedContourColorB = Color.FromArgb(0, 80, 0);
            chatButtonRight4.Size = new Size(61, 22);
            chatButtonRight4.TabIndex = 37;
            chatButtonRight4.Text = "Add";
            chatButtonRight4.TextAlignment = StringAlignment.Center;
            // 
            // chatButtonRight2
            // 
            chatButtonRight2.BackColor = Color.Transparent;
            chatButtonRight2.Font = new Font("Consolas", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            chatButtonRight2.ForeColor = Color.LightGray;
            chatButtonRight2.Image = null;
            chatButtonRight2.ImageAlign = ContentAlignment.MiddleLeft;
            chatButtonRight2.InactiveColorA = Color.FromArgb(0, 120, 0);
            chatButtonRight2.InactiveColorB = Color.FromArgb(0, 120, 0);
            chatButtonRight2.Location = new Point(396, 237);
            chatButtonRight2.Name = "chatButtonRight2";
            chatButtonRight2.PressedColorA = Color.FromArgb(0, 80, 0);
            chatButtonRight2.PressedColorB = Color.FromArgb(0, 80, 0);
            chatButtonRight2.PressedContourColorA = Color.FromArgb(0, 80, 0);
            chatButtonRight2.PressedContourColorB = Color.FromArgb(0, 80, 0);
            chatButtonRight2.Size = new Size(61, 22);
            chatButtonRight2.TabIndex = 36;
            chatButtonRight2.Text = "Remove";
            chatButtonRight2.TextAlignment = StringAlignment.Center;
            // 
            // chatButtonRight1
            // 
            chatButtonRight1.BackColor = Color.Transparent;
            chatButtonRight1.Font = new Font("Consolas", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            chatButtonRight1.ForeColor = Color.LightGray;
            chatButtonRight1.Image = null;
            chatButtonRight1.ImageAlign = ContentAlignment.MiddleLeft;
            chatButtonRight1.InactiveColorA = Color.FromArgb(0, 120, 0);
            chatButtonRight1.InactiveColorB = Color.FromArgb(0, 120, 0);
            chatButtonRight1.Location = new Point(325, 237);
            chatButtonRight1.Name = "chatButtonRight1";
            chatButtonRight1.PressedColorA = Color.FromArgb(0, 80, 0);
            chatButtonRight1.PressedColorB = Color.FromArgb(0, 80, 0);
            chatButtonRight1.PressedContourColorA = Color.FromArgb(0, 80, 0);
            chatButtonRight1.PressedContourColorB = Color.FromArgb(0, 80, 0);
            chatButtonRight1.Size = new Size(61, 22);
            chatButtonRight1.TabIndex = 35;
            chatButtonRight1.Text = "Add";
            chatButtonRight1.TextAlignment = StringAlignment.Center;
            // 
            // foreverClose1
            // 
            foreverClose1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            foreverClose1.BackColor = Color.White;
            foreverClose1.BaseColor = Color.FromArgb(45, 47, 49);
            foreverClose1.DefaultLocation = true;
            foreverClose1.DownColor = Color.FromArgb(30, 0, 0, 0);
            foreverClose1.Font = new Font("Marlett", 10F, FontStyle.Regular, GraphicsUnit.Point);
            foreverClose1.Location = new Point(440, 16);
            foreverClose1.Name = "foreverClose1";
            foreverClose1.OverColor = Color.FromArgb(30, 255, 255, 255);
            foreverClose1.Size = new Size(18, 18);
            foreverClose1.TabIndex = 19;
            foreverClose1.Text = "foreverClose1";
            foreverClose1.TextColor = Color.FromArgb(243, 243, 243);
            // 
            // XMLEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(470, 620);
            Controls.Add(themeForm1);
            Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.None;
            MinimumSize = new Size(261, 61);
            Name = "XMLEditor";
            Text = "Virtual Display Driver XML Editor";
            TransparencyKey = Color.Fuchsia;
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            themeForm1.ResumeLayout(false);
            themeForm1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private NumericUpDown numericUpDown1;
        private Label monitorCountLabel;
        private Label label2;
        private ListBox listBox1;
        private Label label3;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn width;
        private DataGridViewTextBoxColumn height;
        private DataGridViewTextBoxColumn RefreshRate;
        private Label label5;
        private CheckedListBox checkedListBox1;
        private Label label6;
        private ComboBox comboBox1;
        private ReaLTaiizor.Forms.ThemeForm themeForm1;
        private ReaLTaiizor.Controls.ForeverClose foreverClose1;
        private ReaLTaiizor.Controls.ChatButtonRight chatButtonRight5;
        private ReaLTaiizor.Controls.ChatButtonRight chatButtonRight6;
        private ReaLTaiizor.Controls.ChatButtonRight chatButtonRight3;
        private ReaLTaiizor.Controls.ChatButtonRight chatButtonRight4;
        private ReaLTaiizor.Controls.ChatButtonRight chatButtonRight2;
        private ReaLTaiizor.Controls.ChatButtonRight chatButtonRight1;
    }
}