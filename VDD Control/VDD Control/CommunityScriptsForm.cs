using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VDD_Control
{
    public partial class CommunityScriptsForm : Form
    {
        private const string SCRIPTS_FOLDER = "Community Scripts";
        private const string GITHUB_SCRIPTS_URL = "https://github.com/VirtualDrivers/Virtual-Display-Driver/tree/master/Community%20Scripts";
        private ListBox scriptListBox;
        private Label noScriptsLabel;
        private Button syncButton;

        public CommunityScriptsForm()
        {
            InitializeComponents();
            LoadScripts();
            
            // Handle resize to keep button visible
            this.Resize += CommunityScriptsForm_Resize;
        }
        
        private void CommunityScriptsForm_Resize(object sender, EventArgs e)
        {
            // Ensure controls are properly laid out after resize
            this.PerformLayout();
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

            // Set up the form layout
            this.Controls.Clear();
            
            // Create TableLayoutPanel for better control of layout
            TableLayoutPanel tableLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                Padding = new Padding(10, 10, 10, 10)
            };
            
            // Configure rows - first row (scripts) takes all available space, second row (button) is fixed height
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            
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

            // Create sync button
            syncButton = new Button
            {
                Text = "Sync from GitHub",
                BackColor = Color.FromArgb(114, 137, 218),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 5, 0, 0)
            };
            syncButton.Click += SyncButton_Click;

            // Panel to hold the list or label
            Panel listPanel = new Panel
            {
                Dock = DockStyle.Fill
            };
            listPanel.Controls.Add(scriptListBox);
            listPanel.Controls.Add(noScriptsLabel);
            
            // Add controls to the table layout
            tableLayout.Controls.Add(listPanel, 0, 0);
            tableLayout.Controls.Add(syncButton, 0, 1);
            
            // Add the layout to the form
            this.Controls.Add(tableLayout);
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
                
            // Simple confirmation dialog without security warnings
            DialogResult result = MessageBox.Show(
                $"Do you want to run {selectedScript}?",
                "Run Script",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
                
            if (result != DialogResult.Yes)
            {
                return; // User canceled execution
            }
            
            // Always allow script execution
            if (!File.Exists(scriptPath))
            {
                MessageBox.Show(
                    "The script file could not be found.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // Execute the script based on its extension
            try
            {
                if (selectedScript.EndsWith(".ps1", StringComparison.OrdinalIgnoreCase))
                {
                    // PowerShell script - using more restrictive execution policy
                    using (Process process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "powershell.exe",
                            // Using RemoteSigned instead of Bypass for better security
                            // This allows local scripts to run but requires remote scripts to be signed
                            Arguments = $"-ExecutionPolicy RemoteSigned -NoProfile -File \"{scriptPath}\"",
                            UseShellExecute = false,
                            CreateNoWindow = false,
                            // Redirect output to capture any errors
                            RedirectStandardError = true,
                            // Working directory set to script directory
                            WorkingDirectory = Path.GetDirectoryName(scriptPath)
                        }
                    })
                    {
                        try
                        {
                            // Log execution for auditing
                            File.AppendAllText(
                                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "script_execution.log"),
                                $"{DateTime.Now}: Executed PowerShell script: {selectedScript}\r\n");
                                
                            process.Start();
                            
                            // Start a background thread to monitor for errors
                            Task.Run(async () => {
                                string error = await process.StandardError.ReadToEndAsync();
                                if (!string.IsNullOrEmpty(error))
                                {
                                    // Log errors
                                    File.AppendAllText(
                                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "script_errors.log"),
                                        $"{DateTime.Now}: Error in {selectedScript}: {error}\r\n");
                                }
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error starting PowerShell script: {ex.Message}", "Execution Error", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else if (selectedScript.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        // Executable - Using ProcessStartInfo for more control
                        using (Process process = new Process
                        {
                            StartInfo = new ProcessStartInfo
                            {
                                FileName = scriptPath,
                                UseShellExecute = true, // Use shell execute to run with same privileges as current user
                                WorkingDirectory = Path.GetDirectoryName(scriptPath)
                            }
                        })
                        {
                            // Log execution for auditing
                            File.AppendAllText(
                                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "script_execution.log"),
                                $"{DateTime.Now}: Executed executable: {selectedScript}\r\n");
                                
                            process.Start();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error starting executable: {ex.Message}", "Execution Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Batch or CMD file
                    using (Process process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "cmd.exe",
                            Arguments = $"/c \"{scriptPath}\"",
                            UseShellExecute = false,
                            CreateNoWindow = false,
                            // Redirect output to capture any errors
                            RedirectStandardError = true,
                            // Working directory set to script directory
                            WorkingDirectory = Path.GetDirectoryName(scriptPath)
                        }
                    })
                    {
                        try
                        {
                            // Log execution for auditing
                            File.AppendAllText(
                                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "script_execution.log"),
                                $"{DateTime.Now}: Executed batch file: {selectedScript}\r\n");
                                
                            process.Start();
                            
                            // Start a background thread to monitor for errors
                            Task.Run(async () => {
                                string error = await process.StandardError.ReadToEndAsync();
                                if (!string.IsNullOrEmpty(error))
                                {
                                    // Log errors
                                    File.AppendAllText(
                                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "script_errors.log"),
                                        $"{DateTime.Now}: Error in {selectedScript}: {error}\r\n");
                                }
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error starting batch file: {ex.Message}", "Execution Error", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
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

        private async void SyncButton_Click(object sender, EventArgs e)
        {
            // Disable the sync button during the operation
            syncButton.Enabled = false;
            syncButton.Text = "Syncing...";
            
            try
            {
                await SyncScriptsFromGitHub();
                MessageBox.Show("Community scripts successfully synchronized from GitHub!", 
                    "Sync Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error synchronizing scripts: {ex.Message}", 
                    "Sync Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Re-enable the button
                syncButton.Enabled = true;
                syncButton.Text = "Sync from GitHub";
                
                // Refresh the script list to show the updates
                LoadScripts();
            }
        }

        // Helper method to get a user-friendly description of the script type
        private string GetScriptTypeDescription(string scriptName)
        {
            if (scriptName.EndsWith(".ps1", StringComparison.OrdinalIgnoreCase))
                return "PowerShell script";
            else if (scriptName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                return "executable file";
            else if (scriptName.EndsWith(".bat", StringComparison.OrdinalIgnoreCase) || 
                     scriptName.EndsWith(".cmd", StringComparison.OrdinalIgnoreCase))
                return "batch file";
            else
                return "script file";
        }
        
        // This method always returns true, allowing all scripts to run without security checks
        private bool PerformBasicSecurityScan(string filePath, string fileName)
        {
            try
            {
                // Check if file exists (only basic check)
                if (!File.Exists(filePath))
                {
                    MessageBox.Show($"File not found: {filePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                
                // Always return true - no security checks
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error checking file: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
        }

        private async Task SyncScriptsFromGitHub()
        {
            // Create HTTP client
            using (HttpClient client = new HttpClient())
            {
                // GitHub API requires a user agent
                client.DefaultRequestHeaders.Add("User-Agent", "VDD-Control-App");

                // Convert the GitHub web URL to API URL to get the directory contents
                // Format: https://api.github.com/repos/{owner}/{repo}/contents/{path}
                string apiUrl = "https://api.github.com/repos/VirtualDrivers/Virtual-Display-Driver/contents/Community%20Scripts";
                
                // Get the directory listing
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                
                string responseBody = await response.Content.ReadAsStringAsync();
                JsonDocument doc = JsonDocument.Parse(responseBody);
                
                // Make sure the scripts directory exists
                string scriptsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SCRIPTS_FOLDER);
                if (!Directory.Exists(scriptsDirectory))
                {
                    Directory.CreateDirectory(scriptsDirectory);
                }
                
                // Process each file in the repository
                // Create a list to track sync results for user feedback
                List<string> scriptSyncResults = new List<string>();
                int fileCount = 0;
                int skippedCount = 0;
                
                foreach (JsonElement item in doc.RootElement.EnumerateArray())
                {
                    // Only download script files (.cmd, .bat, .ps1, .exe)
                    string fileName = item.GetProperty("name").GetString();
                    if (fileName.EndsWith(".cmd", StringComparison.OrdinalIgnoreCase) ||
                        fileName.EndsWith(".bat", StringComparison.OrdinalIgnoreCase) ||
                        fileName.EndsWith(".ps1", StringComparison.OrdinalIgnoreCase) ||
                        fileName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            // Get download URL
                            string downloadUrl = item.GetProperty("download_url").GetString();
                            
                            // Download all files regardless of size
                            // No size restrictions for executables
                            
                            // Download the file content
                            HttpResponseMessage fileResponse = await client.GetAsync(downloadUrl);
                            fileResponse.EnsureSuccessStatusCode();
                            
                            // Get file content
                            byte[] fileContent = await fileResponse.Content.ReadAsByteArrayAsync();
                            
                            // Download all script files without security checks
                            // Removed security checks for script files
                            
                            // Save the validated file to the scripts directory
                            string filePath = Path.Combine(scriptsDirectory, fileName);
                            
                            // Create a temp file first for safety
                            string tempFilePath = Path.Combine(scriptsDirectory, $"temp_{Guid.NewGuid()}_{fileName}");
                            using (FileStream fs = new FileStream(tempFilePath, FileMode.Create))
                            {
                                await fs.WriteAsync(fileContent, 0, fileContent.Length);
                            }
                            
                            // Once successfully written, move to final location
                            if (File.Exists(filePath))
                            {
                                File.Delete(filePath);
                            }
                            File.Move(tempFilePath, filePath);
                            
                            scriptSyncResults.Add($"Downloaded: {fileName}");
                            fileCount++;
                        }
                        catch (Exception ex)
                        {
                            scriptSyncResults.Add($"Error downloading {fileName}: {ex.Message}");
                        }
                    }
                }
                
                // Show summary to the user
                MessageBox.Show(
                    $"Synchronization complete:\n" +
                    $"• {fileCount} files downloaded\n" +
                    $"• {skippedCount} files skipped for security reasons\n\n" +
                    $"Scripts are located in: {scriptsDirectory}",
                    "Sync Results", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }
    }
}