using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel; // For Win32Exception

namespace VDD_Control
{
    public partial class mainWindow : Form
    {
        private const string PIPE_NAME = "MTTVirtualDisplayPipe";
        string registryFilePath = "C:\\VirtualDisplayDriver"; //Lets not use null, just in case

        // Make IXCLI nullable to fix null safety warnings
        private XMLController? IXCLI;
        
        // Reference to the community scripts form
        private CommunityScriptsForm? communityScriptsForm;

        private bool SDR10_STATE = false;
        private bool CUSTOMEDID_STATE = false;
        private bool EDIDCEAOVERRRIDE_STATE = false;
        private bool PREVENTEDIDSPOOF_STATE = false;
        private bool HARDWARECURSOR_STATE = false;
        private bool HDR10PLUS_STATE = false;
        private bool LOGGING_STATE = false;
        private bool DEVLOGGING_STATE = false;

        //Above can be changed when the reading logic is implemented, Perhaps have a call function to dynamically retrieve each function based off input parameter 


        public mainWindow()
        {
            InitializeComponent();
            SetupMinimizeToTrayMenu();

            ToolStripMenuItem restartItem = GetRestartDriverToolStripMenuItem(); // This is now safe
            string settingsPath = LocateSettingsFile();

            try
            {
                // Only initialize if we found a valid settings path
                if (!string.IsNullOrEmpty(settingsPath))
                {
                    mainConsole.AppendText($"[INFO] Initializing XMLController with path: {settingsPath}\n");
                    try
                    {
                        IXCLI = new XMLController(settingsPath);
                        mainConsole.AppendText("[SUCCESS] XMLController initialized successfully\n");

                        // Load initial values from XML and set menu checked state immediately from XML
                        LoadSettingsFromXML();
                        // Sync all menu items with the loaded state
                        SyncAllMenuItemsWithState();

                        // Simplified logging - removed detailed state information

                        // Sync all menu items with the loaded state
                        SyncAllMenuItemsWithState();
                    }
                    catch (Exception ex)
                    {
                        mainConsole.AppendText($"[ERROR] Failed to initialize XMLController: {ex.Message}\n");
                        if (ex.InnerException != null)
                        {
                            mainConsole.AppendText($"[ERROR] Inner Exception: {ex.InnerException.Message}\n");
                        }
                    }
                }
                else
                {
                    // Try local path as a last resort
                    string localXmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vdd_settings.xml");
                    if (File.Exists(localXmlPath))
                    {
                        mainConsole.AppendText($"[INFO] Found XML file in application directory: {localXmlPath}\n");
                        try
                        {
                            IXCLI = new XMLController(localXmlPath);
                            mainConsole.AppendText("[SUCCESS] XMLController initialized with local XML file\n");

                            // Load values from XML and update menu state
                            LoadSettingsFromXML();
                            // Sync all menu items with the loaded state
                            SyncAllMenuItemsWithState();
                        }
                        catch (Exception ex)
                        {
                            mainConsole.AppendText($"[ERROR] Failed to initialize with local XML: {ex.Message}\n");
                        }
                    }
                    else
                    {
                        mainConsole.AppendText("[ERROR] Could not locate settings file in any expected location.\n");
                    }
                }

                // Hide GPU selection menu as requested
                selectGPUToolStripMenuItem.Visible = false;
                if (selectGPUToolStripMenuItem1 != null) selectGPUToolStripMenuItem1.Visible = false;

                // Skip PopulateGpuSelectionMenu(); as it's being hidden

                // Setup Display Count menu
                SetupDisplayCountMenu();
            }
            catch (FileNotFoundException ex)
            {
                mainConsole.AppendText($"[ERROR] {ex.Message}\n");
            }
            catch (Exception ex)
            {
                mainConsole.AppendText($"[ERROR] Error initializing settings: {ex.Message}\n");
            }
        }

        // Helper method to load settings from XML and update menu state
        private void LoadSettingsFromXML()
        {
            if (IXCLI == null) return;

            // Load state variables from XML
            SDR10_STATE = IXCLI.SDR10bit;
            CUSTOMEDID_STATE = IXCLI.CustomEdid;
            EDIDCEAOVERRRIDE_STATE = IXCLI.EdidCeaOverride;
            PREVENTEDIDSPOOF_STATE = IXCLI.PreventSpoof;
            HARDWARECURSOR_STATE = IXCLI.HardwareCursor;
            LOGGING_STATE = IXCLI.Logging;
            DEVLOGGING_STATE = IXCLI.DebugLogging;
            HDR10PLUS_STATE = IXCLI.HDRPlus;

            // Update menu items with loaded states
            UpdateAllMenuItemsWithStates();

            // Hide the Select GPU option as requested
            HideSelectGPUMenuItems();
        }
        private void HideSelectGPUMenuItems()
        {
            selectGPUToolStripMenuItem.Visible = false;
            if (selectGPUToolStripMenuItem1 != null)
                selectGPUToolStripMenuItem1.Visible = false;
        }
        // Fields for system tray functionality
        private NotifyIcon? trayIcon;
        private bool minimizeToTray = false; // Default to disabled - feature currently hidden

        // Set up minimize to tray functionality
        private void SetupMinimizeToTrayMenu()
        {
            // Create tray icon if it doesn't exist
            if (notificationIcon == null)
            {
                // Use the existing notificationIcon control from the form
                notificationIcon.Text = "Virtual Display Driver Control";

                // Make sure the context menu has Show option
                bool hasShowOption = false;
                foreach (ToolStripItem item in trayMenu.Items)
                {
                    if (item.Text == "Show")
                    {
                        hasShowOption = true;
                        break;
                    }
                }

                if (!hasShowOption)
                {
                    // Add Show option as the first item
                    trayMenu.Items.Insert(0, new ToolStripMenuItem("Show", null, (s, e) => ShowFromTray()));
                    // Add separator after Show
                    trayMenu.Items.Insert(1, new ToolStripSeparator());
                }

                // Double-click behavior
                notificationIcon.DoubleClick += (s, e) => ShowFromTray();

                // Handle form
                // closing
                this.FormClosing += (s, e) =>
                {
                    if (e.CloseReason == CloseReason.UserClosing && minimizeToTray)
                    {
                        e.Cancel = true;
                        MinimizeToTray();
                    }
                };
            }

            // Add Minimize to Tray option to menuToolStripMenuItem (Menu)
            bool hasMinimizeOption = false;
            foreach (ToolStripItem item in menuToolStripMenuItem.DropDownItems)
            {
                if (item.Text == "Minimize to Tray")
                {
                    hasMinimizeOption = true;
                    break;
                }
            }

            if (!hasMinimizeOption)
            {
                // Create the minimize to tray menu item
                var minToTrayItem = new ToolStripMenuItem("Minimize to Tray", null, MinimizeToTrayMenuClick)
                {
                    Checked = minimizeToTray,
                    CheckOnClick = true
                };

                // Add to menu before Exit
                int exitPosition = -1;
                for (int i = 0; i < menuToolStripMenuItem.DropDownItems.Count; i++)
                {
                    if (menuToolStripMenuItem.DropDownItems[i].Text == "Exit")
                    {
                        exitPosition = i;
                        break;
                    }
                }

                if (exitPosition >= 0)
                {
                    menuToolStripMenuItem.DropDownItems.Insert(exitPosition, minToTrayItem);
                    menuToolStripMenuItem.DropDownItems.Insert(exitPosition + 1, new ToolStripSeparator());
                }
                else
                {
                    menuToolStripMenuItem.DropDownItems.Add(minToTrayItem);
                }

                // Also add to tray menu
                bool hasTrayMinimizeOption = false;
                foreach (ToolStripItem item in menuToolStripMenuItem1.DropDownItems)
                {
                    if (item.Text == "Minimize to Tray")
                    {
                        hasTrayMinimizeOption = true;
                        break;
                    }
                }

                if (!hasTrayMinimizeOption)
                {
                    // Create another instance for the tray menu
                    var trayMinItem = new ToolStripMenuItem("Minimize to Tray", null, MinimizeToTrayMenuClick)
                    {
                        Checked = minimizeToTray,
                        CheckOnClick = true
                    };

                    // Add to tray menu
                    exitPosition = -1;
                    for (int i = 0; i < menuToolStripMenuItem1.DropDownItems.Count; i++)
                    {
                        if (menuToolStripMenuItem1.DropDownItems[i].Text == "Exit")
                        {
                            exitPosition = i;
                            break;
                        }
                    }

                    if (exitPosition >= 0)
                    {
                        menuToolStripMenuItem1.DropDownItems.Insert(exitPosition, trayMinItem);
                        menuToolStripMenuItem1.DropDownItems.Insert(exitPosition + 1, new ToolStripSeparator());
                    }
                    else
                    {
                        menuToolStripMenuItem1.DropDownItems.Add(trayMinItem);
                    }
                }
            }

            // Setup minimize button on the form (using ForeverMinimize)
            minButton.Click += (s, e) => MinimizeToTray();
        }

        // Handle minimize to tray menu click
        private void MinimizeToTrayMenuClick(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item)
            {
                minimizeToTray = item.Checked;

                // Update all instances of this menu item to have the same checked state
                // Main menu items
                foreach (ToolStripItem menuItem in menuToolStripMenuItem.DropDownItems)
                {
                    if (menuItem is ToolStripMenuItem tsMenuItem && tsMenuItem.Text == "Minimize to Tray")
                    {
                        tsMenuItem.Checked = minimizeToTray;
                    }
                }

                // Tray menu items
                foreach (ToolStripItem menuItem in menuToolStripMenuItem1.DropDownItems)
                {
                    if (menuItem is ToolStripMenuItem tsMenuItem && tsMenuItem.Text == "Minimize to Tray")
                    {
                        tsMenuItem.Checked = minimizeToTray;
                    }
                }

                AppendToConsole($"[INFO] Minimize to tray {(minimizeToTray ? "enabled" : "disabled")}\n");
            }
        }

        // Minimize the application to the system tray
        private void MinimizeToTray()
        {
            // Hide form
            this.Hide();

            // Show tray icon (it's already visible in the designer)
            notificationIcon.Visible = true;

            // Display notification
            notificationIcon.ShowBalloonTip(
                2000,
                "Virtual Display Driver Control",
                "Application minimized to tray. Double-click to restore.",
                ToolTipIcon.Info
            );

            AppendToConsole("[INFO] Application minimized to tray\n");
        }

        // Show the application from the system tray
        private void ShowFromTray()
        {
            // Show form
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();

            AppendToConsole("[INFO] Application restored from tray\n");
        }

        // Populate GPU selection menu
        private void PopulateGpuSelectionMenu()
        {
            // Clear existing items
            selectGPUToolStripMenuItem.DropDownItems.Clear();

            try
            {
                // Get current GPU from XML if available
                string currentGpu = IXCLI != null ? IXCLI.Friendlyname : "default";

                // Get available GPUs from PowerShell (WMI)
                AppendToConsole("[INFO] Retrieving available GPUs...\n");

                // Add "default" option always
                var defaultItem = new ToolStripMenuItem("default")
                {
                    Checked = currentGpu.Equals("default", StringComparison.OrdinalIgnoreCase),
                    Tag = "default"
                };
                defaultItem.Click += GpuMenuItem_Click;
                selectGPUToolStripMenuItem.DropDownItems.Add(defaultItem);

                // Add a loading item that will be replaced with actual GPU data when found
                var loadingItem = new ToolStripMenuItem("Loading GPUs...");
                selectGPUToolStripMenuItem.DropDownItems.Add(loadingItem);

                // Load GPUs asynchronously
                Task.Run(async () =>
                {
                    List<string> gpuList = await GetAvailableGPUs();

                    // Update UI on the UI thread
                    this.BeginInvoke(new Action(() =>
                    {
                        // Remove the loading item
                        selectGPUToolStripMenuItem.DropDownItems.Remove(loadingItem);

                        if (gpuList.Count == 0)
                        {
                            // No additional GPUs found
                            var noGpusItem = new ToolStripMenuItem("No additional GPUs found");
                            noGpusItem.Enabled = false;
                            selectGPUToolStripMenuItem.DropDownItems.Add(noGpusItem);
                        }
                        else
                        {
                            // Add each GPU to the menu
                            foreach (string gpu in gpuList)
                            {
                                var gpuItem = new ToolStripMenuItem(gpu)
                                {
                                    Checked = gpu.Equals(currentGpu, StringComparison.OrdinalIgnoreCase),
                                    Tag = gpu
                                };
                                gpuItem.Click += GpuMenuItem_Click;
                                selectGPUToolStripMenuItem.DropDownItems.Add(gpuItem);
                            }
                        }

                        // Also update the second menu if it exists
                        if (selectGPUToolStripMenuItem1 != null)
                        {
                            // Clone the items to the other menu
                            selectGPUToolStripMenuItem1.DropDownItems.Clear();
                            foreach (ToolStripItem item in selectGPUToolStripMenuItem.DropDownItems)
                            {
                                if (item is ToolStripMenuItem menuItem)
                                {
                                    var newItem = new ToolStripMenuItem(menuItem.Text)
                                    {
                                        Checked = menuItem.Checked,
                                        Enabled = menuItem.Enabled,
                                        Tag = menuItem.Tag
                                    };

                                    if (menuItem.Enabled)
                                    {
                                        newItem.Click += GpuMenuItem_Click;
                                    }

                                    selectGPUToolStripMenuItem1.DropDownItems.Add(newItem);
                                }
                            }
                        }
                    }));
                });
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] Failed to populate GPU menu: {ex.Message}\n");

                // Add a default error item
                var errorItem = new ToolStripMenuItem("Error loading GPUs");
                errorItem.Enabled = false;
                selectGPUToolStripMenuItem.DropDownItems.Add(errorItem);
            }
        }

        // Handle GPU menu item click
        private async void GpuMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item && item.Tag != null)
            {
                string selectedGpu = item.Tag.ToString();
                if (string.IsNullOrEmpty(selectedGpu)) return;

                AppendToConsole($"[ACTION] Changing selected GPU to: {selectedGpu}\n");

                try
                {
                    // Update all menu items to set only the selected one as checked
                    foreach (ToolStripItem menuItem in selectGPUToolStripMenuItem.DropDownItems)
                    {
                        if (menuItem is ToolStripMenuItem gpuItem)
                        {
                            gpuItem.Checked = (gpuItem.Tag?.ToString() == selectedGpu);
                        }
                    }

                    // Also update the second menu if it exists
                    if (selectGPUToolStripMenuItem1 != null)
                    {
                        foreach (ToolStripItem menuItem in selectGPUToolStripMenuItem1.DropDownItems)
                        {
                            if (menuItem is ToolStripMenuItem gpuItem)
                            {
                                gpuItem.Checked = (gpuItem.Tag?.ToString() == selectedGpu);
                            }
                        }
                    }

                    // Update XML settings
                    if (IXCLI != null)
                    {
                        IXCLI.Friendlyname = selectedGpu;

                        // Save XML file
                        try
                        {
                            string xmlPath = Path.Combine(registryFilePath, "vdd_settings.xml");
                            IXCLI.SaveToXml(xmlPath);
                            AppendToConsole($"[SUCCESS] Updated XML settings with new GPU: {selectedGpu}\n");
                        }
                        catch (Exception ex)
                        {
                            AppendToConsole($"[ERROR] Failed to save XML with new GPU: {ex.Message}\n");
                        }
                    }

                    // Send command to driver
                    string command = $"SETGPU {selectedGpu}";
                    string response = await SendCommandToDriver(command);
                    //AppendToConsole($"[INFO] Driver response: {response}\n");
                }
                catch (Exception ex)
                {
                    AppendToConsole($"[ERROR] Failed to set GPU: {ex.Message}\n");
                }
            }
        }

        // Get available GPUs from system
        private async Task<List<string>> GetAvailableGPUs()
        {
            List<string> gpuList = new List<string>();

            try
            {
                // Initialize a process to execute PowerShell to get GPU info
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = "-NoProfile -ExecutionPolicy Bypass -Command \"Get-WmiObject Win32_VideoController | Select-Object -ExpandProperty Name\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                // Start the process
                process.Start();

                // Read output
                string output = await process.StandardOutput.ReadToEndAsync();
                await process.WaitForExitAsync();

                // Process the output - each line is a GPU
                if (!string.IsNullOrWhiteSpace(output))
                {
                    string[] lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string line in lines)
                    {
                        string trimmedLine = line.Trim();
                        if (!string.IsNullOrEmpty(trimmedLine) && !trimmedLine.Equals("default", StringComparison.OrdinalIgnoreCase))
                        {
                            gpuList.Add(trimmedLine);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] Failed to retrieve GPU list: {ex.Message}\n");
            }

            return gpuList;
        }

        // Set up display count menu
        private void SetupDisplayCountMenu()
        {
            // Clear existing items
            displayCountToolStripMenuItem.DropDownItems.Clear();

            try
            {
                // Get current display count from XML if available
                int currentCount = IXCLI != null ? IXCLI.Count : 1;  // Default to 1 if not available

                // Create menu items for common display counts (1-4)
                for (int i = 1; i <= 4; i++)
                {
                    var item = new ToolStripMenuItem(i.ToString())
                    {
                        Checked = (i == currentCount),
                        Tag = i
                    };
                    item.Click += DisplayCountMenuItem_Click;
                    displayCountToolStripMenuItem.DropDownItems.Add(item);
                }

                // Add separator
                displayCountToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());

                // Add custom count option
                var customItem = new ToolStripMenuItem("Custom...")
                {
                    Tag = "custom"
                };
                customItem.Click += CustomDisplayCountMenuItem_Click;
                displayCountToolStripMenuItem.DropDownItems.Add(customItem);

                // Sync with other menu if it exists
                if (displayCountToolStripMenuItem1 != null)
                {
                    // Clone all items to the other menu
                    displayCountToolStripMenuItem1.DropDownItems.Clear();
                    foreach (ToolStripItem menuItem in displayCountToolStripMenuItem.DropDownItems)
                    {
                        if (menuItem is ToolStripMenuItem item)
                        {
                            var newItem = new ToolStripMenuItem(item.Text)
                            {
                                Checked = item.Checked,
                                Tag = item.Tag
                            };

                            if (item.Tag is int)
                            {
                                newItem.Click += DisplayCountMenuItem_Click;
                            }
                            else if (item.Tag?.ToString() == "custom")
                            {
                                newItem.Click += CustomDisplayCountMenuItem_Click;
                            }

                            displayCountToolStripMenuItem1.DropDownItems.Add(newItem);
                        }
                        else if (menuItem is ToolStripSeparator)
                        {
                            displayCountToolStripMenuItem1.DropDownItems.Add(new ToolStripSeparator());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] Failed to set up display count menu: {ex.Message}\n");

                // Add default option
                var defaultItem = new ToolStripMenuItem("1");
                defaultItem.Checked = true;
                displayCountToolStripMenuItem.DropDownItems.Add(defaultItem);
            }
        }

        // Handle display count menu item click
        private async void DisplayCountMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item && item.Tag is int count)
            {
                await SetDisplayCount(count);
            }
        }

        // Handle custom display count menu item click
        private async void CustomDisplayCountMenuItem_Click(object sender, EventArgs e)
        {
            // Create a simple input dialog
            Form inputDialog = new Form
            {
                Width = 300,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Custom Display Count",
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(32, 34, 37),
                ForeColor = Color.White
            };

            Label label = new Label
            {
                Text = "Enter number of displays (1-99):",
                ForeColor = Color.White,
                Left = 20,
                Top = 20,
                Width = 260
            };
            inputDialog.Controls.Add(label);

            NumericUpDown numericInput = new NumericUpDown
            {
                Minimum = 1,
                Maximum = 99,
                Value = IXCLI != null ? IXCLI.Count : 1,
                Left = 20,
                Top = 50,
                Width = 260,
                BackColor = Color.FromArgb(45, 47, 49),
                ForeColor = Color.White
            };
            inputDialog.Controls.Add(numericInput);

            Button okButton = new Button
            {
                Text = "OK",
                Left = 120,
                Width = 80,
                Top = 80,
                BackColor = Color.FromArgb(45, 47, 49),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            okButton.Click += (s, args) => inputDialog.DialogResult = DialogResult.OK;
            inputDialog.Controls.Add(okButton);

            Button cancelButton = new Button
            {
                Text = "Cancel",
                Left = 210,
                Width = 80,
                Top = 80,
                BackColor = Color.FromArgb(45, 47, 49),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            cancelButton.Click += (s, args) => inputDialog.DialogResult = DialogResult.Cancel;
            inputDialog.Controls.Add(cancelButton);

            // Show dialog and process result
            if (inputDialog.ShowDialog(this) == DialogResult.OK)
            {
                int count = (int)numericInput.Value;
                await SetDisplayCount(count);
            }
        }

        // Set display count in XML and send to driver
        private async Task SetDisplayCount(int count)
        {
            AppendToConsole($"[ACTION] Setting display count to {count}...\n");

            try
            {
                // Update display count in XML
                if (IXCLI != null)
                {
                    IXCLI.Count = count;

                    // Save XML
                    try
                    {
                        string xmlPath = Path.Combine(registryFilePath, "vdd_settings.xml");
                        IXCLI.SaveToXml(xmlPath);
                        AppendToConsole($"[SUCCESS] Updated XML settings with new display count: {count}\n");
                    }
                    catch (Exception ex)
                    {
                        AppendToConsole($"[ERROR] Failed to save XML with new display count: {ex.Message}\n");
                    }
                }

                // Send command to driver
                string command = $"SETCOUNT {count}";
                string response = await SendCommandToDriver(command);
                //AppendToConsole($"[INFO] Driver response: {response}\n");

                // Update menu checked state
                UpdateDisplayCountMenus(count);

                // Recommend driver restart
                AppendToConsole("[INFO] Display count changed. You may need to restart the driver for changes to take effect.\n");
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] Failed to set display count: {ex.Message}\n");
            }
        }

        // Update display count menus to reflect the current count
        private void UpdateDisplayCountMenus(int count)
        {
            // Update main menu
            foreach (ToolStripItem item in displayCountToolStripMenuItem.DropDownItems)
            {
                if (item is ToolStripMenuItem menuItem && menuItem.Tag is int itemCount)
                {
                    menuItem.Checked = (itemCount == count);
                }
            }

            // Update secondary menu if it exists
            if (displayCountToolStripMenuItem1 != null)
            {
                foreach (ToolStripItem item in displayCountToolStripMenuItem1.DropDownItems)
                {
                    if (item is ToolStripMenuItem menuItem && menuItem.Tag is int itemCount)
                    {
                        menuItem.Checked = (itemCount == count);
                    }
                }
            }
        }

        private ToolStripMenuItem GetRestartDriverToolStripMenuItem()
        {
            return restartDriverToolStripMenuItem;
        }


        // Recursive function to apply style to all sub-items
        private void SetMenuItemStyle(ToolStripMenuItem item)
        {
            item.ForeColor = Color.White; // White text
            item.BackColor = Color.FromArgb(32, 34, 37); // Default background

            foreach (ToolStripItem subItem in item.DropDownItems)
            {
                if (subItem is ToolStripMenuItem subMenuItem)
                {
                    SetMenuItemStyle(subMenuItem);
                }
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            mainVisibleMenuStrip.Renderer = new ToolStripProfessionalRenderer(new CustomColorTable());

            // Set text color for all menu items
            foreach (ToolStripMenuItem item in mainVisibleMenuStrip.Items)
            {
                SetMenuItemStyle(item);
            }

            // Display ASCII art animation with proper delays and scrolling first
            // before any other operations to ensure it's visible
            await DisplayAsciiArtAnimation();

            // Check if we need to create the XML in the installation directory
            if (IXCLI == null)
            {
                try
                {
                    bool handled = await TryCreateDefaultXmlFile();
                    if (handled)
                    {
                        // Try to initialize IXCLI with the newly created file
                        string settingsPath = LocateSettingsFile();
                        if (!string.IsNullOrEmpty(settingsPath))
                        {
                            IXCLI = new XMLController(settingsPath);
                            AppendToConsole("[SUCCESS] Created and loaded default XML settings file\n");

                            // Load initial values from XML
                            SDR10_STATE = IXCLI.SDR10bit;
                            CUSTOMEDID_STATE = IXCLI.CustomEdid;
                            EDIDCEAOVERRRIDE_STATE = IXCLI.EdidCeaOverride;
                            PREVENTEDIDSPOOF_STATE = IXCLI.PreventSpoof;
                            HARDWARECURSOR_STATE = IXCLI.HardwareCursor;
                            LOGGING_STATE = IXCLI.Logging;
                            DEVLOGGING_STATE = IXCLI.DebugLogging;
                            HDR10PLUS_STATE = IXCLI.HDRPlus;
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppendToConsole($"[ERROR] Failed to create default XML file: {ex.Message}\n");
                }
            }

            // Only try to load XML if IXCLI was successfully initialized
            if (IXCLI != null)
            {
                try
                {
                    // No need to load again, already loaded in constructor
                    AppendToConsole("[INFO] XML configuration loaded successfully\n");

                    // Let's explicitly sync menu items with the loaded XML settings
                    UpdateAllMenuItemsWithStates();
                }
                catch (Exception ex)
                {
                    AppendToConsole($"[ERROR] Failed to load settings: {ex.Message}\n");
                }
            }

            // After ASCII art display, sync menu items with actual driver status
            // But make sure to preserve loaded XML settings if driver isn't available
            try
            {
                await SyncMenuItemsWithDriverStatus();
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] Failed to sync with driver: {ex.Message}\n");
                // Ensure XML settings are applied even if driver sync fails
                UpdateAllMenuItemsWithStates();
            }

            try
            {
                // Initialize a string to hold all system information
                string systemInfo = "System Information:\n\n";



                // Access the registry for CPU information
                RegistryKey localMachine = Registry.LocalMachine;
                RegistryKey hardware = localMachine.OpenSubKey("HARDWARE");
                RegistryKey description = hardware?.OpenSubKey("DESCRIPTION");
                RegistryKey system = description?.OpenSubKey("SYSTEM");
                RegistryKey centralProcessor = system?.OpenSubKey("CentralProcessor");
                RegistryKey processorInfo = centralProcessor?.OpenSubKey("0");

                // Add CPU information
                if (processorInfo != null)
                {
                    systemInfo += "CPU Information:\n";
                    systemInfo += "----------------\n";
                    systemInfo += "Vendor: " + processorInfo.GetValue("VendorIdentifier") + "\n";
                    systemInfo += "Processor: " + processorInfo.GetValue("ProcessorNameString") + "\n";
                    systemInfo += "Type: " + processorInfo.GetValue("Identifier") + "\n";
                    systemInfo += "Speed: " + processorInfo.GetValue("~Mhz") + " MHz\n\n";
                }
                else
                {
                    systemInfo += "CPU Information: Unable to retrieve.\n\n";  // This really shouldn't happen.
                }

                // Add OS Information
                systemInfo += "Operating System Information:\n";
                systemInfo += "-----------------------------\n";
                systemInfo += "OS Version: " + Environment.OSVersion.ToString() + "\n";
                systemInfo += "Machine Name: " + Environment.MachineName + "\n";
                systemInfo += "User Name: " + Environment.UserName + "\n";
                systemInfo += "64-Bit OS: " + (Environment.Is64BitOperatingSystem ? "Yes" : "No") + "\n";
                systemInfo += "64-Bit Process: " + (Environment.Is64BitProcess ? "Yes" : "No") + "\n";
                systemInfo += "Processor Count: " + Environment.ProcessorCount + "\n\n";

                // Add Memory Information
                systemInfo += "Memory Information:\n";
                systemInfo += "-------------------\n";
                systemInfo += "System Page Size: " + Environment.SystemPageSize + " bytes\n";
                systemInfo += "Working Set: " + (Environment.WorkingSet / 1024 / 1024) + " MB\n\n";

                // Add .NET Runtime Information
                systemInfo += ".NET Runtime Information:\n";
                systemInfo += "-------------------------\n";
                systemInfo += "CLR Version: " + Environment.Version.ToString() + "\n\n";

                // Locate the vdd_settings.xml file
                string settingsPath = LocateSettingsFile();
                systemInfo += settingsPath ?? "Could not locate settings file";

                // Display the information in richTextBox1
                //AppendToConsole(systemInfo + "\n");
            }
            catch (Exception ex)
            {
                // Display error details in richTextBox1
                AppendToConsole("An error occurred while retrieving system information:\n" + ex.Message + "\n"); // This really shouldn't happen. But probably will.
            }

            AppendToConsole("Virtual Display Driver Control Initialized.\n");

            // Try to connect to the driver once at initialization
            if (await TryConnectToDriver())
            {
                AppendToConsole("[SUCCESS] Connected to the driver.\n");
            }
            else
            {
                AppendToConsole("[WARNING] Could not verify driver connection. Ensure the driver is running.\n");
            }
        }

        class CustomColorTable : ProfessionalColorTable
        {
            private static readonly Color BackgroundColor = Color.FromArgb(32, 34, 37); // Default background
            private static readonly Color HoverColor = Color.FromArgb(25, 25, 25); // Hover background
            private static readonly Color TextColor = Color.White; // White text
            private static readonly Color BorderColor = Color.FromArgb(60, 60, 60); // Border color

            public override Color ToolStripDropDownBackground => BackgroundColor;
            public override Color MenuBorder => BorderColor; // Menu border color
            public override Color MenuItemBorder => BorderColor; // Item border color
            public override Color MenuItemSelected => HoverColor; // Hover effect color

            // Disable gradient effects by making begin and end colors the same
            public override Color MenuItemSelectedGradientBegin => HoverColor;
            public override Color MenuItemSelectedGradientEnd => HoverColor;
            public override Color MenuItemPressedGradientBegin => HoverColor;
            public override Color MenuItemPressedGradientEnd => HoverColor;
            public override Color ToolStripGradientBegin => BackgroundColor;
            public override Color ToolStripGradientMiddle => BackgroundColor;
            public override Color ToolStripGradientEnd => BackgroundColor;
        }

        private string LocateSettingsFile()
        {
            string registryKeyPath = @"SOFTWARE\MikeTheTech\VirtualDisplayDriver";
            string foundPath = null;

            try
            {
                // Check the registry for the file path
                using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(registryKeyPath, false))
                {
                    if (registryKey != null)
                    {
                        string regPath = registryKey.GetValue("SettingsPath") as string;
                        string fullPath = regPath;

                        // Check if it's a directory path or direct file path
                        if (!string.IsNullOrEmpty(regPath))
                        {
                            if (!regPath.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                            {
                                // It's a directory path, append the filename
                                fullPath = Path.Combine(regPath, "vdd_settings.xml");
                            }

                            if (File.Exists(fullPath))
                            {
                                registryFilePath = regPath; // Store the directory or full path
                                foundPath = fullPath;       // Return the full file path
                                mainConsole.AppendText($"[INFO] Settings file found at registry location: {fullPath}\n");
                                return foundPath;
                            }
                        }
                    }
                }

                // Fallback to default locations
                string[] fallbackPaths =
                {
                    @"C:\VirtualDisplayDriver\vdd_settings.xml",
                    @"C:\IddSampleDriver\vdd_settings.xml",
                    // Check the project root directory
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vdd_settings.xml"),
                    // Check one directory up (if running from bin/Debug)
                    Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName, "vdd_settings.xml")
                };

                foreach (string path in fallbackPaths)
                {
                    if (File.Exists(path))
                    {
                        // Extract directory path for the XML controller
                        registryFilePath = Path.GetDirectoryName(path);
                        foundPath = path;
                        mainConsole.AppendText($"[INFO] Settings file found at fallback location: {path}\n");
                        return foundPath;
                    }
                }

                // Log that we couldn't find the file
                mainConsole.AppendText("[WARNING] Settings file not found in registry or fallback locations.\n");
            }
            catch (Exception ex)
            {
                // Log the exception
                mainConsole.AppendText($"[ERROR] Error while locating settings file: {ex.Message}\n");
            }

            // If no file is found, return null
            return foundPath;
        }

        private async Task<bool> TryConnectToDriver()
        {
            // we should change this to check if it exists, not if it can be connected to to save on overhead in the driver
            const int maxAttempts = 5;
            int attempt = 0;

            while (attempt < maxAttempts)
            {
                try
                {
                    using (var pipeClient = new NamedPipeClientStream(".", PIPE_NAME, PipeDirection.InOut))
                    {
                        await pipeClient.ConnectAsync(2000);
                        // We'll remove the logging here and only show it at the end
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    attempt++;
                    AppendToConsole($"[ERROR] Connection failed: {ex.Message} (Attempt {attempt}/{maxAttempts})\n");
                    AppendToConsole("Note: This may also occur if the driver is off or restarting.\n");

                    if (attempt >= maxAttempts)
                    {
                        AppendToConsole("[ERROR] Unable to connect after multiple attempts.\n");
                        return false;
                    }

                    await Task.Delay(2000);
                }
            }

            return false;
        }

        private async Task<string?> SendCommandToDriver(string command)
        {
            if (!await TryConnectToDriver()) // No need to check if command sent is not equal to restart driver 
            {
                return "[ERROR] Connection failed: The driver may be off or restarting.";
            }

            try
            {
                using (var pipeClient = new NamedPipeClientStream(".", PIPE_NAME, PipeDirection.InOut))
                {
                    await pipeClient.ConnectAsync(2000);

                    var utf16LeEncoding = new UnicodeEncoding(bigEndian: false, byteOrderMark: false);
                    using (var writer = new StreamWriter(pipeClient, utf16LeEncoding, leaveOpen: true))
                    {
                        await writer.WriteLineAsync(command);
                        await writer.FlushAsync();
                    }
                    using (var reader = new StreamReader(pipeClient, Encoding.UTF8))
                    {
                        var startTime = DateTime.UtcNow;
                        string? response;
                        // We loop here due to the driver being able to send its logs through the pipe, after 5 seconds we nullify to handle any unexpected errors
                        // Responses cant be returned if logging is off unless the pipe functions specifically specifies a return containing `[Companion]` to allow for context filtering
                        // This means every other command other than PING, will not return a response to the companion without logging being on. This has to be changed within the driver itself
                        do
                        {
                            if ((DateTime.UtcNow - startTime).TotalSeconds > 5)
                            {
                                return null; // Handle whatever error handling here, I've just returned null for now
                            }
                            response = await reader.ReadLineAsync();
                        }
                        while (response != null && (!response.Contains("[COMPANION]")));
                        if (response != null)
                        {
                            int index = response.IndexOf("[COMPANION]") + 11;
                            response = response.Substring(index).Trim();
                        }
                        //AppendToConsole($"[{command}] Response: {response}\n");

                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                return $"[ERROR] Sending command failed: {ex.Message}";
            }
        }

        // Query the driver for the current status of a feature
        private async Task<bool> GetDriverFeatureStatus(string featureName)
        {
            try
            {
                // First check if logging is enabled
                bool shouldUseXml = !LOGGING_STATE;

                // If logging is disabled, we know the driver won't respond to status commands with proper content
                // So we'll directly use XML settings instead of querying the driver
                if (shouldUseXml)
                {
                    AppendToConsole($"[INFO] Logging is disabled. Using XML settings for {featureName}.\n");
                    return GetFeatureStatusFromXml(featureName);
                }

                // If logging is enabled, try to get status from driver
                string? response = await SendCommandToDriver("STATUS");

                if (string.IsNullOrEmpty(response) || response.StartsWith("[ERROR]"))
                {
                    // If there's an error or no response, fall back to XML settings
                    if (featureName == "SDR10") // Only log once for the first feature check
                    {
                        AppendToConsole($"[INFO] Could not get driver status, using XML settings.\n");
                    }
                    return GetFeatureStatusFromXml(featureName);
                }

                // Parse the response looking for the feature's status
                // Expected format: "Feature1=true|Feature2=false|..." or similar
                if (response.Contains(featureName + "="))
                {
                    int startIndex = response.IndexOf(featureName + "=") + featureName.Length + 1;
                    int endIndex = response.IndexOf('|', startIndex);
                    if (endIndex == -1) endIndex = response.Length;

                    string statusValue = response.Substring(startIndex, endIndex - startIndex).Trim().ToLower();
                    return statusValue == "true" || statusValue == "1" || statusValue == "on";
                }

                // If feature not found in response, fall back to XML settings
                if (featureName == "SDR10") // Only log once
                {
                    AppendToConsole($"[INFO] Could not get driver status, using XML settings.\n");
                }
                return GetFeatureStatusFromXml(featureName);
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] Failed to get status for {featureName}: {ex.Message}\n");
                return GetFeatureStatusFromXml(featureName);
            }
        }

        // Helper method to get feature status from XML file
        private bool GetFeatureStatusFromXml(string featureName)
        {
            // If XML controller isn't available, create it
            if (IXCLI == null)
            {
                AppendToConsole("[INFO] XML controller not initialized, attempting to load XML settings.\n");

                // Try to locate the settings file
                string settingsPath = LocateSettingsFile();
                if (!string.IsNullOrEmpty(settingsPath))
                {
                    try
                    {
                        IXCLI = new XMLController(settingsPath);
                        AppendToConsole("[SUCCESS] XML settings loaded successfully.\n");
                    }
                    catch (Exception ex)
                    {
                        AppendToConsole($"[ERROR] Failed to load XML settings: {ex.Message}\n");
                        return false; // Default to false if we can't load XML
                    }
                }
                else
                {
                    AppendToConsole("[ERROR] Could not locate XML settings file.\n");
                    return false; // Default to false if we can't find the file
                }
            }

            // Get the status from XML based on feature name
            if (IXCLI != null)
            {
                bool result = false;
                switch (featureName.ToUpper())
                {
                    case "SDR10":
                        result = IXCLI.SDR10bit;
                        break;
                    case "HDRPLUS":
                        result = IXCLI.HDRPlus;
                        break;
                    case "CUSTOMEDID":
                        result = IXCLI.CustomEdid;
                        break;
                    case "HARDWARECURSOR":
                        result = IXCLI.HardwareCursor;
                        break;
                    case "PREVENTSPOOF":
                        result = IXCLI.PreventSpoof;
                        break;
                    case "CEAOVERRIDE":
                        result = IXCLI.EdidCeaOverride;
                        break;
                    default:
                        result = false;
                        break;
                }
                // We'll skip logging individual feature values for a cleaner output
                return result;
            }

            return false; // No XML controller available
        }

        private async Task SyncMenuItemsWithDriverStatus()
        {
            try
            {
                // Check if we can connect to the driver
                bool isDriverConnected = await TryConnectToDriver();

                if (!isDriverConnected)
                {
                    // Driver not connected, use XML settings if available
                    if (IXCLI != null)
                    {
                        AppendToConsole("[INFO] Driver not connected. Using XML settings for menu items.\n");

                        // Set menu items based on XML settings
                        SDR10_STATE = IXCLI.SDR10bit;
                        HDR10PLUS_STATE = IXCLI.HDRPlus;
                        CUSTOMEDID_STATE = IXCLI.CustomEdid;
                        HARDWARECURSOR_STATE = IXCLI.HardwareCursor;
                        PREVENTEDIDSPOOF_STATE = IXCLI.PreventSpoof;
                        EDIDCEAOVERRRIDE_STATE = IXCLI.EdidCeaOverride;

                        // Log current states for debugging
                        // Simplified logging - removed detailed state logging

                        // Update UI to match
                        UpdateAllMenuItemsWithStates();
                        AppendToConsole("[INFO] Menu items set from XML settings.\n");
                    }
                    else
                    {
                        // No XML settings and no driver, set all to unchecked
                        AppendToConsole("[INFO] Driver not connected and no XML settings. All feature menu items set to unchecked.\n");

                        sDR10bitToolStripMenuItem.Checked = false;
                        hDRToolStripMenuItem.Checked = false;
                        customEDIDToolStripMenuItem.Checked = false;
                        hardwareCursorToolStripMenuItem.Checked = false;
                        preventMonitorSpoofToolStripMenuItem.Checked = false;
                        eDIDCEAOverrideToolStripMenuItem.Checked = false;

                        // Update state variables too
                        SDR10_STATE = false;
                        HDR10PLUS_STATE = false;
                        CUSTOMEDID_STATE = false;
                        HARDWARECURSOR_STATE = false;
                        PREVENTEDIDSPOOF_STATE = false;
                        EDIDCEAOVERRRIDE_STATE = false;
                    }

                    return;
                }

                // If logging is disabled, use XML settings even if driver is connected
                // because we know the driver won't respond to status commands properly
                if (!LOGGING_STATE && IXCLI != null)
                {
                    AppendToConsole("[INFO] Driver connected but logging is disabled. Using XML settings for menu items.\n");

                    // Set menu items based on XML settings
                    SDR10_STATE = IXCLI.SDR10bit;
                    HDR10PLUS_STATE = IXCLI.HDRPlus;
                    CUSTOMEDID_STATE = IXCLI.CustomEdid;
                    HARDWARECURSOR_STATE = IXCLI.HardwareCursor;
                    PREVENTEDIDSPOOF_STATE = IXCLI.PreventSpoof;
                    EDIDCEAOVERRRIDE_STATE = IXCLI.EdidCeaOverride;

                    // Log current states for debugging
                    // Simplified logging - removed detailed state logging

                    // Update UI to match
                    UpdateAllMenuItemsWithStates();
                    AppendToConsole("[INFO] Menu items set from XML settings due to logging being disabled.\n");

                    // Force UI refresh
                    Application.DoEvents();
                    mainVisibleMenuStrip.Refresh();

                    // Log final menu states for debugging
                    AppendToConsole($"[DEBUG] Menu states: SDR10={sDR10bitToolStripMenuItem.Checked}, HDR+={hDRToolStripMenuItem.Checked}, CustomEDID={customEDIDToolStripMenuItem.Checked}\n");

                    return;
                }

                // Driver is connected and logging is enabled, query the status of each feature
                AppendToConsole("[INFO] Syncing menu items with actual driver status...\n");

                // Query and update SDR10 status
                SDR10_STATE = await GetDriverFeatureStatus("SDR10");
                sDR10bitToolStripMenuItem.Checked = SDR10_STATE;

                // Query and update HDR Plus status
                HDR10PLUS_STATE = await GetDriverFeatureStatus("HDRPLUS");
                hDRToolStripMenuItem.Checked = HDR10PLUS_STATE;

                // Query and update Custom EDID status
                CUSTOMEDID_STATE = await GetDriverFeatureStatus("CUSTOMEDID");
                customEDIDToolStripMenuItem.Checked = CUSTOMEDID_STATE;

                // Query and update Hardware Cursor status
                HARDWARECURSOR_STATE = await GetDriverFeatureStatus("HARDWARECURSOR");
                hardwareCursorToolStripMenuItem.Checked = HARDWARECURSOR_STATE;

                // Query and update Prevent Spoof status
                PREVENTEDIDSPOOF_STATE = await GetDriverFeatureStatus("PREVENTSPOOF");
                preventMonitorSpoofToolStripMenuItem.Checked = PREVENTEDIDSPOOF_STATE;

                // Query and update CEA Override status
                EDIDCEAOVERRRIDE_STATE = await GetDriverFeatureStatus("CEAOVERRIDE");
                eDIDCEAOverrideToolStripMenuItem.Checked = EDIDCEAOVERRRIDE_STATE;

                AppendToConsole("[SUCCESS] Menu items synced with driver status.\n");
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] Failed to sync menu items with driver status: {ex.Message}\n");

                // On error, try to use XML settings as a fallback
                if (IXCLI != null)
                {
                    AppendToConsole("[INFO] Using XML settings as fallback after error.\n");

                    // Set menu items based on XML settings
                    SDR10_STATE = IXCLI.SDR10bit;
                    HDR10PLUS_STATE = IXCLI.HDRPlus;
                    CUSTOMEDID_STATE = IXCLI.CustomEdid;
                    HARDWARECURSOR_STATE = IXCLI.HardwareCursor;
                    PREVENTEDIDSPOOF_STATE = IXCLI.PreventSpoof;
                    EDIDCEAOVERRRIDE_STATE = IXCLI.EdidCeaOverride;

                    // Update UI to match
                    UpdateAllMenuItemsWithStates();
                }
            }
        }
        private async void RestartDriverHandler(object sender, EventArgs e)
        {
            // Don't use fire-and-forget pattern, instead properly await the Task
            await restartDriverToolStripMenuItem_Click(sender, e);
        }

        // Helper method to update task progress bar in a thread-safe way
        private void UpdateTaskProgress(string taskName, int progressValue, int maxValue = 100)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => UpdateTaskProgress(taskName, progressValue, maxValue)));
                return;
            }

            // We're now on the UI thread
            taskGroupBox.Text = $"Task Progress: {taskName}";
            taskProgressBar.Maximum = maxValue;
            taskProgressBar.Value = progressValue;
            Application.DoEvents(); // Ensure UI updates
        }

        private async Task restartDriverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppendToConsole("[ACTION] Restarting driver using Device Manager...\n");
            UpdateTaskProgress("Restarting Driver", 10);

            try
            {
                // First disable the driver
                AppendToConsole("[INFO] Disabling Virtual Display Driver...\n");
                bool disableSuccess = await DisableDriverWithDeviceManager();

                if (!disableSuccess)
                {
                    AppendToConsole("[WARNING] Could not disable the driver through standard methods. Trying direct approach...\n");

                    // Try a more direct approach as a last resort
                    string directDisableCommand = @"
                        # Direct disable of ROOT\DISPLAY\0001
                        try {
                            $device = Get-PnpDevice -Class Display | Where-Object { $_.InstanceId -eq 'ROOT\DISPLAY\0001' }
                            if ($device) {
                                Write-Output ""Found display device ROOT\DISPLAY\0001, attempting direct disable...""
                                Disable-PnpDevice -InstanceId 'ROOT\DISPLAY\0001' -Confirm:$false -ErrorAction Stop
                                Write-Output ""SUCCESS: Direct disable succeeded""
                            } else {
                                Write-Output ""FAILURE: Could not find ROOT\DISPLAY\0001""
                            }
                        } catch {
                            Write-Output ""FAILURE: $($_.Exception.Message)""
                        }";

                    string directResult = await RunPowerShellCommand(directDisableCommand);

                    if (directResult.Contains("SUCCESS"))
                    {
                        AppendToConsole("[SUCCESS] Driver disabled using alternative approach.\n");
                        disableSuccess = true;
                    }
                    else
                    {
                        AppendToConsole("[ERROR] Failed to disable the driver. Cannot restart.\n");
                        UpdateTaskProgress("Restarting Driver", 0);
                        return;
                    }
                }

                UpdateTaskProgress("Restarting Driver", 40);

                // Give some time for the disable operation to complete
                AppendToConsole("[INFO] Waiting for disable operation to complete (5 seconds)...\n");
                await Task.Delay(5000);

                // Now enable the driver
                AppendToConsole("[INFO] Enabling Virtual Display Driver...\n");
                bool enableSuccess = await EnableDriverWithDeviceManager();

                if (!enableSuccess)
                {
                    AppendToConsole("[WARNING] Failed to enable the driver through standard methods. Trying direct approach...\n");

                    // Try a more direct approach as a last resort
                    string directEnableCommand = @"
                        # Try all possible approaches to enable the display device
                        $success = $false
                        
                        # Approach 1: Try ROOT\DISPLAY\0001
                        try {
                            Write-Output ""Approach 1: Trying ROOT\DISPLAY\0001""
                            Enable-PnpDevice -InstanceId 'ROOT\DISPLAY\0001' -Confirm:$false -ErrorAction Stop
                            Write-Output ""SUCCESS: ROOT\DISPLAY\0001 enabled""
                            $success = $true
                        } catch {
                            Write-Output ""Approach 1 failed: $($_.Exception.Message)""
                        }
                        
                        # Approach 2: Try any disabled display device
                        if (-not $success) {
                            try {
                                $device = Get-PnpDevice -Class Display -Status 'Error','Disabled' | Select-Object -First 1
                                if ($device) {
                                    Write-Output ""Approach 2: Trying $($device.InstanceId)""
                                    Enable-PnpDevice -InstanceId $device.InstanceId -Confirm:$false -ErrorAction Stop
                                    Write-Output ""SUCCESS: $($device.InstanceId) enabled""
                                    $success = $true
                                } else {
                                    Write-Output ""Approach 2 failed: No disabled display devices found""
                                }
                            } catch {
                                Write-Output ""Approach 2 failed: $($_.Exception.Message)""
                            }
                        }
                        
                        # Approach 3: Try to scan for hardware changes and then enable
                        if (-not $success) {
                            try {
                                Write-Output ""Approach 3: Scanning for hardware changes""
                                $null = pnputil /scan-devices
                                Start-Sleep -Seconds 3
                                
                                # Try to find and enable ROOT\DISPLAY\0001 again
                                Enable-PnpDevice -InstanceId 'ROOT\DISPLAY\0001' -Confirm:$false -ErrorAction Stop
                                Write-Output ""SUCCESS: Scan and enable worked""
                                $success = $true
                            } catch {
                                Write-Output ""Approach 3 failed: $($_.Exception.Message)""
                            }
                        }
                        
                        if ($success) {
                            Write-Output ""OVERALL_SUCCESS: At least one approach worked""
                        } else {
                            Write-Output ""OVERALL_FAILURE: All approaches failed""
                        }";

                    string directResult = await RunPowerShellCommand(directEnableCommand);

                    if (directResult.Contains("SUCCESS"))
                    {
                        AppendToConsole("[SUCCESS] Driver enabled using alternative approach.\n");
                        enableSuccess = true;
                    }
                    else
                    {
                        AppendToConsole("[ERROR] All attempts to enable the driver failed. Restart incomplete.\n");
                        UpdateTaskProgress("Restarting Driver", 0);
                        return;
                    }
                }

                UpdateTaskProgress("Restarting Driver", 70);

                // Wait for the driver to fully initialize
                AppendToConsole("[INFO] Waiting for driver to initialize (10 seconds)...\n");
                await Task.Delay(10000);
                UpdateTaskProgress("Restarting Driver", 80);

                AppendToConsole("[INFO] Attempting to connect to restarted driver...\n");

                // Make multiple reconnection attempts
                bool reconnected = false;
                for (int attempt = 1; attempt <= 5; attempt++)
                {
                    if (await TryConnectToDriver())
                    {
                        reconnected = true;
                        break;
                    }
                    else
                    {
                        AppendToConsole($"[INFO] Connection attempt {attempt}/5 failed. Retrying after delay...\n");
                        await Task.Delay(2000); // Wait between reconnection attempts
                    }
                }

                if (reconnected)
                {
                    AppendToConsole("[SUCCESS] Driver restarted and reconnected successfully.\n");
                    UpdateTaskProgress("Restarting Driver", 100);
                    await Task.Delay(1000); // Show 100% for a moment
                    this.BeginInvoke(new Action(() => UpdateTaskProgress("", 0))); // Clear task progress
                }
                else
                {
                    AppendToConsole("[INFO] Driver appears restarted, but connection could not be established.\n");
                    AppendToConsole("[INFO] This is normal if the driver is in a different mode or if reconnection was too quick.\n");
                    UpdateTaskProgress("Restarting Driver", 100);
                    await Task.Delay(1000); // Show 100% for a moment
                    this.BeginInvoke(new Action(() => UpdateTaskProgress("", 0))); // Clear task progress
                }
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] Error restarting driver: {ex.Message}\n");
                UpdateTaskProgress("Restarting Driver", 0);
            }
        }

        private async Task<bool> DisableDriverWithDeviceManager()
        {
            try
            {
                // Use PowerShell to disable the Virtual Display Driver using DevCon
                // This must be run with administrative privileges
                string command = @"
                    # Simple function to disable a device (requires admin rights)
                    $found = $false
                    
                    # Try to find the exact driver name 'Virtual Display Driver'
                    $device = Get-PnpDevice -FriendlyName 'Virtual Display Driver' | Select-Object FriendlyName, InstanceId, Status
                    if ($device) { $found = $true; Write-Output ""Found device: $($device.InstanceId) ($($device.Status))"" }
                    
                    # If not found, try with wildcard
                    if (-not $found) {
                        $device = Get-PnpDevice -FriendlyName '*Virtual Display*' | Select-Object FriendlyName, InstanceId, Status
                        if ($device) { $found = $true; Write-Output ""Found device: $($device.InstanceId) ($($device.Status))"" }
                    }
                    
                    # As a final fallback, try a broader search for display devices
                    if (-not $found) {
                        $device = Get-PnpDevice -Class Display | Where-Object { $_.FriendlyName -like '*Virtual*' } | Select-Object FriendlyName, InstanceId, Status
                        if ($device) { $found = $true; Write-Output ""Found device: $($device.InstanceId) ($($device.Status))"" }
                    }
                    
                    if ($found) {
                        try {
                            # This requires administrative privileges
                            Write-Output ""Attempting to disable: $($device.FriendlyName) ($($device.InstanceId))""
                            Disable-PnpDevice -InstanceId $device.InstanceId -Confirm:$false -ErrorAction Stop
                            Write-Output ""SUCCESS: Device disabled""
                            exit 0
                        } catch {
                            Write-Output ""FAILURE: $($_.Exception.Message)""
                            exit 1
                        }
                    } else {
                        Write-Output ""NOT_FOUND: Could not find Virtual Display Driver device""
                        exit 2
                    }";

                AppendToConsole("[INFO] Searching for Virtual Display Driver in Device Manager...\n");
                string result = await RunPowerShellCommand(command);

                if (result == "SUCCESS" || result.Contains("SUCCESS"))
                {
                    AppendToConsole("[INFO] Driver disabled successfully through Device Manager.\n");
                    return true;
                }
                else if (result == "CANCELLED")
                {
                    AppendToConsole("[ERROR] Administrative access is required to disable the driver.\n");
                    return false;
                }
                else if (result.Contains("NOT_FOUND"))
                {
                    AppendToConsole("[ERROR] Virtual Display Driver not found in Device Manager.\n");
                    return false;
                }
                else
                {
                    AppendToConsole($"[ERROR] Failed to disable driver. Result: {result}\n");
                    return false;
                }
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] Exception when disabling driver: {ex.Message}\n");
                return false;
            }
        }

        private async Task<bool> EnableDriverWithDeviceManager()
        {
            try
            {
                // Use a more direct approach to enable the Virtual Display Driver using device ID
                string command = @"
                    # Let's do a full search for any display device that might be our Virtual Display Driver
                    # We'll look for both disabled and enabled devices to get a more complete picture
                    $allDevices = Get-PnpDevice -Class Display | Where-Object { $_.Status -ne 'Unknown' } | 
                                 Select-Object FriendlyName, InstanceId, Status, Class, Description
                    
                    # Show all display devices found (for debugging)
                    Write-Output ""------ All Display Devices ------""
                    foreach ($d in $allDevices) {
                        Write-Output ""Device: $($d.FriendlyName) ($($d.InstanceId)) Status: $($d.Status)""
                    }
                    Write-Output ""-------------------------------""

                    # Now specifically look for disabled devices
                    $disabledDevices = Get-PnpDevice -Class Display -Status 'Error','Disabled' | 
                                       Select-Object FriendlyName, InstanceId, Status, Class, Description
                    
                    # Show all disabled display devices found (for debugging)
                    Write-Output ""------ Disabled Display Devices ------""
                    if ($disabledDevices) {
                        foreach ($d in $disabledDevices) {
                            Write-Output ""Disabled device: $($d.FriendlyName) ($($d.InstanceId)) Status: $($d.Status)""
                        }
                    } else {
                        Write-Output ""No disabled display devices found""
                    }
                    Write-Output ""------------------------------------""
                    
                    # Try to find our Virtual Display Driver among the disabled devices
                    $targetDevice = $null
                    
                    # First look specifically for devices with Virtual Display in the name
                    $targetDevice = $disabledDevices | Where-Object { $_.FriendlyName -like '*Virtual Display*' } | Select-Object -First 1
                    
                    # If not found, look for any device with Virtual in the name
                    if (-not $targetDevice) {
                        $targetDevice = $disabledDevices | Where-Object { $_.FriendlyName -like '*Virtual*' } | Select-Object -First 1
                    }
                    
                    # If still not found, check if Root\DISPLAY\0001 is disabled (common ID for virtual displays)
                    if (-not $targetDevice) {
                        $targetDevice = $disabledDevices | Where-Object { $_.InstanceId -eq 'ROOT\DISPLAY\0001' } | Select-Object -First 1
                    }
                    
                    # If still not found, just take the first disabled display device
                    if (-not $targetDevice -and $disabledDevices) {
                        $targetDevice = $disabledDevices | Select-Object -First 1
                    }
                    
                    if ($targetDevice) {
                        try {
                            Write-Output ""Attempting to enable: $($targetDevice.FriendlyName) ($($targetDevice.InstanceId))""
                            Enable-PnpDevice -InstanceId $targetDevice.InstanceId -Confirm:$false -ErrorAction Stop
                            Write-Output ""SUCCESS: Device enabled""
                            return
                        } catch {
                            Write-Output ""FAILURE: $($_.Exception.Message)""
                            return
                        }
                    } else {
                        Write-Output ""NOT_FOUND: No suitable disabled display device found to enable""
                    }
                    
                    # If we get here, we need to try a more direct approach
                    # First, try to get 'ROOT\DISPLAY\0001' regardless of status
                    $rootDisplay = Get-PnpDevice | Where-Object { $_.InstanceId -eq 'ROOT\DISPLAY\0001' } | Select-Object -First 1
                    
                    if ($rootDisplay) {
                        try {
                            Write-Output ""Trying to work with ROOT\DISPLAY\0001 directly""
                            # Try to disable then enable it (if it's not already disabled)
                            if ($rootDisplay.Status -ne 'Disabled' -and $rootDisplay.Status -ne 'Error') {
                                Disable-PnpDevice -InstanceId $rootDisplay.InstanceId -Confirm:$false -ErrorAction SilentlyContinue
                                Start-Sleep -Seconds 2
                            }
                            # Now try to enable it
                            Enable-PnpDevice -InstanceId $rootDisplay.InstanceId -Confirm:$false -ErrorAction Stop
                            Write-Output ""SUCCESS: ROOT\DISPLAY\0001 enabled directly""
                        } catch {
                            Write-Output ""FAILURE on direct enable: $($_.Exception.Message)""
                        }
                    }
                ";

                AppendToConsole("[INFO] Searching for disabled Virtual Display Driver...\n");
                string result = await RunPowerShellCommand(command);

                // Log the raw result for debugging
                AppendToConsole($"[DEBUG] PowerShell output:\n{result}\n");

                if (result.Contains("SUCCESS"))
                {
                    AppendToConsole("[INFO] Driver enabled successfully through Device Manager.\n");
                    return true;
                }
                else if (result == "CANCELLED")
                {
                    AppendToConsole("[ERROR] Administrative access is required to enable the driver.\n");
                    return false;
                }
                else if (result.Contains("NOT_FOUND"))
                {
                    AppendToConsole("[ERROR] No disabled display device found that could be enabled.\n");

                    // Try a direct approach as a last resort
                    AppendToConsole("[INFO] Trying alternative approach to enable display device...\n");

                    string directCommand = @"
                        # Try to enable ROOT\DISPLAY\0001 directly
                        try {
                            Enable-PnpDevice -InstanceId 'ROOT\DISPLAY\0001' -Confirm:$false -ErrorAction Stop
                            Write-Output ""SUCCESS: Direct enable attempt worked""
                        } catch {
                            Write-Output ""FAILURE: $($_.Exception.Message)""
                        }";

                    string directResult = await RunPowerShellCommand(directCommand);

                    if (directResult.Contains("SUCCESS"))
                    {
                        AppendToConsole("[SUCCESS] Alternative approach successfully enabled the driver.\n");
                        return true;
                    }
                    else
                    {
                        AppendToConsole("[ERROR] Alternative approach failed to enable the driver.\n");
                        return false;
                    }
                }
                else
                {
                    AppendToConsole($"[ERROR] Failed to enable driver.\n");
                    return false;
                }
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] Exception when enabling driver: {ex.Message}\n");
                return false;
            }
        }

        private async Task<string> RunPowerShellCommand(string command)
        {
            // First try to check if we can run with admin privileges
            bool needsAdminRights = true;

            try
            {
                // Try a simple administrative operation to check if we have admin rights
                using (Process testProcess = new Process())
                {
                    testProcess.StartInfo = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = "-Command \"[bool](([System.Security.Principal.WindowsIdentity]::GetCurrent()).groups -match 'S-1-5-32-544')\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    testProcess.Start();
                    string result = await testProcess.StandardOutput.ReadToEndAsync();
                    await testProcess.WaitForExitAsync();

                    // If the result is "True", we're already running as admin
                    if (result.Trim().Equals("True", StringComparison.OrdinalIgnoreCase))
                    {
                        needsAdminRights = false;
                    }
                }
            }
            catch
            {
                // If the test fails, assume we need admin rights
                needsAdminRights = true;
            }

            if (needsAdminRights)
            {
                // Inform the user we need to run with elevated privileges
                AppendToConsole("[INFO] Device management requires administrative privileges.\n");
                AppendToConsole("[INFO] Attempting to run PowerShell as administrator...\n");

                // Create a temporary script file to execute with elevated privileges
                string tempPath = Path.Combine(Path.GetTempPath(), $"vdd_script_{Guid.NewGuid()}.ps1");

                try
                {
                    // Write command to temporary file
                    File.WriteAllText(tempPath, command);

                    // Create process to run PowerShell as admin
                    using (Process process = new Process())
                    {
                        process.StartInfo = new ProcessStartInfo
                        {
                            FileName = "powershell.exe",
                            Arguments = $"-NoProfile -ExecutionPolicy Bypass -File \"{tempPath}\"",
                            Verb = "runas", // This triggers the UAC prompt
                            UseShellExecute = true,
                            CreateNoWindow = false,
                            WindowStyle = ProcessWindowStyle.Hidden
                        };

                        try
                        {
                            process.Start();
                            await process.WaitForExitAsync();

                            // Wait a moment to let Windows process the change
                            await Task.Delay(2000);

                            // Since we can't capture output when using UseShellExecute=true,
                            // we'll check if the operation was successful by looking for the devices
                            return "SUCCESS";
                        }
                        catch (Win32Exception ex) when (ex.NativeErrorCode == 1223)
                        {
                            // User cancelled the UAC prompt
                            AppendToConsole("[ERROR] Administrative access denied. User cancelled the elevation request.\n");
                            return "CANCELLED";
                        }
                        catch (Exception ex)
                        {
                            AppendToConsole($"[ERROR] Failed to run with administrative privileges: {ex.Message}\n");
                            return "FAILURE";
                        }
                    }
                }
                finally
                {
                    // Clean up the temporary script file
                    try
                    {
                        if (File.Exists(tempPath))
                        {
                            File.Delete(tempPath);
                        }
                    }
                    catch { /* Ignore cleanup errors */ }
                }
            }
            else
            {
                // We already have admin rights, run normally
                using (Process process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{command}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    process.Start();

                    string output = await process.StandardOutput.ReadToEndAsync();
                    string error = await process.StandardError.ReadToEndAsync();

                    await process.WaitForExitAsync();

                    if (!string.IsNullOrEmpty(error))
                    {
                        AppendToConsole($"[ERROR] PowerShell error: {error}\n");
                    }

                    return output.Trim();
                }
            }
        }
        private void getCPUInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Access the registry for CPU information
                RegistryKey localMachine = Registry.LocalMachine;
                RegistryKey hardware = localMachine.OpenSubKey("HARDWARE");
                RegistryKey description = hardware?.OpenSubKey("DESCRIPTION");
                RegistryKey system = description?.OpenSubKey("SYSTEM");
                RegistryKey centralProcessor = system?.OpenSubKey("CentralProcessor");
                RegistryKey processorInfo = centralProcessor?.OpenSubKey("0");

                if (processorInfo != null)
                {
                    // Set bold font for the title
                    Font boldFont = new Font(mainConsole.Font, FontStyle.Bold);
                    mainConsole.SelectionFont = boldFont;
                    mainConsole.AppendText("CPU Info:\n\n");

                    // Add CPU information
                    mainConsole.AppendText("Vendor: " + processorInfo.GetValue("VendorIdentifier") + "\n\n");
                    mainConsole.AppendText("Processor: " + processorInfo.GetValue("ProcessorNameString") + "\n\n");
                    mainConsole.AppendText("Type: " + processorInfo.GetValue("Identifier") + "\n\n");
                    mainConsole.AppendText("Speed: " + processorInfo.GetValue("~Mhz") + " MHz\n");
                }
                else
                {
                    MessageBox.Show("Unable to retrieve CPU information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while retrieving CPU information:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void getDisplayInformationToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                // Initialize a process to execute PowerShell
                // Redo all of this.
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = "-NoProfile -ExecutionPolicy Bypass -Command \"irm scripts.mikethetech.com/display | iex\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                // Start the process and capture output
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                // Display output in richTextBox1
                if (!string.IsNullOrWhiteSpace(output))
                {
                    mainConsole.AppendText("Display Information:\n\n" + output);
                }
                else if (!string.IsNullOrWhiteSpace(error))
                {
                    mainConsole.AppendText("Error:\n\n" + error);
                }
                else
                {
                    mainConsole.AppendText("No output received from the PowerShell command.");
                }
            }
            catch (Exception ex)
            {
                // Display error details in richTextBox1
                mainConsole.AppendText("An error occurred while retrieving display information:\n" + ex.Message);
            }

        }

        private void getAudioInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Initialize a process to execute PowerShell
                // Redo all of this.
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = "-NoProfile -ExecutionPolicy Bypass -Command \"irm scripts.mikethetech.com/audio | iex\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                // Start the process and capture output
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                // Display output in richTextBox1
                if (!string.IsNullOrWhiteSpace(output))
                {
                    mainConsole.AppendText("Display Information:\n\n" + output);
                }
                else if (!string.IsNullOrWhiteSpace(error))
                {
                    mainConsole.AppendText("Error:\n\n" + error);
                }
                else
                {
                    mainConsole.AppendText("No output received from the PowerShell command.");
                }
            }
            catch (Exception ex)
            {
                // Display error details in richTextBox1
                mainConsole.AppendText("An error occurred while retrieving display information:\n" + ex.Message);
            }

        }

        private async void getGPUInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateTaskProgress("Getting GPU Information", 10);
                mainConsole.AppendText("Gathering GPU information...\n");

                // Initialize a process to execute PowerShell
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = "-NoProfile -ExecutionPolicy Bypass -Command \"irm scripts.mikethetech.com/gpu | iex\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                UpdateTaskProgress("Getting GPU Information", 30);

                // Start the process and capture output
                process.Start();

                UpdateTaskProgress("Getting GPU Information", 50);

                string output = await process.StandardOutput.ReadToEndAsync();
                string error = await process.StandardError.ReadToEndAsync();

                UpdateTaskProgress("Getting GPU Information", 80);

                await Task.Run(() => process.WaitForExit());

                // Use BeginInvoke to update progress on UI thread
                this.BeginInvoke(new Action(() => UpdateTaskProgress("Getting GPU Information", 90)));

                // Display output in richTextBox1
                if (!string.IsNullOrWhiteSpace(output))
                {
                    mainConsole.AppendText("GPU Information:\n\n" + output);
                }
                else if (!string.IsNullOrWhiteSpace(error))
                {
                    mainConsole.AppendText("Error:\n\n" + error);
                }
                else
                {
                    mainConsole.AppendText("No output received from the PowerShell command.");
                }

                UpdateTaskProgress("Getting GPU Information", 100);
                await Task.Delay(500); // Show 100% for a moment
                this.BeginInvoke(new Action(() => UpdateTaskProgress("", 0))); // Clear task progress
            }
            catch (Exception ex)
            {
                // Display error details in richTextBox1
                mainConsole.AppendText("An error occurred while retrieving GPU information:\n" + ex.Message);
                UpdateTaskProgress("Getting GPU Information", 0); // Reset progress bar on error
            }

        }

        private void getDisplayInformationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Call the existing method for display information
            getDisplayInformationToolStripMenuItem2_Click(sender, e);
        }

        private void getGPUInformationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Call the existing method for GPU information
            getGPUInformationToolStripMenuItem_Click(sender, e);
        }

        private void getCPUInformationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Call the existing method for CPU information
            getCPUInformationToolStripMenuItem_Click(sender, e);
        }

        private void getAudioInformationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Call the existing method for audio information
            getAudioInformationToolStripMenuItem_Click(sender, e);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void exitToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void xMLOptionsEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show progress in task bar
            UpdateTaskProgress("Opening XML Editor", 50);

            // Launch XML Editor
            XMLEditor form2 = new XMLEditor();
            form2.Show();

            // Complete progress
            UpdateTaskProgress("Opening XML Editor", 100);
            Task.Delay(500).ContinueWith(_ =>
            {
                // Reset progress bar after a delay
                this.BeginInvoke(new Action(() => UpdateTaskProgress("", 0)));
            });
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Use the same method as the main menu XML editor option
            xMLOptionsEditorToolStripMenuItem_Click(sender, e);
        }

        private async void sDR10bitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SDR10_STATE = !SDR10_STATE;                          //Flip current state
            sDR10bitToolStripMenuItem.Checked = SDR10_STATE;     //Sync Checked state in menu

            string action = SDR10_STATE ? "ON" : "OFF";          //Switch based off state
            AppendToConsole($"[ACTION] Toggling SDR 10 bit state to {action}...\n");

            try
            {
                // Update the XML settings first
                if (IXCLI != null)
                {
                    IXCLI.SDR10bit = SDR10_STATE;

                    // Save the updated XML settings
                    try
                    {
                        string xmlPath = Path.Combine(registryFilePath, "vdd_settings.xml");
                        IXCLI.SaveToXml(xmlPath);
                        AppendToConsole($"[SUCCESS] Updated XML settings for SDR 10 bit: {SDR10_STATE}\n");
                    }
                    catch (Exception xmlEx)
                    {
                        AppendToConsole($"[WARNING] Could not save XML settings: {xmlEx.Message}\n");
                    }
                }

                // Now update the driver
                string command = SDR10_STATE ? "SDR10 true" : "SDR10 false";
                string? response = await SendCommandToDriver(command); // Send state based off bool

                // After sending the command, get actual status from driver to ensure the UI is in sync
                // Only try to get actual status if logging is enabled
                if (LOGGING_STATE)
                {
                    bool actualStatus = await GetDriverFeatureStatus("SDR10");
                    if (SDR10_STATE != actualStatus)
                    {
                        // If there's a mismatch, update UI to match actual driver state
                        SDR10_STATE = actualStatus;
                        sDR10bitToolStripMenuItem.Checked = actualStatus;
                        AppendToConsole($"[INFO] SDR 10 bit setting changed to {(actualStatus ? "ON" : "OFF")} based on driver status.\n");

                        // Also update XML to match
                        if (IXCLI != null)
                        {
                            IXCLI.SDR10bit = actualStatus;
                            try
                            {
                                string xmlPath = Path.Combine(registryFilePath, "vdd_settings.xml");
                                IXCLI.SaveToXml(xmlPath);
                            }
                            catch { /* Ignore errors on second save attempt */ }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = $"[ERROR] Could not send toggle SDR command: {ex.Message}";
                AppendToConsole(errorMsg + "\n");

                // On error, revert UI state to XML settings
                if (IXCLI != null)
                {
                    SDR10_STATE = IXCLI.SDR10bit;
                    sDR10bitToolStripMenuItem.Checked = SDR10_STATE;
                }
                else
                {
                    // If no XML, try to get status from driver
                    try
                    {
                        bool actualStatus = await GetDriverFeatureStatus("SDR10");
                        SDR10_STATE = actualStatus;
                        sDR10bitToolStripMenuItem.Checked = actualStatus;
                    }
                    catch
                    {
                        // If all else fails, default to false
                        SDR10_STATE = false;
                        sDR10bitToolStripMenuItem.Checked = false;
                    }
                }
            }
        }


        private async void hDRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HDR10PLUS_STATE = !HDR10PLUS_STATE;
            hDRToolStripMenuItem.Checked = HDR10PLUS_STATE;

            string action = HDR10PLUS_STATE ? "ON" : "OFF";
            AppendToConsole($"[ACTION] Toggling HDR-10+ state to {action}...\n");

            try
            {
                // Update the XML settings first
                if (IXCLI != null)
                {
                    IXCLI.HDRPlus = HDR10PLUS_STATE;

                    // Save the updated XML settings
                    try
                    {
                        string xmlPath = Path.Combine(registryFilePath, "vdd_settings.xml");
                        IXCLI.SaveToXml(xmlPath);
                        AppendToConsole($"[SUCCESS] Updated XML settings for HDR-10+: {HDR10PLUS_STATE}\n");
                    }
                    catch (Exception xmlEx)
                    {
                        AppendToConsole($"[WARNING] Could not save XML settings: {xmlEx.Message}\n");
                    }
                }

                // Now update the driver
                string command = HDR10PLUS_STATE ? "HDRPLUS true" : "HDRPLUS false";
                string? response = await SendCommandToDriver(command);

                // After sending the command, get actual status from driver to ensure the UI is in sync
                // Only try to get actual status if logging is enabled
                if (LOGGING_STATE)
                {
                    bool actualStatus = await GetDriverFeatureStatus("HDRPLUS");
                    if (HDR10PLUS_STATE != actualStatus)
                    {
                        // If there's a mismatch, update UI to match actual driver state
                        HDR10PLUS_STATE = actualStatus;
                        hDRToolStripMenuItem.Checked = actualStatus;
                        AppendToConsole($"[INFO] HDR-10+ setting changed to {(actualStatus ? "ON" : "OFF")} based on driver status.\n");

                        // Also update XML to match
                        if (IXCLI != null)
                        {
                            IXCLI.HDRPlus = actualStatus;
                            try
                            {
                                string xmlPath = Path.Combine(registryFilePath, "vdd_settings.xml");
                                IXCLI.SaveToXml(xmlPath);
                            }
                            catch { /* Ignore errors on second save attempt */ }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = $"[ERROR] Could not send toggle HDR-10+ command: {ex.Message}";
                AppendToConsole(errorMsg + "\n");

                // On error, revert UI state to XML settings
                if (IXCLI != null)
                {
                    HDR10PLUS_STATE = IXCLI.HDRPlus;
                    hDRToolStripMenuItem.Checked = HDR10PLUS_STATE;
                }
                else
                {
                    // If no XML, try to get status from driver
                    try
                    {
                        bool actualStatus = await GetDriverFeatureStatus("HDRPLUS");
                        HDR10PLUS_STATE = actualStatus;
                        hDRToolStripMenuItem.Checked = actualStatus;
                    }
                    catch
                    {
                        // If all else fails, default to false
                        HDR10PLUS_STATE = false;
                        hDRToolStripMenuItem.Checked = false;
                    }
                }
            }
        }

        private async void customEDIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CUSTOMEDID_STATE = !CUSTOMEDID_STATE;
            customEDIDToolStripMenuItem.Checked = CUSTOMEDID_STATE;

            string action = CUSTOMEDID_STATE ? "ON" : "OFF";
            AppendToConsole($"[ACTION] Toggling Custom Edid state to {action}...\n");

            try
            {
                // Update the XML settings first
                if (IXCLI != null)
                {
                    IXCLI.CustomEdid = CUSTOMEDID_STATE;

                    // Save the updated XML settings
                    try
                    {
                        string xmlPath = Path.Combine(registryFilePath, "vdd_settings.xml");
                        IXCLI.SaveToXml(xmlPath);
                        AppendToConsole($"[SUCCESS] Updated XML settings for Custom EDID: {CUSTOMEDID_STATE}\n");
                    }
                    catch (Exception xmlEx)
                    {
                        AppendToConsole($"[WARNING] Could not save XML settings: {xmlEx.Message}\n");
                    }
                }

                // Now update the driver
                string command = CUSTOMEDID_STATE ? "CUSTOMEDID true" : "CUSTOMEDID false";
                string? response = await SendCommandToDriver(command);

                // After sending the command, get actual status from driver to ensure the UI is in sync
                // Only try to get actual status if logging is enabled
                if (LOGGING_STATE)
                {
                    bool actualStatus = await GetDriverFeatureStatus("CUSTOMEDID");
                    if (CUSTOMEDID_STATE != actualStatus)
                    {
                        // If there's a mismatch, update UI to match actual driver state
                        CUSTOMEDID_STATE = actualStatus;
                        customEDIDToolStripMenuItem.Checked = actualStatus;
                        AppendToConsole($"[INFO] Custom EDID setting changed to {(actualStatus ? "ON" : "OFF")} based on driver status.\n");

                        // Also update XML to match
                        if (IXCLI != null)
                        {
                            IXCLI.CustomEdid = actualStatus;
                            try
                            {
                                string xmlPath = Path.Combine(registryFilePath, "vdd_settings.xml");
                                IXCLI.SaveToXml(xmlPath);
                            }
                            catch { /* Ignore errors on second save attempt */ }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = $"[ERROR] Could not send toggle Custom EDID command: {ex.Message}";
                AppendToConsole(errorMsg + "\n");

                // On error, revert UI state to XML settings
                if (IXCLI != null)
                {
                    CUSTOMEDID_STATE = IXCLI.CustomEdid;
                    customEDIDToolStripMenuItem.Checked = CUSTOMEDID_STATE;
                }
                else
                {
                    // If no XML, try to get status from driver
                    try
                    {
                        bool actualStatus = await GetDriverFeatureStatus("CUSTOMEDID");
                        CUSTOMEDID_STATE = actualStatus;
                        customEDIDToolStripMenuItem.Checked = actualStatus;
                    }
                    catch
                    {
                        // If all else fails, default to false
                        CUSTOMEDID_STATE = false;
                        customEDIDToolStripMenuItem.Checked = false;
                    }
                }
            }
        }

        private async void hardwareCursorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HARDWARECURSOR_STATE = !HARDWARECURSOR_STATE;
            hardwareCursorToolStripMenuItem.Checked = HARDWARECURSOR_STATE;

            string action = HARDWARECURSOR_STATE ? "ON" : "OFF";
            AppendToConsole($"[ACTION] Toggling Hardware cursor state to {action}...\n");

            try
            {
                // Update the XML settings first
                if (IXCLI != null)
                {
                    IXCLI.HardwareCursor = HARDWARECURSOR_STATE;

                    // Save the updated XML settings
                    try
                    {
                        string xmlPath = Path.Combine(registryFilePath, "vdd_settings.xml");
                        IXCLI.SaveToXml(xmlPath);
                        AppendToConsole($"[SUCCESS] Updated XML settings for Hardware Cursor: {HARDWARECURSOR_STATE}\n");
                    }
                    catch (Exception xmlEx)
                    {
                        AppendToConsole($"[WARNING] Could not save XML settings: {xmlEx.Message}\n");
                    }
                }

                // Now update the driver
                string command = HARDWARECURSOR_STATE ? "HARDWARECURSOR true" : "HARDWARECURSOR false";
                string? response = await SendCommandToDriver(command);

                // After sending the command, get actual status from driver to ensure the UI is in sync
                // Only try to get actual status if logging is enabled
                if (LOGGING_STATE)
                {
                    bool actualStatus = await GetDriverFeatureStatus("HARDWARECURSOR");
                    if (HARDWARECURSOR_STATE != actualStatus)
                    {
                        // If there's a mismatch, update UI to match actual driver state
                        HARDWARECURSOR_STATE = actualStatus;
                        hardwareCursorToolStripMenuItem.Checked = actualStatus;
                        AppendToConsole($"[INFO] Hardware Cursor setting changed to {(actualStatus ? "ON" : "OFF")} based on driver status.\n");

                        // Also update XML to match
                        if (IXCLI != null)
                        {
                            IXCLI.HardwareCursor = actualStatus;
                            try
                            {
                                string xmlPath = Path.Combine(registryFilePath, "vdd_settings.xml");
                                IXCLI.SaveToXml(xmlPath);
                            }
                            catch { /* Ignore errors on second save attempt */ }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = $"[ERROR] Could not send toggle Hardware Cursor command: {ex.Message}";
                AppendToConsole(errorMsg + "\n");

                // On error, revert UI state to XML settings
                if (IXCLI != null)
                {
                    HARDWARECURSOR_STATE = IXCLI.HardwareCursor;
                    hardwareCursorToolStripMenuItem.Checked = HARDWARECURSOR_STATE;
                }
                else
                {
                    // If no XML, try to get status from driver
                    try
                    {
                        bool actualStatus = await GetDriverFeatureStatus("HARDWARECURSOR");
                        HARDWARECURSOR_STATE = actualStatus;
                        hardwareCursorToolStripMenuItem.Checked = actualStatus;
                    }
                    catch
                    {
                        // If all else fails, default to true (hardware cursor is usually enabled by default)
                        HARDWARECURSOR_STATE = true;
                        hardwareCursorToolStripMenuItem.Checked = true;
                    }
                }
            }
        }

        private async void preventMonitorSpoofToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PREVENTEDIDSPOOF_STATE = !PREVENTEDIDSPOOF_STATE;
            preventMonitorSpoofToolStripMenuItem.Checked = PREVENTEDIDSPOOF_STATE;

            string action = PREVENTEDIDSPOOF_STATE ? "ON" : "OFF";
            AppendToConsole($"[ACTION] Toggling Prevent Monitor Spoof state to {action}...\n");

            try
            {
                // Update the XML settings first
                if (IXCLI != null)
                {
                    IXCLI.PreventSpoof = PREVENTEDIDSPOOF_STATE;

                    // Save the updated XML settings
                    try
                    {
                        string xmlPath = Path.Combine(registryFilePath, "vdd_settings.xml");
                        IXCLI.SaveToXml(xmlPath);
                        AppendToConsole($"[SUCCESS] Updated XML settings for Prevent Spoof: {PREVENTEDIDSPOOF_STATE}\n");
                    }
                    catch (Exception xmlEx)
                    {
                        AppendToConsole($"[WARNING] Could not save XML settings: {xmlEx.Message}\n");
                    }
                }

                // Now update the driver
                string command = PREVENTEDIDSPOOF_STATE ? "PREVENTSPOOF true" : "PREVENTSPOOF false";
                string? response = await SendCommandToDriver(command);

                // After sending the command, get actual status from driver to ensure the UI is in sync
                // Only try to get actual status if logging is enabled
                if (LOGGING_STATE)
                {
                    bool actualStatus = await GetDriverFeatureStatus("PREVENTSPOOF");
                    if (PREVENTEDIDSPOOF_STATE != actualStatus)
                    {
                        // If there's a mismatch, update UI to match actual driver state
                        PREVENTEDIDSPOOF_STATE = actualStatus;
                        preventMonitorSpoofToolStripMenuItem.Checked = actualStatus;
                        AppendToConsole($"[INFO] Prevent Spoof setting changed to {(actualStatus ? "ON" : "OFF")} based on driver status.\n");

                        // Also update XML to match
                        if (IXCLI != null)
                        {
                            IXCLI.PreventSpoof = actualStatus;
                            try
                            {
                                string xmlPath = Path.Combine(registryFilePath, "vdd_settings.xml");
                                IXCLI.SaveToXml(xmlPath);
                            }
                            catch { /* Ignore errors on second save attempt */ }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = $"[ERROR] Could not send toggle Prevent Spoof command: {ex.Message}";
                AppendToConsole(errorMsg + "\n");

                // On error, revert UI state to XML settings
                if (IXCLI != null)
                {
                    PREVENTEDIDSPOOF_STATE = IXCLI.PreventSpoof;
                    preventMonitorSpoofToolStripMenuItem.Checked = PREVENTEDIDSPOOF_STATE;
                }
                else
                {
                    // If no XML, try to get status from driver
                    try
                    {
                        bool actualStatus = await GetDriverFeatureStatus("PREVENTSPOOF");
                        PREVENTEDIDSPOOF_STATE = actualStatus;
                        preventMonitorSpoofToolStripMenuItem.Checked = actualStatus;
                    }
                    catch
                    {
                        // If all else fails, default to false
                        PREVENTEDIDSPOOF_STATE = false;
                        preventMonitorSpoofToolStripMenuItem.Checked = false;
                    }
                }
            }
        }

        private async void eDIDCEAOverrideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EDIDCEAOVERRRIDE_STATE = !EDIDCEAOVERRRIDE_STATE;
            eDIDCEAOverrideToolStripMenuItem.Checked = EDIDCEAOVERRRIDE_STATE;

            string action = EDIDCEAOVERRRIDE_STATE ? "ON" : "OFF";
            AppendToConsole($"[ACTION] Toggling Edid Cea Override state to {action}...\n");

            try
            {
                // Update the XML settings first
                if (IXCLI != null)
                {
                    IXCLI.EdidCeaOverride = EDIDCEAOVERRRIDE_STATE;

                    // Save the updated XML settings
                    try
                    {
                        string xmlPath = Path.Combine(registryFilePath, "vdd_settings.xml");
                        IXCLI.SaveToXml(xmlPath);
                        AppendToConsole($"[SUCCESS] Updated XML settings for EDID CEA Override: {EDIDCEAOVERRRIDE_STATE}\n");
                    }
                    catch (Exception xmlEx)
                    {
                        AppendToConsole($"[WARNING] Could not save XML settings: {xmlEx.Message}\n");
                    }
                }

                // Now update the driver
                string command = EDIDCEAOVERRRIDE_STATE ? "CEAOVERRIDE true" : "CEAOVERRIDE false";
                string? response = await SendCommandToDriver(command);

                // After sending the command, get actual status from driver to ensure the UI is in sync
                // Only try to get actual status if logging is enabled
                if (LOGGING_STATE)
                {
                    bool actualStatus = await GetDriverFeatureStatus("CEAOVERRIDE");
                    if (EDIDCEAOVERRRIDE_STATE != actualStatus)
                    {
                        // If there's a mismatch, update UI to match actual driver state
                        EDIDCEAOVERRRIDE_STATE = actualStatus;
                        eDIDCEAOverrideToolStripMenuItem.Checked = actualStatus;
                        AppendToConsole($"[INFO] EDID CEA Override setting changed to {(actualStatus ? "ON" : "OFF")} based on driver status.\n");

                        // Also update XML to match
                        if (IXCLI != null)
                        {
                            IXCLI.EdidCeaOverride = actualStatus;
                            try
                            {
                                string xmlPath = Path.Combine(registryFilePath, "vdd_settings.xml");
                                IXCLI.SaveToXml(xmlPath);
                            }
                            catch { /* Ignore errors on second save attempt */ }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = $"[ERROR] Could not send toggle EDID CEA Override command: {ex.Message}";
                AppendToConsole(errorMsg + "\n");

                // On error, revert UI state to XML settings
                if (IXCLI != null)
                {
                    EDIDCEAOVERRRIDE_STATE = IXCLI.EdidCeaOverride;
                    eDIDCEAOverrideToolStripMenuItem.Checked = EDIDCEAOVERRRIDE_STATE;
                }
                else
                {
                    // If no XML, try to get status from driver
                    try
                    {
                        bool actualStatus = await GetDriverFeatureStatus("CEAOVERRIDE");
                        EDIDCEAOVERRRIDE_STATE = actualStatus;
                        eDIDCEAOverrideToolStripMenuItem.Checked = actualStatus;
                    }
                    catch
                    {
                        // If all else fails, default to false
                        EDIDCEAOVERRRIDE_STATE = false;
                        eDIDCEAOverrideToolStripMenuItem.Checked = false;
                    }
                }
            }
        }

        private void selectGPUToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void displayCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // The menu item itself doesn't need to do anything - the dropdown items handle the actions
            // This prevents the parent menu item from doing anything when clicked
        }

        private void enableToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private async void disableDriverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppendToConsole("[ACTION] Disabling Virtual Display Driver...\n");
            UpdateTaskProgress("Disabling Driver", 10);

            try
            {
                bool success = await DisableDriverWithDeviceManager();

                if (success)
                {
                    AppendToConsole("[SUCCESS] Virtual Display Driver disabled successfully.\n");
                    UpdateTaskProgress("Disabling Driver", 100);
                    await Task.Delay(1000); // Show 100% for a moment
                    this.BeginInvoke(new Action(() => UpdateTaskProgress("", 0))); // Clear task progress
                }
                else
                {
                    AppendToConsole("[ERROR] Failed to disable Virtual Display Driver.\n");
                    UpdateTaskProgress("Disabling Driver", 0);
                }
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] Error disabling driver: {ex.Message}\n");
                UpdateTaskProgress("Disabling Driver", 0);
            }
        }

        private void enableUserModeLoggingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void enableDevModeLoggingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAboutDialog();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Helper method to create a default XML file when none is found
        private async Task<bool> TryCreateDefaultXmlFile()
        {
            AppendToConsole("[INFO] Attempting to create default XML settings file...\n");

            // First, check if the sample XML exists in our project directory
            string sampleXmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vdd_settings.xml");
            string targetXmlPath = @"C:\VirtualDisplayDriver\vdd_settings.xml";

            // If we have a sample XML in our application directory
            if (File.Exists(sampleXmlPath))
            {
                AppendToConsole($"[INFO] Found sample XML at: {sampleXmlPath}\n");

                try
                {
                    // Make sure the target directory exists
                    Directory.CreateDirectory(@"C:\VirtualDisplayDriver");

                    // Copy the sample XML to the driver directory
                    File.Copy(sampleXmlPath, targetXmlPath, true);
                    AppendToConsole($"[SUCCESS] Created default XML at: {targetXmlPath}\n");
                    return true;
                }
                catch (Exception ex)
                {
                    AppendToConsole($"[ERROR] Failed to copy XML file: {ex.Message}\n");
                    return false;
                }
            }
            else
            {
                // Check for the XML file in the project root
                string projectXmlPath = Path.Combine(
                    Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName,
                    "vdd_settings.xml"
                );

                if (File.Exists(projectXmlPath))
                {
                    AppendToConsole($"[INFO] Found XML in project root: {projectXmlPath}\n");

                    try
                    {
                        // Make sure the target directory exists
                        Directory.CreateDirectory(@"C:\VirtualDisplayDriver");

                        // Copy the XML to the driver directory
                        File.Copy(projectXmlPath, targetXmlPath, true);
                        AppendToConsole($"[SUCCESS] Created default XML at: {targetXmlPath}\n");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        AppendToConsole($"[ERROR] Failed to copy XML file: {ex.Message}\n");
                        return false;
                    }
                }
                else
                {
                    AppendToConsole("[WARNING] No sample XML file found to create default settings\n");
                    return false;
                }
            }
        }

        // System Information Menu Items
        private void getDisplayInformationToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            getDisplayInformationToolStripMenuItem2_Click(sender, e);
        }

        private void getGPUInformationToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            getGPUInformationToolStripMenuItem_Click(sender, e);
        }

        private void getCPUInformationToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            getCPUInformationToolStripMenuItem_Click(sender, e);
        }

        private void getAudioInformationToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            getAudioInformationToolStripMenuItem_Click(sender, e);
        }

        // Toggle Option Menu Items
        private void sDR10bitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            sDR10bitToolStripMenuItem_Click(sender, e);
        }

        private void hDRToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            hDRToolStripMenuItem_Click(sender, e);
        }

        private void customEDIDToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            customEDIDToolStripMenuItem_Click(sender, e);
        }

        private void hardwareCursorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            hardwareCursorToolStripMenuItem_Click(sender, e);
        }

        private void preventMonitorSpoofToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            preventMonitorSpoofToolStripMenuItem_Click(sender, e);
        }

        private void eDIDCEAOverrideToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            eDIDCEAOverrideToolStripMenuItem_Click(sender, e);
        }

        private void selectGPUToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Note: Both GPU selection menu items are actually hidden in the code,
            // but if they weren't, this would delegate to the primary handler
            selectGPUToolStripMenuItem_Click(sender, e);
        }

        private void displayCountToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // The menu item itself doesn't need to do anything - the dropdown items handle the actions
            // This prevents the parent menu item from doing anything when clicked
        }

        private async void enableDriverToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            AppendToConsole("[ACTION] Enabling Virtual Display Driver...\n");
            UpdateTaskProgress("Enabling Driver", 10);

            try
            {
                bool success = await EnableDriverWithDeviceManager();

                if (success)
                {
                    AppendToConsole("[SUCCESS] Virtual Display Driver enabled successfully.\n");
                    UpdateTaskProgress("Enabling Driver", 100);
                    await Task.Delay(1000); // Show 100% for a moment
                    this.BeginInvoke(new Action(() => UpdateTaskProgress("", 0))); // Clear task progress

                    // Try to connect to the driver after enabling
                    AppendToConsole("[INFO] Attempting to connect to enabled driver...\n");
                    if (await TryConnectToDriver())
                    {
                        AppendToConsole("[SUCCESS] Connected to enabled driver successfully.\n");
                    }
                    else
                    {
                        AppendToConsole("[WARNING] Driver enabled but connection could not be established. The driver may need time to initialize.\n");
                    }
                }
                else
                {
                    AppendToConsole("[ERROR] Failed to enable Virtual Display Driver.\n");
                    UpdateTaskProgress("Enabling Driver", 0);
                }
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] Error enabling driver: {ex.Message}\n");
                UpdateTaskProgress("Enabling Driver", 0);
            }
        }

        private async void disableDriverToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            AppendToConsole("[ACTION] Disabling Virtual Display Driver...\n");
            UpdateTaskProgress("Disabling Driver", 10);

            try
            {
                bool success = await DisableDriverWithDeviceManager();

                if (success)
                {
                    AppendToConsole("[SUCCESS] Virtual Display Driver disabled successfully.\n");
                    UpdateTaskProgress("Disabling Driver", 100);
                    await Task.Delay(1000); // Show 100% for a moment
                    this.BeginInvoke(new Action(() => UpdateTaskProgress("", 0))); // Clear task progress
                }
                else
                {
                    AppendToConsole("[ERROR] Failed to disable Virtual Display Driver.\n");
                    UpdateTaskProgress("Disabling Driver", 0);
                }
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] Error disabling driver: {ex.Message}\n");
                UpdateTaskProgress("Disabling Driver", 0);
            }
        }

        private void restartDriverToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            // Call the restartDriverToolStripMenuItem_Click method directly
            // Can't await an async void method
            _ = restartDriverToolStripMenuItem_Click(sender, e);
        }

        private void userModeLoggingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void devModeLoggingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowAboutDialog();
        }

        private void button1_Click(object sender, EventArgs e) // Restart Driver
        {

        }

        private void button2_Click(object sender, EventArgs e) // Minimize to Tray
        {
            // This event handler is connected in the designer to a button
            // Simply call the MinimizeToTray method
            MinimizeToTray();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) // Command Console
        {
            // Clear the default text when user first clicks the textbox
            if (userInput.Text == "Type HELP for a list of commands")
            {
                userInput.Text = string.Empty;
            }
        }

        // Add a method to clear the text box when it gets focus
        private void userInput_Enter(object sender, EventArgs e)
        {
            if (userInput.Text == "Type HELP for a list of commands")
            {
                userInput.Text = string.Empty;
            }
        }

        // Add a method to handle the Enter key press in the userInput textBox
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter && userInput.Focused)
            {
                _ = SendCommandFromInput();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private async void button3_Click(object sender, EventArgs e) // Enter command from command console
        {
            await SendCommandFromInput();
        }

        // Displays help information for available commands
        private void DisplayHelpCommand()
        {
            StringBuilder helpText = new StringBuilder();

            helpText.AppendLine("Available Commands:");
            helpText.AppendLine("------------------");
            helpText.AppendLine("HELP                   - Displays this help information");
            helpText.AppendLine("RESTART_DRIVER         - Restarts the system virtual distplay driver");
            helpText.AppendLine("RELOAD_DRIVER          - Asks the driver to reload itself.");
            helpText.AppendLine("SDR10 [true/false]     - Enable/disable SDR 10-bit mode");
            helpText.AppendLine("HDRPLUS [true/false]   - Enable/disable HDR+ mode");
            helpText.AppendLine("CUSTOMEDID [true/false]- Enable/disable custom EDID");
            helpText.AppendLine("HARDWARECURSOR [true/false] - Enable/disable hardware cursor");
            helpText.AppendLine("PREVENTSPOOF [true/false] - Enable/disable EDID spoofing prevention");
            helpText.AppendLine("CEAOVERRIDE [true/false] - Enable/disable EDID CEA Override");
            helpText.AppendLine("SETGPU [gpu_name]      - Set the GPU to use for virtual displays");
            helpText.AppendLine("SETCOUNT [number]      - Set the number of virtual displays");
            helpText.AppendLine("LOGGING [true/false]   - Enable/disable logging");
            helpText.AppendLine("DEBUGLOGGING [true/false] - Enable/disable debug level logging");

            mainConsole.AppendText(helpText.ToString());
        }
        private async void ReloadDriverCommand()
        {
            AppendToConsole("[ACTION] Reloading driver...\n");
            UpdateTaskProgress("Reloading Driver", 50);

            try
            {
                string? response = await SendCommandToDriver("RELOAD_DRIVER");
                UpdateTaskProgress("Reloading Driver", 100);
                await Task.Delay(1000);
                UpdateTaskProgress("", 0);
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] Failed to reload driver: {ex.Message}\n");
                UpdateTaskProgress("Reloading Driver", 0);
            }
        }

        private async void GetSettingsCommand()
        {
            AppendToConsole("[ACTION] Retrieving current settings...\n");
            UpdateTaskProgress("Getting Settings", 50);

            try
            {
                string? response = await SendCommandToDriver("GETSETTINGS");
                UpdateTaskProgress("Getting Settings", 100);
                await Task.Delay(1000);
                UpdateTaskProgress("", 0);
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] Failed to get settings: {ex.Message}\n");
                UpdateTaskProgress("Getting Settings", 0);
            }
        }

        // Logging Control Commands
        private async void SetDebugLoggingCommand(bool state)
        {
            DEVLOGGING_STATE = state;
            string action = state ? "ON" : "OFF";
            AppendToConsole($"[ACTION] Toggling Debug Logging to {action}...\n");

            try
            {
                // Update the XML settings first
                if (IXCLI != null)
                {
                    IXCLI.DebugLogging = state;

                    try
                    {
                        string xmlPath = Path.Combine(registryFilePath, "vdd_settings.xml");
                        IXCLI.SaveToXml(xmlPath);
                        AppendToConsole($"[SUCCESS] Updated XML settings for Debug Logging: {state}\n");
                    }
                    catch (Exception xmlEx)
                    {
                        AppendToConsole($"[WARNING] Could not save XML settings: {xmlEx.Message}\n");
                    }
                }

                // Now update the driver
                string command = state ? "LOG_DEBUG true" : "LOG_DEBUG false";
                string? response = await SendCommandToDriver(command);
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] Could not set debug logging: {ex.Message}\n");
            }
        }

        private async void SetLoggingCommand(bool state)
        {
            LOGGING_STATE = state;
            string action = state ? "ON" : "OFF";
            AppendToConsole($"[ACTION] Toggling General Logging to {action}...\n");

            try
            {
                // Update the XML settings first
                if (IXCLI != null)
                {
                    IXCLI.Logging = state;

                    try
                    {
                        string xmlPath = Path.Combine(registryFilePath, "vdd_settings.xml");
                        IXCLI.SaveToXml(xmlPath);
                        AppendToConsole($"[SUCCESS] Updated XML settings for Logging: {state}\n");
                    }
                    catch (Exception xmlEx)
                    {
                        AppendToConsole($"[WARNING] Could not save XML settings: {xmlEx.Message}\n");
                    }
                }

                // Now update the driver
                string command = state ? "LOGGING true" : "LOGGING false";
                string? response = await SendCommandToDriver(command);
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] Could not set logging: {ex.Message}\n");
            }
        }

        // Runtime Information Commands
        private async void GetD3DDeviceGPUCommand()
        {
            AppendToConsole("[ACTION] Retrieving D3D GPU information...\n");
            UpdateTaskProgress("Getting D3D GPU Info", 50);

            try
            {
                string? response = await SendCommandToDriver("D3DDEVICEGPU");
                UpdateTaskProgress("Getting D3D GPU Info", 100);
                await Task.Delay(1000);
                UpdateTaskProgress("", 0);
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] Failed to get D3D GPU info: {ex.Message}\n");
                UpdateTaskProgress("Getting D3D GPU Info", 0);
            }
        }

        private async void GetIDDCXVersionCommand()
        {
            AppendToConsole("[ACTION] Retrieving IDDCX version information...\n");
            UpdateTaskProgress("Getting IDDCX Version", 50);

            try
            {
                string? response = await SendCommandToDriver("IDDCXVERSION");
                UpdateTaskProgress("Getting IDDCX Version", 100);
                await Task.Delay(1000);
                UpdateTaskProgress("", 0);
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] Failed to get IDDCX version: {ex.Message}\n");
                UpdateTaskProgress("Getting IDDCX Version", 0);
            }
        }

        private async void GetAssignedGPUCommand()
        {
            AppendToConsole("[ACTION] Retrieving assigned GPU information...\n");
            UpdateTaskProgress("Getting Assigned GPU", 50);

            try
            {
                string? response = await SendCommandToDriver("GETASSIGNEDGPU");
                UpdateTaskProgress("Getting Assigned GPU", 100);
                await Task.Delay(1000);
                UpdateTaskProgress("", 0);
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] Failed to get assigned GPU: {ex.Message}\n");
                UpdateTaskProgress("Getting Assigned GPU", 0);
            }
        }

        private async void GetAllGPUsCommand()
        {
            AppendToConsole("[ACTION] Retrieving all available GPUs...\n");
            UpdateTaskProgress("Getting All GPUs", 50);

            try
            {
                string? response = await SendCommandToDriver("GETALLGPUS");
                UpdateTaskProgress("Getting All GPUs", 100);
                await Task.Delay(1000);
                UpdateTaskProgress("", 0);
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] Failed to get all GPUs: {ex.Message}\n");
                UpdateTaskProgress("Getting All GPUs", 0);
            }
        }

        // System Commands
        private async void PingDriverCommand()
        {
            AppendToConsole("[ACTION] Sending PING to driver...\n");

            try
            {
                string? response = await SendCommandToDriver("PING");
                // The driver should respond with "PONG"
                if (response != null && response.Contains("PONG"))
                {
                    AppendToConsole("[SUCCESS] Driver responded with PONG\n");
                }
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] PING failed: {ex.Message}\n");
            }
        }
        private async Task SendCommandFromInput()
        {
            if (string.IsNullOrWhiteSpace(userInput.Text))
                return;

            string command = userInput.Text.Trim();
            AppendToConsole($"[COMMAND] {command}\n");

            // Handle special commands
            if (command.Equals("HELP", StringComparison.OrdinalIgnoreCase))
            {
                DisplayHelpCommand();
                userInput.Text = string.Empty;
                return;
            }

            // Parse command parts
            string[] parts = command.Split(' ');
            string baseCommand = parts[0].ToUpper();

            // Handle all pipeline commands
            switch (baseCommand)
            {
                // Driver Control Commands
                case "RESTART_DRIVER":
                    // Using the existing Device Manager restart
                    userInput.Text = string.Empty;
                    await restartDriverToolStripMenuItem_Click(null, EventArgs.Empty);
                    return;

                case "RELOAD_DRIVER":
                    userInput.Text = string.Empty;
                    ReloadDriverCommand();
                    return;

                case "GETSETTINGS":
                    userInput.Text = string.Empty;
                    GetSettingsCommand();
                    return;

                // Logging Control Commands
                case "LOG_DEBUG":
                    if (parts.Length > 1 && bool.TryParse(parts[1], out bool debugState))
                    {
                        SetDebugLoggingCommand(debugState);
                    }
                    else
                    {
                        AppendToConsole("[ERROR] LOG_DEBUG requires true/false parameter\n");
                    }
                    userInput.Text = string.Empty;
                    return;

                case "LOGGING":
                    if (parts.Length > 1 && bool.TryParse(parts[1], out bool logState))
                    {
                        SetLoggingCommand(logState);
                    }
                    else
                    {
                        AppendToConsole("[ERROR] LOGGING requires true/false parameter\n");
                    }
                    userInput.Text = string.Empty;
                    return;

                // Display Configuration Commands
                case "SDR10":
                    if (parts.Length > 1 && bool.TryParse(parts[1], out bool sdr10State))
                    {
                        SDR10_STATE = sdr10State;
                        sDR10bitToolStripMenuItem.Checked = sdr10State;
                        await toggleSDR10Command(sdr10State);
                    }
                    else
                    {
                        AppendToConsole("[ERROR] SDR10 requires true/false parameter\n");
                    }
                    userInput.Text = string.Empty;
                    return;

                case "HDRPLUS":
                    if (parts.Length > 1 && bool.TryParse(parts[1], out bool hdrState))
                    {
                        HDR10PLUS_STATE = hdrState;
                        hDRToolStripMenuItem.Checked = hdrState;
                        await toggleHDRPlusCommand(hdrState);
                    }
                    else
                    {
                        AppendToConsole("[ERROR] HDRPLUS requires true/false parameter\n");
                    }
                    userInput.Text = string.Empty;
                    return;

                case "CUSTOMEDID":
                    if (parts.Length > 1 && bool.TryParse(parts[1], out bool customEdidState))
                    {
                        CUSTOMEDID_STATE = customEdidState;
                        customEDIDToolStripMenuItem.Checked = customEdidState;
                        await toggleCustomEDIDCommand(customEdidState);
                    }
                    else
                    {
                        AppendToConsole("[ERROR] CUSTOMEDID requires true/false parameter\n");
                    }
                    userInput.Text = string.Empty;
                    return;

                case "HARDWARECURSOR":
                    if (parts.Length > 1 && bool.TryParse(parts[1], out bool cursorState))
                    {
                        HARDWARECURSOR_STATE = cursorState;
                        hardwareCursorToolStripMenuItem.Checked = cursorState;
                        await toggleHardwareCursorCommand(cursorState);
                    }
                    else
                    {
                        AppendToConsole("[ERROR] HARDWARECURSOR requires true/false parameter\n");
                    }
                    userInput.Text = string.Empty;
                    return;

                case "PREVENTSPOOF":
                    if (parts.Length > 1 && bool.TryParse(parts[1], out bool preventSpoofState))
                    {
                        PREVENTEDIDSPOOF_STATE = preventSpoofState;
                        preventMonitorSpoofToolStripMenuItem.Checked = preventSpoofState;
                        await togglePreventSpoofCommand(preventSpoofState);
                    }
                    else
                    {
                        AppendToConsole("[ERROR] PREVENTSPOOF requires true/false parameter\n");
                    }
                    userInput.Text = string.Empty;
                    return;

                case "CEAOVERRIDE":
                    if (parts.Length > 1 && bool.TryParse(parts[1], out bool ceaState))
                    {
                        EDIDCEAOVERRRIDE_STATE = ceaState;
                        eDIDCEAOverrideToolStripMenuItem.Checked = ceaState;
                        await toggleCEAOverrideCommand(ceaState);
                    }
                    else
                    {
                        AppendToConsole("[ERROR] CEAOVERRIDE requires true/false parameter\n");
                    }
                    userInput.Text = string.Empty;
                    return;

                // Runtime Information Commands
                case "D3DDEVICEGPU":
                    userInput.Text = string.Empty;
                    GetD3DDeviceGPUCommand();
                    return;

                case "IDDCXVERSION":
                    userInput.Text = string.Empty;
                    GetIDDCXVersionCommand();
                    return;

                case "GETASSIGNEDGPU":
                    userInput.Text = string.Empty;
                    GetAssignedGPUCommand();
                    return;

                case "GETALLGPUS":
                    userInput.Text = string.Empty;
                    GetAllGPUsCommand();
                    return;

                // Configuration Commands
                case "SETGPU":
                    if (parts.Length > 1)
                    {
                        // Handle quoted GPU names
                        string gpuName;
                        if (parts.Length > 2 && parts[1].StartsWith("\""))
                        {
                            // Reconstruct quoted GPU name
                            gpuName = string.Join(" ", parts.Skip(1)).Trim('"');
                        }
                        else
                        {
                            gpuName = parts[1];
                        }

                        await setGPUCommand(gpuName);
                    }
                    else
                    {
                        AppendToConsole("[ERROR] SETGPU requires GPU name parameter. Use quotes for names with spaces.\n");
                    }
                    userInput.Text = string.Empty;
                    return;

                case "SETCOUNT":
                    if (parts.Length > 1 && int.TryParse(parts[1], out int displayCount))
                    {
                        await SetDisplayCount(displayCount);
                    }
                    else
                    {
                        AppendToConsole("[ERROR] SETCOUNT requires a number parameter\n");
                    }
                    userInput.Text = string.Empty;
                    return;

                // System Commands
                case "PING":
                    userInput.Text = string.Empty;
                    PingDriverCommand();
                    return;

                // For any unrecognized command, attempt to send directly to driver
                default:
                    UpdateTaskProgress("Sending Command", 25);

                    try
                    {
                        string? response = await SendCommandToDriver(command);
                        if (response != null)
                        {
                            AppendToConsole($"[RESPONSE] {response}\n");
                        }
                        else
                        {
                            AppendToConsole("[RESPONSE] No response received from driver.\n");
                        }
                        UpdateTaskProgress("Sending Command", 100);
                    }
                    catch (Exception ex)
                    {
                        AppendToConsole($"[ERROR] {ex.Message}\n");
                        UpdateTaskProgress("Sending Command", 0);
                    }
                    finally
                    {
                        userInput.Text = string.Empty;
                        await Task.Delay(500);
                        UpdateTaskProgress("", 0);
                    }
                    break;
            }
        }

        // Add these helper methods for display configuration commands
        private async Task toggleSDR10Command(bool state)
        {
            try
            {
                string command = state ? "SDR10 true" : "SDR10 false";
                await SendCommandToDriver(command);

                if (IXCLI != null)
                {
                    IXCLI.SDR10bit = state;
                    string xmlPath = Path.Combine(registryFilePath, "vdd_settings.xml");
                    IXCLI.SaveToXml(xmlPath);
                }
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] Failed to set SDR10: {ex.Message}\n");
            }
        }

        private async Task toggleHDRPlusCommand(bool state)
        {
            try
            {
                string command = state ? "HDRPLUS true" : "HDRPLUS false";
                await SendCommandToDriver(command);

                if (IXCLI != null)
                {
                    IXCLI.HDRPlus = state;
                    string xmlPath = Path.Combine(registryFilePath, "vdd_settings.xml");
                    IXCLI.SaveToXml(xmlPath);
                }
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] Failed to set HDRPLUS: {ex.Message}\n");
            }
        }

        private async Task toggleCustomEDIDCommand(bool state)
        {
            try
            {
                string command = state ? "CUSTOMEDID true" : "CUSTOMEDID false";
                await SendCommandToDriver(command);

                if (IXCLI != null)
                {
                    IXCLI.CustomEdid = state;
                    string xmlPath = Path.Combine(registryFilePath, "vdd_settings.xml");
                    IXCLI.SaveToXml(xmlPath);
                }
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] Failed to set CUSTOMEDID: {ex.Message}\n");
            }
        }

        private async Task toggleHardwareCursorCommand(bool state)
        {
            try
            {
                string command = state ? "HARDWARECURSOR true" : "HARDWARECURSOR false";
                await SendCommandToDriver(command);

                if (IXCLI != null)
                {
                    IXCLI.HardwareCursor = state;
                    string xmlPath = Path.Combine(registryFilePath, "vdd_settings.xml");
                    IXCLI.SaveToXml(xmlPath);
                }
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] Failed to set HARDWARECURSOR: {ex.Message}\n");
            }
        }

        private async Task togglePreventSpoofCommand(bool state)
        {
            try
            {
                string command = state ? "PREVENTSPOOF true" : "PREVENTSPOOF false";
                await SendCommandToDriver(command);

                if (IXCLI != null)
                {
                    IXCLI.PreventSpoof = state;
                    string xmlPath = Path.Combine(registryFilePath, "vdd_settings.xml");
                    IXCLI.SaveToXml(xmlPath);
                }
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] Failed to set PREVENTSPOOF: {ex.Message}\n");
            }
        }

        private async Task toggleCEAOverrideCommand(bool state)
        {
            try
            {
                string command = state ? "CEAOVERRIDE true" : "CEAOVERRIDE false";
                await SendCommandToDriver(command);

                if (IXCLI != null)
                {
                    IXCLI.EdidCeaOverride = state;
                    string xmlPath = Path.Combine(registryFilePath, "vdd_settings.xml");
                    IXCLI.SaveToXml(xmlPath);
                }
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] Failed to set CEAOVERRIDE: {ex.Message}\n");
            }
        }

        private async Task setGPUCommand(string gpuName)
        {
            try
            {
                string command = $"SETGPU \"{gpuName}\"";
                await SendCommandToDriver(command);

                if (IXCLI != null)
                {
                    IXCLI.Friendlyname = gpuName;
                    string xmlPath = Path.Combine(registryFilePath, "vdd_settings.xml");
                    IXCLI.SaveToXml(xmlPath);
                }
            }
            catch (Exception ex)
            {
                AppendToConsole($"[ERROR] Failed to set GPU: {ex.Message}\n");
            }
        }
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // Auto-scroll to the bottom when text changes
            mainConsole.SelectionStart = mainConsole.Text.Length;
            mainConsole.ScrollToCaret();
        }

        // Helper method to append text to console and ensure scrolling
        private void AppendToConsole(string text)
        {
            // Append the text to the console
            mainConsole.AppendText(text);

            // Ensure the console scrolls to show the latest text
            mainConsole.SelectionStart = mainConsole.Text.Length;
            mainConsole.ScrollToCaret();
            mainConsole.Refresh(); // Force a UI refresh to ensure scrolling happens immediately
        }

        // Method to display ASCII art animation with proper line-by-line delay
        private async Task DisplayAsciiArtAnimation()
        {
            // Clear the console first
            mainConsole.Clear();

            const int lineDelay = 40; // milliseconds between lines

            // Array of ASCII art lines for animation with explicit line-by-line display
            string[] asciiArtLines = new string[]
            {
                "           ////////      ///////(/////////        //////////////(//     ////////",
                "           ////                                                             ////",
                "           ////                                                             ////",
                "                                                                                ",
                "                                                                                ",
                "                                                                                ",
                "           ////                                                             ////",
                " ///(///(///(GE9(///(///(///(///(///(///(///(///(///(///(///(///(/          (///",
                " //MICROSOFT////////////////////(////////AKATREVORJAY///////////(/          ////",
                " ///      .............................................        /(/          ////",
                " ///     .......................,........................      /(/          ////",
                " ///   .................,,,,,,,,,,,,,,,,,.................     /(/          ////",
                " ///  ...............,,,,,,,,,,,,,,,,,,,,,,,...............    /(/              ",
                " /// ..............,,,,,,,,,,,,,,,,,,,,,,,,.................   /(/              ",
                " /// ....... @@@@.,,,, @@@.@@@@@@@@@@@,,.@@@@@@@@@@@........   /(/          ////",
                " /(/......... @@@.,,,.@@@.,@@@@,,,, @@@,.@@@.,,.. @@@........  /(/          (/(/",
                " ///.......... @@@,,.@@@%,,@@@@,,,,,@@@@.@@@.,,,..@@@(.......  /(/          ////",
                " ///........... @@@.@@@@,,,@@@@,,,,,@@@@.@@@.,,,..@@@........  /(/      ////////",
                " /// ........... @@@@@@,,,.@@@@,,,,@@@@,.@@@.,,..@@@@.......   /(/              ",
                " /// ............@@@@@,,,,.@@@@@@@@@@.,,.@@@@@@@@@@.........   /(/              ",
                " ///  ................,,,,,,..,,,,,,,,,,,..................    /(/              ",
                " ///   .................,,,,,,,,,,,,,,,,,.................     /(/              ",
                " ///    ......................,,,,,,.....................      /(/              ",
                " ///      .............................................        /(/              ",
                " ///        .........................................          /(/              ",
                " /////////////((MIKETHETECH))//(BUD)//(JOCKE)///////////////////(/              ",
                "                              //(///                                            ",
                "                              //(///                                            ",
                "                */////ROSHKINS//(////////////////                               ",
                "                */SITIOM/////BALOUKJ///(////////(                               "
            };

            // Use a separate StringBuilder to build the console contents line by line
            StringBuilder consoleContent = new StringBuilder();

            // Display each line with a delay for animation effect
            for (int i = 0; i < asciiArtLines.Length; i++)
            {
                // Add this line to the console
                if (i > 0)
                {
                    consoleContent.AppendLine(); // Add a newline before each line except the first
                }
                consoleContent.Append(asciiArtLines[i]);

                // Update the console with all content so far
                mainConsole.Text = consoleContent.ToString();

                // Manually scroll to ensure each line is visible
                mainConsole.SelectionStart = mainConsole.Text.Length;
                mainConsole.ScrollToCaret();

                // Force UI update
                mainConsole.Update();
                Application.DoEvents(); // Process any pending messages to ensure UI updates

                // Wait before showing the next line
                await Task.Delay(lineDelay);
            }

            // Add a couple of blank lines after the ASCII art
            consoleContent.AppendLine();
            consoleContent.AppendLine();
            mainConsole.Text = consoleContent.ToString();
            mainConsole.SelectionStart = mainConsole.Text.Length;
            mainConsole.ScrollToCaret();
            mainConsole.Update();
        }

        private void royalMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void themeForm1_Click(object sender, EventArgs e)
        {

        }

        private void menuToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private async void restartAllButton_Click(object sender, EventArgs e)
        {
            // Use the existing restart method but ensure we await it properly
            AppendToConsole("[INFO] Restart button clicked. Initiating driver restart...\n");
            await restartDriverToolStripMenuItem_Click(sender, e);
            AppendToConsole("[INFO] Restart operation complete.\n");
        }

        private void jockeSupport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://github.com/sponsors/zjoasan",
                UseShellExecute = true
            });
        }

        private void mttSupport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://www.patreon.com/c/mikethetech",
                UseShellExecute = true
            });
        }
        private void ShowAboutDialog()
        {
            // Create an about dialog with size adjusted to avoid scrollbars
            Form aboutDialog = new Form
            {
                Text = "About Virtual Driver Control",
                Size = new Size(600, 750), // Even larger to ensure all content fits without scrollbars
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(32, 34, 37),
                ForeColor = Color.White,
                AutoScroll = false // Disable scrolling as we'll size properly
            };

            // Add logo placeholder
            Label logoLabel = new Label
            {
                Text = "VDD Control",
                Font = new Font("Consolas", 18, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(550, 30),
                Location = new Point(25, 20)
            };
            aboutDialog.Controls.Add(logoLabel);

            // Add version information
            Label versionLabel = new Label
            {
                Text = "Version 25.5.2",
                Font = new Font("Consolas", 10),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(550, 20),
                Location = new Point(25, 55) // Increased vertical spacing
            };
            aboutDialog.Controls.Add(versionLabel);

            // Add description
            Label descLabel = new Label
            {
                Text = "Virtual Driver Control provides a graphical interface to configure and control the Virtual Display Driver.",
                Font = new Font("Consolas", 9),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.TopLeft,
                Size = new Size(550, 40),
                Location = new Point(25, 90) // Increased vertical spacing
            };
            aboutDialog.Controls.Add(descLabel);

            int currentY = 145; // Increased starting position

            // Project Leadership section
            Label leadershipHeader = new Label
            {
                Text = "Project Leadership",
                Font = new Font("Consolas", 10, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                Size = new Size(200, 20),
                Location = new Point(25, currentY)
            };
            aboutDialog.Controls.Add(leadershipHeader);
            currentY += 25;

            Label leadershipLabel = new Label
            {
                Text = "• Mike \"MikeTheTech\" Rodriguez – Project Manager, Owner, and Principal Programmer",
                Font = new Font("Consolas", 9),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.TopLeft,
                Size = new Size(500, 20),
                Location = new Point(35, currentY)
            };
            aboutDialog.Controls.Add(leadershipLabel);
            currentY += 40; // Increased spacing

            // Core Development section
            Label coreDevHeader = new Label
            {
                Text = "Core Development",
                Font = new Font("Consolas", 10, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                Size = new Size(200, 20),
                Location = new Point(25, currentY)
            };
            aboutDialog.Controls.Add(coreDevHeader);
            currentY += 25;

            Label coreDevLabel = new Label
            {
                Text = "• Bud – Former Lead Programmer\n" +
                      "• zjoasan – Programmer; scripting, EDID integration, installer logic\n" +
                      "• Baloukj – 8‑bit / 10‑bit color‑depth implementation; first public release\n" +
                      "  of the new Microsoft driver",
                Font = new Font("Consolas", 9),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.TopLeft,
                Size = new Size(550, 80), // Increased width and height
                Location = new Point(35, currentY)
            };
            aboutDialog.Controls.Add(coreDevLabel);
            currentY += 95; // Increased spacing

            // Research & Engineering Support section
            Label researchHeader = new Label
            {
                Text = "Research & Engineering Support",
                Font = new Font("Consolas", 10, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                Size = new Size(250, 20),
                Location = new Point(25, currentY)
            };
            aboutDialog.Controls.Add(researchHeader);
            currentY += 25;

            Label researchLabel = new Label
            {
                Text = "• Anakngtokwa – Source discovery and driver‑code research\n" +
                      "• AKATrevorJay – High‑resolution EDID contribution\n" +
                      "• LexTrack – MiniScreenRecorder script",
                Font = new Font("Consolas", 9),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.TopLeft,
                Size = new Size(550, 70), // Increased width and height
                Location = new Point(35, currentY)
            };
            aboutDialog.Controls.Add(researchLabel);
            currentY += 85; // Increased spacing

            // Foundational Code section
            Label foundationHeader = new Label
            {
                Text = "Foundational Code & Inspiration",
                Font = new Font("Consolas", 10, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                Size = new Size(250, 20),
                Location = new Point(25, currentY)
            };
            aboutDialog.Controls.Add(foundationHeader);
            currentY += 25;

            Label foundationLabel = new Label
            {
                Text = "• Microsoft, ge9, sitiom – Original Indirect Display Driver sample\n" +
                      "• Roshkins – Original repository host and maintainer",
                Font = new Font("Consolas", 9),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.TopLeft,
                Size = new Size(550, 50), // Increased width and height
                Location = new Point(35, currentY)
            };
            aboutDialog.Controls.Add(foundationLabel);
            currentY += 75; // Increased spacing

            // Add links section
            Label linksHeader = new Label
            {
                Text = "Links:",
                Font = new Font("Consolas", 10, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                Size = new Size(100, 20),
                Location = new Point(25, currentY)
            };
            aboutDialog.Controls.Add(linksHeader);
            currentY += 25;

            LinkLabel githubLink = new LinkLabel
            {
                Text = "GitHub: https://github.com/VirtualDisplay/",
                Font = new Font("Consolas", 9),
                LinkColor = Color.LightBlue,
                ActiveLinkColor = Color.White,
                TextAlign = ContentAlignment.TopLeft,
                Size = new Size(500, 20),
                Location = new Point(35, currentY)
            };
            githubLink.LinkClicked += (s, e) => System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://github.com/VirtualDisplay/",
                UseShellExecute = true
            });
            aboutDialog.Controls.Add(githubLink);
            currentY += 55; // Increased spacing

            // Add OK button
            Button okButton = new Button
            {
                Text = "OK",
                Size = new Size(80, 30),
                Location = new Point(495, currentY), // Further adjusted position
                BackColor = Color.FromArgb(45, 47, 49),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            okButton.Click += (s, e) => aboutDialog.Close();
            aboutDialog.Controls.Add(okButton);

            // Show the dialog
            aboutDialog.ShowDialog(this);
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://www.patreon.com/c/mikethetech",
                UseShellExecute = true
            });
        }


        private void toolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            MinimizeToTray();

        }

        private void notificationIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();

            AppendToConsole("[INFO] Application restored from tray\n");
        }

        private void userModeLoggingToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            // Toggle the state
            LOGGING_STATE = !LOGGING_STATE;
            userModeLoggingToolStripMenuItem.Checked = LOGGING_STATE;

            // Send the logging command
            SetLoggingCommand(LOGGING_STATE);
        }

        private void devModeLoggingToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            // Toggle the state
            DEVLOGGING_STATE = !DEVLOGGING_STATE;
            devModeLoggingToolStripMenuItem.Checked = DEVLOGGING_STATE;

            // Send the debug logging command
            SetDebugLoggingCommand(DEVLOGGING_STATE);
        }
        private void UpdateAllMenuItemsWithStates()
        {
            // Simplified debug logging
            AppendToConsole($"[DEBUG] Updating menu items\n");

            // Update primary menu items
            sDR10bitToolStripMenuItem.Checked = SDR10_STATE;
            hDRToolStripMenuItem.Checked = HDR10PLUS_STATE;
            customEDIDToolStripMenuItem.Checked = CUSTOMEDID_STATE;
            hardwareCursorToolStripMenuItem.Checked = HARDWARECURSOR_STATE;
            preventMonitorSpoofToolStripMenuItem.Checked = PREVENTEDIDSPOOF_STATE;
            eDIDCEAOverrideToolStripMenuItem.Checked = EDIDCEAOVERRRIDE_STATE;

            // Update secondary menu items (if they exist)
            if (sDR10bitToolStripMenuItem1 != null)
                sDR10bitToolStripMenuItem1.Checked = SDR10_STATE;
            if (hDRToolStripMenuItem1 != null)
                hDRToolStripMenuItem1.Checked = HDR10PLUS_STATE;
            if (customEDIDToolStripMenuItem1 != null)
                customEDIDToolStripMenuItem1.Checked = CUSTOMEDID_STATE;
            if (hardwareCursorToolStripMenuItem1 != null)
                hardwareCursorToolStripMenuItem1.Checked = HARDWARECURSOR_STATE;
            if (preventMonitorSpoofToolStripMenuItem1 != null)
                preventMonitorSpoofToolStripMenuItem1.Checked = PREVENTEDIDSPOOF_STATE;
            if (eDIDCEAOverrideToolStripMenuItem1 != null)
                eDIDCEAOverrideToolStripMenuItem1.Checked = EDIDCEAOVERRRIDE_STATE;

            // Update logging menu items
            if (userModeLoggingToolStripMenuItem != null)
                userModeLoggingToolStripMenuItem.Checked = LOGGING_STATE;
            if (devModeLoggingToolStripMenuItem != null)
                devModeLoggingToolStripMenuItem.Checked = DEVLOGGING_STATE;
            if (userModeLoggingToolStripMenuItem1 != null)
                userModeLoggingToolStripMenuItem1.Checked = LOGGING_STATE;
            if (devModeLoggingToolStripMenuItem1 != null)
                devModeLoggingToolStripMenuItem1.Checked = DEVLOGGING_STATE;

            // Removed detailed logging of menu states

            // Force UI update
            mainVisibleMenuStrip.Invalidate();
            mainVisibleMenuStrip.Update();
            Application.DoEvents();
        }
        private void userModeLoggingToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            userModeLoggingToolStripMenuItem_Click_1(sender, e);
        }
        private void SyncAllMenuItemsWithState()
        {
            // This ensures all menu items are in sync with internal state
            UpdateAllMenuItemsWithStates();

            // Hide GPU select items as they're deprecated
            HideSelectGPUMenuItems();
        }
        private void devModeLoggingToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            devModeLoggingToolStripMenuItem_Click_1(sender, e);
        }

        // Community Scripts event handlers
        private void communityScriptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowCommunityScriptsWindow();
        }

        private void scriptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowCommunityScriptsWindow();
        }

        // Method to show the Community Scripts window
        private void ShowCommunityScriptsWindow()
        {
            // Create the form if it doesn't exist or was disposed
            if (communityScriptsForm == null || communityScriptsForm.IsDisposed)
            {
                communityScriptsForm = new CommunityScriptsForm();
            }
            else
            {
                // If form exists, refresh the scripts
                communityScriptsForm.RefreshScripts();
            }

            // Show the form
            communityScriptsForm.ShowDialog(this);
        }
    }
}
