using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VDD_Control;


namespace VDD_Control
{
    public partial class Form2 : Form
    {
        private VDDSettings currentSettings;

        public Form2(VDDSettings settings)
        {
            InitializeComponent();
            this.currentSettings = settings;
            UpdateFormWithSettings();
        }

        private void UpdateFormWithSettings()
        {
            // Update Monitor Count
            numericUpDown1.Value = currentSettings.Count;

            // Update Friendly Name (Assuming you're using comboBox1 for this)
            comboBox1.Text = currentSettings.Friendlyname;

            // Update Global Refresh Rates
            listBox1.DataSource = currentSettings.G_refresh_rate;

            // Update Resolutions in DataGridView
            dataGridView1.Rows.Clear();
            foreach (var resolution in currentSettings.Resolutions)
            {
                dataGridView1.Rows.Add(resolution.Width, resolution.Height, resolution.Refresh_rate);
            }

            // Update Options in CheckedListBox
            checkedListBox1.SetItemChecked(0, currentSettings.CustomEdid);
            checkedListBox1.SetItemChecked(1, currentSettings.PreventSpoof);
            checkedListBox1.SetItemChecked(2, currentSettings.EdidCeaOverride);
            checkedListBox1.SetItemChecked(3, currentSettings.HardwareCursor);
            checkedListBox1.SetItemChecked(4, currentSettings.SDR10bit);
            checkedListBox1.SetItemChecked(5, currentSettings.HDRPlus);
            checkedListBox1.SetItemChecked(6, currentSettings.Logging);
            checkedListBox1.SetItemChecked(7, currentSettings.DebugLogging);
        }

        // You might want to add event handlers for the buttons to handle adding/removing items
        private void chatButtonRight1_Click(object sender, EventArgs e)
        {
            // Add resolution logic
        }

        private void chatButtonRight2_Click(object sender, EventArgs e)
        {
            // Remove resolution logic
        }

        private void chatButtonRight4_Click(object sender, EventArgs e)
        {
            // Add global refresh rate logic
        }

        private void chatButtonRight3_Click(object sender, EventArgs e)
        {
            // Remove global refresh rate logic
        }

        private void chatButtonRight5_Click(object sender, EventArgs e)
        {
            // Save XML logic
            currentSettings.Count = (int)numericUpDown1.Value;
            currentSettings.Friendlyname = comboBox1.Text;
            currentSettings.G_refresh_rate = (List<string>)listBox1.DataSource;
            currentSettings.Resolutions.Clear();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    currentSettings.Resolutions.Add(new VDDSettings.Resolution
                    {
                        Width = int.Parse(row.Cells["width"].Value?.ToString() ?? "0"),
                        Height = int.Parse(row.Cells["height"].Value?.ToString() ?? "0"),
                        Refresh_rate = float.Parse(row.Cells["RefreshRate"].Value?.ToString() ?? "0")
                    });
                }
            }
            currentSettings.CustomEdid = checkedListBox1.GetItemChecked(0);
            currentSettings.PreventSpoof = checkedListBox1.GetItemChecked(1);
            currentSettings.EdidCeaOverride = checkedListBox1.GetItemChecked(2);
            currentSettings.HardwareCursor = checkedListBox1.GetItemChecked(3);
            currentSettings.SDR10bit = checkedListBox1.GetItemChecked(4);
            currentSettings.HDRPlus = checkedListBox1.GetItemChecked(5);
            currentSettings.Logging = checkedListBox1.GetItemChecked(6);
            currentSettings.DebugLogging = checkedListBox1.GetItemChecked(7);

            // Call SaveToXml method here
            // currentSettings.SaveToXml("path/to/your/xmlfile.xml");
        }

        private void chatButtonRight6_Click(object sender, EventArgs e)
        {
            // Close without saving logic
            this.Close();
        }
 
        private void themeForm1_Click(object sender, EventArgs e)
        {

        }
    }
}
