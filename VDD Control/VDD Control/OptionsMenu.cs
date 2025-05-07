using System;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;

namespace VDD_Control
{
    public partial class mainWindow
    {
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem locateDriverInstallationToolStripMenuItem;
        
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
            
            // Add items to Options menu
            optionsToolStripMenuItem.DropDownItems.Add(locateDriverInstallationToolStripMenuItem);
            
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