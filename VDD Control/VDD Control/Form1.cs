using Microsoft.Win32;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace VDD_Control
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
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
                    systemInfo += "CPU Information: Unable to retrieve.\n\n";
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
                systemInfo += "CLR Version: " + Environment.Version.ToString() + "\n";

                // Clear and display the information in richTextBox1
                richTextBox1.Clear();
                richTextBox1.AppendText(systemInfo);
            }
            catch (Exception ex)
            {
                // Display error details in richTextBox1
                richTextBox1.Clear();
                richTextBox1.AppendText("An error occurred while retrieving system information:\n" + ex.Message);
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

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Handle context menu opening logic here, if needed.
        }

        private void enableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Handle enable menu item click here, if needed.
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Handle label click logic here, if needed.
        }
    }
}
