using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace VDD_Control
{
    public partial class mainWindow
    {
        private ToolStripMenuItem runOnStartupTrayMenuItem = null!;
        
        /// <summary>
        /// Initializes the Options menu in the tray icon context menu
        /// </summary>
        private void InitializeTrayOptionsMenu()
        {
            try
            {
                // Create Options menu item for tray menu if it doesn't exist
                ToolStripMenuItem optionsMenuItem = null;
                
                // Check if Options menu already exists in tray menu
                foreach (ToolStripItem item in trayMenu.Items)
                {
                    if (item is ToolStripMenuItem menuItem && menuItem.Text == "Options")
                    {
                        optionsMenuItem = menuItem;
                        break;
                    }
                }
                
                // If Options menu doesn't exist, create it
                if (optionsMenuItem == null)
                {
                    optionsMenuItem = new ToolStripMenuItem
                    {
                        Name = "optionsToolStripMenuItemTray",
                        Size = new System.Drawing.Size(183, 22),
                        Text = "Options"
                    };
                    
                    // Insert the Options menu before the Exit item
                    int exitIndex = -1;
                    for (int i = 0; i < trayMenu.Items.Count; i++)
                    {
                        if (trayMenu.Items[i].Text == "Exit")
                        {
                            exitIndex = i;
                            break;
                        }
                    }
                    
                    if (exitIndex >= 0)
                    {
                        trayMenu.Items.Insert(exitIndex, optionsMenuItem);
                        // Add a separator before Exit
                        trayMenu.Items.Insert(exitIndex + 1, new ToolStripSeparator());
                    }
                    else
                    {
                        trayMenu.Items.Add(optionsMenuItem);
                    }
                }
                
                // Create Run on Startup menu item
                runOnStartupTrayMenuItem = new ToolStripMenuItem
                {
                    Name = "runOnStartupTrayMenuItem",
                    Size = new System.Drawing.Size(220, 22),
                    Text = "Run on Startup",
                    CheckOnClick = true
                };
                runOnStartupTrayMenuItem.Click += runOnStartupTrayMenuItem_Click;
                
                // Check if app is already set to run at startup and update the menu item checked state
                runOnStartupTrayMenuItem.Checked = IsRunAtStartupEnabled();
                
                // Add to Options menu
                optionsMenuItem.DropDownItems.Add(runOnStartupTrayMenuItem);
                
                mainConsole.AppendText("[INFO] Added Run on Startup option to tray menu\n");
            }
            catch (Exception ex)
            {
                mainConsole.AppendText($"[ERROR] Error initializing tray options menu: {ex.Message}\n");
            }
        }
        
        /// <summary>
        /// Event handler for Run On Startup tray menu item click
        /// </summary>
        private void runOnStartupTrayMenuItem_Click(object sender, EventArgs e)
        {
            bool isChecked = ((ToolStripMenuItem)sender).Checked;
            SetRunAtStartup(isChecked);
            
            // Update UI menu item to match
            if (runOnStartupToolStripMenuItem != null)
            {
                runOnStartupToolStripMenuItem.Checked = isChecked;
            }
        }
    }
}