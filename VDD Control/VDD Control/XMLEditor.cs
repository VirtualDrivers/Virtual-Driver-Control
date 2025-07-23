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
        private ToolTip optionsTooltip;

        public XMLEditor()
        {
            InitializeComponent();
            
            // Create and configure tooltips for options
            optionsTooltip = new ToolTip
            {
                AutoPopDelay = 10000,
                InitialDelay = 500,
                ReshowDelay = 200,
                ShowAlways = true
            };
        }

        public XMLEditor(string filePath) : this()
        {
            xmlFilePath = filePath;
            
            // Set the ThemeForm image from project resources
            try {
                if (Properties.Resources.ResourceManager.GetObject("Virtual_Display_Driver") is System.Drawing.Image img) {
                    themeForm.Image = img;
                } else {
                    // Fallback - create a blank image
                    themeForm.Image = new Bitmap(16, 16);
                }
            } catch {
                // If any exception occurs, create a fallback image
                themeForm.Image = new Bitmap(16, 16);
            }
            
            // Set up button event handlers
            buttonSave.Click += SaveXML_Click;
            buttonClose.Click += CloseWithoutSaving_Click;
            buttonAddResolution.Click += AddResolution_Click;
            buttonRemoveResolution.Click += RemoveResolution_Click;
            buttonRemoveRefreshRate.Click += RemoveRefreshRate_Click;
            buttonAddRefreshRate.Click += AddRefreshRate_Click;
            buttonViewXml.Click += ViewRawXml_Click;
            
            // Fix for ForeverClose control - handle it directly
            // Explicitly handle close button click to avoid null reference exception
            if (foreverClose != null && foreverClose.Parent != null)
            {
                foreverClose.Click += (s, e) => this.Close();
            }
            else
            {
                // Fallback: ensure control is properly initialized after form load
                this.Load += (s, e) =>
                {
                    if (foreverClose != null && foreverClose.Parent != null)
                    {
                        foreverClose.Click -= (sender, args) => this.Close(); // Remove any existing handler
                        foreverClose.Click += (sender, args) => this.Close();
                    }
                };
            }
            
            // Add handler for the form's FormClosing event
            this.FormClosing += (s, e) => 
            {
                // If closing caused by user (not by the application itself)
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    // Show confirmation dialog
                    DialogResult result = MessageBox.Show(
                        "Are you sure you want to close the XML Editor? Any unsaved changes will be lost.",
                        "Confirm Close",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                    
                    // Cancel closing if the user clicks "No"
                    if (result == DialogResult.No)
                    {
                        e.Cancel = true;
                    }
                }
            };
            
            // Set up data grid view for resolutions with improved formatting
            dataGridResolutions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridResolutions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridResolutions.AllowUserToResizeRows = false;
            dataGridResolutions.AllowUserToResizeColumns = true;
            dataGridResolutions.MultiSelect = false;
            dataGridResolutions.RowHeadersVisible = false;
            dataGridResolutions.AllowUserToAddRows = false; // Use the Add button instead
            dataGridResolutions.DefaultCellStyle.BackColor = Color.FromArgb(32, 34, 37);
            dataGridResolutions.DefaultCellStyle.ForeColor = Color.White;
            dataGridResolutions.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215);
            dataGridResolutions.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 47, 49);
            dataGridResolutions.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridResolutions.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dataGridResolutions.EnableHeadersVisualStyles = false;
            dataGridResolutions.GridColor = Color.FromArgb(50, 50, 50);
            
            // Configure column percentages (Width: 35%, Height: 35%, Refresh Rate: 30%)
            dataGridResolutions.Columns[0].FillWeight = 35; // Width column
            dataGridResolutions.Columns[1].FillWeight = 35; // Height column
            dataGridResolutions.Columns[2].FillWeight = 30; // Refresh Rate column
            
            // Configure number formatting
            DataGridViewCellStyle numberStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleRight,
                Format = "N0" // No decimal places for Width and Height
            };
            
            DataGridViewCellStyle decimalStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleRight,
                Format = "N1" // One decimal place for Refresh Rate
            };
            
            dataGridResolutions.Columns[0].DefaultCellStyle = numberStyle;
            dataGridResolutions.Columns[1].DefaultCellStyle = numberStyle;
            dataGridResolutions.Columns[2].DefaultCellStyle = decimalStyle;
            
            // Initialize all descriptions and tooltips
            InitializeOptionDescriptions();
            InitializeGeneralTooltips();
            InitializeResolutionTooltips();
            InitializeRefreshRateTooltips();
            
            // Load XML data
            LoadXmlData();
        }

        /// <summary>
        /// Loads XML data from a specified file path
        /// </summary>
        /// <param name="filePath">Path to the XML file to load</param>
        public void LoadXmlFile(string filePath)
        {
            // Update file path
            xmlFilePath = filePath;
            
            // Load the XML data
            LoadXmlData();
        }

        private void LoadXmlData()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                
                // Create a new XMLController with the specified file path
                xmlController = new XMLController(xmlFilePath);
                
                // Update the form title to show the current file
                string fileName = Path.GetFileName(xmlFilePath);
                themeForm.Text = $"XML Editor - {fileName}";
                
                // Populate monitor count
                numericMonitorCount.Value = xmlController.Count;
                
                // Populate GPU name
                comboBoxGPU.Text = xmlController.Friendlyname;
                
                // Populate refresh rates list
                listBoxRefreshRates.BeginUpdate();
                listBoxRefreshRates.Items.Clear();
                foreach (var rate in xmlController.G_refresh_rate)
                {
                    listBoxRefreshRates.Items.Add(rate);
                }
                listBoxRefreshRates.EndUpdate();
                
                // Populate resolutions grid with proper error handling
                dataGridResolutions.SuspendLayout();
                dataGridResolutions.Rows.Clear();
                
                Console.WriteLine($"[DEBUG] Loading {xmlController.Resolutions.Count} resolutions");
                
                try
                {
                    if (xmlController.Resolutions != null)
                    {
                        foreach (var resolution in xmlController.Resolutions)
                        {
                            // Verify resolution data is valid before adding
                            if (resolution != null && resolution.Width > 0 && resolution.Height > 0)
                            {
                                Console.WriteLine($"[DEBUG] Adding resolution: {resolution.Width}x{resolution.Height}@{resolution.Refresh_rate}");
                                dataGridResolutions.Rows.Add(resolution.Width, resolution.Height, resolution.Refresh_rate);
                            }
                        }
                    }
                    
                    // If no resolutions were loaded, add a default one
                    if (dataGridResolutions.Rows.Count == 0)
                    {
                        Console.WriteLine("[DEBUG] No resolutions found, adding default");
                        dataGridResolutions.Rows.Add(1920, 1080, 60.0);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Failed to load resolutions: {ex.Message}");
                    // Add a default resolution if an error occurs
                    dataGridResolutions.Rows.Add(1920, 1080, 60.0);
                }
                
                dataGridResolutions.ResumeLayout();
                
                // Set options checkboxes
                checkedListOptions.BeginUpdate();
                SetOptionCheckbox("Custom EDID", xmlController.CustomEdid);
                SetOptionCheckbox("Prevent EDID Spoofing", xmlController.PreventSpoof);
                SetOptionCheckbox("EDID CEA Override", xmlController.EdidCeaOverride);
                SetOptionCheckbox("Hardware Cursor", xmlController.HardwareCursor);
                SetOptionCheckbox("SDR 10 Bit", xmlController.SDR10bit);
                SetOptionCheckbox("HDR+", xmlController.HDRPlus);
                SetOptionCheckbox("User-Mode Logging", xmlController.Logging);
                SetOptionCheckbox("Dev-Mode Logging", xmlController.DebugLogging);
                checkedListOptions.EndUpdate();
                
                // Set the first tab as active
                tabControl.SelectedIndex = 0;
                
                this.Cursor = Cursors.Default;
                
                // Notify user of successful load
                MessageBox.Show($"Successfully loaded XML data from {fileName}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Error loading XML data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void SetOptionCheckbox(string optionText, bool value)
        {
            for (int i = 0; i < checkedListOptions.Items.Count; i++)
            {
                if (checkedListOptions.Items[i].ToString() == optionText)
                {
                    checkedListOptions.SetItemChecked(i, value);
                    break;
                }
            }
        }

        private void SaveXmlData()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                
                // Validate inputs
                if (string.IsNullOrWhiteSpace(comboBoxGPU.Text))
                {
                    MessageBox.Show("GPU name cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tabControl.SelectedIndex = 0; // Switch to general tab
                    comboBoxGPU.Focus();
                    this.Cursor = Cursors.Default;
                    return;
                }
                
                // Update XML controller with form data
                xmlController.Count = (int)numericMonitorCount.Value;
                xmlController.Friendlyname = comboBoxGPU.Text;
                
                // Update refresh rates
                xmlController.G_refresh_rate.Clear();
                foreach (var item in listBoxRefreshRates.Items)
                {
                    xmlController.G_refresh_rate.Add(item.ToString());
                }
                
                // Validate and update resolutions
                bool hasInvalidResolution = false;
                xmlController.Resolutions.Clear();
                
                foreach (DataGridViewRow row in dataGridResolutions.Rows)
                {
                    if (!row.IsNewRow && row.Cells[0].Value != null && row.Cells[1].Value != null && row.Cells[2].Value != null)
                    {
                        try 
                        {
                            var resolution = new XMLController.Resolution
                            {
                                Width = Convert.ToInt32(row.Cells[0].Value),
                                Height = Convert.ToInt32(row.Cells[1].Value),
                                Refresh_rate = Convert.ToDouble(row.Cells[2].Value)
                            };
                            
                            // Basic validation
                            if (resolution.Width <= 0 || resolution.Height <= 0 || resolution.Refresh_rate <= 0)
                            {
                                hasInvalidResolution = true;
                                continue;
                            }
                            
                            xmlController.Resolutions.Add(resolution);
                        }
                        catch
                        {
                            hasInvalidResolution = true;
                        }
                    }
                }
                
                if (hasInvalidResolution)
                {
                    DialogResult result = MessageBox.Show(
                        "Some resolutions contain invalid values and will be skipped. Do you want to continue?", 
                        "Validation Warning", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Warning);
                        
                    if (result == DialogResult.No)
                    {
                        tabControl.SelectedIndex = 1; // Switch to resolutions tab
                        this.Cursor = Cursors.Default;
                        return;
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
                
                // Create a backup before saving (just in case)
                string backupPath = xmlFilePath + ".bak";
                try
                {
                    File.Copy(xmlFilePath, backupPath, true);
                }
                catch (Exception backupEx)
                {
                    // Just log this, don't stop the save process
                    Console.WriteLine($"Error creating backup: {backupEx.Message}");
                }
                
                // Save to XML file
                xmlController.SaveToXml(xmlFilePath);
                
                this.Cursor = Cursors.Default;
                MessageBox.Show($"XML file saved successfully to {xmlFilePath}!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Error saving XML: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private bool IsOptionChecked(string optionText)
        {
            for (int i = 0; i < checkedListOptions.Items.Count; i++)
            {
                if (checkedListOptions.Items[i].ToString() == optionText)
                {
                    return checkedListOptions.GetItemChecked(i);
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
            // Confirm before closing without saving
            DialogResult result = MessageBox.Show(
                "Are you sure you want to close without saving changes?", 
                "Confirm", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);
                
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }
        
        private void AddResolution_Click(object sender, EventArgs e)
        {
            // Create a context menu with common resolution options
            ContextMenuStrip menu = new ContextMenuStrip();
            
            // Add common resolution presets
            menu.Items.Add("1920×1080 @ 60Hz (Full HD)").Click += (s, args) => AddCommonResolution(1920, 1080, 60.0);
            menu.Items.Add("1920×1080 @ 144Hz").Click += (s, args) => AddCommonResolution(1920, 1080, 144.0);
            menu.Items.Add("2560×1440 @ 60Hz (QHD)").Click += (s, args) => AddCommonResolution(2560, 1440, 60.0);
            menu.Items.Add("2560×1440 @ 144Hz").Click += (s, args) => AddCommonResolution(2560, 1440, 144.0);
            menu.Items.Add("3840×2160 @ 60Hz (4K)").Click += (s, args) => AddCommonResolution(3840, 2160, 60.0);
            menu.Items.Add("1280×720 @ 60Hz (HD)").Click += (s, args) => AddCommonResolution(1280, 720, 60.0);
            menu.Items.Add("1366×768 @ 60Hz").Click += (s, args) => AddCommonResolution(1366, 768, 60.0);
            
            menu.Items.Add(new ToolStripSeparator());
            
            // Add a custom option that opens a dialog
            menu.Items.Add("Custom Resolution...").Click += (s, args) => 
            {
                // Create a custom resolution dialog
                Form customResDialog = new Form
                {
                    Text = "Add Custom Resolution",
                    Size = new Size(320, 200),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false,
                    BackColor = Color.FromArgb(32, 41, 50),
                    ForeColor = Color.White
                };
                
                Label widthLabel = new Label
                {
                    Text = "Width:",
                    AutoSize = true,
                    Location = new Point(20, 20),
                    ForeColor = Color.White
                };
                
                NumericUpDown widthValue = new NumericUpDown
                {
                    Minimum = 640,
                    Maximum = 7680,
                    Value = 1920,
                    Location = new Point(120, 18),
                    Width = 150,
                    BackColor = Color.FromArgb(32, 32, 32),
                    ForeColor = Color.White
                };
                
                Label heightLabel = new Label
                {
                    Text = "Height:",
                    AutoSize = true,
                    Location = new Point(20, 50),
                    ForeColor = Color.White
                };
                
                NumericUpDown heightValue = new NumericUpDown
                {
                    Minimum = 480,
                    Maximum = 4320,
                    Value = 1080,
                    Location = new Point(120, 48),
                    Width = 150,
                    BackColor = Color.FromArgb(32, 32, 32),
                    ForeColor = Color.White
                };
                
                Label refreshLabel = new Label
                {
                    Text = "Refresh Rate:",
                    AutoSize = true,
                    Location = new Point(20, 80),
                    ForeColor = Color.White
                };
                
                NumericUpDown refreshValue = new NumericUpDown
                {
                    Minimum = 24,
                    Maximum = 360,
                    DecimalPlaces = 2,
                    Value = 60,
                    Location = new Point(120, 78),
                    Width = 150,
                    BackColor = Color.FromArgb(32, 32, 32),
                    ForeColor = Color.White
                };
                
                Button addButton = new Button
                {
                    Text = "Add",
                    DialogResult = DialogResult.OK,
                    Location = new Point(120, 120),
                    Width = 80,
                    BackColor = Color.FromArgb(0, 100, 0),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                
                Button cancelButton = new Button
                {
                    Text = "Cancel",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(210, 120),
                    Width = 80,
                    BackColor = Color.FromArgb(45, 47, 49),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                
                customResDialog.Controls.Add(widthLabel);
                customResDialog.Controls.Add(widthValue);
                customResDialog.Controls.Add(heightLabel);
                customResDialog.Controls.Add(heightValue);
                customResDialog.Controls.Add(refreshLabel);
                customResDialog.Controls.Add(refreshValue);
                customResDialog.Controls.Add(addButton);
                customResDialog.Controls.Add(cancelButton);
                
                customResDialog.AcceptButton = addButton;
                customResDialog.CancelButton = cancelButton;
                
                if (customResDialog.ShowDialog(this) == DialogResult.OK)
                {
                    AddCommonResolution((int)widthValue.Value, (int)heightValue.Value, (double)refreshValue.Value);
                }
            };
            
            // Show context menu near the button
            menu.Show(buttonAddResolution, new Point(0, buttonAddResolution.Height));
        }
        
        private void RemoveResolution_Click(object sender, EventArgs e)
        {
            if (dataGridResolutions.SelectedRows.Count > 0 && !dataGridResolutions.SelectedRows[0].IsNewRow)
            {
                dataGridResolutions.Rows.Remove(dataGridResolutions.SelectedRows[0]);
            }
            else if (dataGridResolutions.SelectedCells.Count > 0)
            {
                int rowIndex = dataGridResolutions.SelectedCells[0].RowIndex;
                if (rowIndex >= 0 && !dataGridResolutions.Rows[rowIndex].IsNewRow)
                {
                    dataGridResolutions.Rows.RemoveAt(rowIndex);
                }
            }
            else
            {
                MessageBox.Show("Please select a resolution to remove.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        private void AddRefreshRate_Click(object sender, EventArgs e)
        {
            // Get the rate from the numeric up down
            string newRate = numericAddRefresh.Value.ToString();
            
            // Check if it already exists
            if (!listBoxRefreshRates.Items.Contains(newRate))
            {
                listBoxRefreshRates.Items.Add(newRate);
            }
            else
            {
                MessageBox.Show($"Refresh rate {newRate} already exists in the list.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        private void RemoveRefreshRate_Click(object sender, EventArgs e)
        {
            if (listBoxRefreshRates.SelectedIndex != -1)
            {
                listBoxRefreshRates.Items.RemoveAt(listBoxRefreshRates.SelectedIndex);
            }
            else
            {
                MessageBox.Show("Please select a refresh rate to remove.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        /// <summary>
        /// Shows the raw XML content in a dialog
        /// </summary>
        private void ViewRawXml_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(xmlFilePath) || !File.Exists(xmlFilePath))
                {
                    MessageBox.Show("XML file not found or path is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                // Check if there are unsaved changes and prompt to save
                DialogResult saveResult = MessageBox.Show(
                    "Would you like to save any changes before viewing the raw XML?",
                    "Save Changes",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);
                    
                if (saveResult == DialogResult.Cancel)
                {
                    return;
                }
                else if (saveResult == DialogResult.Yes)
                {
                    SaveXmlData();
                }
                
                // Read the XML file content
                string xmlContent = File.ReadAllText(xmlFilePath);
                
                // Format the XML for better readability
                xmlContent = FormatXml(xmlContent);
                
                // Create a dialog to display the raw XML
                Form xmlDialog = new Form
                {
                    Text = $"Raw XML Content - {Path.GetFileName(xmlFilePath)}",
                    Size = new Size(800, 600),
                    StartPosition = FormStartPosition.CenterParent,
                    MinimizeBox = false,
                    MaximizeBox = true,
                    ShowIcon = false,
                    BackColor = Color.FromArgb(32, 41, 50),
                    ForeColor = Color.White
                };
                
                // Create a panel for buttons
                Panel buttonPanel = new Panel
                {
                    Dock = DockStyle.Bottom,
                    Height = 60,
                    BackColor = Color.FromArgb(40, 49, 60)
                };
                
                // Create a RichTextBox to display the XML content
                RichTextBox xmlTextBox = new RichTextBox
                {
                    Text = xmlContent,
                    Dock = DockStyle.Fill,
                    ReadOnly = true,
                    BackColor = Color.FromArgb(3, 13, 17),
                    ForeColor = Color.White,
                    Font = new Font("Consolas", 11, FontStyle.Regular),
                    BorderStyle = BorderStyle.None,
                    WordWrap = false
                };
                
                // Add find feature to help navigate longer XML files
                TextBox findTextBox = new TextBox
                {
                    Width = 200,
                    Height = 24,
                    Location = new Point(10, 18),
                    BackColor = Color.FromArgb(32, 32, 32),
                    ForeColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle
                };
                
                Button findButton = new Button
                {
                    Text = "Find",
                    BackColor = Color.FromArgb(45, 47, 49),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Width = 80,
                    Height = 24,
                    Location = new Point(220, 18)
                };
                
                int lastFoundIndex = 0;
                findButton.Click += (s, args) => 
                {
                    string searchText = findTextBox.Text;
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        // Start search from current position or beginning if at the end
                        int startIndex = xmlTextBox.SelectionStart + xmlTextBox.SelectionLength;
                        if (startIndex >= xmlTextBox.Text.Length || lastFoundIndex > startIndex)
                        {
                            startIndex = 0;
                        }
                        
                        int foundIndex = xmlTextBox.Text.IndexOf(searchText, startIndex, StringComparison.OrdinalIgnoreCase);
                        if (foundIndex >= 0)
                        {
                            xmlTextBox.Select(foundIndex, searchText.Length);
                            xmlTextBox.ScrollToCaret();
                            lastFoundIndex = foundIndex;
                        }
                        else if (startIndex > 0)
                        {
                            // If not found from current position, try from beginning
                            foundIndex = xmlTextBox.Text.IndexOf(searchText, 0, StringComparison.OrdinalIgnoreCase);
                            if (foundIndex >= 0)
                            {
                                xmlTextBox.Select(foundIndex, searchText.Length);
                                xmlTextBox.ScrollToCaret();
                                lastFoundIndex = foundIndex;
                                MessageBox.Show("Reached the end of the document, continuing from the beginning.", 
                                    "Find", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show($"Text '{searchText}' not found.", "Find", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Text '{searchText}' not found.", "Find", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                };
                
                findTextBox.KeyDown += (s, args) => 
                {
                    if (args.KeyCode == Keys.Enter)
                    {
                        findButton.PerformClick();
                        args.SuppressKeyPress = true;
                    }
                };
                
                // Add close button
                Button closeButton = new Button
                {
                    Text = "Close",
                    BackColor = Color.FromArgb(45, 47, 49),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Width = 120,
                    Height = 30,
                    Location = new Point(xmlDialog.Width - 140, 18)
                };
                closeButton.Click += (s, args) => xmlDialog.Close();
                
                // Add resize logic for button placement
                closeButton.Anchor = AnchorStyles.Right;
                buttonPanel.Resize += (s, args) => 
                {
                    closeButton.Top = (buttonPanel.Height - closeButton.Height) / 2;
                };
                
                // Add controls to the form
                buttonPanel.Controls.Add(findTextBox);
                buttonPanel.Controls.Add(findButton);
                buttonPanel.Controls.Add(closeButton);
                xmlDialog.Controls.Add(xmlTextBox);
                xmlDialog.Controls.Add(buttonPanel);
                
                // Show the dialog
                xmlDialog.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error viewing raw XML: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// Formats XML string with proper indentation for better readability
        /// </summary>
        private string FormatXml(string xml)
        {
            try
            {
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.LoadXml(xml);
                
                StringBuilder sb = new StringBuilder();
                System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = Environment.NewLine,
                    NewLineHandling = System.Xml.NewLineHandling.Replace,
                    OmitXmlDeclaration = false
                };
                
                using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(sb, settings))
                {
                    doc.Save(writer);
                }
                
                return sb.ToString();
            }
            catch (Exception)
            {
                // If formatting fails, return the original XML
                return xml;
            }
        }
        
        /// <summary>
        /// Creates a common resolution entry based on preset values
        /// </summary>
        private void AddCommonResolution(int width, int height, double refreshRate)
        {
            try
            {
                dataGridResolutions.Rows.Add(width, height, refreshRate);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding resolution: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// Initializes tooltips and descriptions for the General Settings tab
        /// </summary>
        private void InitializeGeneralTooltips()
        {
            try
            {
                // Monitor count tooltip
                optionsTooltip.SetToolTip(numericMonitorCount, 
                    "Sets the number of virtual monitors to create.\n" +
                    "Increasing this number will create multiple virtual displays.");
                
                optionsTooltip.SetToolTip(labelMonitorCount, 
                    "Sets the number of virtual monitors to create.");
                
                // GPU tooltip
                optionsTooltip.SetToolTip(comboBoxGPU, 
                    "Specifies which GPU should be used for the virtual display.\n" + 
                    "Use '(Automatic)' to let the driver select the best option.");
                
                optionsTooltip.SetToolTip(labelGPU, 
                    "Specifies which GPU should be used for the virtual display.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to initialize general tooltips: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Initializes tooltips and descriptions for the Resolutions tab
        /// </summary>
        private void InitializeResolutionTooltips()
        {
            try
            {
                // Resolution grid tooltip
                optionsTooltip.SetToolTip(dataGridResolutions, 
                    "List of supported resolutions for the virtual display.\n" + 
                    "Each row defines a Width × Height @ Refresh Rate configuration.");
                
                // Button tooltips
                optionsTooltip.SetToolTip(buttonAddResolution, 
                    "Add a new resolution from preset options or create a custom resolution.");
                
                optionsTooltip.SetToolTip(buttonRemoveResolution, 
                    "Remove the selected resolution from the list.");
                
                // Add a header label
                Label resolutionsHeader = new Label
                {
                    Text = "Configure supported resolutions for your virtual displays",
                    Font = new Font("Segoe UI", 9, FontStyle.Regular),
                    Location = new Point(16, 20 - 18),
                    AutoSize = true,
                    ForeColor = Color.White
                };
                
                tabResolutions.Controls.Add(resolutionsHeader);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to initialize resolution tooltips: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Initializes tooltips and descriptions for the Refresh Rates tab
        /// </summary>
        private void InitializeRefreshRateTooltips()
        {
            try
            {
                // Refresh rate list tooltip
                optionsTooltip.SetToolTip(listBoxRefreshRates, 
                    "List of global refresh rates available to all resolutions.\n" + 
                    "These are used as fallback values if a resolution doesn't specify its own refresh rate.");
                
                // Button tooltips
                optionsTooltip.SetToolTip(buttonAddRefreshRate, 
                    "Add the specified refresh rate value from the numeric input to the list.");
                
                optionsTooltip.SetToolTip(buttonRemoveRefreshRate, 
                    "Remove the selected refresh rate from the list.");
                
                // Numeric input tooltip
                optionsTooltip.SetToolTip(numericAddRefresh, 
                    "Specify the refresh rate value to add, in Hz (e.g., 60, 120, 144).");
                
                // Add a header label
                Label refreshRatesHeader = new Label
                {
                    Text = "Configure global refresh rates available across all resolutions",
                    Font = new Font("Segoe UI", 9, FontStyle.Regular),
                    Location = new Point(16, 20 - 18),
                    AutoSize = true,
                    ForeColor = Color.White
                };
                
                tabRefreshRates.Controls.Add(refreshRatesHeader);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to initialize refresh rate tooltips: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Initialize detailed descriptions for each option in the options list
        /// </summary>
        private void InitializeOptionDescriptions()
        {
            try
            {
                // First, create a label to show extended descriptions
                Label descriptionLabel = new Label
                {
                    Location = new Point(16, groupBoxOptions.Bottom - 80),
                    Size = new Size(groupBoxOptions.Width - 40, 60),
                    ForeColor = Color.LightGray,
                    BackColor = Color.FromArgb(25, 35, 45),
                    BorderStyle = BorderStyle.FixedSingle,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(5),
                    Visible = false,
                    AutoSize = false
                };
                
                tabOptions.Controls.Add(descriptionLabel);
                
                // Add descriptions for each option
                Dictionary<string, string> optionDescriptions = new Dictionary<string, string>
                {
                    { "Custom EDID", "Enables the use of custom EDID (Extended Display Identification Data) for the virtual display. This allows you to define custom display characteristics." },
                    { "Prevent EDID Spoofing", "Prevents other applications from modifying or spoofing the EDID data of the virtual display, providing better security and stability." },
                    { "EDID CEA Override", "Overrides the Consumer Electronics Association (CEA) extension block in the EDID, which is used for features like HDR and advanced audio capabilities." },
                    { "Hardware Cursor", "Enables hardware acceleration for cursor rendering on the virtual display, which can improve performance and reduce CPU usage." },
                    { "SDR 10 Bit", "Enables 10-bit color depth in Standard Dynamic Range (SDR) mode, providing over 1 billion colors instead of the standard 16.7 million colors of 8-bit." },
                    { "HDR+", "Enables Enhanced High Dynamic Range support for better contrast, wider color gamut, and higher brightness levels on compatible content." },
                    { "User-Mode Logging", "Enables logging of driver operations in user mode, useful for troubleshooting display issues without requiring administrator privileges." },
                    { "Dev-Mode Logging", "Enables extensive developer-mode logging with detailed information about the virtual display driver operations for advanced debugging." }
                };
                
                // Process the options list
                for (int i = 0; i < checkedListOptions.Items.Count; i++)
                {
                    string optionText = checkedListOptions.Items[i].ToString();
                    if (optionDescriptions.ContainsKey(optionText))
                    {
                        // Set tooltip for each item
                        optionsTooltip.SetToolTip(checkedListOptions, optionDescriptions[optionText]);
                    }
                }
                
                // Add hover event to show detailed descriptions
                checkedListOptions.MouseMove += (sender, e) => 
                {
                    // Get the item under the mouse cursor
                    int index = checkedListOptions.IndexFromPoint(e.Location);
                    if (index != ListBox.NoMatches)
                    {
                        string optionText = checkedListOptions.Items[index].ToString();
                        if (optionDescriptions.ContainsKey(optionText))
                        {
                            descriptionLabel.Text = optionDescriptions[optionText];
                            descriptionLabel.Visible = true;
                        }
                    }
                    else
                    {
                        descriptionLabel.Visible = false;
                    }
                };
                
                // Hide description when mouse leaves
                checkedListOptions.MouseLeave += (sender, e) => 
                {
                    descriptionLabel.Visible = false;
                };
                
                // Add labels above the CheckedListBox to provide guidance
                Label optionsHeader = new Label
                {
                    Text = "Driver Features and Settings",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Location = new Point(14, 30 - 22),
                    AutoSize = true,
                    ForeColor = Color.White
                };
                
                Label optionsSubheader = new Label
                {
                    Text = "Hover over an option for details. Check the box to enable the feature.",
                    Font = new Font("Segoe UI", 8, FontStyle.Italic),
                    Location = new Point(14, 30 - 4),
                    AutoSize = true,
                    ForeColor = Color.LightGray
                };
                
                groupBoxOptions.Controls.Add(optionsHeader);
                groupBoxOptions.Controls.Add(optionsSubheader);
                
                // Update the position of the checkedListOptions to make room for the header
                checkedListOptions.Location = new Point(checkedListOptions.Location.X, checkedListOptions.Location.Y + 20);
                checkedListOptions.Height = checkedListOptions.Height - 20;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to initialize option descriptions: {ex.Message}");
            }
        }
    }
}