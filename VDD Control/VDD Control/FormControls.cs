using System;
using System.Drawing;
using System.Windows.Forms;

namespace VDD_Control
{
    public partial class mainWindow
    {
        // Custom close and minimize buttons to replace ReaLTaiizor controls
        private Button customCloseButton;
        private Button customMinButton;
        
        /// <summary>
        /// Replaces ReaLTaiizor controls with standard Windows Forms controls
        /// </summary>
        private void InitializeStandardControls()
        {
            try
            {
                // Remove existing ReaLTaiizor controls
                if (mainTheme.Controls.Contains(closeButton))
                {
                    mainTheme.Controls.Remove(closeButton);
                }
                
                if (mainTheme.Controls.Contains(minButton))
                {
                    mainTheme.Controls.Remove(minButton);
                }
                
                // Create standard close button
                customCloseButton = new Button
                {
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    BackColor = Color.FromArgb(45, 47, 49),
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 8F, FontStyle.Bold, GraphicsUnit.Point),
                    ForeColor = Color.White,
                    Location = new Point(786, 16),
                    Name = "customCloseButton",
                    Size = new Size(20, 20),
                    TabIndex = 3,
                    Text = "X",
                    UseVisualStyleBackColor = false
                };
                customCloseButton.FlatAppearance.BorderSize = 0;
                customCloseButton.Click += customCloseButton_Click;
                
                // Create standard minimize button
                customMinButton = new Button
                {
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    BackColor = Color.FromArgb(45, 47, 49),
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 8F, FontStyle.Bold, GraphicsUnit.Point),
                    ForeColor = Color.White,
                    Location = new Point(760, 16),
                    Name = "customMinButton",
                    Size = new Size(20, 20),
                    TabIndex = 33,
                    Text = "-",
                    UseVisualStyleBackColor = false
                };
                customMinButton.FlatAppearance.BorderSize = 0;
                customMinButton.Click += customMinButton_Click;
                
                // Add new controls to form
                mainTheme.Controls.Add(customCloseButton);
                mainTheme.Controls.Add(customMinButton);
                
                // Make sure they are displayed on top
                customCloseButton.BringToFront();
                customMinButton.BringToFront();
                
                mainConsole.AppendText("[INFO] Replaced custom controls with standard Windows Forms controls\n");
            }
            catch (Exception ex)
            {
                mainConsole.AppendText($"[ERROR] Failed to initialize standard controls: {ex.Message}\n");
            }
        }
        
        /// <summary>
        /// Event handler for custom close button click
        /// </summary>
        private void customCloseButton_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error closing application: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// Event handler for custom minimize button click
        /// </summary>
        private void customMinButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.WindowState = FormWindowState.Minimized;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error minimizing window: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}