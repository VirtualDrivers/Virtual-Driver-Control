using System;
using System.IO;
using System.Windows.Forms;

namespace VDD_Control
{
    // Add this class to the MainWindow.cs file
    public partial class mainWindow
    {
        // Track the XML Editor form instance
        private XMLEditor? xmlEditorForm;
        
        //private ToolStripMenuItem xmlEditorToolStripMenuItem;
        //private ToolStripMenuItem xmlEditorToolStripMenuItem1;
        
        /// <summary>
        /// Initializes XML Editor menu items and adds them to the main menu and tray menu
        /// </summary>
        private void InitializeXMLEditorMenuItems()
        {
            // Create XML Editor menu item for main menu
            var xmlEditorToolStripMenuItem = new ToolStripMenuItem
            {
                Name = "xmlEditorToolStripMenuItem",
                Size = new System.Drawing.Size(199, 22),
                Text = "XML Editor"
            };
            xmlEditorToolStripMenuItem.Click += xmlEditorToolStripMenuItem_Click;
            
            // Add to main menu Tools dropdown
            if (toolsToolStripMenuItem != null && toolsToolStripMenuItem.DropDownItems != null)
            {
                // Check if it already exists
                bool exists = false;
                foreach (ToolStripItem item in toolsToolStripMenuItem.DropDownItems)
                {
                    if (item.Name == "xmlEditorToolStripMenuItem")
                    {
                        exists = true;
                        break;
                    }
                }
                
                if (!exists)
                {
                    toolsToolStripMenuItem.DropDownItems.Add(xmlEditorToolStripMenuItem);
                    mainConsole.AppendText("[INFO] Added XML Editor to main menu\n");
                }
            }
            
            // Create XML Editor menu item for tray menu
            var xmlEditorToolStripMenuItem1 = new ToolStripMenuItem
            {
                Name = "xmlEditorToolStripMenuItem1",
                Size = new System.Drawing.Size(199, 22),
                Text = "XML Editor"
            };
            xmlEditorToolStripMenuItem1.Click += xmlEditorToolStripMenuItem_Click;
            
            // Add to tray menu Tools dropdown
            if (toolsToolStripMenuItem1 != null && toolsToolStripMenuItem1.DropDownItems != null)
            {
                // Check if it already exists
                bool exists = false;
                foreach (ToolStripItem item in toolsToolStripMenuItem1.DropDownItems)
                {
                    if (item.Name == "xmlEditorToolStripMenuItem1")
                    {
                        exists = true;
                        break;
                    }
                }
                
                if (!exists)
                {
                    toolsToolStripMenuItem1.DropDownItems.Add(xmlEditorToolStripMenuItem1);
                    mainConsole.AppendText("[INFO] Added XML Editor to tray menu\n");
                }
            }
        }
        
        /// <summary>
        /// Event handler for XML Editor menu item click
        /// </summary>
        private void xmlEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowXMLEditorWindow();
        }
        
        /// <summary>
        /// Shows the XML Editor window, creating it if necessary or reusing existing instance
        /// </summary>
        private void ShowXMLEditorWindow()
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
                
                // Create the form if it doesn't exist or was disposed
                if (xmlEditorForm == null || xmlEditorForm.IsDisposed)
                {
                    xmlEditorForm = new XMLEditor(xmlFilePath);
                    
                    // Subscribe to form closed event to clean up reference
                    xmlEditorForm.FormClosed += (s, args) => 
                    {
                        // Null out the reference when the form is closed
                        if (s == xmlEditorForm)
                        {
                            xmlEditorForm = null;
                        }
                    };
                }
                else
                {
                    // If the form exists, update its file path if needed
                    xmlEditorForm.LoadXmlFile(xmlFilePath);
                    xmlEditorForm.BringToFront();
                }
                
                // Show the form
                xmlEditorForm.Show();
                xmlEditorForm.BringToFront();
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
        
        /// <summary>
        /// Dispose the XML Editor form specifically
        /// </summary>
        private void DisposeXMLEditorForm()
        {
            // Dispose XML Editor form if it exists
            if (xmlEditorForm != null && !xmlEditorForm.IsDisposed)
            {
                try
                {
                    xmlEditorForm.Close();
                    xmlEditorForm.Dispose();
                    xmlEditorForm = null;
                }
                catch (Exception ex)
                {
                    // Just log, don't rethrow as we're in cleanup code
                    mainConsole.AppendText($"[WARNING] Error disposing XML Editor: {ex.Message}\n");
                }
            }
        }
    }
}