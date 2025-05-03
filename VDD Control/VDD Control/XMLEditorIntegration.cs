using System;
using System.IO;
using System.Windows.Forms;

namespace VDD_Control
{
    // Add this class to the MainWindow.cs file
    public partial class mainWindow
    {
        //private ToolStripMenuItem xmlEditorToolStripMenuItem;
        //private ToolStripMenuItem xmlEditorToolStripMenuItem1;
        
        /// <summary>
        /// Initializes XML Editor menu items and adds them to the main menu and tray menu
        /// </summary>
        private void InitializeXMLEditorMenuItems()
        {
            // Create XML Editor menu item for main menu
            xmlEditorToolStripMenuItem = new ToolStripMenuItem
            {
                Name = "xmlEditorToolStripMenuItem",
                Size = new System.Drawing.Size(199, 22),
                Text = "XML Editor"
            };
            xmlEditorToolStripMenuItem.Click += xmlEditorToolStripMenuItem_Click;
            
            // Add to main menu Tools dropdown
            if (toolsToolStripMenuItem != null && toolsToolStripMenuItem.DropDownItems != null)
            {
                toolsToolStripMenuItem.DropDownItems.Add(xmlEditorToolStripMenuItem);
                mainConsole.AppendText("[INFO] Added XML Editor to main menu\n");
            }
            
            // Create XML Editor menu item for tray menu
            xmlEditorToolStripMenuItem1 = new ToolStripMenuItem
            {
                Name = "xmlEditorToolStripMenuItem1",
                Size = new System.Drawing.Size(199, 22),
                Text = "XML Editor"
            };
            xmlEditorToolStripMenuItem1.Click += xmlEditorToolStripMenuItem_Click;
            
            // Add to tray menu Tools dropdown
            if (toolsToolStripMenuItem1 != null && toolsToolStripMenuItem1.DropDownItems != null)
            {
                toolsToolStripMenuItem1.DropDownItems.Add(xmlEditorToolStripMenuItem1);
                mainConsole.AppendText("[INFO] Added XML Editor to tray menu\n");
            }
        }
        
        /// <summary>
        /// Event handler for XML Editor menu item click
        /// </summary>
        private void xmlEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Determine which XML file to edit
                string xmlFilePath = LocateSettingsFile();
                if (string.IsNullOrEmpty(xmlFilePath))
                {
                    // Try local path as a last resort
                    xmlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vdd_settings.xml");
                    if (!File.Exists(xmlFilePath))
                    {
                        mainConsole.AppendText("[ERROR] Could not locate settings file for XML Editor.\n");
                        MessageBox.Show("Could not locate settings file. Please ensure vdd_settings.xml exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                
                mainConsole.AppendText($"[INFO] Opening XML Editor with file: {xmlFilePath}\n");
                
                // Create and show the XML Editor form
                XMLEditor xmlEditor = new XMLEditor(xmlFilePath);
                xmlEditor.Show();
                xmlEditor.BringToFront();
            }
            catch (Exception ex)
            {
                mainConsole.AppendText($"[ERROR] Failed to open XML Editor: {ex.Message}\n");
                if (ex.InnerException != null)
                {
                    mainConsole.AppendText($"[ERROR] Inner Exception: {ex.InnerException.Message}\n");
                }
                MessageBox.Show($"Error opening XML Editor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}