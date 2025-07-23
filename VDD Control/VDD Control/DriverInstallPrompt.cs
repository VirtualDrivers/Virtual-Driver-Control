using System;
using System.Drawing;
using System.Windows.Forms;

namespace VDD_Control
{
    public partial class DriverInstallPrompt : Form
    {
        public enum PromptResult
        {
            Install,
            DontInstall
        }

        public PromptResult Result { get; private set; } = PromptResult.DontInstall;
        public bool DontShowAgain { get; private set; } = false;

        public DriverInstallPrompt()
        {
            InitializeComponent();
            
            // Set dialog result to Cancel by default
            this.DialogResult = DialogResult.Cancel;
            
            // Load icon from application resources if available
            try
            {
                if (Properties.Resources.IconYellow != null)
                {
                    pictureBoxIcon.Image = Properties.Resources.IconYellow.ToBitmap();
                }
            }
            catch
            {
                // Fallback to system icon if resource loading fails
                pictureBoxIcon.Image = SystemIcons.Question.ToBitmap();
            }
        }

        private void buttonInstall_Click(object sender, EventArgs e)
        {
            Result = PromptResult.Install;
            DontShowAgain = checkBoxDontShow.Checked;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonDontInstall_Click(object sender, EventArgs e)
        {
            Result = PromptResult.DontInstall;
            DontShowAgain = checkBoxDontShow.Checked;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            
            // Handle Enter key for Install button
            if (e.KeyCode == Keys.Enter)
            {
                buttonInstall_Click(this, EventArgs.Empty);
            }
            // Handle Escape key for Don't Install button
            else if (e.KeyCode == Keys.Escape)
            {
                buttonDontInstall_Click(this, EventArgs.Empty);
            }
        }

        // Static method to show the prompt and return the result
        public static (PromptResult result, bool dontShowAgain) ShowPrompt(IWin32Window owner = null)
        {
            using (var prompt = new DriverInstallPrompt())
            {
                var dialogResult = owner != null ? prompt.ShowDialog(owner) : prompt.ShowDialog();
                return (prompt.Result, prompt.DontShowAgain);
            }
        }
    }
}