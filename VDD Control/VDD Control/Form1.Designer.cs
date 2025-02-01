
namespace VDD_Control
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }



        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            notificationIcon = new NotifyIcon(components);
            trayMenu = new ContextMenuStrip(components);
            menuToolStripMenuItem1 = new ToolStripMenuItem();
            exitToolStripMenuItem1 = new ToolStripMenuItem();
            toolsToolStripMenuItem1 = new ToolStripMenuItem();
            getDisplayInformationToolStripMenuItem1 = new ToolStripMenuItem();
            getGPUInformationToolStripMenuItem1 = new ToolStripMenuItem();
            getCPUInformationToolStripMenuItem1 = new ToolStripMenuItem();
            getAudioInformationToolStripMenuItem1 = new ToolStripMenuItem();
            virtualDisplayDriverToolStripMenuItem1 = new ToolStripMenuItem();
            sDR10bitToolStripMenuItem1 = new ToolStripMenuItem();
            hDRToolStripMenuItem1 = new ToolStripMenuItem();
            customEDIDToolStripMenuItem1 = new ToolStripMenuItem();
            hardwareCursorToolStripMenuItem1 = new ToolStripMenuItem();
            preventMonitorSpoofToolStripMenuItem1 = new ToolStripMenuItem();
            eDIDCEAOverrideToolStripMenuItem1 = new ToolStripMenuItem();
            selectGPUToolStripMenuItem1 = new ToolStripMenuItem();
            displayCountToolStripMenuItem1 = new ToolStripMenuItem();
            systemToolStripMenuItem1 = new ToolStripMenuItem();
            enableDriverToolStripMenuItem2 = new ToolStripMenuItem();
            disableDriverToolStripMenuItem2 = new ToolStripMenuItem();
            restartDriverToolStripMenuItem2 = new ToolStripMenuItem();
            loggingToolStripMenuItem1 = new ToolStripMenuItem();
            userModeLoggingToolStripMenuItem = new ToolStripMenuItem();
            devModeLoggingToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripMenuItem();
            virtualAudioDriverToolStripMenuItem1 = new ToolStripMenuItem();
            systemToolStripMenuItem2 = new ToolStripMenuItem();
            enableDriverToolStripMenuItem3 = new ToolStripMenuItem();
            disableDriverToolStripMenuItem3 = new ToolStripMenuItem();
            restartDriverToolStripMenuItem3 = new ToolStripMenuItem();
            aboutToolStripMenuItem1 = new ToolStripMenuItem();
            exitToolStripMenuItem2 = new ToolStripMenuItem();
            menuToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            virtualDisplayDriverToolStripMenuItem = new ToolStripMenuItem();
            sDR10bitToolStripMenuItem = new ToolStripMenuItem();
            hDRToolStripMenuItem = new ToolStripMenuItem();
            customEDIDToolStripMenuItem = new ToolStripMenuItem();
            hardwareCursorToolStripMenuItem = new ToolStripMenuItem();
            preventMonitorSpoofToolStripMenuItem = new ToolStripMenuItem();
            eDIDCEAOverrideToolStripMenuItem = new ToolStripMenuItem();
            selectGPUToolStripMenuItem = new ToolStripMenuItem();
            displayCountToolStripMenuItem = new ToolStripMenuItem();
            enableToolStripMenuItem = new ToolStripMenuItem();
            enableDriverToolStripMenuItem = new ToolStripMenuItem();
            disableDriverToolStripMenuItem = new ToolStripMenuItem();
            restartDriverToolStripMenuItem = new ToolStripMenuItem();
            loggingToolStripMenuItem = new ToolStripMenuItem();
            enableUserModeLoggingToolStripMenuItem = new ToolStripMenuItem();
            enableDevModeLoggingToolStripMenuItem = new ToolStripMenuItem();
            xMLOptionsEditorToolStripMenuItem = new ToolStripMenuItem();
            virtualAudioDriverToolStripMenuItem = new ToolStripMenuItem();
            systemToolStripMenuItem = new ToolStripMenuItem();
            enableDriverToolStripMenuItem1 = new ToolStripMenuItem();
            disableDriverToolStripMenuItem1 = new ToolStripMenuItem();
            restartDriverToolStripMenuItem1 = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            mainVisibleMenuStrip = new MenuStrip();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            getGPUInformationToolStripMenuItem = new ToolStripMenuItem();
            getCPUInformationToolStripMenuItem = new ToolStripMenuItem();
            getDisplayInformationToolStripMenuItem2 = new ToolStripMenuItem();
            getAudioInformationToolStripMenuItem = new ToolStripMenuItem();
            mainConsole = new RichTextBox();
            gitLabel = new Label();
            discordLink = new LinkLabel();
            discordLabel = new Label();
            vddLink = new LinkLabel();
            mttLabel = new Label();
            budLabel = new Label();
            jockeLabel = new Label();
            mttSupport = new LinkLabel();
            linkLabel6 = new LinkLabel();
            jockeSupport = new LinkLabel();
            budSupport = new LinkLabel();
            patreonGroupBox = new GroupBox();
            patreonMembersListBox = new ListBox();
            devsGroupBox = new GroupBox();
            resourcesGroupBox = new GroupBox();
            userInput = new TextBox();
            taskGroupBox = new GroupBox();
            taskProgressBar = new ReaLTaiizor.Controls.RibbonProgressBarCenter();
            mainTheme = new ReaLTaiizor.Forms.ThemeForm();
            restartAllButton = new ReaLTaiizor.Controls.ChatButtonRight();
            enterButton = new ReaLTaiizor.Controls.ChatButtonRight();
            minButton = new ReaLTaiizor.Controls.ForeverMinimize();
            closeButton = new ReaLTaiizor.Controls.ForeverClose();
            trayMenu.SuspendLayout();
            mainVisibleMenuStrip.SuspendLayout();
            patreonGroupBox.SuspendLayout();
            devsGroupBox.SuspendLayout();
            resourcesGroupBox.SuspendLayout();
            taskGroupBox.SuspendLayout();
            mainTheme.SuspendLayout();
            SuspendLayout();
            // 
            // notificationIcon
            // 
            notificationIcon.ContextMenuStrip = trayMenu;
            notificationIcon.Icon = (Icon)resources.GetObject("notificationIcon.Icon");
            notificationIcon.Text = "notifyIcon1";
            notificationIcon.Visible = true;
            // 
            // trayMenu
            // 
            trayMenu.Items.AddRange(new ToolStripItem[] { menuToolStripMenuItem1, toolsToolStripMenuItem1, virtualDisplayDriverToolStripMenuItem1, virtualAudioDriverToolStripMenuItem1, aboutToolStripMenuItem1, exitToolStripMenuItem2 });
            trayMenu.Name = "contextMenuStrip1";
            trayMenu.Size = new Size(184, 136);
            trayMenu.Text = "Menu";
            // 
            // menuToolStripMenuItem1
            // 
            menuToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { exitToolStripMenuItem1 });
            menuToolStripMenuItem1.Name = "menuToolStripMenuItem1";
            menuToolStripMenuItem1.Size = new Size(183, 22);
            menuToolStripMenuItem1.Text = "Menu";
            // 
            // exitToolStripMenuItem1
            // 
            exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            exitToolStripMenuItem1.Size = new Size(92, 22);
            exitToolStripMenuItem1.Text = "Exit";
            exitToolStripMenuItem1.Click += exitToolStripMenuItem1_Click;
            // 
            // toolsToolStripMenuItem1
            // 
            toolsToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { getDisplayInformationToolStripMenuItem1, getGPUInformationToolStripMenuItem1, getCPUInformationToolStripMenuItem1, getAudioInformationToolStripMenuItem1 });
            toolsToolStripMenuItem1.Name = "toolsToolStripMenuItem1";
            toolsToolStripMenuItem1.Size = new Size(183, 22);
            toolsToolStripMenuItem1.Text = "Tools";
            // 
            // getDisplayInformationToolStripMenuItem1
            // 
            getDisplayInformationToolStripMenuItem1.Name = "getDisplayInformationToolStripMenuItem1";
            getDisplayInformationToolStripMenuItem1.Size = new Size(199, 22);
            getDisplayInformationToolStripMenuItem1.Text = "Get Display Information";
            getDisplayInformationToolStripMenuItem1.Click += getDisplayInformationToolStripMenuItem1_Click_1;
            // 
            // getGPUInformationToolStripMenuItem1
            // 
            getGPUInformationToolStripMenuItem1.Name = "getGPUInformationToolStripMenuItem1";
            getGPUInformationToolStripMenuItem1.Size = new Size(199, 22);
            getGPUInformationToolStripMenuItem1.Text = "Get GPU Information";
            getGPUInformationToolStripMenuItem1.Click += getGPUInformationToolStripMenuItem1_Click_1;
            // 
            // getCPUInformationToolStripMenuItem1
            // 
            getCPUInformationToolStripMenuItem1.Name = "getCPUInformationToolStripMenuItem1";
            getCPUInformationToolStripMenuItem1.Size = new Size(199, 22);
            getCPUInformationToolStripMenuItem1.Text = "Get CPU information";
            getCPUInformationToolStripMenuItem1.Click += getCPUInformationToolStripMenuItem1_Click_1;
            // 
            // getAudioInformationToolStripMenuItem1
            // 
            getAudioInformationToolStripMenuItem1.Name = "getAudioInformationToolStripMenuItem1";
            getAudioInformationToolStripMenuItem1.Size = new Size(199, 22);
            getAudioInformationToolStripMenuItem1.Text = "Get Audio Information";
            getAudioInformationToolStripMenuItem1.Click += getAudioInformationToolStripMenuItem1_Click_1;
            // 
            // virtualDisplayDriverToolStripMenuItem1
            // 
            virtualDisplayDriverToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { sDR10bitToolStripMenuItem1, hDRToolStripMenuItem1, customEDIDToolStripMenuItem1, hardwareCursorToolStripMenuItem1, preventMonitorSpoofToolStripMenuItem1, eDIDCEAOverrideToolStripMenuItem1, selectGPUToolStripMenuItem1, displayCountToolStripMenuItem1, systemToolStripMenuItem1, loggingToolStripMenuItem1, toolStripMenuItem1 });
            virtualDisplayDriverToolStripMenuItem1.Name = "virtualDisplayDriverToolStripMenuItem1";
            virtualDisplayDriverToolStripMenuItem1.Size = new Size(183, 22);
            virtualDisplayDriverToolStripMenuItem1.Text = "Virtual Display Driver";
            // 
            // sDR10bitToolStripMenuItem1
            // 
            sDR10bitToolStripMenuItem1.Name = "sDR10bitToolStripMenuItem1";
            sDR10bitToolStripMenuItem1.Size = new Size(194, 22);
            sDR10bitToolStripMenuItem1.Text = "SDR 10bit";
            sDR10bitToolStripMenuItem1.Click += sDR10bitToolStripMenuItem1_Click;
            // 
            // hDRToolStripMenuItem1
            // 
            hDRToolStripMenuItem1.Name = "hDRToolStripMenuItem1";
            hDRToolStripMenuItem1.Size = new Size(194, 22);
            hDRToolStripMenuItem1.Text = "HDR+";
            hDRToolStripMenuItem1.Click += hDRToolStripMenuItem1_Click;
            // 
            // customEDIDToolStripMenuItem1
            // 
            customEDIDToolStripMenuItem1.Name = "customEDIDToolStripMenuItem1";
            customEDIDToolStripMenuItem1.Size = new Size(194, 22);
            customEDIDToolStripMenuItem1.Text = "Custom EDID";
            customEDIDToolStripMenuItem1.Click += customEDIDToolStripMenuItem1_Click;
            // 
            // hardwareCursorToolStripMenuItem1
            // 
            hardwareCursorToolStripMenuItem1.Name = "hardwareCursorToolStripMenuItem1";
            hardwareCursorToolStripMenuItem1.Size = new Size(194, 22);
            hardwareCursorToolStripMenuItem1.Text = "Hardware Cursor";
            hardwareCursorToolStripMenuItem1.Click += hardwareCursorToolStripMenuItem1_Click;
            // 
            // preventMonitorSpoofToolStripMenuItem1
            // 
            preventMonitorSpoofToolStripMenuItem1.Name = "preventMonitorSpoofToolStripMenuItem1";
            preventMonitorSpoofToolStripMenuItem1.Size = new Size(194, 22);
            preventMonitorSpoofToolStripMenuItem1.Text = "Prevent Monitor Spoof";
            preventMonitorSpoofToolStripMenuItem1.Click += preventMonitorSpoofToolStripMenuItem1_Click;
            // 
            // eDIDCEAOverrideToolStripMenuItem1
            // 
            eDIDCEAOverrideToolStripMenuItem1.Name = "eDIDCEAOverrideToolStripMenuItem1";
            eDIDCEAOverrideToolStripMenuItem1.Size = new Size(194, 22);
            eDIDCEAOverrideToolStripMenuItem1.Text = "EDID CEA Override";
            eDIDCEAOverrideToolStripMenuItem1.Click += eDIDCEAOverrideToolStripMenuItem1_Click;
            // 
            // selectGPUToolStripMenuItem1
            // 
            selectGPUToolStripMenuItem1.Name = "selectGPUToolStripMenuItem1";
            selectGPUToolStripMenuItem1.Size = new Size(194, 22);
            selectGPUToolStripMenuItem1.Text = "Select GPU";
            selectGPUToolStripMenuItem1.Click += selectGPUToolStripMenuItem1_Click;
            // 
            // displayCountToolStripMenuItem1
            // 
            displayCountToolStripMenuItem1.Name = "displayCountToolStripMenuItem1";
            displayCountToolStripMenuItem1.Size = new Size(194, 22);
            displayCountToolStripMenuItem1.Text = "Display Count";
            displayCountToolStripMenuItem1.Click += displayCountToolStripMenuItem1_Click;
            // 
            // systemToolStripMenuItem1
            // 
            systemToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { enableDriverToolStripMenuItem2, disableDriverToolStripMenuItem2, restartDriverToolStripMenuItem2 });
            systemToolStripMenuItem1.Name = "systemToolStripMenuItem1";
            systemToolStripMenuItem1.Size = new Size(194, 22);
            systemToolStripMenuItem1.Text = "System";
            // 
            // enableDriverToolStripMenuItem2
            // 
            enableDriverToolStripMenuItem2.Name = "enableDriverToolStripMenuItem2";
            enableDriverToolStripMenuItem2.Size = new Size(146, 22);
            enableDriverToolStripMenuItem2.Text = "Enable Driver";
            enableDriverToolStripMenuItem2.Click += enableDriverToolStripMenuItem2_Click;
            // 
            // disableDriverToolStripMenuItem2
            // 
            disableDriverToolStripMenuItem2.Name = "disableDriverToolStripMenuItem2";
            disableDriverToolStripMenuItem2.Size = new Size(146, 22);
            disableDriverToolStripMenuItem2.Text = "Disable Driver";
            disableDriverToolStripMenuItem2.Click += disableDriverToolStripMenuItem2_Click;
            // 
            // restartDriverToolStripMenuItem2
            // 
            restartDriverToolStripMenuItem2.Name = "restartDriverToolStripMenuItem2";
            restartDriverToolStripMenuItem2.Size = new Size(146, 22);
            restartDriverToolStripMenuItem2.Text = "Restart Driver";
            restartDriverToolStripMenuItem2.Click += restartDriverToolStripMenuItem2_Click;
            // 
            // loggingToolStripMenuItem1
            // 
            loggingToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { userModeLoggingToolStripMenuItem, devModeLoggingToolStripMenuItem });
            loggingToolStripMenuItem1.Name = "loggingToolStripMenuItem1";
            loggingToolStripMenuItem1.Size = new Size(194, 22);
            loggingToolStripMenuItem1.Text = "Logging";
            // 
            // userModeLoggingToolStripMenuItem
            // 
            userModeLoggingToolStripMenuItem.Name = "userModeLoggingToolStripMenuItem";
            userModeLoggingToolStripMenuItem.Size = new Size(180, 22);
            userModeLoggingToolStripMenuItem.Text = "User-Mode Logging";
            userModeLoggingToolStripMenuItem.Click += userModeLoggingToolStripMenuItem_Click;
            // 
            // devModeLoggingToolStripMenuItem
            // 
            devModeLoggingToolStripMenuItem.Name = "devModeLoggingToolStripMenuItem";
            devModeLoggingToolStripMenuItem.Size = new Size(180, 22);
            devModeLoggingToolStripMenuItem.Text = "Dev-Mode Logging";
            devModeLoggingToolStripMenuItem.Click += devModeLoggingToolStripMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(194, 22);
            toolStripMenuItem1.Text = "XML/Options Editor";
            toolStripMenuItem1.Click += toolStripMenuItem1_Click;
            // 
            // virtualAudioDriverToolStripMenuItem1
            // 
            virtualAudioDriverToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { systemToolStripMenuItem2 });
            virtualAudioDriverToolStripMenuItem1.Name = "virtualAudioDriverToolStripMenuItem1";
            virtualAudioDriverToolStripMenuItem1.Size = new Size(183, 22);
            virtualAudioDriverToolStripMenuItem1.Text = "Virtual Audio Driver";
            // 
            // systemToolStripMenuItem2
            // 
            systemToolStripMenuItem2.DropDownItems.AddRange(new ToolStripItem[] { enableDriverToolStripMenuItem3, disableDriverToolStripMenuItem3, restartDriverToolStripMenuItem3 });
            systemToolStripMenuItem2.Name = "systemToolStripMenuItem2";
            systemToolStripMenuItem2.Size = new Size(112, 22);
            systemToolStripMenuItem2.Text = "System";
            // 
            // enableDriverToolStripMenuItem3
            // 
            enableDriverToolStripMenuItem3.Name = "enableDriverToolStripMenuItem3";
            enableDriverToolStripMenuItem3.Size = new Size(146, 22);
            enableDriverToolStripMenuItem3.Text = "Enable Driver";
            // 
            // disableDriverToolStripMenuItem3
            // 
            disableDriverToolStripMenuItem3.Name = "disableDriverToolStripMenuItem3";
            disableDriverToolStripMenuItem3.Size = new Size(146, 22);
            disableDriverToolStripMenuItem3.Text = "Disable Driver";
            // 
            // restartDriverToolStripMenuItem3
            // 
            restartDriverToolStripMenuItem3.Name = "restartDriverToolStripMenuItem3";
            restartDriverToolStripMenuItem3.Size = new Size(146, 22);
            restartDriverToolStripMenuItem3.Text = "Restart Driver";
            // 
            // aboutToolStripMenuItem1
            // 
            aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            aboutToolStripMenuItem1.Size = new Size(183, 22);
            aboutToolStripMenuItem1.Text = "About";
            aboutToolStripMenuItem1.Click += aboutToolStripMenuItem1_Click;
            // 
            // exitToolStripMenuItem2
            // 
            exitToolStripMenuItem2.Name = "exitToolStripMenuItem2";
            exitToolStripMenuItem2.Size = new Size(183, 22);
            exitToolStripMenuItem2.Text = "Exit";
            exitToolStripMenuItem2.Click += exitToolStripMenuItem2_Click;
            // 
            // menuToolStripMenuItem
            // 
            menuToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            menuToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { exitToolStripMenuItem });
            menuToolStripMenuItem.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            menuToolStripMenuItem.ForeColor = SystemColors.ButtonFace;
            menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            menuToolStripMenuItem.Size = new Size(50, 20);
            menuToolStripMenuItem.Text = "Menu";
            menuToolStripMenuItem.Click += menuToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(92, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // virtualDisplayDriverToolStripMenuItem
            // 
            virtualDisplayDriverToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            virtualDisplayDriverToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { sDR10bitToolStripMenuItem, hDRToolStripMenuItem, customEDIDToolStripMenuItem, hardwareCursorToolStripMenuItem, preventMonitorSpoofToolStripMenuItem, eDIDCEAOverrideToolStripMenuItem, selectGPUToolStripMenuItem, displayCountToolStripMenuItem, enableToolStripMenuItem, loggingToolStripMenuItem, xMLOptionsEditorToolStripMenuItem });
            virtualDisplayDriverToolStripMenuItem.ForeColor = SystemColors.ButtonFace;
            virtualDisplayDriverToolStripMenuItem.Name = "virtualDisplayDriverToolStripMenuItem";
            virtualDisplayDriverToolStripMenuItem.Size = new Size(128, 20);
            virtualDisplayDriverToolStripMenuItem.Text = "Virtual Display Driver";
            // 
            // sDR10bitToolStripMenuItem
            // 
            sDR10bitToolStripMenuItem.Name = "sDR10bitToolStripMenuItem";
            sDR10bitToolStripMenuItem.Size = new Size(194, 22);
            sDR10bitToolStripMenuItem.Text = "SDR 10bit";
            sDR10bitToolStripMenuItem.Click += sDR10bitToolStripMenuItem_Click;
            // 
            // hDRToolStripMenuItem
            // 
            hDRToolStripMenuItem.Name = "hDRToolStripMenuItem";
            hDRToolStripMenuItem.Size = new Size(194, 22);
            hDRToolStripMenuItem.Text = "HDR+";
            hDRToolStripMenuItem.Click += hDRToolStripMenuItem_Click;
            // 
            // customEDIDToolStripMenuItem
            // 
            customEDIDToolStripMenuItem.Name = "customEDIDToolStripMenuItem";
            customEDIDToolStripMenuItem.Size = new Size(194, 22);
            customEDIDToolStripMenuItem.Text = "Custom EDID";
            customEDIDToolStripMenuItem.Click += customEDIDToolStripMenuItem_Click;
            // 
            // hardwareCursorToolStripMenuItem
            // 
            hardwareCursorToolStripMenuItem.Name = "hardwareCursorToolStripMenuItem";
            hardwareCursorToolStripMenuItem.Size = new Size(194, 22);
            hardwareCursorToolStripMenuItem.Text = "Hardware Cursor";
            hardwareCursorToolStripMenuItem.Click += hardwareCursorToolStripMenuItem_Click;
            // 
            // preventMonitorSpoofToolStripMenuItem
            // 
            preventMonitorSpoofToolStripMenuItem.Name = "preventMonitorSpoofToolStripMenuItem";
            preventMonitorSpoofToolStripMenuItem.Size = new Size(194, 22);
            preventMonitorSpoofToolStripMenuItem.Text = "Prevent Monitor Spoof";
            preventMonitorSpoofToolStripMenuItem.Click += preventMonitorSpoofToolStripMenuItem_Click;
            // 
            // eDIDCEAOverrideToolStripMenuItem
            // 
            eDIDCEAOverrideToolStripMenuItem.Name = "eDIDCEAOverrideToolStripMenuItem";
            eDIDCEAOverrideToolStripMenuItem.Size = new Size(194, 22);
            eDIDCEAOverrideToolStripMenuItem.Text = "EDID CEA Override";
            eDIDCEAOverrideToolStripMenuItem.Click += eDIDCEAOverrideToolStripMenuItem_Click;
            // 
            // selectGPUToolStripMenuItem
            // 
            selectGPUToolStripMenuItem.Name = "selectGPUToolStripMenuItem";
            selectGPUToolStripMenuItem.Size = new Size(194, 22);
            selectGPUToolStripMenuItem.Text = "Select GPU";
            selectGPUToolStripMenuItem.Click += selectGPUToolStripMenuItem_Click;
            // 
            // displayCountToolStripMenuItem
            // 
            displayCountToolStripMenuItem.Name = "displayCountToolStripMenuItem";
            displayCountToolStripMenuItem.Size = new Size(194, 22);
            displayCountToolStripMenuItem.Text = "Display Count";
            displayCountToolStripMenuItem.Click += displayCountToolStripMenuItem_Click;
            // 
            // enableToolStripMenuItem
            // 
            enableToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { enableDriverToolStripMenuItem, disableDriverToolStripMenuItem, restartDriverToolStripMenuItem });
            enableToolStripMenuItem.Name = "enableToolStripMenuItem";
            enableToolStripMenuItem.Size = new Size(194, 22);
            enableToolStripMenuItem.Text = "System";
            enableToolStripMenuItem.Click += enableToolStripMenuItem_Click;
            // 
            // enableDriverToolStripMenuItem
            // 
            enableDriverToolStripMenuItem.Name = "enableDriverToolStripMenuItem";
            enableDriverToolStripMenuItem.Size = new Size(146, 22);
            enableDriverToolStripMenuItem.Text = "Enable Driver";
            // 
            // disableDriverToolStripMenuItem
            // 
            disableDriverToolStripMenuItem.Name = "disableDriverToolStripMenuItem";
            disableDriverToolStripMenuItem.Size = new Size(146, 22);
            disableDriverToolStripMenuItem.Text = "Disable Driver";
            disableDriverToolStripMenuItem.Click += disableDriverToolStripMenuItem_Click;
            // 
            // restartDriverToolStripMenuItem
            // 
            restartDriverToolStripMenuItem.Name = "restartDriverToolStripMenuItem";
            restartDriverToolStripMenuItem.Size = new Size(146, 22);
            restartDriverToolStripMenuItem.Text = "Restart Driver";
            restartDriverToolStripMenuItem.Click += RestartDriverHandler;
            // 
            // loggingToolStripMenuItem
            // 
            loggingToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { enableUserModeLoggingToolStripMenuItem, enableDevModeLoggingToolStripMenuItem });
            loggingToolStripMenuItem.Name = "loggingToolStripMenuItem";
            loggingToolStripMenuItem.Size = new Size(194, 22);
            loggingToolStripMenuItem.Text = "Logging";
            // 
            // enableUserModeLoggingToolStripMenuItem
            // 
            enableUserModeLoggingToolStripMenuItem.Name = "enableUserModeLoggingToolStripMenuItem";
            enableUserModeLoggingToolStripMenuItem.Size = new Size(180, 22);
            enableUserModeLoggingToolStripMenuItem.Text = "User-Mode Logging";
            enableUserModeLoggingToolStripMenuItem.Click += enableUserModeLoggingToolStripMenuItem_Click;
            // 
            // enableDevModeLoggingToolStripMenuItem
            // 
            enableDevModeLoggingToolStripMenuItem.Name = "enableDevModeLoggingToolStripMenuItem";
            enableDevModeLoggingToolStripMenuItem.Size = new Size(180, 22);
            enableDevModeLoggingToolStripMenuItem.Text = "Dev-Mode Logging";
            enableDevModeLoggingToolStripMenuItem.Click += enableDevModeLoggingToolStripMenuItem_Click;
            // 
            // xMLOptionsEditorToolStripMenuItem
            // 
            xMLOptionsEditorToolStripMenuItem.Name = "xMLOptionsEditorToolStripMenuItem";
            xMLOptionsEditorToolStripMenuItem.Size = new Size(194, 22);
            xMLOptionsEditorToolStripMenuItem.Text = "XML/Options Editor";
            xMLOptionsEditorToolStripMenuItem.Click += xMLOptionsEditorToolStripMenuItem_Click;
            // 
            // virtualAudioDriverToolStripMenuItem
            // 
            virtualAudioDriverToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { systemToolStripMenuItem });
            virtualAudioDriverToolStripMenuItem.ForeColor = SystemColors.ButtonFace;
            virtualAudioDriverToolStripMenuItem.Name = "virtualAudioDriverToolStripMenuItem";
            virtualAudioDriverToolStripMenuItem.Size = new Size(122, 20);
            virtualAudioDriverToolStripMenuItem.Text = "Virtual Audio Driver";
            // 
            // systemToolStripMenuItem
            // 
            systemToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { enableDriverToolStripMenuItem1, disableDriverToolStripMenuItem1, restartDriverToolStripMenuItem1 });
            systemToolStripMenuItem.Name = "systemToolStripMenuItem";
            systemToolStripMenuItem.Size = new Size(112, 22);
            systemToolStripMenuItem.Text = "System";
            // 
            // enableDriverToolStripMenuItem1
            // 
            enableDriverToolStripMenuItem1.Name = "enableDriverToolStripMenuItem1";
            enableDriverToolStripMenuItem1.Size = new Size(146, 22);
            enableDriverToolStripMenuItem1.Text = "Enable Driver";
            // 
            // disableDriverToolStripMenuItem1
            // 
            disableDriverToolStripMenuItem1.Name = "disableDriverToolStripMenuItem1";
            disableDriverToolStripMenuItem1.Size = new Size(146, 22);
            disableDriverToolStripMenuItem1.Text = "Disable Driver";
            // 
            // restartDriverToolStripMenuItem1
            // 
            restartDriverToolStripMenuItem1.Name = "restartDriverToolStripMenuItem1";
            restartDriverToolStripMenuItem1.Size = new Size(146, 22);
            restartDriverToolStripMenuItem1.Text = "Restart Driver";
            restartDriverToolStripMenuItem1.Click += RestartDriverHandler;
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.ForeColor = SystemColors.ButtonFace;
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(52, 20);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // mainVisibleMenuStrip
            // 
            mainVisibleMenuStrip.BackColor = Color.FromArgb(32, 34, 37);
            mainVisibleMenuStrip.Dock = DockStyle.None;
            mainVisibleMenuStrip.Items.AddRange(new ToolStripItem[] { menuToolStripMenuItem, toolsToolStripMenuItem, virtualDisplayDriverToolStripMenuItem, virtualAudioDriverToolStripMenuItem, aboutToolStripMenuItem });
            mainVisibleMenuStrip.Location = new Point(255, 22);
            mainVisibleMenuStrip.Name = "mainVisibleMenuStrip";
            mainVisibleMenuStrip.Size = new Size(407, 24);
            mainVisibleMenuStrip.TabIndex = 1;
            mainVisibleMenuStrip.Text = "mainVisibleToolstrip";
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { getGPUInformationToolStripMenuItem, getCPUInformationToolStripMenuItem, getDisplayInformationToolStripMenuItem2, getAudioInformationToolStripMenuItem });
            toolsToolStripMenuItem.ForeColor = SystemColors.ButtonFace;
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(47, 20);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // getGPUInformationToolStripMenuItem
            // 
            getGPUInformationToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            getGPUInformationToolStripMenuItem.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            getGPUInformationToolStripMenuItem.Name = "getGPUInformationToolStripMenuItem";
            getGPUInformationToolStripMenuItem.Size = new Size(199, 22);
            getGPUInformationToolStripMenuItem.Text = "Get GPU Information";
            getGPUInformationToolStripMenuItem.Click += getGPUInformationToolStripMenuItem_Click;
            // 
            // getCPUInformationToolStripMenuItem
            // 
            getCPUInformationToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            getCPUInformationToolStripMenuItem.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            getCPUInformationToolStripMenuItem.Name = "getCPUInformationToolStripMenuItem";
            getCPUInformationToolStripMenuItem.Size = new Size(199, 22);
            getCPUInformationToolStripMenuItem.Text = "Get CPU Information";
            getCPUInformationToolStripMenuItem.Click += getCPUInformationToolStripMenuItem_Click;
            // 
            // getDisplayInformationToolStripMenuItem2
            // 
            getDisplayInformationToolStripMenuItem2.DisplayStyle = ToolStripItemDisplayStyle.Text;
            getDisplayInformationToolStripMenuItem2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            getDisplayInformationToolStripMenuItem2.Name = "getDisplayInformationToolStripMenuItem2";
            getDisplayInformationToolStripMenuItem2.Size = new Size(199, 22);
            getDisplayInformationToolStripMenuItem2.Text = "Get Display Information";
            getDisplayInformationToolStripMenuItem2.Click += getDisplayInformationToolStripMenuItem2_Click;
            // 
            // getAudioInformationToolStripMenuItem
            // 
            getAudioInformationToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            getAudioInformationToolStripMenuItem.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            getAudioInformationToolStripMenuItem.Name = "getAudioInformationToolStripMenuItem";
            getAudioInformationToolStripMenuItem.Size = new Size(199, 22);
            getAudioInformationToolStripMenuItem.Text = "Get Audio Information";
            getAudioInformationToolStripMenuItem.Click += getAudioInformationToolStripMenuItem_Click;
            // 
            // mainConsole
            // 
            mainConsole.BackColor = Color.FromArgb(32, 32, 32);
            mainConsole.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
            mainConsole.ForeColor = Color.FromArgb(0, 192, 0);
            mainConsole.Location = new Point(13, 72);
            mainConsole.Name = "mainConsole";
            mainConsole.Size = new Size(520, 382);
            mainConsole.TabIndex = 2;
            mainConsole.Text = "";
            mainConsole.WordWrap = false;
            mainConsole.ZoomFactor = 0.85F;
            mainConsole.TextChanged += richTextBox1_TextChanged;
            // 
            // gitLabel
            // 
            gitLabel.AutoSize = true;
            gitLabel.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            gitLabel.Location = new Point(6, 19);
            gitLabel.Name = "gitLabel";
            gitLabel.Size = new Size(43, 13);
            gitLabel.TabIndex = 4;
            gitLabel.Text = "GitHub";
            // 
            // discordLink
            // 
            discordLink.AutoSize = true;
            discordLink.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            discordLink.LinkColor = Color.White;
            discordLink.Location = new Point(6, 73);
            discordLink.Name = "discordLink";
            discordLink.Size = new Size(199, 13);
            discordLink.TabIndex = 5;
            discordLink.TabStop = true;
            discordLink.Text = "https://discord.mikethetech.com/";
            discordLink.LinkClicked += linkLabel1_LinkClicked;
            // 
            // discordLabel
            // 
            discordLabel.AutoSize = true;
            discordLabel.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            discordLabel.Location = new Point(6, 58);
            discordLabel.Name = "discordLabel";
            discordLabel.Size = new Size(49, 13);
            discordLabel.TabIndex = 6;
            discordLabel.Text = "Discord";
            // 
            // vddLink
            // 
            vddLink.AutoSize = true;
            vddLink.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            vddLink.LinkColor = Color.White;
            vddLink.Location = new Point(6, 34);
            vddLink.Name = "vddLink";
            vddLink.Size = new Size(211, 13);
            vddLink.TabIndex = 7;
            vddLink.TabStop = true;
            vddLink.Text = "https://github.com/VirtualDisplay/";
            vddLink.LinkClicked += linkLabel2_LinkClicked;
            // 
            // mttLabel
            // 
            mttLabel.AutoSize = true;
            mttLabel.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            mttLabel.Location = new Point(6, 22);
            mttLabel.Name = "mttLabel";
            mttLabel.Size = new Size(85, 13);
            mttLabel.TabIndex = 11;
            mttLabel.Text = "- MikeTheTech";
            // 
            // budLabel
            // 
            budLabel.AutoSize = true;
            budLabel.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            budLabel.Location = new Point(6, 44);
            budLabel.Name = "budLabel";
            budLabel.Size = new Size(37, 13);
            budLabel.TabIndex = 12;
            budLabel.Text = "- Bud";
            // 
            // jockeLabel
            // 
            jockeLabel.AutoSize = true;
            jockeLabel.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            jockeLabel.Location = new Point(6, 65);
            jockeLabel.Name = "jockeLabel";
            jockeLabel.Size = new Size(49, 13);
            jockeLabel.TabIndex = 13;
            jockeLabel.Text = "- Jocke";
            // 
            // mttSupport
            // 
            mttSupport.AutoSize = true;
            mttSupport.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point);
            mttSupport.LinkColor = Color.White;
            mttSupport.Location = new Point(158, 23);
            mttSupport.Name = "mttSupport";
            mttSupport.Size = new Size(84, 12);
            mttSupport.TabIndex = 16;
            mttSupport.TabStop = true;
            mttSupport.Text = "(Support this Dev)";
            // 
            // linkLabel6
            // 
            linkLabel6.AutoSize = true;
            linkLabel6.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point);
            linkLabel6.LinkColor = Color.White;
            linkLabel6.Location = new Point(203, 94);
            linkLabel6.Name = "linkLabel6";
            linkLabel6.Size = new Size(39, 12);
            linkLabel6.TabIndex = 22;
            linkLabel6.TabStop = true;
            linkLabel6.Text = "Patreon";
            // 
            // jockeSupport
            // 
            jockeSupport.AutoSize = true;
            jockeSupport.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point);
            jockeSupport.LinkColor = Color.White;
            jockeSupport.Location = new Point(158, 66);
            jockeSupport.Name = "jockeSupport";
            jockeSupport.Size = new Size(84, 12);
            jockeSupport.TabIndex = 23;
            jockeSupport.TabStop = true;
            jockeSupport.Text = "(Support this Dev)";
            // 
            // budSupport
            // 
            budSupport.AutoSize = true;
            budSupport.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point);
            budSupport.LinkColor = Color.White;
            budSupport.Location = new Point(158, 45);
            budSupport.Name = "budSupport";
            budSupport.Size = new Size(84, 12);
            budSupport.TabIndex = 24;
            budSupport.TabStop = true;
            budSupport.Text = "(Support this Dev)";
            // 
            // patreonGroupBox
            // 
            patreonGroupBox.Controls.Add(linkLabel6);
            patreonGroupBox.Controls.Add(patreonMembersListBox);
            patreonGroupBox.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            patreonGroupBox.ForeColor = SystemColors.ButtonHighlight;
            patreonGroupBox.Location = new Point(555, 274);
            patreonGroupBox.Name = "patreonGroupBox";
            patreonGroupBox.Size = new Size(248, 115);
            patreonGroupBox.TabIndex = 25;
            patreonGroupBox.TabStop = false;
            patreonGroupBox.Text = "Patreon Supporters";
            // 
            // patreonMembersListBox
            // 
            patreonMembersListBox.BackColor = Color.FromArgb(32, 41, 50);
            patreonMembersListBox.BorderStyle = BorderStyle.None;
            patreonMembersListBox.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            patreonMembersListBox.ForeColor = SystemColors.Window;
            patreonMembersListBox.FormattingEnabled = true;
            patreonMembersListBox.Items.AddRange(new object[] { "- Gabriel Posso" });
            patreonMembersListBox.Location = new Point(6, 22);
            patreonMembersListBox.Name = "patreonMembersListBox";
            patreonMembersListBox.Size = new Size(236, 78);
            patreonMembersListBox.TabIndex = 23;
            // 
            // devsGroupBox
            // 
            devsGroupBox.Controls.Add(mttLabel);
            devsGroupBox.Controls.Add(budLabel);
            devsGroupBox.Controls.Add(jockeLabel);
            devsGroupBox.Controls.Add(jockeSupport);
            devsGroupBox.Controls.Add(budSupport);
            devsGroupBox.Controls.Add(mttSupport);
            devsGroupBox.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            devsGroupBox.ForeColor = SystemColors.ButtonHighlight;
            devsGroupBox.Location = new Point(555, 173);
            devsGroupBox.Name = "devsGroupBox";
            devsGroupBox.Size = new Size(248, 95);
            devsGroupBox.TabIndex = 26;
            devsGroupBox.TabStop = false;
            devsGroupBox.Text = "Developers";
            // 
            // resourcesGroupBox
            // 
            resourcesGroupBox.Controls.Add(gitLabel);
            resourcesGroupBox.Controls.Add(vddLink);
            resourcesGroupBox.Controls.Add(discordLabel);
            resourcesGroupBox.Controls.Add(discordLink);
            resourcesGroupBox.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            resourcesGroupBox.ForeColor = SystemColors.ButtonHighlight;
            resourcesGroupBox.Location = new Point(555, 72);
            resourcesGroupBox.Name = "resourcesGroupBox";
            resourcesGroupBox.Size = new Size(248, 95);
            resourcesGroupBox.TabIndex = 27;
            resourcesGroupBox.TabStop = false;
            resourcesGroupBox.Text = "Resources";
            // 
            // userInput
            // 
            userInput.BackColor = Color.FromArgb(32, 32, 32);
            userInput.BorderStyle = BorderStyle.FixedSingle;
            userInput.ForeColor = Color.White;
            userInput.Location = new Point(13, 461);
            userInput.Name = "userInput";
            userInput.Size = new Size(453, 21);
            userInput.TabIndex = 29;
            userInput.Text = "Command Console";
            userInput.WordWrap = false;
            userInput.TextChanged += textBox1_TextChanged;
            // 
            // taskGroupBox
            // 
            taskGroupBox.Controls.Add(taskProgressBar);
            taskGroupBox.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            taskGroupBox.ForeColor = SystemColors.ButtonHighlight;
            taskGroupBox.Location = new Point(555, 395);
            taskGroupBox.Name = "taskGroupBox";
            taskGroupBox.Size = new Size(248, 59);
            taskGroupBox.TabIndex = 32;
            taskGroupBox.TabStop = false;
            taskGroupBox.Text = "Task Progress";
            // 
            // taskProgressBar
            // 
            taskProgressBar.BackColor = Color.Transparent;
            taskProgressBar.BaseColor = Color.FromArgb(75, 255, 255, 255);
            taskProgressBar.BorderColor = Color.FromArgb(117, 120, 117);
            taskProgressBar.ColorA = Color.FromArgb(203, 201, 205);
            taskProgressBar.ColorB = Color.FromArgb(188, 186, 190);
            taskProgressBar.EdgeColor = Color.FromArgb(125, 97, 94, 90);
            taskProgressBar.ForeColor = Color.Black;
            taskProgressBar.HatchType = System.Drawing.Drawing2D.HatchStyle.DarkUpwardDiagonal;
            taskProgressBar.Location = new Point(6, 19);
            taskProgressBar.Maximum = 100;
            taskProgressBar.Name = "taskProgressBar";
            taskProgressBar.PercentageText = "%";
            taskProgressBar.ProgressBorderColorA = Color.FromArgb(150, 97, 94, 90);
            taskProgressBar.ProgressBorderColorB = Color.FromArgb(0, 120, 0);
            taskProgressBar.ProgressColorA = Color.FromArgb(0, 120, 0);
            taskProgressBar.ProgressColorB = Color.FromArgb(0, 120, 0);
            taskProgressBar.ProgressLineColorA = Color.FromArgb(40, 255, 255, 255);
            taskProgressBar.ProgressLineColorB = Color.FromArgb(20, 255, 255, 255);
            taskProgressBar.ShowEdge = false;
            taskProgressBar.ShowPercentage = false;
            taskProgressBar.Size = new Size(236, 34);
            taskProgressBar.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            taskProgressBar.TabIndex = 0;
            taskProgressBar.Text = "ribbonProgressBarCenter1";
            taskProgressBar.Value = 37;
            // 
            // mainTheme
            // 
            mainTheme.BackColor = Color.FromArgb(32, 41, 50);
            mainTheme.Controls.Add(restartAllButton);
            mainTheme.Controls.Add(enterButton);
            mainTheme.Controls.Add(minButton);
            mainTheme.Controls.Add(closeButton);
            mainTheme.Controls.Add(mainConsole);
            mainTheme.Controls.Add(resourcesGroupBox);
            mainTheme.Controls.Add(taskGroupBox);
            mainTheme.Controls.Add(devsGroupBox);
            mainTheme.Controls.Add(mainVisibleMenuStrip);
            mainTheme.Controls.Add(patreonGroupBox);
            mainTheme.Controls.Add(userInput);
            mainTheme.Dock = DockStyle.Fill;
            mainTheme.Font = new Font("Arial Rounded MT Bold", 9F, FontStyle.Regular, GraphicsUnit.Point);
            mainTheme.ForeColor = Color.FromArgb(32, 41, 50);
            mainTheme.Image = (Image)resources.GetObject("mainTheme.Image");
            mainTheme.Location = new Point(0, 0);
            mainTheme.Name = "mainTheme";
            mainTheme.Padding = new Padding(10, 70, 10, 9);
            mainTheme.RoundCorners = true;
            mainTheme.Sizable = true;
            mainTheme.Size = new Size(816, 489);
            mainTheme.SmartBounds = true;
            mainTheme.StartPosition = FormStartPosition.WindowsDefaultLocation;
            mainTheme.TabIndex = 33;
            mainTheme.Text = "Virtual Driver Control";
            mainTheme.Click += themeForm1_Click;
            // 
            // restartAllButton
            // 
            restartAllButton.BackColor = Color.Transparent;
            restartAllButton.Font = new Font("Consolas", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            restartAllButton.ForeColor = Color.LightGray;
            restartAllButton.Image = null;
            restartAllButton.ImageAlign = ContentAlignment.MiddleLeft;
            restartAllButton.InactiveColorA = Color.FromArgb(0, 120, 0);
            restartAllButton.InactiveColorB = Color.FromArgb(0, 120, 0);
            restartAllButton.Location = new Point(650, 460);
            restartAllButton.Name = "restartAllButton";
            restartAllButton.PressedColorA = Color.FromArgb(0, 80, 0);
            restartAllButton.PressedColorB = Color.FromArgb(0, 80, 0);
            restartAllButton.PressedContourColorA = Color.FromArgb(0, 80, 0);
            restartAllButton.PressedContourColorB = Color.FromArgb(0, 80, 0);
            restartAllButton.Size = new Size(154, 22);
            restartAllButton.TabIndex = 35;
            restartAllButton.Text = "Restart All Drivers";
            restartAllButton.TextAlignment = StringAlignment.Center;
            // 
            // enterButton
            // 
            enterButton.BackColor = Color.Transparent;
            enterButton.Font = new Font("Consolas", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            enterButton.ForeColor = Color.LightGray;
            enterButton.Image = null;
            enterButton.ImageAlign = ContentAlignment.MiddleLeft;
            enterButton.InactiveColorA = Color.FromArgb(0, 120, 0);
            enterButton.InactiveColorB = Color.FromArgb(0, 120, 0);
            enterButton.Location = new Point(472, 460);
            enterButton.Name = "enterButton";
            enterButton.PressedColorA = Color.FromArgb(0, 80, 0);
            enterButton.PressedColorB = Color.FromArgb(0, 80, 0);
            enterButton.PressedContourColorA = Color.FromArgb(0, 80, 0);
            enterButton.PressedContourColorB = Color.FromArgb(0, 80, 0);
            enterButton.Size = new Size(61, 22);
            enterButton.TabIndex = 34;
            enterButton.Text = "Enter";
            enterButton.TextAlignment = StringAlignment.Center;
            // 
            // minButton
            // 
            minButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            minButton.BackColor = Color.White;
            minButton.BaseColor = Color.FromArgb(45, 47, 49);
            minButton.DefaultLocation = true;
            minButton.DownColor = Color.FromArgb(30, 0, 0, 0);
            minButton.Font = new Font("Marlett", 12F, FontStyle.Regular, GraphicsUnit.Point);
            minButton.Location = new Point(738, 16);
            minButton.Name = "minButton";
            minButton.OverColor = Color.FromArgb(30, 255, 255, 255);
            minButton.Size = new Size(18, 18);
            minButton.TabIndex = 33;
            minButton.Text = "foreverMinimize1";
            minButton.TextColor = Color.FromArgb(243, 243, 243);
            // 
            // closeButton
            // 
            closeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            closeButton.BackColor = Color.White;
            closeButton.BaseColor = Color.FromArgb(45, 47, 49);
            closeButton.DefaultLocation = true;
            closeButton.DownColor = Color.FromArgb(30, 0, 0, 0);
            closeButton.Font = new Font("Marlett", 10F, FontStyle.Regular, GraphicsUnit.Point);
            closeButton.Location = new Point(786, 16);
            closeButton.Name = "closeButton";
            closeButton.OverColor = Color.FromArgb(30, 255, 255, 255);
            closeButton.Size = new Size(18, 18);
            closeButton.TabIndex = 3;
            closeButton.Text = "foreverClose1";
            closeButton.TextColor = Color.FromArgb(243, 243, 243);
            // 
            // Form1
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(816, 489);
            ContextMenuStrip = trayMenu;
            Controls.Add(mainTheme);
            ForeColor = SystemColors.ControlLightLight;
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = mainVisibleMenuStrip;
            MaximizeBox = false;
            MaximumSize = new Size(816, 489);
            MinimumSize = new Size(261, 61);
            Name = "Form1";
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "Virtual Driver Control";
            TransparencyKey = Color.Fuchsia;
            Load += Form1_Load;
            trayMenu.ResumeLayout(false);
            mainVisibleMenuStrip.ResumeLayout(false);
            mainVisibleMenuStrip.PerformLayout();
            patreonGroupBox.ResumeLayout(false);
            patreonGroupBox.PerformLayout();
            devsGroupBox.ResumeLayout(false);
            devsGroupBox.PerformLayout();
            resourcesGroupBox.ResumeLayout(false);
            resourcesGroupBox.PerformLayout();
            taskGroupBox.ResumeLayout(false);
            mainTheme.ResumeLayout(false);
            mainTheme.PerformLayout();
            ResumeLayout(false);
        }



        #endregion
        private NotifyIcon notificationIcon;
        private ToolStripMenuItem menuToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem virtualDisplayDriverToolStripMenuItem;
        private ToolStripMenuItem virtualAudioDriverToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private MenuStrip mainVisibleMenuStrip;
        private ContextMenuStrip trayMenu;
        private ToolStripMenuItem menuToolStripMenuItem1;
        private ToolStripMenuItem virtualDisplayDriverToolStripMenuItem1;
        private ToolStripMenuItem virtualAudioDriverToolStripMenuItem1;
        private ToolStripMenuItem aboutToolStripMenuItem1;
        private ToolStripMenuItem sDR10bitToolStripMenuItem;
        private ToolStripMenuItem hDRToolStripMenuItem;
        private ToolStripMenuItem customEDIDToolStripMenuItem;
        private ToolStripMenuItem hardwareCursorToolStripMenuItem;
        private ToolStripMenuItem preventMonitorSpoofToolStripMenuItem;
        private ToolStripMenuItem eDIDCEAOverrideToolStripMenuItem;
        private ToolStripMenuItem selectGPUToolStripMenuItem;
        private ToolStripMenuItem displayCountToolStripMenuItem;
        private ToolStripMenuItem enableToolStripMenuItem;
        private ToolStripMenuItem enableDriverToolStripMenuItem;
        private ToolStripMenuItem disableDriverToolStripMenuItem;
        private ToolStripMenuItem restartDriverToolStripMenuItem;
        private ToolStripMenuItem systemToolStripMenuItem;
        private ToolStripMenuItem enableDriverToolStripMenuItem1;
        private ToolStripMenuItem disableDriverToolStripMenuItem1;
        private ToolStripMenuItem restartDriverToolStripMenuItem1;
        private ToolStripMenuItem exitToolStripMenuItem1;
        private ToolStripMenuItem sDR10bitToolStripMenuItem1;
        private ToolStripMenuItem hDRToolStripMenuItem1;
        private ToolStripMenuItem customEDIDToolStripMenuItem1;
        private ToolStripMenuItem hardwareCursorToolStripMenuItem1;
        private ToolStripMenuItem preventMonitorSpoofToolStripMenuItem1;
        private ToolStripMenuItem eDIDCEAOverrideToolStripMenuItem1;
        private ToolStripMenuItem selectGPUToolStripMenuItem1;
        private ToolStripMenuItem displayCountToolStripMenuItem1;
        private ToolStripMenuItem systemToolStripMenuItem1;
        private ToolStripMenuItem enableDriverToolStripMenuItem2;
        private ToolStripMenuItem disableDriverToolStripMenuItem2;
        private ToolStripMenuItem restartDriverToolStripMenuItem2;
        private ToolStripMenuItem systemToolStripMenuItem2;
        private ToolStripMenuItem enableDriverToolStripMenuItem3;
        private ToolStripMenuItem disableDriverToolStripMenuItem3;
        private ToolStripMenuItem restartDriverToolStripMenuItem3;
        private ToolStripMenuItem exitToolStripMenuItem2;
        private RichTextBox mainConsole;
        private ToolStripMenuItem toolsToolStripMenuItem1;
        private ToolStripMenuItem getDisplayInformationToolStripMenuItem1;
        private ToolStripMenuItem getGPUInformationToolStripMenuItem1;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem getGPUInformationToolStripMenuItem;
        private Label gitLabel;
        private LinkLabel discordLink;
        private Label discordLabel;
        private LinkLabel vddLink;
        private ToolStripMenuItem loggingToolStripMenuItem1;
        private ToolStripMenuItem userModeLoggingToolStripMenuItem;
        private ToolStripMenuItem devModeLoggingToolStripMenuItem;
        private ToolStripMenuItem loggingToolStripMenuItem;
        private ToolStripMenuItem enableUserModeLoggingToolStripMenuItem;
        private ToolStripMenuItem enableDevModeLoggingToolStripMenuItem;
        private ToolStripMenuItem getCPUInformationToolStripMenuItem;
        private ToolStripMenuItem getDisplayInformationToolStripMenuItem2;
        private ToolStripMenuItem getAudioInformationToolStripMenuItem;
        private ToolStripMenuItem getCPUInformationToolStripMenuItem1;
        private ToolStripMenuItem getAudioInformationToolStripMenuItem1;
        private Label mttLabel;
        private Label budLabel;
        private Label jockeLabel;
        private LinkLabel mttSupport;
        private LinkLabel linkLabel6;
        private LinkLabel jockeSupport;
        private LinkLabel budSupport;
        private ToolStripMenuItem xMLOptionsEditorToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem1;
        private GroupBox patreonGroupBox;
        private ListBox patreonMembersListBox;
        private GroupBox devsGroupBox;
        private GroupBox resourcesGroupBox;
        private TextBox userInput;
        private GroupBox taskGroupBox;
        private ReaLTaiizor.Forms.ThemeForm mainTheme;
        private ReaLTaiizor.Controls.ForeverClose closeButton;
        private ReaLTaiizor.Controls.ForeverMinimize minButton;
        private ReaLTaiizor.Controls.ChatButtonRight enterButton;
        private ReaLTaiizor.Controls.ChatButtonRight restartAllButton;
        private ReaLTaiizor.Controls.RibbonProgressBarCenter taskProgressBar;
    }
}
