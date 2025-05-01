using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace VDD_Control
{
    public partial class mainWindow : Form
    {
        private const string PIPE_NAME = "MTTVirtualDisplayPipe";
        string registryFilePath = "C:\\VirtualDisplayDriver"; //Lets not use null, just in case

        private XMLController IXCLI;

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
            ToolStripMenuItem restartItem = GetRestartDriverToolStripMenuItem(); // This is now safe
            string settingsPath = LocateSettingsFile();

            try
            {
                // Only initialize if we found a valid settings path
                if (!string.IsNullOrEmpty(settingsPath))
                {
                    IXCLI = new XMLController(settingsPath);
                    SDR10_STATE = IXCLI.SDR10bit;
                    CUSTOMEDID_STATE = IXCLI.CustomEdid;
                    EDIDCEAOVERRRIDE_STATE = IXCLI.EdidCeaOverride;
                    PREVENTEDIDSPOOF_STATE = IXCLI.PreventSpoof;
                    HARDWARECURSOR_STATE = IXCLI.HardwareCursor;
                    LOGGING_STATE = IXCLI.Logging;
                    DEVLOGGING_STATE = IXCLI.DebugLogging;

                    sDR10bitToolStripMenuItem.Checked = SDR10_STATE;
                }
                else
                {
                    mainConsole.AppendText("[ERROR] Could not locate settings file in any expected location.\n");
                }
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

            // Only try to load XML if IXCLI was successfully initialized
            if (IXCLI != null)
            {
                try
                {
                    // No need to load again, already loaded in constructor
                }
                catch (Exception ex)
                {
                    mainConsole.AppendText($"[ERROR] Failed to load settings: {ex.Message}\n");
                }
            }

            // Set text color for all menu items
            foreach (ToolStripMenuItem item in mainVisibleMenuStrip.Items)
            {
                SetMenuItemStyle(item);
            }
            mainConsole.AppendText("           ////////      ///////(/////////        //////////////(//     ////////\n");
            await Task.Delay(40);
            mainConsole.AppendText("           ////                                                             ////\n");
            await Task.Delay(40);
            mainConsole.AppendText("           ////                                                             ////\n");
            await Task.Delay(40);
            mainConsole.AppendText("                                                                                \n");
            await Task.Delay(40);
            mainConsole.AppendText("                                                                                \n");
            await Task.Delay(40);
            mainConsole.AppendText("                                                                                \n");
            await Task.Delay(40);
            mainConsole.AppendText("           ////                                                             ////\n");
            await Task.Delay(40);
            mainConsole.AppendText(" ///(///(///(///(///(///(///(///(///(///(///(///(///(///(///(///(/          (///\n");
            await Task.Delay(40);
            mainConsole.AppendText(" ///////////////////////////////(///////////////////////////////(/          ////\n");
            await Task.Delay(40);
            mainConsole.AppendText(" ///      .............................................        /(/          ////\n");
            await Task.Delay(40);
            mainConsole.AppendText(" ///     .......................,........................      /(/          ////\n");
            await Task.Delay(40);
            mainConsole.AppendText(" ///   .................,,,,,,,,,,,,,,,,,.................     /(/          ////\n");
            await Task.Delay(40);
            mainConsole.AppendText(" ///  ...............,,,,,,,,,,,,,,,,,,,,,,,...............    /(/              \n");
            await Task.Delay(40);
            mainConsole.AppendText(" /// ..............,,,,,,,,,,,,,,,,,,,,,,,,.................   /(/              \n");
            await Task.Delay(40);
            mainConsole.AppendText(" /// ....... @@@@.,,,, @@@.@@@@@@@@@@@,,.@@@@@@@@@@@........   /(/          ////\n");
            await Task.Delay(40);
            mainConsole.AppendText(" /(/......... @@@.,,,.@@@.,@@@@,,,, @@@,.@@@.,,.. @@@........  /(/          (/(/\n");
            await Task.Delay(40);
            mainConsole.AppendText(" ///.......... @@@,,.@@@%,,@@@@,,,,,@@@@.@@@.,,,..@@@(.......  /(/          ////\n");
            await Task.Delay(40);
            mainConsole.AppendText(" ///........... @@@.@@@@,,,@@@@,,,,,@@@@.@@@.,,,..@@@........  /(/      ////////\n");
            await Task.Delay(40);
            mainConsole.AppendText(" /// ........... @@@@@@,,,.@@@@,,,,@@@@,.@@@.,,..@@@@.......   /(/              \n");
            await Task.Delay(40);
            mainConsole.AppendText(" /// ............@@@@@,,,,.@@@@@@@@@@.,,.@@@@@@@@@@.........   /(/              \n");
            await Task.Delay(40);
            mainConsole.AppendText(" ///  ................,,,,,,..,,,,,,,,,,,..................    /(/              \n");
            await Task.Delay(40);
            mainConsole.AppendText(" ///   .................,,,,,,,,,,,,,,,,,.................     /(/              \n");
            await Task.Delay(40);
            mainConsole.AppendText(" ///    ......................,,,,,,.....................      /(/              \n");
            await Task.Delay(40);
            mainConsole.AppendText(" ///      .............................................        /(/              \n");
            await Task.Delay(40);
            mainConsole.AppendText(" ///        .........................................          /(/              \n");
            await Task.Delay(40);
            mainConsole.AppendText(" /////////////((MIKETHETECH))//(BUD)//(JOCKE)///////////////////(/              \n");
            await Task.Delay(40);
            mainConsole.AppendText("                              //(///                                            \n");
            await Task.Delay(40);
            mainConsole.AppendText("                              //(///                                            \n");
            await Task.Delay(40);
            mainConsole.AppendText("                *///////////////(////////////////                               \n");
            await Task.Delay(40);
            mainConsole.AppendText("                *///////////////(///////////////(\n\n");
            await Task.Delay(40);

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
                mainConsole.AppendText(systemInfo);
                mainConsole.Refresh(); // Ensure the UI is updated
            }
            catch (Exception ex)
            {
                // Display error details in richTextBox1
                mainConsole.AppendText("An error occurred while retrieving system information:\n" + ex.Message); // This really shouldn't happen. But probably will.
            }

            mainConsole.AppendText("Virtual Display Driver Control Initialized.\n");

            if (!await TryConnectToDriver())
            {
                mainConsole.AppendText("[WARNING] Could not verify driver connection. Ensure the driver is running.\n");
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
                            if (!regPath.EndsWith(".xml"))
                            {
                                // It's a directory path, append the filename
                                fullPath = Path.Combine(regPath, "vdd_settings.xml");
                            }

                            if (File.Exists(fullPath))
                            {
                                registryFilePath = regPath; // Store the directory or full path
                                foundPath = fullPath;       // Return the full file path
                                return foundPath;
                            }
                        }
                    }
                }

                // Fallback to default locations
                string[] fallbackPaths =
                {
                    @"C:\VirtualDisplayDriver\vdd_settings.xml",
                    @"C:\IddSampleDriver\vdd_settings.xml"
                };

                foreach (string path in fallbackPaths)
                {
                    if (File.Exists(path))
                    {
                        // Extract directory path for the XML controller
                        registryFilePath = Path.GetDirectoryName(path);
                        foundPath = path;
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
                        mainConsole.AppendText("[SUCCESS] Connected to the driver.\n");
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    attempt++;
                    mainConsole.AppendText($"[ERROR] Connection failed: {ex.Message} (Attempt {attempt}/{maxAttempts})\n");
                    mainConsole.AppendText("Note: This may also occur if the driver is off or restarting.\n");

                    if (attempt >= maxAttempts)
                    {
                        mainConsole.AppendText("[ERROR] Unable to connect after multiple attempts.\n");
                        return false;
                    }

                    await Task.Delay(2000);
                }
            }

            return false;
        }

        private async Task<string> SendCommandToDriver(string command)
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
                        string response;
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
                        mainConsole.AppendText($"[{command}] Response: {response}\n");

                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                return $"[ERROR] Sending command failed: {ex.Message}";
            }
        }

        private void RestartDriverHandler(object sender, EventArgs e)
        {
            _ = restartDriverToolStripMenuItem_Click(sender, e); // Fire and forget (safe async call)
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
            mainConsole.AppendText("[ACTION] Restarting driver...\n");
            UpdateTaskProgress("Restarting Driver", 10);

            string response;
            try
            {
                response = await SendCommandToDriver("RESTART_DRIVER");
                UpdateTaskProgress("Restarting Driver", 40);
            }
            catch (Exception ex)
            {
                response = $"[ERROR] Could not send restart command: {ex.Message}";
                UpdateTaskProgress("Restarting Driver", 0); // Reset progress bar on error
                return;
            }

            mainConsole.AppendText(response + "\n");
            UpdateTaskProgress("Restarting Driver", 60);

            await Task.Delay(5000);  // Wait for the restart process
            UpdateTaskProgress("Restarting Driver", 80);

            mainConsole.AppendText("[INFO] Attempting to reconnect...\n");

            if (await TryConnectToDriver())
            {
                mainConsole.AppendText("[SUCCESS] Driver restarted and reconnected successfully.\n");
                UpdateTaskProgress("Restarting Driver", 100);
                await Task.Delay(1000); // Show 100% for a moment
                this.BeginInvoke(new Action(() => UpdateTaskProgress("", 0))); // Clear task progress
            }
            else
            {
                mainConsole.AppendText("[WARNING] Driver restart detected, but reconnection failed. Ensure the driver is running.\n");
                UpdateTaskProgress("Restarting Driver", 0); // Reset progress bar on warning
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
            Task.Delay(500).ContinueWith(_ => {
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
            mainConsole.AppendText($"[ACTION] Toggling SDR 10 bit state to {action}...\n");

            string response;
            try
            {
                string command = SDR10_STATE ? "SDR10 true" : "SDR10 false";
                response = await SendCommandToDriver(command); // Send state based off bool
            }
            catch (Exception ex)
            {
                response = $"[ERROR] Could not send toggle SDR command: {ex.Message}";
            }
        }


        private async void hDRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HDR10PLUS_STATE = !HDR10PLUS_STATE;
            hDRToolStripMenuItem.Checked = HDR10PLUS_STATE;

            string action = HDR10PLUS_STATE ? "ON" : "OFF";
            mainConsole.AppendText($"[ACTION] Toggling HDR-10+ state to {action}...\n");

            string response;
            try
            {
                string command = HDR10PLUS_STATE ? "HDRPLUS true" : "HDRPLUS false";
                response = await SendCommandToDriver(command);
            }
            catch (Exception ex)
            {
                response = $"[ERROR] Could not send toggle HDR-10+ command: {ex.Message}";
            }
        }

        private async void customEDIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CUSTOMEDID_STATE = !CUSTOMEDID_STATE;
            customEDIDToolStripMenuItem.Checked = CUSTOMEDID_STATE;

            string action = CUSTOMEDID_STATE ? "ON" : "OFF";
            mainConsole.AppendText($"[ACTION] Toggling Custom Edid state to {action}...\n");

            string response;
            try
            {
                string command = CUSTOMEDID_STATE ? "CUSTOMEDID true" : "CUSTOMEDID false";
                response = await SendCommandToDriver(command);
            }
            catch (Exception ex)
            {
                response = $"[ERROR] Could not send toggle Custom Edid command: {ex.Message}";
            }
        }

        private async void hardwareCursorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HARDWARECURSOR_STATE = !HARDWARECURSOR_STATE;
            hardwareCursorToolStripMenuItem.Checked = HARDWARECURSOR_STATE;

            string action = HARDWARECURSOR_STATE ? "ON" : "OFF";
            mainConsole.AppendText($"[ACTION] Toggling Hardware cursor state to {action}...\n");

            string response;
            try
            {
                string command = HARDWARECURSOR_STATE ? "HARDWARECURSOR true" : "HARDWARECURSOR false";
                response = await SendCommandToDriver(command);
            }
            catch (Exception ex)
            {
                response = $"[ERROR] Could not send toggle Hardware cursor command: {ex.Message}";
            }
        }

        private async void preventMonitorSpoofToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PREVENTEDIDSPOOF_STATE = !PREVENTEDIDSPOOF_STATE;
            preventMonitorSpoofToolStripMenuItem.Checked = PREVENTEDIDSPOOF_STATE;

            string action = PREVENTEDIDSPOOF_STATE ? "ON" : "OFF";
            mainConsole.AppendText($"[ACTION] Toggling Prevent Monitor Spoof state to {action}...\n");

            string response;
            try
            {
                string command = PREVENTEDIDSPOOF_STATE ? "PREVENTSPOOF true" : "PREVENTSPOOF false";
                response = await SendCommandToDriver(command);
            }
            catch (Exception ex)
            {
                response = $"[ERROR] Could not send toggle Prevent Monitor Spoof command: {ex.Message}";
            }
        }

        private async void eDIDCEAOverrideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EDIDCEAOVERRRIDE_STATE = !EDIDCEAOVERRRIDE_STATE;
            eDIDCEAOverrideToolStripMenuItem.Checked = EDIDCEAOVERRRIDE_STATE;

            string action = EDIDCEAOVERRRIDE_STATE ? "ON" : "OFF";
            mainConsole.AppendText($"[ACTION] Toggling Edid Cea Override state to {action}...\n");

            string response;
            try
            {
                string command = EDIDCEAOVERRRIDE_STATE ? "CEAOVERRIDE true" : "CEAOVERRIDE false";
                response = await SendCommandToDriver(command);
            }
            catch (Exception ex)
            {
                response = $"[ERROR] Could not send toggle Edid Cea Override command: {ex.Message}";
            }
        }

        private void selectGPUToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void displayCountToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void enableToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void disableDriverToolStripMenuItem_Click(object sender, EventArgs e)
        {

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

        }

        private void getDisplayInformationToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {

        }

        private void getGPUInformationToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {

        }

        private void getCPUInformationToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {

        }

        private void getAudioInformationToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {

        }

        private void sDR10bitToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void hDRToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void customEDIDToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void hardwareCursorToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void preventMonitorSpoofToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void eDIDCEAOverrideToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void selectGPUToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void displayCountToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void enableDriverToolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void disableDriverToolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void restartDriverToolStripMenuItem2_Click(object sender, EventArgs e)
        {

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
            helpText.AppendLine("RESTART_DRIVER         - Restarts the virtual display driver");
            helpText.AppendLine("SDR10 [true/false]     - Enable/disable SDR 10-bit mode");
            helpText.AppendLine("HDRPLUS [true/false]   - Enable/disable HDR+ mode");
            helpText.AppendLine("CUSTOMEDID [true/false]- Enable/disable custom EDID");
            helpText.AppendLine("HARDWARECURSOR [true/false] - Enable/disable hardware cursor");
            helpText.AppendLine("PREVENTSPOOF [true/false] - Enable/disable EDID spoofing prevention");
            helpText.AppendLine("CEAOVERRIDE [true/false] - Enable/disable EDID CEA Override");
            helpText.AppendLine("SETGPU [gpu_name]      - Set the GPU to use for virtual displays");
            helpText.AppendLine("SETCOUNT [number]      - Set the number of virtual displays");
            helpText.AppendLine("STATUS                 - Get current driver status");
            helpText.AppendLine("VERSION                - Get driver version information");
            helpText.AppendLine("LOGGING [true/false]   - Enable/disable logging");
            helpText.AppendLine("DEBUGLOGGING [true/false] - Enable/disable debug level logging");
            
            mainConsole.AppendText(helpText.ToString());
        }
        
        private async Task SendCommandFromInput()
        {
            if (string.IsNullOrWhiteSpace(userInput.Text))
                return;
                
            string command = userInput.Text.Trim();
            mainConsole.AppendText($"[COMMAND] {command}\n");
            
            // Handle special commands
            if (command.Equals("HELP", StringComparison.OrdinalIgnoreCase))
            {
                DisplayHelpCommand();
                // Clear input after sending
                userInput.Text = string.Empty;
                return;
            }
            
            UpdateTaskProgress("Sending Command", 25);
            
            try
            {
                string response = await SendCommandToDriver(command);
                mainConsole.AppendText($"[RESPONSE] {response}\n");
                UpdateTaskProgress("Sending Command", 100);
            }
            catch (Exception ex)
            {
                mainConsole.AppendText($"[ERROR] {ex.Message}\n");
                UpdateTaskProgress("Sending Command", 0);
            }
            finally
            {
                // Clear input after sending
                userInput.Text = string.Empty;
                await Task.Delay(500);
                UpdateTaskProgress("", 0);
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
            // Use the existing restart method but as a Task
            await restartDriverToolStripMenuItem_Click(sender, e);
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
            // Create an about dialog
            Form aboutDialog = new Form
            {
                Text = "About Virtual Driver Control",
                Size = new Size(450, 300),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(32, 34, 37),
                ForeColor = Color.White
            };

            // Add logo placeholder (could be replaced with an actual logo)
            Label logoLabel = new Label
            {
                Text = "VDD Control",
                Font = new Font("Consolas", 18, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(400, 30),
                Location = new Point(25, 20)
            };
            aboutDialog.Controls.Add(logoLabel);

            // Add version information
            Label versionLabel = new Label
            {
                Text = "Version 1.0",
                Font = new Font("Consolas", 10),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(400, 20),
                Location = new Point(25, 50)
            };
            aboutDialog.Controls.Add(versionLabel);

            // Add description
            Label descLabel = new Label
            {
                Text = "Virtual Driver Control provides a graphical interface to configure and control the Virtual Display Driver.",
                Font = new Font("Consolas", 9),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.TopLeft,
                Size = new Size(400, 40),
                Location = new Point(25, 80)
            };
            aboutDialog.Controls.Add(descLabel);

            // Add developers section
            Label developersHeader = new Label
            {
                Text = "Developers:",
                Font = new Font("Consolas", 9, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                Size = new Size(100, 20),
                Location = new Point(25, 130)
            };
            aboutDialog.Controls.Add(developersHeader);

            Label developersLabel = new Label
            {
                Text = "- MikeTheTech\n- Jocke",
                Font = new Font("Consolas", 9),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.TopLeft,
                Size = new Size(400, 40),
                Location = new Point(35, 150)
            };
            aboutDialog.Controls.Add(developersLabel);

            // Add links section
            Label linksHeader = new Label
            {
                Text = "Links:",
                Font = new Font("Consolas", 9, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                Size = new Size(100, 20),
                Location = new Point(25, 190)
            };
            aboutDialog.Controls.Add(linksHeader);

            LinkLabel githubLink = new LinkLabel
            {
                Text = "GitHub: https://github.com/VirtualDisplay/",
                Font = new Font("Consolas", 9),
                LinkColor = Color.LightBlue,
                ActiveLinkColor = Color.White,
                TextAlign = ContentAlignment.TopLeft,
                Size = new Size(400, 20),
                Location = new Point(35, 210)
            };
            githubLink.LinkClicked += (s, e) => System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://github.com/VirtualDisplay/",
                UseShellExecute = true
            });
            aboutDialog.Controls.Add(githubLink);

            // Add OK button
            Button okButton = new Button
            {
                Text = "OK",
                Size = new Size(80, 30),
                Location = new Point(350, 230),
                BackColor = Color.FromArgb(45, 47, 49),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            okButton.Click += (s, e) => aboutDialog.Close();
            aboutDialog.Controls.Add(okButton);

            // Show the dialog
            aboutDialog.ShowDialog(this);
        }
    }
}
