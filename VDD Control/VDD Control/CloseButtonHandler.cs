using System;
using System.Windows.Forms;

namespace VDD_Control
{
    public partial class mainWindow
    {
        /// <summary>
        /// Event handler for closeButton click event
        /// </summary>
        private void closeButton_Click(object sender, EventArgs e)
        {
            // Close the application
            Application.Exit();
        }
        
        /// <summary>
        /// Event handler for minButton click event
        /// </summary>
        private void minButton_Click(object sender, EventArgs e)
        {
            // Minimize the window
            this.WindowState = FormWindowState.Minimized;
        }
    }
}