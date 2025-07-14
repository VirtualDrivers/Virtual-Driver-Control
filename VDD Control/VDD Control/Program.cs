using System.Diagnostics;
using System.Security.Principal;

namespace VDD_Control
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Check if running as administrator
            if (!IsRunningAsAdministrator())
            {
                // Check if this is already a restart attempt (to prevent infinite loops)
                bool isRestart = args.Length > 0 && args[0] == "--elevated";
                
                if (!isRestart)
                {
                    try
                    {
                        // Restart the application with administrator privileges
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            FileName = Process.GetCurrentProcess().MainModule?.FileName ?? Application.ExecutablePath,
                            Arguments = "--elevated",
                            UseShellExecute = true,
                            Verb = "runas" // This requests elevation
                        };

                        Process.Start(startInfo);
                        return; // Exit the current non-elevated process
                    }
                    catch (Exception)
                    {
                        // If elevation fails (user cancels UAC), continue without elevation
                        // The app will show warnings when driver operations are attempted
                    }
                }
            }

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new mainWindow());
        }

        private static bool IsRunningAsAdministrator()
        {
            try
            {
                var identity = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch
            {
                return false;
            }
        }
    }
}