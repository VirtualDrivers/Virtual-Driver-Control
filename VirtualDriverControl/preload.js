/**
 * Preload script for secure IPC communication
 * This script runs in a context that has access to both DOM and Node.js APIs
 * but isolates the renderer process from direct Node.js access
 */

const { contextBridge, ipcRenderer } = require('electron');

// Expose protected methods that allow the renderer process to use
// the ipcRenderer without exposing the entire object
contextBridge.exposeInMainWorld('electronAPI', {
    // File system operations
    readFile: (path) => ipcRenderer.invoke('read-file', path),
    writeFile: (path, content) => ipcRenderer.invoke('write-file', path, content),
    existsFile: (path) => ipcRenderer.invoke('exists-file', path),
    mkdir: (path) => ipcRenderer.invoke('mkdir', path),
    readdir: (path) => ipcRenderer.invoke('readdir', path),
    unlink: (path) => ipcRenderer.invoke('unlink', path),
    stat: (path) => ipcRenderer.invoke('stat', path),
    
    // Process execution (with sanitization)
    execCommand: (command, args, options) => ipcRenderer.invoke('exec-command', command, args, options),
    
    // System information
    getSystemInfo: () => ipcRenderer.invoke('get-system-info'),
    
    // Driver operations
    checkDriverStatus: () => ipcRenderer.invoke('check-driver-status'),
    reloadDriver: () => ipcRenderer.invoke('reload-driver'),
    
    // App control
    quitApp: () => ipcRenderer.send('quit-app'),
    updateIcon: (statusClass) => ipcRenderer.send('driver-status-changed', statusClass),
    
    // Shell operations
    openExternal: (url) => ipcRenderer.invoke('open-external', url),
    openPath: (path) => ipcRenderer.invoke('open-path', path),
    
    // Named pipe operations
    sendPipeCommand: (command) => ipcRenderer.invoke('send-pipe-command', command),
    
    // Window controls
    minimizeWindow: () => ipcRenderer.send('window-minimize'),
    maximizeWindow: () => ipcRenderer.send('window-maximize'),
    closeWindow: () => ipcRenderer.send('window-close'),
    
    // Event listeners
    on: (channel, callback) => {
        const validChannels = ['driver-status-update', 'window-maximized', 'window-unmaximized'];
        if (validChannels.includes(channel)) {
            ipcRenderer.on(channel, (event, ...args) => callback(...args));
        }
    },
    
    removeListener: (channel, callback) => {
        ipcRenderer.removeListener(channel, callback);
    }
});

// Expose a safe console object for debugging
contextBridge.exposeInMainWorld('safeConsole', {
    log: (...args) => console.log('[Renderer]', ...args),
    error: (...args) => console.error('[Renderer]', ...args),
    warn: (...args) => console.warn('[Renderer]', ...args)
});

