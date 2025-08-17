const { app, BrowserWindow, ipcMain, nativeImage } = require('electron');
const path = require('path');
const { exec } = require('child_process');
const { promisify } = require('util');

const execPromise = promisify(exec);
let mainWindow;

// Icon paths for different driver states
const icons = {
  default: path.join(__dirname, 'Virtual Display Driver.ico'),
  installed: path.join(__dirname, 'Virtual Display Driver.ico'), // Green/default
  warning: path.join(__dirname, 'VDD_Yellow.ico'), // Yellow for warnings
  error: path.join(__dirname, 'VDD_Red.ico') // Red for errors/not installed
};

// Function to update app icon based on driver status
function updateAppIcon(statusClass) {
  if (!mainWindow) return;
  
  let iconPath;
  switch (statusClass) {
    case 'success':
      iconPath = icons.installed;
      break;
    case 'warning':
      iconPath = icons.warning;
      break;
    case 'danger':
    case 'error':
      iconPath = icons.error;
      break;
    default:
      iconPath = icons.default;
      break;
  }
  
  try {
    const icon = nativeImage.createFromPath(iconPath);
    mainWindow.setIcon(icon);
    console.log(`App icon updated to: ${iconPath} (status: ${statusClass})`);
  } catch (error) {
    console.error('Failed to update app icon:', error);
  }
}

// Check if running as Administrator
async function checkAdministratorPrivileges() {
  try {
    if (process.platform === 'win32') {
      // Use PowerShell to check if running as Administrator
      const command = 'powershell -Command "([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)"';
      const result = await execPromise(command);
      return result.stdout.trim().toLowerCase() === 'true';
    }
    return true; // Non-Windows platforms don't need elevation for this app
  } catch (error) {
    console.error('Error checking administrator privileges:', error);
    return false;
  }
}

// Restart application as Administrator
async function restartAsAdministrator() {
  try {
    const appPath = app.getAppPath();
    const exePath = process.execPath;
    
    console.log('Restarting as Administrator...');
    console.log('Executable path:', exePath);
    console.log('App path:', appPath);
    
    // Use PowerShell to restart as Administrator
    const command = `Start-Process -FilePath "${exePath}" -ArgumentList "${appPath}" -Verb RunAs`;
    const psCommand = `powershell -Command "${command}"`;
    
    await execPromise(psCommand);
    console.log('Administrator restart command executed successfully');
    
    // Close current instance
    app.quit();
  } catch (error) {
    console.error('Failed to restart as Administrator:', error);
    // Continue without elevation
    createWindow();
  }
}

function createWindow() {
  mainWindow = new BrowserWindow({
    width: 1200,
    height: 800,
    minWidth: 800,
    minHeight: 600,
    icon: path.join(__dirname, 'Virtual Display Driver.ico'),
    webPreferences: {
      nodeIntegration: true,
      contextIsolation: false,
      enableRemoteModule: true
    },
    titleBarStyle: 'default',
    frame: true,
    show: false,
    backgroundColor: '#f3f3f3',
    autoHideMenuBar: true
  });

  mainWindow.loadFile('index.html');

  mainWindow.once('ready-to-show', () => {
    mainWindow.show();
    // Set initial icon (will be updated when driver status is detected)
    updateAppIcon('default');
  });

  mainWindow.on('closed', () => {
    mainWindow = null;
  });
}

// Check for Administrator privileges before creating window
async function initializeApp() {
  try {
    const isAdmin = await checkAdministratorPrivileges();
    console.log('Running as Administrator:', isAdmin);
    
    if (!isAdmin) {
      console.log('Not running as Administrator - requesting elevation');
      await restartAsAdministrator();
      return; // Exit here as we're restarting
    }
    
    console.log('Administrator privileges confirmed - creating window');
    createWindow();
  } catch (error) {
    console.error('Error during app initialization:', error);
    // Fallback: create window anyway
    createWindow();
  }
}

app.whenReady().then(initializeApp);

// Handle IPC messages
ipcMain.on('quit-app', () => {
  console.log('Received quit-app message, closing application');
  app.quit();
});

// Handle driver status updates from renderer process
ipcMain.on('driver-status-changed', (event, statusClass) => {
  console.log(`Received driver status update: ${statusClass}`);
  updateAppIcon(statusClass);
});

app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') {
    app.quit();
  }
});

app.on('activate', () => {
  if (BrowserWindow.getAllWindows().length === 0) {
    createWindow();
  }
});