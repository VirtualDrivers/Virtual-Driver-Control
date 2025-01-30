using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VDD_Control
{
    public partial class Form1 : Form
    {
        private const string PIPE_NAME = "MTTVirtualDisplayPipe";

        public Form1()
        {
            InitializeComponent(GetRestartDriverToolStripMenuItem());
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
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
                systemInfo += "OS Version: " + Environment.OSVersion.ToString() + "\n";
                systemInfo += "Machine Name: " + Environment.MachineName + "\n";
                systemInfo += "User Name: " + Environment.UserName + "\n";
                systemInfo += "64-Bit OS: " + (Environment.Is64BitOperatingSystem ? "Yes" : "No") + "\n";
                systemInfo += "64-Bit Process: " + (Environment.Is64BitProcess ? "Yes" : "No") + "\n";
                systemInfo += "Processor Count: " + Environment.ProcessorCount + "\n\n";

                // Add Memory Information
                systemInfo += "Memory Information:\n";
                systemInfo += "System Page Size: " + Environment.SystemPageSize + " bytes\n";
                systemInfo += "Working Set: " + (Environment.WorkingSet / 1024 / 1024) + " MB\n\n";

                // Add .NET Runtime Information
                systemInfo += ".NET Runtime Information:\n";
                systemInfo += "CLR Version: " + Environment.Version.ToString() + "\n\n";

                // Locate the vdd_settings.xml file
                systemInfo += LocateSettingsFile();

                // Display the information in richTextBox1
                richTextBox1.AppendText(systemInfo);
                richTextBox1.Refresh(); // Ensure the UI is updated
            }
            catch (Exception ex)
            {
                // Display error details in richTextBox1
                richTextBox1.AppendText("An error occurred while retrieving system information:\n" + ex.Message); // This really shouldn't happen. But probably will.
            }
            
            richTextBox1.AppendText("Virtual Display Driver Control Initialized.\n");

            if (!await TryConnectToDriver())
            {
                richTextBox1.AppendText("[WARNING] Could not verify driver connection. Ensure the driver is running.\n");
            }
        }
        private string LocateSettingsFile()
        {
            // Yo XML. Where u at?
            string settingsInfo = "Settings File Information:\n";
            string registryKeyPath = @"SOFTWARE\MikeTheTech\VirtualDisplayDriver";
            string registryFilePath = null;

            try
            {
                // Check the registry for the file path
                using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(registryKeyPath, false))
                {
                    if (registryKey != null)
                    {
                        registryFilePath = registryKey.GetValue("SettingsPath") as string;
                        if (!string.IsNullOrEmpty(registryFilePath) && File.Exists(registryFilePath))
                        {
                            settingsInfo += $"Found in Registry: {registryFilePath}\n";
                            return settingsInfo;
                        }
                        else
                        {
                            settingsInfo += "Registry key found but file is missing.\n";
                        }
                    }
                    else
                    {
                        settingsInfo += "Registry key is missing.\n";
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
                        settingsInfo += $"Found in Fallback Path: {path}\n";
                        return settingsInfo;
                    }
                }

                // If no file is found
                settingsInfo += "vdd_settings.xml not found in default locations. Are you using an older version of the driver?\n";
            }
            catch (Exception ex)
            {
                settingsInfo += $"Error while locating settings file: {ex.Message}\n";
            }

            return settingsInfo;
        }
        private async Task<bool> TryConnectToDriver()
        {
            const int maxAttempts = 5;
            int attempt = 0;

            while (attempt < maxAttempts)
            {
                try
                {
                    using (var pipeClient = new NamedPipeClientStream(".", PIPE_NAME, PipeDirection.InOut))
                    {
                        await pipeClient.ConnectAsync(2000);
                        richTextBox1.AppendText("[SUCCESS] Connected to the driver.\n");
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    attempt++;
                    richTextBox1.AppendText($"[ERROR] Connection failed: {ex.Message} (Attempt {attempt}/{maxAttempts})\n");
                    richTextBox1.AppendText("Note: This may also occur if the driver is off or restarting.\n");

                    if (attempt >= maxAttempts)
                    {
                        richTextBox1.AppendText("[ERROR] Unable to connect after multiple attempts.\n");
                        return false;
                    }

                    await Task.Delay(2000);
                }
            }

            return false;
        }

        private async Task<string> SendCommandToDriver(string command)
        {
            if (command != "RESTART_DRIVER" && !await TryConnectToDriver())
            {
                return "[ERROR] Connection failed: The driver may be off or restarting.";
            }

            try
            {
                using (var pipeClient = new NamedPipeClientStream(".", PIPE_NAME, PipeDirection.InOut))
                {
                    await pipeClient.ConnectAsync(2000);

                    using (var writer = new StreamWriter(pipeClient, Encoding.UTF8, leaveOpen: true))
                    using (var reader = new StreamReader(pipeClient, Encoding.UTF8))
                    {
                        await writer.WriteLineAsync(command);
                        await writer.FlushAsync();

                        string response = await reader.ReadLineAsync();

                        richTextBox1.AppendText($"[{command}] Response: {response}\n");

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

        private async Task restartDriverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.AppendText("[ACTION] Restarting driver...\n");

            string response;
            try
            {
                response = await SendCommandToDriver("RESTART_DRIVER");
            }
            catch (Exception ex)
            {
                response = $"[ERROR] Could not send restart command: {ex.Message}";
            }

            richTextBox1.AppendText(response + "\n");

            await Task.Delay(5000);  // Wait for the restart process

            richTextBox1.AppendText("[INFO] Attempting to reconnect...\n");

            if (await TryConnectToDriver())
            {
                richTextBox1.AppendText("[SUCCESS] Driver restarted and reconnected successfully.\n");
            }
            else
            {
                richTextBox1.AppendText("[WARNING] Driver restart detected, but reconnection failed. Ensure the driver is running.\n");
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
                    // Clear the RichTextBox
                    richTextBox1.Clear();

                    // Set bold font for the title
                    Font boldFont = new Font(richTextBox1.Font, FontStyle.Bold);
                    richTextBox1.SelectionFont = boldFont;
                    richTextBox1.AppendText("CPU Info:\n\n");

                    // Add CPU information
                    richTextBox1.AppendText("Vendor: " + processorInfo.GetValue("VendorIdentifier") + "\n\n");
                    richTextBox1.AppendText("Processor: " + processorInfo.GetValue("ProcessorNameString") + "\n\n");
                    richTextBox1.AppendText("Type: " + processorInfo.GetValue("Identifier") + "\n\n");
                    richTextBox1.AppendText("Speed: " + processorInfo.GetValue("~Mhz") + " MHz\n");
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
                richTextBox1.Clear();
                if (!string.IsNullOrWhiteSpace(output))
                {
                    richTextBox1.AppendText("Display Information:\n\n" + output);
                }
                else if (!string.IsNullOrWhiteSpace(error))
                {
                    richTextBox1.AppendText("Error:\n\n" + error);
                }
                else
                {
                    richTextBox1.AppendText("No output received from the PowerShell command.");
                }
            }
            catch (Exception ex)
            {
                // Display error details in richTextBox1
                richTextBox1.Clear();
                richTextBox1.AppendText("An error occurred while retrieving display information:\n" + ex.Message);
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
                richTextBox1.Clear();
                if (!string.IsNullOrWhiteSpace(output))
                {
                    richTextBox1.AppendText("Display Information:\n\n" + output);
                }
                else if (!string.IsNullOrWhiteSpace(error))
                {
                    richTextBox1.AppendText("Error:\n\n" + error);
                }
                else
                {
                    richTextBox1.AppendText("No output received from the PowerShell command.");
                }
            }
            catch (Exception ex)
            {
                // Display error details in richTextBox1
                richTextBox1.Clear();
                richTextBox1.AppendText("An error occurred while retrieving display information:\n" + ex.Message);
            }

        }

        private void getGPUInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Initialize a process to execute PowerShell
                // This needs to be changed to just run the script directly.
                // I'm just lazy.
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

                // Start the process and capture output
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                // Display output in richTextBox1
                richTextBox1.Clear();
                if (!string.IsNullOrWhiteSpace(output))
                {
                    richTextBox1.AppendText("Display Information:\n\n" + output);
                }
                else if (!string.IsNullOrWhiteSpace(error))
                {
                    richTextBox1.AppendText("Error:\n\n" + error);
                }
                else
                {
                    richTextBox1.AppendText("No output received from the PowerShell command.");
                }
            }
            catch (Exception ex)
            {
                // Display error details in richTextBox1
                richTextBox1.Clear();
                richTextBox1.AppendText("An error occurred while retrieving display information:\n" + ex.Message);
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
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }

    }
}
