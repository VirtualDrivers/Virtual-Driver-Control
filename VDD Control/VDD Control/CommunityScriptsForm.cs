using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VDD_Control
{
    public partial class CommunityScriptsForm : Form
    {
        private const string SCRIPTS_FOLDER = "Community Scripts";
        private ListBox scriptListBox;
        private Label noScriptsLabel;

        public CommunityScriptsForm()
        {
            InitializeComponents();
            LoadScripts();
        }

        private void InitializeComponents()
        {
            // Configure form
            this.Text = "Community Scripts";
            this.Size = new Size(500, 400);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(32, 34, 37);
            this.ForeColor = Color.White;

            // Create script list box
            scriptListBox = new ListBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(45, 47, 49),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            scriptListBox.DoubleClick += ScriptListBox_DoubleClick;

            // Create no scripts label
            noScriptsLabel = new Label
            {
                Text = "No scripts found in the Community Scripts folder.",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(45, 47, 49),
                Visible = false
            };

            // Add controls to form
            this.Controls.Add(scriptListBox);
            this.Controls.Add(noScriptsLabel);
        }

        private void LoadScripts()
        {
            // Make sure the scripts directory exists
            string scriptsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SCRIPTS_FOLDER);
            
            // Create the directory if it doesn't exist
            if (!Directory.Exists(scriptsDirectory))
            {
                Directory.CreateDirectory(scriptsDirectory);
            }

            // Find script files
            string[] scriptFiles = Directory.GetFiles(scriptsDirectory, "*.*")
                .Where(file => 
                    file.EndsWith(".cmd", StringComparison.OrdinalIgnoreCase) ||
                    file.EndsWith(".bat", StringComparison.OrdinalIgnoreCase) ||
                    file.EndsWith(".ps1", StringComparison.OrdinalIgnoreCase) ||
                    file.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            // Clear the list
            scriptListBox.Items.Clear();

            // If no scripts found, show the label
            if (scriptFiles.Length == 0)
            {
                scriptListBox.Visible = false;
                noScriptsLabel.Visible = true;
                return;
            }

            // Otherwise, hide the label and populate the list
            scriptListBox.Visible = true;
            noScriptsLabel.Visible = false;

            // Add scripts to list
            foreach (string scriptFile in scriptFiles)
            {
                scriptListBox.Items.Add(Path.GetFileName(scriptFile));
            }
        }

        private void ScriptListBox_DoubleClick(object sender, EventArgs e)
        {
            if (scriptListBox.SelectedItem == null)
                return;

            string selectedScript = scriptListBox.SelectedItem.ToString();
            string scriptPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, 
                SCRIPTS_FOLDER, 
                selectedScript);

            // Execute the script based on its extension
            try
            {
                if (selectedScript.EndsWith(".ps1", StringComparison.OrdinalIgnoreCase))
                {
                    // PowerShell script
                    Process process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "powershell.exe",
                            Arguments = $"-ExecutionPolicy Bypass -File \"{scriptPath}\"",
                            UseShellExecute = false,
                            CreateNoWindow = false
                        }
                    };
                    process.Start();
                }
                else if (selectedScript.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                {
                    // Executable
                    Process.Start(scriptPath);
                }
                else
                {
                    // Batch or CMD file
                    Process process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "cmd.exe",
                            Arguments = $"/c \"{scriptPath}\"",
                            UseShellExecute = false,
                            CreateNoWindow = false
                        }
                    };
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error executing script: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Method to refresh the script list (can be called externally)
        public void RefreshScripts()
        {
            LoadScripts();
        }
    }
}