using System;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Reflection;
using System.Diagnostics;

namespace VDD_Control
{
    public partial class mainWindow
    {
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem locateDriverInstallationToolStripMenuItem;
        private ToolStripMenuItem runOnStartupToolStripMenuItem;
        
        /// <summary>
        /// Initializes Options menu items and adds them to the main menu
        /// </summary>
        private void InitializeOptionsMenu()
        {
            // Create Options menu item for main menu
            optionsToolStripMenuItem = new ToolStripMenuItem
            {
                Name = "optionsToolStripMenuItem",
                Size = new System.Drawing.Size(199, 22),
                Text = "Options"
            };
            
            // Create Locate Driver Installation menu item
            locateDriverInstallationToolStripMenuItem = new ToolStripMenuItem
            {
                Name = "locateDriverInstallationToolStripMenuItem",
                Size = new System.Drawing.Size(220, 22),
                Text = "Locate Driver Installation"
            };
            locateDriverInstallationToolStripMenuItem.Click += locateDriverInstallationToolStripMenuItem_Click;

            // Create Run on Startup menu item
            runOnStartupToolStripMenuItem = new ToolStripMenuItem
            {
                Name = "runOnStartupToolStripMenuItem",
                Size = new System.Drawing.Size(220, 22),
                Text = "Run on Startup",
                CheckOnClick = true
            };
            runOnStartupToolStripMenuItem.Click += runOnStartupToolStripMenuItem_Click;
            
            // Check if app is already set to run at startup and update the menu item checked state
            runOnStartupToolStripMenuItem.Checked = IsRunAtStartupEnabled();
            
            // Add items to Options menu
            optionsToolStripMenuItem.DropDownItems.Add(locateDriverInstallationToolStripMenuItem);
            optionsToolStripMenuItem.DropDownItems.Add(runOnStartupToolStripMenuItem);
            
            // Add Options menu to main menu
            if (menuToolStripMenuItem != null && menuToolStripMenuItem.DropDownItems != null)
            {
                // Check if it already exists
                bool exists = false;
                foreach (ToolStripItem item in menuToolStripMenuItem.DropDownItems)
                {
                    if (item.Name == "optionsToolStripMenuItem")
                    {
                        exists = true;
                        break;
                    }
                }
                
                if (!exists)
                {
                    // Add before Exit item
                    int exitIndex = -1;
                    for (int i = 0; i < menuToolStripMenuItem.DropDownItems.Count; i++)
                    {
                        if (menuToolStripMenuItem.DropDownItems[i].Name == "exitToolStripMenuItem")
                        {
                            exitIndex = i;
                            break;
                        }
                    }
                    
                    if (exitIndex >= 0)
                    {
                        menuToolStripMenuItem.DropDownItems.Insert(exitIndex, optionsToolStripMenuItem);
                        // Add a separator before Exit
                        menuToolStripMenuItem.DropDownItems.Insert(exitIndex + 1, new ToolStripSeparator());
                    }
                    else
                    {
                        menuToolStripMenuItem.DropDownItems.Add(optionsToolStripMenuItem);
                    }
                    
                    mainConsole.AppendText("[INFO] Added Options menu to main menu\n");
                }
            }
        }

        /// <summary>
        /// Checks if the application is configured to run at startup
        /// </summary>
        /// <returns>True if the application is set to run at startup, otherwise false</returns>
        private bool IsRunAtStartupEnabled()
        {
            try
            {
                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", false))
                {
                    if (key != null)
                    {
                        string? value = key.GetValue("VirtualDriverControl") as string;
                        if (!string.IsNullOrEmpty(value))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                mainConsole.AppendText($"[WARNING] Error checking startup registry: {ex.Message}\n");
            }
            
            return false;
        }

        /// <summary>
        /// Enables or disables running the application at startup
        /// </summary>
        /// <param name="enable">True to enable run at startup, false to disable</param>
        private void SetRunAtStartup(bool enable)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
                {
                    if (key != null)
                    {
                        if (enable)
                        {
                            string appPath = Process.GetCurrentProcess().MainModule.FileName;
                            key.SetValue("VirtualDriverControl", $"\"{appPath}\"");
                            mainConsole.AppendText("[INFO] Added application to startup\n");
                        }
                        else
                        {
                            key.DeleteValue("VirtualDriverControl", false);
                            mainConsole.AppendText("[INFO] Removed application from startup\n");
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show(
                    "Cannot update registry - insufficient permissions. Try running the application as administrator.",
                    "Registry Access Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                mainConsole.AppendText($"[ERROR] Error updating startup registry: {ex.Message}\n");
                MessageBox.Show(
                    $"Error updating startup registry: {ex.Message}",
                    "Registry Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Event handler for Run On Startup menu item click
        /// </summary>
        private void runOnStartupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool isChecked = ((ToolStripMenuItem)sender).Checked;
            SetRunAtStartup(isChecked);
            
            // Update tray menu item to match
            if (runOnStartupTrayMenuItem != null)
            {
                runOnStartupTrayMenuItem.Checked = isChecked;
            }
        }
        
        /// <summary>
        /// Event handler for Locate Driver Installation menu item click
        /// </summary>
        private void locateDriverInstallationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "Locate Virtual Display Driver Installation Directory";
                    folderDialog.ShowNewFolderButton = false;
                    
                    // Check if we can find the current path and set it as the initial directory
                    string? currentPath = null;
                    
                    // Try to get path from registry
                    try
                    {
                        using (RegistryKey? key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\MikeTheTech\VirtualDisplayDriver"))
                        {
                            if (key != null)
                            {
                                // Try VDDPATH first, then SettingsPath
                                currentPath = key.GetValue("VDDPATH") as string;
                                if (string.IsNullOrEmpty(currentPath))
                                {
                                    currentPath = key.GetValue("SettingsPath") as string;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        mainConsole.AppendText($"[WARNING] Error reading registry: {ex.Message}\n");
                    }
                    
                    if (!string.IsNullOrEmpty(currentPath) && Directory.Exists(currentPath))
                    {
                        folderDialog.SelectedPath = currentPath;
                    }
                    else if (Directory.Exists(registryFilePath))
                    {
                        folderDialog.SelectedPath = registryFilePath;
                    }
                    
                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        string selectedPath = folderDialog.SelectedPath;
                        
                        // Check if the directory contains vdd_settings.xml
                        bool xmlExists = File.Exists(Path.Combine(selectedPath, "vdd_settings.xml"));
                        
                        if (!xmlExists)
                        {
                            DialogResult createResult = MessageBox.Show(
                                "The selected directory does not contain vdd_settings.xml. Do you want to create a new settings file?",
                                "Create Settings File",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);
                                
                            if (createResult == DialogResult.Yes)
                            {
                                // Create a default settings file
                                CreateDefaultXmlSettings(Path.Combine(selectedPath, "vdd_settings.xml"));
                                xmlExists = true;
                            }
                        }
                        
                        if (xmlExists)
                        {
                            // Update the registry key
                            try
                            {
                                using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\MikeTheTech\VirtualDisplayDriver", true))
                                {
                                    if (key != null)
                                    {
                                        key.SetValue("VDDPATH", selectedPath);
                                        key.SetValue("SettingsPath", selectedPath);
                                        mainConsole.AppendText($"[SUCCESS] Updated registry with new driver path: {selectedPath}\n");
                                        
                                        // Update registryFilePath
                                        registryFilePath = selectedPath;
                                        
                                        // Reload XML settings
                                        string settingsPath = Path.Combine(selectedPath, "vdd_settings.xml");
                                        IXCLI = new XMLController(settingsPath);
                                        mainConsole.AppendText("[SUCCESS] Reloaded XML settings from new location\n");
                                        LoadSettingsFromXML();
                                    }
                                }
                            }
                            catch (UnauthorizedAccessException)
                            {
                                MessageBox.Show(
                                    "Cannot update registry - insufficient permissions. Try running the application as administrator.",
                                    "Registry Access Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                                    
                                // Still use the path for this session
                                registryFilePath = selectedPath;
                                mainConsole.AppendText($"[INFO] Using selected path for this session: {selectedPath}\n");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(
                                    $"Error updating registry: {ex.Message}",
                                    "Registry Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                mainConsole.AppendText($"[ERROR] Error locating driver installation: {ex.Message}\n");
                MessageBox.Show(
                    $"Error: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}