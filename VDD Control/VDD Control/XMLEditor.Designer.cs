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
            themeForm = new ReaLTaiizor.Forms.ThemeForm();
            panelFooter = new Panel();
            buttonClose = new ReaLTaiizor.Controls.Button();
            buttonSave = new ReaLTaiizor.Controls.Button();
            buttonViewXml = new ReaLTaiizor.Controls.Button();
            tabControl = new TabControl();
            tabGeneral = new TabPage();
            groupBox1 = new GroupBox();
            labelGPU = new Label();
            comboBoxGPU = new ComboBox();
            labelMonitorCount = new Label();
            numericMonitorCount = new NumericUpDown();
            tabResolutions = new TabPage();
            panelResolutionButtons = new Panel();
            buttonRemoveResolution = new ReaLTaiizor.Controls.Button();
            buttonAddResolution = new ReaLTaiizor.Controls.Button();
            groupBoxResolutions = new GroupBox();
            dataGridResolutions = new DataGridView();
            colWidth = new DataGridViewTextBoxColumn();
            colHeight = new DataGridViewTextBoxColumn();
            colRefreshRate = new DataGridViewTextBoxColumn();
            tabRefreshRates = new TabPage();
            panelRefreshButtons = new Panel();
            buttonRemoveRefreshRate = new ReaLTaiizor.Controls.Button();
            buttonAddRefreshRate = new ReaLTaiizor.Controls.Button();
            numericAddRefresh = new NumericUpDown();
            groupBoxRefreshRates = new GroupBox();
            listBoxRefreshRates = new ListBox();
            tabOptions = new TabPage();
            groupBoxOptions = new GroupBox();
            checkedListOptions = new CheckedListBox();
            foreverClose = new ReaLTaiizor.Controls.ForeverClose();
            ((System.ComponentModel.ISupportInitialize)numericMonitorCount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridResolutions).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericAddRefresh).BeginInit();
            themeForm.SuspendLayout();
            panelFooter.SuspendLayout();
            tabControl.SuspendLayout();
            tabGeneral.SuspendLayout();
            groupBox1.SuspendLayout();
            tabResolutions.SuspendLayout();
            panelResolutionButtons.SuspendLayout();
            groupBoxResolutions.SuspendLayout();
            tabRefreshRates.SuspendLayout();
            panelRefreshButtons.SuspendLayout();
            groupBoxRefreshRates.SuspendLayout();
            tabOptions.SuspendLayout();
            groupBoxOptions.SuspendLayout();
            SuspendLayout();
            // 
            // themeForm
            // 
            themeForm.BackColor = Color.FromArgb(32, 41, 50);
            themeForm.Controls.Add(tabControl);
            themeForm.Controls.Add(panelFooter);
            themeForm.Controls.Add(foreverClose);
            themeForm.Dock = DockStyle.Fill;
            themeForm.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point);
            // Don't use resources object to set image - will be set in code
            themeForm.Image = null;
            themeForm.Location = new Point(0, 0);
            themeForm.Name = "themeForm";
            themeForm.Padding = new Padding(10, 70, 10, 9);
            themeForm.RoundCorners = true;
            themeForm.Sizable = true;
            themeForm.Size = new Size(600, 650);
            themeForm.SmartBounds = true;
            themeForm.StartPosition = FormStartPosition.WindowsDefaultLocation;
            themeForm.TabIndex = 19;
            themeForm.Text = "Virtual Display Driver XML Editor";
            // 
            // panelFooter
            // 
            panelFooter.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelFooter.BackColor = Color.FromArgb(40, 49, 60);
            panelFooter.Controls.Add(buttonClose);
            panelFooter.Controls.Add(buttonSave);
            panelFooter.Controls.Add(buttonViewXml);
            panelFooter.Location = new Point(10, 580);
            panelFooter.Margin = new Padding(0);
            panelFooter.Name = "panelFooter";
            panelFooter.Size = new Size(580, 61);
            panelFooter.TabIndex = 21;
            // 
            // buttonClose
            // 
            buttonClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonClose.BackColor = Color.Transparent;
            buttonClose.BorderColor = Color.FromArgb(180, 180, 180);
            buttonClose.Cursor = Cursors.Hand;
            buttonClose.EnteredBorderColor = Color.FromArgb(120, 120, 120);
            buttonClose.EnteredColor = Color.FromArgb(60, 60, 60);
            buttonClose.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            buttonClose.Image = null;
            buttonClose.ImageAlign = ContentAlignment.MiddleLeft;
            buttonClose.InactiveColor = Color.FromArgb(45, 47, 49);
            buttonClose.Location = new Point(290, 18);
            buttonClose.Name = "buttonClose";
            buttonClose.PressedBorderColor = Color.FromArgb(120, 120, 120);
            buttonClose.PressedColor = Color.FromArgb(80, 80, 80);
            buttonClose.Size = new Size(130, 30);
            buttonClose.TabIndex = 2;
            buttonClose.Text = "Close Without Saving";
            buttonClose.TextAlignment = StringAlignment.Center;
            // 
            // buttonSave
            // 
            buttonSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonSave.BackColor = Color.Transparent;
            buttonSave.BorderColor = Color.FromArgb(32, 192, 64);
            buttonSave.Cursor = Cursors.Hand;
            buttonSave.EnteredBorderColor = Color.FromArgb(32, 192, 64);
            buttonSave.EnteredColor = Color.FromArgb(0, 120, 0);
            buttonSave.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            buttonSave.Image = null;
            buttonSave.ImageAlign = ContentAlignment.MiddleLeft;
            buttonSave.InactiveColor = Color.FromArgb(0, 100, 0);
            buttonSave.Location = new Point(426, 18);
            buttonSave.Name = "buttonSave";
            buttonSave.PressedBorderColor = Color.FromArgb(32, 192, 64);
            buttonSave.PressedColor = Color.FromArgb(0, 80, 0);
            buttonSave.Size = new Size(140, 30);
            buttonSave.TabIndex = 1;
            buttonSave.Text = "Save Changes";
            buttonSave.TextAlignment = StringAlignment.Center;
            // 
            // buttonViewXml
            // 
            buttonViewXml.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonViewXml.BackColor = Color.Transparent;
            buttonViewXml.BorderColor = Color.FromArgb(100, 100, 100);
            buttonViewXml.Cursor = Cursors.Hand;
            buttonViewXml.EnteredBorderColor = Color.FromArgb(180, 180, 180);
            buttonViewXml.EnteredColor = Color.FromArgb(60, 60, 60);
            buttonViewXml.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            buttonViewXml.Image = null;
            buttonViewXml.ImageAlign = ContentAlignment.MiddleLeft;
            buttonViewXml.InactiveColor = Color.FromArgb(45, 47, 49);
            buttonViewXml.Location = new Point(13, 18);
            buttonViewXml.Name = "buttonViewXml";
            buttonViewXml.PressedBorderColor = Color.FromArgb(180, 180, 180);
            buttonViewXml.PressedColor = Color.FromArgb(80, 80, 80);
            buttonViewXml.Size = new Size(140, 30);
            buttonViewXml.TabIndex = 0;
            buttonViewXml.Text = "View Raw XML";
            buttonViewXml.TextAlignment = StringAlignment.Center;
            // 
            // tabControl
            // 
            tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl.Controls.Add(tabGeneral);
            tabControl.Controls.Add(tabResolutions);
            tabControl.Controls.Add(tabRefreshRates);
            tabControl.Controls.Add(tabOptions);
            tabControl.Location = new Point(13, 73);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(577, 504);
            tabControl.TabIndex = 20;
            // 
            // tabGeneral
            // 
            tabGeneral.BackColor = Color.FromArgb(40, 49, 60);
            tabGeneral.Controls.Add(groupBox1);
            tabGeneral.Location = new Point(4, 24);
            tabGeneral.Name = "tabGeneral";
            tabGeneral.Padding = new Padding(3);
            tabGeneral.Size = new Size(569, 476);
            tabGeneral.TabIndex = 0;
            tabGeneral.Text = "General Settings";
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(labelGPU);
            groupBox1.Controls.Add(comboBoxGPU);
            groupBox1.Controls.Add(labelMonitorCount);
            groupBox1.Controls.Add(numericMonitorCount);
            groupBox1.ForeColor = Color.White;
            groupBox1.Location = new Point(16, 20);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(536, 130);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Basic Configuration";
            // 
            // labelGPU
            // 
            labelGPU.AutoSize = true;
            labelGPU.ForeColor = Color.White;
            labelGPU.Location = new Point(18, 74);
            labelGPU.Name = "labelGPU";
            labelGPU.Size = new Size(118, 15);
            labelGPU.TabIndex = 3;
            labelGPU.Text = "Select Default GPU:";
            // 
            // comboBoxGPU
            // 
            comboBoxGPU.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            comboBoxGPU.BackColor = Color.FromArgb(32, 32, 32);
            comboBoxGPU.FlatStyle = FlatStyle.Flat;
            comboBoxGPU.ForeColor = Color.White;
            comboBoxGPU.FormattingEnabled = true;
            comboBoxGPU.Items.AddRange(new object[] { "(Automatic)" });
            comboBoxGPU.Location = new Point(142, 71);
            comboBoxGPU.Name = "comboBoxGPU";
            comboBoxGPU.Size = new Size(370, 23);
            comboBoxGPU.TabIndex = 2;
            // 
            // labelMonitorCount
            // 
            labelMonitorCount.AutoSize = true;
            labelMonitorCount.ForeColor = Color.White;
            labelMonitorCount.Location = new Point(18, 35);
            labelMonitorCount.Name = "labelMonitorCount";
            labelMonitorCount.Size = new Size(88, 15);
            labelMonitorCount.TabIndex = 1;
            labelMonitorCount.Text = "Monitor Count:";
            // 
            // numericMonitorCount
            // 
            numericMonitorCount.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            numericMonitorCount.BackColor = Color.FromArgb(32, 32, 32);
            numericMonitorCount.BorderStyle = BorderStyle.FixedSingle;
            numericMonitorCount.ForeColor = Color.White;
            numericMonitorCount.Location = new Point(142, 33);
            numericMonitorCount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericMonitorCount.Name = "numericMonitorCount";
            numericMonitorCount.Size = new Size(370, 21);
            numericMonitorCount.TabIndex = 0;
            numericMonitorCount.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // tabResolutions
            // 
            tabResolutions.BackColor = Color.FromArgb(40, 49, 60);
            tabResolutions.Controls.Add(panelResolutionButtons);
            tabResolutions.Controls.Add(groupBoxResolutions);
            tabResolutions.Location = new Point(4, 24);
            tabResolutions.Name = "tabResolutions";
            tabResolutions.Padding = new Padding(3);
            tabResolutions.Size = new Size(569, 476);
            tabResolutions.TabIndex = 1;
            tabResolutions.Text = "Resolutions";
            // 
            // panelResolutionButtons
            // 
            panelResolutionButtons.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelResolutionButtons.Controls.Add(buttonRemoveResolution);
            panelResolutionButtons.Controls.Add(buttonAddResolution);
            panelResolutionButtons.Location = new Point(16, 426);
            panelResolutionButtons.Name = "panelResolutionButtons";
            panelResolutionButtons.Size = new Size(536, 42);
            panelResolutionButtons.TabIndex = 1;
            // 
            // buttonRemoveResolution
            // 
            buttonRemoveResolution.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonRemoveResolution.BackColor = Color.Transparent;
            buttonRemoveResolution.BorderColor = Color.FromArgb(192, 32, 32);
            buttonRemoveResolution.Cursor = Cursors.Hand;
            buttonRemoveResolution.EnteredBorderColor = Color.FromArgb(192, 64, 64);
            buttonRemoveResolution.EnteredColor = Color.FromArgb(120, 20, 20);
            buttonRemoveResolution.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            buttonRemoveResolution.Image = null;
            buttonRemoveResolution.ImageAlign = ContentAlignment.MiddleLeft;
            buttonRemoveResolution.InactiveColor = Color.FromArgb(100, 20, 20);
            buttonRemoveResolution.Location = new Point(366, 6);
            buttonRemoveResolution.Name = "buttonRemoveResolution";
            buttonRemoveResolution.PressedBorderColor = Color.FromArgb(192, 32, 32);
            buttonRemoveResolution.PressedColor = Color.FromArgb(80, 10, 10);
            buttonRemoveResolution.Size = new Size(155, 30);
            buttonRemoveResolution.TabIndex = 1;
            buttonRemoveResolution.Text = "Remove Resolution";
            buttonRemoveResolution.TextAlignment = StringAlignment.Center;
            // 
            // buttonAddResolution
            // 
            buttonAddResolution.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonAddResolution.BackColor = Color.Transparent;
            buttonAddResolution.BorderColor = Color.FromArgb(32, 192, 64);
            buttonAddResolution.Cursor = Cursors.Hand;
            buttonAddResolution.EnteredBorderColor = Color.FromArgb(64, 192, 64);
            buttonAddResolution.EnteredColor = Color.FromArgb(0, 120, 0);
            buttonAddResolution.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            buttonAddResolution.Image = null;
            buttonAddResolution.ImageAlign = ContentAlignment.MiddleLeft;
            buttonAddResolution.InactiveColor = Color.FromArgb(0, 100, 0);
            buttonAddResolution.Location = new Point(14, 6);
            buttonAddResolution.Name = "buttonAddResolution";
            buttonAddResolution.PressedBorderColor = Color.FromArgb(32, 192, 64);
            buttonAddResolution.PressedColor = Color.FromArgb(0, 80, 0);
            buttonAddResolution.Size = new Size(155, 30);
            buttonAddResolution.TabIndex = 0;
            buttonAddResolution.Text = "Add Resolution";
            buttonAddResolution.TextAlignment = StringAlignment.Center;
            // 
            // groupBoxResolutions
            // 
            groupBoxResolutions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxResolutions.Controls.Add(dataGridResolutions);
            groupBoxResolutions.ForeColor = Color.White;
            groupBoxResolutions.Location = new Point(16, 20);
            groupBoxResolutions.Name = "groupBoxResolutions";
            groupBoxResolutions.Size = new Size(536, 400);
            groupBoxResolutions.TabIndex = 0;
            groupBoxResolutions.TabStop = false;
            groupBoxResolutions.Text = "Supported Resolutions";
            // 
            // dataGridResolutions
            // 
            dataGridResolutions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridResolutions.BackgroundColor = Color.FromArgb(32, 42, 52);
            dataGridResolutions.BorderStyle = BorderStyle.None;
            dataGridResolutions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridResolutions.Columns.AddRange(new DataGridViewColumn[] { colWidth, colHeight, colRefreshRate });
            dataGridResolutions.Location = new Point(14, 30);
            dataGridResolutions.Name = "dataGridResolutions";
            dataGridResolutions.RowTemplate.Height = 25;
            dataGridResolutions.Size = new Size(507, 354);
            dataGridResolutions.TabIndex = 0;
            // 
            // colWidth
            // 
            colWidth.HeaderText = "Width";
            colWidth.Name = "colWidth";
            // 
            // colHeight
            // 
            colHeight.HeaderText = "Height";
            colHeight.Name = "colHeight";
            // 
            // colRefreshRate
            // 
            colRefreshRate.HeaderText = "Refresh Rate";
            colRefreshRate.Name = "colRefreshRate";
            // 
            // tabRefreshRates
            // 
            tabRefreshRates.BackColor = Color.FromArgb(40, 49, 60);
            tabRefreshRates.Controls.Add(panelRefreshButtons);
            tabRefreshRates.Controls.Add(groupBoxRefreshRates);
            tabRefreshRates.Location = new Point(4, 24);
            tabRefreshRates.Name = "tabRefreshRates";
            tabRefreshRates.Padding = new Padding(3);
            tabRefreshRates.Size = new Size(569, 476);
            tabRefreshRates.TabIndex = 2;
            tabRefreshRates.Text = "Refresh Rates";
            // 
            // panelRefreshButtons
            // 
            panelRefreshButtons.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelRefreshButtons.Controls.Add(buttonRemoveRefreshRate);
            panelRefreshButtons.Controls.Add(buttonAddRefreshRate);
            panelRefreshButtons.Controls.Add(numericAddRefresh);
            panelRefreshButtons.Location = new Point(16, 426);
            panelRefreshButtons.Name = "panelRefreshButtons";
            panelRefreshButtons.Size = new Size(536, 42);
            panelRefreshButtons.TabIndex = 1;
            // 
            // buttonRemoveRefreshRate
            // 
            buttonRemoveRefreshRate.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonRemoveRefreshRate.BackColor = Color.Transparent;
            buttonRemoveRefreshRate.BorderColor = Color.FromArgb(192, 32, 32);
            buttonRemoveRefreshRate.Cursor = Cursors.Hand;
            buttonRemoveRefreshRate.EnteredBorderColor = Color.FromArgb(192, 64, 64);
            buttonRemoveRefreshRate.EnteredColor = Color.FromArgb(120, 20, 20);
            buttonRemoveRefreshRate.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            buttonRemoveRefreshRate.Image = null;
            buttonRemoveRefreshRate.ImageAlign = ContentAlignment.MiddleLeft;
            buttonRemoveRefreshRate.InactiveColor = Color.FromArgb(100, 20, 20);
            buttonRemoveRefreshRate.Location = new Point(366, 6);
            buttonRemoveRefreshRate.Name = "buttonRemoveRefreshRate";
            buttonRemoveRefreshRate.PressedBorderColor = Color.FromArgb(192, 32, 32);
            buttonRemoveRefreshRate.PressedColor = Color.FromArgb(80, 10, 10);
            buttonRemoveRefreshRate.Size = new Size(155, 30);
            buttonRemoveRefreshRate.TabIndex = 2;
            buttonRemoveRefreshRate.Text = "Remove Selected";
            buttonRemoveRefreshRate.TextAlignment = StringAlignment.Center;
            // 
            // buttonAddRefreshRate
            // 
            buttonAddRefreshRate.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonAddRefreshRate.BackColor = Color.Transparent;
            buttonAddRefreshRate.BorderColor = Color.FromArgb(32, 192, 64);
            buttonAddRefreshRate.Cursor = Cursors.Hand;
            buttonAddRefreshRate.EnteredBorderColor = Color.FromArgb(64, 192, 64);
            buttonAddRefreshRate.EnteredColor = Color.FromArgb(0, 120, 0);
            buttonAddRefreshRate.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            buttonAddRefreshRate.Image = null;
            buttonAddRefreshRate.ImageAlign = ContentAlignment.MiddleLeft;
            buttonAddRefreshRate.InactiveColor = Color.FromArgb(0, 100, 0);
            buttonAddRefreshRate.Location = new Point(167, 6);
            buttonAddRefreshRate.Name = "buttonAddRefreshRate";
            buttonAddRefreshRate.PressedBorderColor = Color.FromArgb(32, 192, 64);
            buttonAddRefreshRate.PressedColor = Color.FromArgb(0, 80, 0);
            buttonAddRefreshRate.Size = new Size(120, 30);
            buttonAddRefreshRate.TabIndex = 1;
            buttonAddRefreshRate.Text = "Add Rate";
            buttonAddRefreshRate.TextAlignment = StringAlignment.Center;
            // 
            // numericAddRefresh
            // 
            numericAddRefresh.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            numericAddRefresh.BackColor = Color.FromArgb(32, 32, 32);
            numericAddRefresh.BorderStyle = BorderStyle.FixedSingle;
            numericAddRefresh.ForeColor = Color.White;
            numericAddRefresh.Location = new Point(14, 11);
            numericAddRefresh.Maximum = new decimal(new int[] { 240, 0, 0, 0 });
            numericAddRefresh.Minimum = new decimal(new int[] { 30, 0, 0, 0 });
            numericAddRefresh.Name = "numericAddRefresh";
            numericAddRefresh.Size = new Size(147, 21);
            numericAddRefresh.TabIndex = 0;
            numericAddRefresh.Value = new decimal(new int[] { 60, 0, 0, 0 });
            // 
            // groupBoxRefreshRates
            // 
            groupBoxRefreshRates.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxRefreshRates.Controls.Add(listBoxRefreshRates);
            groupBoxRefreshRates.ForeColor = Color.White;
            groupBoxRefreshRates.Location = new Point(16, 20);
            groupBoxRefreshRates.Name = "groupBoxRefreshRates";
            groupBoxRefreshRates.Size = new Size(536, 400);
            groupBoxRefreshRates.TabIndex = 0;
            groupBoxRefreshRates.TabStop = false;
            groupBoxRefreshRates.Text = "Global Refresh Rates";
            // 
            // listBoxRefreshRates
            // 
            listBoxRefreshRates.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listBoxRefreshRates.BackColor = Color.FromArgb(32, 32, 32);
            listBoxRefreshRates.BorderStyle = BorderStyle.None;
            listBoxRefreshRates.ForeColor = Color.White;
            listBoxRefreshRates.FormattingEnabled = true;
            listBoxRefreshRates.ItemHeight = 15;
            listBoxRefreshRates.Location = new Point(14, 30);
            listBoxRefreshRates.Name = "listBoxRefreshRates";
            listBoxRefreshRates.Size = new Size(507, 345);
            listBoxRefreshRates.TabIndex = 0;
            // 
            // tabOptions
            // 
            tabOptions.BackColor = Color.FromArgb(40, 49, 60);
            tabOptions.Controls.Add(groupBoxOptions);
            tabOptions.Location = new Point(4, 24);
            tabOptions.Name = "tabOptions";
            tabOptions.Padding = new Padding(3);
            tabOptions.Size = new Size(569, 476);
            tabOptions.TabIndex = 3;
            tabOptions.Text = "Options";
            // 
            // groupBoxOptions
            // 
            groupBoxOptions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxOptions.Controls.Add(checkedListOptions);
            groupBoxOptions.ForeColor = Color.White;
            groupBoxOptions.Location = new Point(16, 20);
            groupBoxOptions.Name = "groupBoxOptions";
            groupBoxOptions.Size = new Size(536, 439);
            groupBoxOptions.TabIndex = 0;
            groupBoxOptions.TabStop = false;
            groupBoxOptions.Text = "Driver Options";
            // 
            // checkedListOptions
            // 
            checkedListOptions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            checkedListOptions.BackColor = Color.FromArgb(32, 32, 32);
            checkedListOptions.BorderStyle = BorderStyle.None;
            checkedListOptions.ForeColor = Color.White;
            checkedListOptions.FormattingEnabled = true;
            checkedListOptions.Items.AddRange(new object[] { "Custom EDID", "Prevent EDID Spoofing", "EDID CEA Override", "Hardware Cursor", "SDR 10 Bit", "HDR+", "User-Mode Logging", "Dev-Mode Logging" });
            checkedListOptions.Location = new Point(14, 30);
            checkedListOptions.Name = "checkedListOptions";
            checkedListOptions.Size = new Size(507, 392);
            checkedListOptions.TabIndex = 0;
            // 
            // foreverClose
            // 
            foreverClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            foreverClose.BackColor = Color.White;
            foreverClose.BaseColor = Color.FromArgb(45, 47, 49);
            foreverClose.DefaultLocation = true;
            foreverClose.DownColor = Color.FromArgb(30, 0, 0, 0);
            foreverClose.Font = new Font("Marlett", 10F, FontStyle.Regular, GraphicsUnit.Point);
            foreverClose.Location = new Point(570, 16);
            foreverClose.Name = "foreverClose";
            foreverClose.OverColor = Color.FromArgb(30, 255, 255, 255);
            foreverClose.Size = new Size(18, 18);
            foreverClose.TabIndex = 19;
            foreverClose.Text = "foreverClose";
            foreverClose.TextColor = Color.FromArgb(243, 243, 243);
            // 
            // XMLEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(600, 650);
            Controls.Add(themeForm);
            Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.None;
            MinimumSize = new Size(500, 400);
            Name = "XMLEditor";
            Text = "Virtual Display Driver XML Editor";
            TransparencyKey = Color.Fuchsia;
            ((System.ComponentModel.ISupportInitialize)numericMonitorCount).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridResolutions).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericAddRefresh).EndInit();
            themeForm.ResumeLayout(false);
            panelFooter.ResumeLayout(false);
            tabControl.ResumeLayout(false);
            tabGeneral.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabResolutions.ResumeLayout(false);
            panelResolutionButtons.ResumeLayout(false);
            groupBoxResolutions.ResumeLayout(false);
            tabRefreshRates.ResumeLayout(false);
            panelRefreshButtons.ResumeLayout(false);
            groupBoxRefreshRates.ResumeLayout(false);
            tabOptions.ResumeLayout(false);
            groupBoxOptions.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ReaLTaiizor.Forms.ThemeForm themeForm;
        private ReaLTaiizor.Controls.ForeverClose foreverClose;
        private TabControl tabControl;
        private TabPage tabGeneral;
        private TabPage tabResolutions;
        private TabPage tabRefreshRates;
        private TabPage tabOptions;
        private Panel panelFooter;
        private ReaLTaiizor.Controls.Button buttonSave;
        private ReaLTaiizor.Controls.Button buttonViewXml;
        private ReaLTaiizor.Controls.Button buttonClose;
        private GroupBox groupBox1;
        private Label labelGPU;
        private ComboBox comboBoxGPU;
        private Label labelMonitorCount;
        private NumericUpDown numericMonitorCount;
        private GroupBox groupBoxResolutions;
        private DataGridView dataGridResolutions;
        private Panel panelResolutionButtons;
        private ReaLTaiizor.Controls.Button buttonRemoveResolution;
        private ReaLTaiizor.Controls.Button buttonAddResolution;
        private DataGridViewTextBoxColumn colWidth;
        private DataGridViewTextBoxColumn colHeight;
        private DataGridViewTextBoxColumn colRefreshRate;
        private GroupBox groupBoxRefreshRates;
        private Panel panelRefreshButtons;
        private ReaLTaiizor.Controls.Button buttonRemoveRefreshRate;
        private ReaLTaiizor.Controls.Button buttonAddRefreshRate;
        private NumericUpDown numericAddRefresh;
        private ListBox listBoxRefreshRates;
        private GroupBox groupBoxOptions;
        private CheckedListBox checkedListOptions;
    }
}