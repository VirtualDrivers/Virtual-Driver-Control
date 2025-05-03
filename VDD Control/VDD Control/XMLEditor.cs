using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace VDD_Control
{
    public partial class XMLEditor : Form
    {
        private XMLController xmlController;
        private string xmlFilePath;

        public XMLEditor()
        {
            InitializeComponent();
        }

        public XMLEditor(string filePath) : this()
        {
            xmlFilePath = filePath;
            LoadXmlData();
            
            // Set up button event handlers
            chatButtonRight5.Click += SaveXML_Click;
            chatButtonRight6.Click += CloseWithoutSaving_Click;
            chatButtonRight1.Click += AddResolution_Click;
            chatButtonRight2.Click += RemoveResolution_Click;
            chatButtonRight3.Click += RemoveRefreshRate_Click;
            chatButtonRight4.Click += AddRefreshRate_Click;
            foreverClose1.Click += ForeverClose1_Click;
        }

        private void LoadXmlData()
        {
            try
            {
                xmlController = new XMLController(xmlFilePath);
                
                // Populate monitor count
                numericUpDown1.Value = xmlController.Count;
                
                // Populate GPU name
                comboBox1.Text = xmlController.Friendlyname;
                
                // Populate refresh rates
                listBox1.Items.Clear();
                foreach (var rate in xmlController.G_refresh_rate)
                {
                    listBox1.Items.Add(rate);
                }
                
                // Populate resolutions grid
                dataGridView1.Rows.Clear();
                foreach (var resolution in xmlController.Resolutions)
                {
                    dataGridView1.Rows.Add(resolution.Width, resolution.Height, resolution.Refresh_rate);
                }
                
                // Set options checkboxes
                SetOptionCheckbox("Custom EDID", xmlController.CustomEdid);
                SetOptionCheckbox("Prevent EDID Spoofing", xmlController.PreventSpoof);
                SetOptionCheckbox("EDID CEA Override", xmlController.EdidCeaOverride);
                SetOptionCheckbox("Hardware Cursor", xmlController.HardwareCursor);
                SetOptionCheckbox("SDR 10 Bit", xmlController.SDR10bit);
                SetOptionCheckbox("HDR+", xmlController.HDRPlus);
                SetOptionCheckbox("User-Mode Logging", xmlController.Logging);
                SetOptionCheckbox("Dev-Mode Logging", xmlController.DebugLogging);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading XML: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void SetOptionCheckbox(string optionText, bool value)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.Items[i].ToString() == optionText)
                {
                    checkedListBox1.SetItemChecked(i, value);
                    break;
                }
            }
        }

        private void SaveXmlData()
        {
            try
            {
                // Update XML controller with form data
                xmlController.Count = (int)numericUpDown1.Value;
                xmlController.Friendlyname = comboBox1.Text;
                
                // Update refresh rates
                xmlController.G_refresh_rate.Clear();
                foreach (var item in listBox1.Items)
                {
                    xmlController.G_refresh_rate.Add(item.ToString());
                }
                
                // Update resolutions
                xmlController.Resolutions.Clear();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow && row.Cells[0].Value != null && row.Cells[1].Value != null && row.Cells[2].Value != null)
                    {
                        var resolution = new XMLController.Resolution
                        {
                            Width = Convert.ToInt32(row.Cells[0].Value),
                            Height = Convert.ToInt32(row.Cells[1].Value),
                            Refresh_rate = Convert.ToDouble(row.Cells[2].Value)
                        };
                        xmlController.Resolutions.Add(resolution);
                    }
                }
                
                // Update options
                xmlController.CustomEdid = IsOptionChecked("Custom EDID");
                xmlController.PreventSpoof = IsOptionChecked("Prevent EDID Spoofing");
                xmlController.EdidCeaOverride = IsOptionChecked("EDID CEA Override");
                xmlController.HardwareCursor = IsOptionChecked("Hardware Cursor");
                xmlController.SDR10bit = IsOptionChecked("SDR 10 Bit");
                xmlController.HDRPlus = IsOptionChecked("HDR+");
                xmlController.Logging = IsOptionChecked("User-Mode Logging");
                xmlController.DebugLogging = IsOptionChecked("Dev-Mode Logging");
                
                // Save to XML file
                xmlController.SaveToXml(xmlFilePath);
                MessageBox.Show("XML saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving XML: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private bool IsOptionChecked(string optionText)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.Items[i].ToString() == optionText)
                {
                    return checkedListBox1.GetItemChecked(i);
                }
            }
            return false;
        }

        private void SaveXML_Click(object sender, EventArgs e)
        {
            SaveXmlData();
            this.Close();
        }

        private void CloseWithoutSaving_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void ForeverClose1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void AddResolution_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add(1920, 1080, 60.0);
        }
        
        private void RemoveResolution_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 && !dataGridView1.SelectedRows[0].IsNewRow)
            {
                dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
            }
            else if (dataGridView1.SelectedCells.Count > 0)
            {
                int rowIndex = dataGridView1.SelectedCells[0].RowIndex;
                if (!dataGridView1.Rows[rowIndex].IsNewRow)
                {
                    dataGridView1.Rows.RemoveAt(rowIndex);
                }
            }
        }
        
        private void AddRefreshRate_Click(object sender, EventArgs e)
        {
            string newRate = "60";
            listBox1.Items.Add(newRate);
        }
        
        private void RemoveRefreshRate_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Cell content click handler
        }

        private void themeForm1_Click(object sender, EventArgs e)
        {
            // Theme form click handler
        }
    }
}
