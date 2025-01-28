namespace VDD_Control
{
    partial class Form2
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
            numericUpDown1 = new NumericUpDown();
            label1 = new Label();
            label2 = new Label();
            listBox1 = new ListBox();
            label3 = new Label();
            button2 = new Button();
            button1 = new Button();
            dataGridView1 = new DataGridView();
            label4 = new Label();
            width = new DataGridViewTextBoxColumn();
            height = new DataGridViewTextBoxColumn();
            RefreshRate = new DataGridViewTextBoxColumn();
            label5 = new Label();
            checkedListBox1 = new CheckedListBox();
            button3 = new Button();
            button4 = new Button();
            label6 = new Label();
            button5 = new Button();
            button6 = new Button();
            comboBox1 = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(128, 45);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(325, 23);
            numericUpDown1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = SystemColors.WindowFrame;
            label1.Location = new Point(12, 45);
            label1.Name = "label1";
            label1.Size = new Size(110, 19);
            label1.TabIndex = 1;
            label1.Text = "Monitor Count:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            label2.ForeColor = SystemColors.WindowFrame;
            label2.Location = new Point(12, 79);
            label2.Name = "label2";
            label2.Size = new Size(138, 19);
            label2.TabIndex = 3;
            label2.Text = "Select Default GPU:";
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(169, 108);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(284, 94);
            listBox1.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            label3.ForeColor = SystemColors.WindowFrame;
            label3.Location = new Point(12, 108);
            label3.Name = "label3";
            label3.Size = new Size(151, 19);
            label3.TabIndex = 5;
            label3.Text = "Global Refresh Rates:";
            // 
            // button2
            // 
            button2.Location = new Point(388, 208);
            button2.Name = "button2";
            button2.Size = new Size(65, 23);
            button2.TabIndex = 7;
            button2.Text = "Remove";
            button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Location = new Point(333, 208);
            button1.Name = "button1";
            button1.Size = new Size(49, 23);
            button1.TabIndex = 8;
            button1.Text = "Add";
            button1.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { width, height, RefreshRate });
            dataGridView1.Location = new Point(107, 237);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(346, 137);
            dataGridView1.TabIndex = 9;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(12, 9);
            label4.Name = "label4";
            label4.Size = new Size(261, 21);
            label4.TabIndex = 10;
            label4.Text = "Virtual Display Driver XML Editor";
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
            label5.ForeColor = SystemColors.WindowFrame;
            label5.Location = new Point(12, 237);
            label5.Name = "label5";
            label5.Size = new Size(89, 19);
            label5.TabIndex = 11;
            label5.Text = "Resolutions:";
            // 
            // checkedListBox1
            // 
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Items.AddRange(new object[] { "Custom EDID", "Prevent EDID Spoofing", "EDID CEA Override", "Hardware Cursor", "SDR 10 Bit", "HDR+", "User-Mode Logging", "Dev-Mode Logging" });
            checkedListBox1.Location = new Point(83, 409);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new Size(370, 148);
            checkedListBox1.TabIndex = 12;
            // 
            // button3
            // 
            button3.Location = new Point(333, 380);
            button3.Name = "button3";
            button3.Size = new Size(49, 23);
            button3.TabIndex = 14;
            button3.Text = "Add";
            button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Location = new Point(388, 380);
            button4.Name = "button4";
            button4.Size = new Size(65, 23);
            button4.TabIndex = 13;
            button4.Text = "Remove";
            button4.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            label6.ForeColor = SystemColors.WindowFrame;
            label6.Location = new Point(12, 409);
            label6.Name = "label6";
            label6.Size = new Size(65, 19);
            label6.TabIndex = 15;
            label6.Text = "Options:";
            // 
            // button5
            // 
            button5.Location = new Point(376, 563);
            button5.Name = "button5";
            button5.Size = new Size(77, 23);
            button5.TabIndex = 16;
            button5.Text = "Save XML";
            button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            button6.Location = new Point(235, 563);
            button6.Name = "button6";
            button6.Size = new Size(135, 23);
            button6.TabIndex = 17;
            button6.Text = "Close Without Saving";
            button6.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "(Automatic)" });
            comboBox1.Location = new Point(156, 78);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(297, 23);
            comboBox1.TabIndex = 18;
            comboBox1.Text = "(Automatic)";
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(470, 599);
            Controls.Add(comboBox1);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(label6);
            Controls.Add(button3);
            Controls.Add(button4);
            Controls.Add(checkedListBox1);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(dataGridView1);
            Controls.Add(button1);
            Controls.Add(button2);
            Controls.Add(label3);
            Controls.Add(listBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(numericUpDown1);
            Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            Name = "Form2";
            Text = "Virtual Display Driver XML Editor";
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NumericUpDown numericUpDown1;
        private Label label1;
        private Label label2;
        private ListBox listBox1;
        private Label label3;
        private Button button2;
        private Button button1;
        private DataGridView dataGridView1;
        private Label label4;
        private DataGridViewTextBoxColumn width;
        private DataGridViewTextBoxColumn height;
        private DataGridViewTextBoxColumn RefreshRate;
        private Label label5;
        private CheckedListBox checkedListBox1;
        private Button button3;
        private Button button4;
        private Label label6;
        private Button button5;
        private Button button6;
        private ComboBox comboBox1;
    }
}