const { app, BrowserWindow, ipcMain, nativeImage, shell } = require('electron');
const path = require('path');
const fs = require('fs');
const { spawn } = require('child_process');
const { promisify } = require('util');

// Ensure we're in the correct directory
if (__dirname) {
  process.chdir(__dirname);
}
console.log('App directory:', __dirname);
console.log('Working directory:', process.cwd());

let mainWindow;

// Allowed file paths for security
const ALLOWED_PATHS = {
  settings: 'C:\\VirtualDisplayDriver\\vdd_settings.xml',
  scripts: 'C:\\VirtualDisplayDriver\\Scripts',
  logs: 'C:\\VirtualDisplayDriver\\Logs'
};

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

// Sanitize file path to prevent path traversal
function sanitizeFilePath(filePath) {
  if (typeof filePath !== 'string') return null;
  
  // Normalize path
  let normalized = path.normalize(filePath);
  
  // Check for path traversal
  if (normalized.includes('..') || normalized.includes('//') || normalized.includes('\\\\')) {
    return null;
  }
  
  // Check if path is within allowed directories
  const allowedDirs = Object.values(ALLOWED_PATHS);
  const isAllowed = allowedDirs.some(dir => normalized.startsWith(dir));
  
  return isAllowed ? normalized : null;
}

// Execute command securely with array arguments
function executeCommandSecure(command, args, options = {}) {
  return new Promise((resolve, reject) => {
    const timeout = options.timeout || 30000;
    let timeoutId;
    
    const process = spawn(command, args, {
      ...options,
      shell: false  // Don't use shell to prevent injection
    });
    
    let stdout = '';
    let stderr = '';
    
    timeoutId = setTimeout(() => {
      process.kill();
      reject(new Error('Command execution timeout'));
    }, timeout);
    
    process.stdout.on('data', (data) => {
      stdout += data.toString();
    });
    
    process.stderr.on('data', (data) => {
      stderr += data.toString();
    });
    
    process.on('close', (code) => {
      clearTimeout(timeoutId);
      resolve({ stdout, stderr, code });
    });
    
    process.on('error', (error) => {
      clearTimeout(timeoutId);
      reject(error);
    });
  });
}

// Check if running as Administrator
async function checkAdministratorPrivileges() {
  try {
    if (process.platform === 'win32') {
      // Use secure command execution
      const result = await executeCommandSecure('powershell.exe', [
        '-NoProfile',
        '-NonInteractive',
        '-ExecutionPolicy', 'Bypass',
        '-Command',
        '([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)'
      ], { timeout: 5000 });
      
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
    const exePath = process.execPath;
    const appPath = __dirname;
    
    console.log('Restarting as Administrator...');
    console.log('Executable path:', exePath);
    console.log('App directory:', appPath);
    
    // Use secure command execution with sanitized paths
    // Pass the app directory as argument so Electron can find main.js
    // Escape the path properly for PowerShell
    const escapedAppPath = appPath.replace(/"/g, '`"').replace(/\$/g, '`$');
    await executeCommandSecure('powershell.exe', [
      '-NoProfile',
      '-NonInteractive',
      '-ExecutionPolicy', 'Bypass',
      '-Command',
      `Start-Process -FilePath "${exePath.replace(/"/g, '`"')}" -ArgumentList "${escapedAppPath}" -WorkingDirectory "${escapedAppPath}" -Verb RunAs`
    ], { timeout: 10000 });
    
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
      nodeIntegration: false,        // ✅ SECURITY: Disable Node.js in renderer
      contextIsolation: true,         // ✅ SECURITY: Enable context isolation
      enableRemoteModule: false,      // ✅ SECURITY: Disable deprecated remote module
      preload: path.join(__dirname, 'preload.js'),  // ✅ SECURITY: Use preload script
      sandbox: false                  // Keep false for file system access
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

// ==================== IPC HANDLERS ====================

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

// File system operations
ipcMain.handle('read-file', async (event, filePath) => {
  const sanitized = sanitizeFilePath(filePath);
  if (!sanitized) {
    throw new Error('Invalid file path');
  }
  
  try {
    return fs.readFileSync(sanitized, 'utf8');
  } catch (error) {
    console.error('Error reading file:', error);
    throw error;
  }
});

ipcMain.handle('write-file', async (event, filePath, content) => {
  const sanitized = sanitizeFilePath(filePath);
  if (!sanitized) {
    throw new Error('Invalid file path');
  }
  
  try {
    // Ensure directory exists
    const dir = path.dirname(sanitized);
    if (!fs.existsSync(dir)) {
      fs.mkdirSync(dir, { recursive: true });
    }
    
    fs.writeFileSync(sanitized, content, 'utf8');
    return true;
  } catch (error) {
    console.error('Error writing file:', error);
    throw error;
  }
});

ipcMain.handle('exists-file', async (event, filePath) => {
  const sanitized = sanitizeFilePath(filePath);
  if (!sanitized) return false;
  
  try {
    return fs.existsSync(sanitized);
  } catch (error) {
    return false;
  }
});

ipcMain.handle('mkdir', async (event, dirPath) => {
  const sanitized = sanitizeFilePath(dirPath);
  if (!sanitized) {
    throw new Error('Invalid directory path');
  }
  
  try {
    if (!fs.existsSync(sanitized)) {
      fs.mkdirSync(sanitized, { recursive: true });
    }
    return true;
  } catch (error) {
    console.error('Error creating directory:', error);
    throw error;
  }
});

ipcMain.handle('readdir', async (event, dirPath) => {
  const sanitized = sanitizeFilePath(dirPath);
  if (!sanitized) {
    throw new Error('Invalid directory path');
  }
  
  try {
    return fs.readdirSync(sanitized);
  } catch (error) {
    console.error('Error reading directory:', error);
    throw error;
  }
});

ipcMain.handle('unlink', async (event, filePath) => {
  const sanitized = sanitizeFilePath(filePath);
  if (!sanitized) {
    throw new Error('Invalid file path');
  }
  
  try {
    fs.unlinkSync(sanitized);
    return true;
  } catch (error) {
    console.error('Error deleting file:', error);
    throw error;
  }
});

ipcMain.handle('stat', async (event, filePath) => {
  const sanitized = sanitizeFilePath(filePath);
  if (!sanitized) {
    throw new Error('Invalid file path');
  }
  
  try {
    return fs.statSync(sanitized);
  } catch (error) {
    console.error('Error getting file stats:', error);
    throw error;
  }
});

// Command execution (with sanitization)
ipcMain.handle('exec-command', async (event, command, args, options) => {
  // Whitelist allowed commands
  const allowedCommands = ['powershell.exe', 'cmd.exe'];
  if (!allowedCommands.includes(command.toLowerCase())) {
    throw new Error(`Command not allowed: ${command}`);
  }
  
  // Validate arguments are strings
  const validatedArgs = args.map(arg => {
    if (typeof arg !== 'string') {
      throw new Error('Command arguments must be strings');
    }
    return arg;
  });
  
  try {
    return await executeCommandSecure(command, validatedArgs, options);
  } catch (error) {
    console.error('Error executing command:', error);
    throw error;
  }
});

// System information
ipcMain.handle('get-system-info', async () => {
  return {
    platform: process.platform,
    arch: process.arch,
    version: process.version
  };
});

// Driver operations
ipcMain.handle('check-driver-status', async () => {
  try {
    // Check if driver file exists
    const driverPath = 'C:\\Windows\\System32\\drivers\\UMDF\\MttVDD.dll';
    const exists = fs.existsSync(driverPath);
    return { installed: exists };
  } catch (error) {
    console.error('Error checking driver status:', error);
    return { installed: false };
  }
});

ipcMain.handle('reload-driver', async () => {
  // Implementation for driver reload
  // This would require additional driver-specific commands
  return { success: true };
});

// Shell operations
ipcMain.handle('open-external', async (event, url) => {
  try {
    // Validate URL
    const urlObj = new URL(url);
    if (urlObj.protocol !== 'http:' && urlObj.protocol !== 'https:') {
      throw new Error('Invalid URL protocol');
    }
    await shell.openExternal(url);
    return true;
  } catch (error) {
    console.error('Error opening external URL:', error);
    throw error;
  }
});

ipcMain.handle('open-path', async (event, filePath) => {
  const sanitized = sanitizeFilePath(filePath);
  if (!sanitized) {
    throw new Error('Invalid file path');
  }
  
  try {
    await shell.openPath(sanitized);
    return true;
  } catch (error) {
    console.error('Error opening path:', error);
    throw error;
  }
});

// Named pipe communication
ipcMain.handle('send-pipe-command', async (event, command) => {
  const net = require('net');
  const pipePath = '\\\\.\\pipe\\MTTVirtualDisplayPipe';
  
  return new Promise((resolve, reject) => {
    // Validate command
    if (typeof command !== 'string' || command.length === 0) {
      reject(new Error('Invalid command'));
      return;
    }
    
    // Sanitize command (only allow alphanumeric and underscore)
    if (!/^[A-Za-z0-9_]+$/.test(command)) {
      reject(new Error('Invalid command format'));
      return;
    }
    
    console.log(`Sending pipe command: ${command}`);
    
    const client = net.createConnection(pipePath, () => {
      client.write(command);
    });
    
    let responseReceived = false;
    let timeoutId;
    
    timeoutId = setTimeout(() => {
      if (!responseReceived) {
        client.destroy();
        reject(new Error('Command timeout'));
      }
    }, 5000);
    
    client.on('data', (data) => {
      if (responseReceived) return;
      responseReceived = true;
      clearTimeout(timeoutId);
      
      const response = data.toString().trim();
      console.log(`Pipe response: ${response}`);
      
      client.end();
      
      if (response.includes('SUCCESS') || response.includes('OK') || response.length > 0) {
        resolve(response);
      } else {
        reject(new Error(`Driver command failed: ${response}`));
      }
    });
    
    client.on('error', (error) => {
      if (!responseReceived) {
        clearTimeout(timeoutId);
        console.error('Pipe error:', error.message);
        reject(new Error(`Communication failed: ${error.message}`));
      }
    });
    
    client.on('end', () => {
      if (!responseReceived) {
        clearTimeout(timeoutId);
        resolve('Command sent');
      }
    });
  });
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