/**
 * Compatibility adapter for migrating from window.require to electronAPI
 * This provides a compatibility layer during migration
 */

class CompatibilityAdapter {
    constructor() {
        this.electronAPI = window.electronAPI;
        this.safeConsole = window.safeConsole || console;
    }

    /**
     * Get file system access (compatible with old window.require('fs'))
     */
    get fs() {
        if (!this.electronAPI) {
            throw new Error('Electron API not available. Make sure preload.js is loaded.');
        }
        
        return {
            readFileSync: async (path, encoding) => {
                const content = await this.electronAPI.readFile(path);
                return content;
            },
            writeFileSync: async (path, content, encoding) => {
                await this.electronAPI.writeFile(path, content);
            },
            appendFileSync: async (path, content) => {
                const existing = await this.electronAPI.readFile(path).catch(() => '');
                await this.electronAPI.writeFile(path, existing + content);
            },
            existsSync: async (path) => {
                return await this.electronAPI.existsFile(path);
            },
            mkdirSync: async (path, options) => {
                await this.electronAPI.mkdir(path);
            },
            readdirSync: async (path) => {
                return await this.electronAPI.readdir(path);
            },
            unlinkSync: async (path) => {
                await this.electronAPI.unlink(path);
            },
            statSync: async (path) => {
                return await this.electronAPI.stat(path);
            }
        };
    }

    /**
     * Get path utilities (compatible with window.require('path'))
     */
    get path() {
        // Return a minimal path-like object
        // Note: Actual path operations should use electronAPI
        return {
            join: (...parts) => {
                // Simple path joining for Windows
                return parts.filter(p => p).join('\\').replace(/\\+/g, '\\');
            },
            dirname: (p) => {
                const parts = p.split(/[/\\]/);
                parts.pop();
                return parts.join('\\');
            },
            basename: (p, ext) => {
                const parts = p.split(/[/\\]/);
                const name = parts[parts.length - 1];
                if (ext && name.endsWith(ext)) {
                    return name.slice(0, -ext.length);
                }
                return name;
            },
            extname: (p) => {
                const match = p.match(/\.[^.]+$/);
                return match ? match[0] : '';
            }
        };
    }

    /**
     * Get child_process utilities
     */
    get child_process() {
        if (!this.electronAPI) {
            throw new Error('Electron API not available');
        }
        
        return {
            exec: async (command, callback) => {
                // Parse command into executable and args
                const parts = command.split(/\s+/);
                const executable = parts[0];
                const args = parts.slice(1);
                
                try {
                    const result = await this.electronAPI.execCommand(executable, args, {});
                    if (callback) {
                        callback(null, result.stdout, result.stderr);
                    }
                    return result;
                } catch (error) {
                    if (callback) {
                        callback(error, null, error.message);
                    }
                    throw error;
                }
            },
            spawn: (command, args, options) => {
                // Note: spawn is more complex, this is a simplified version
                // For full spawn support, use electronAPI.execCommand
                throw new Error('Use electronAPI.execCommand instead of spawn for secure execution');
            }
        };
    }

    /**
     * Get shell utilities (compatible with window.require('electron').shell)
     */
    get shell() {
        if (!this.electronAPI) {
            throw new Error('Electron API not available');
        }
        
        return {
            openExternal: async (url) => {
                await this.electronAPI.openExternal(url);
            },
            openPath: async (path) => {
                await this.electronAPI.openPath(path);
            }
        };
    }

    /**
     * Get util.promisify equivalent
     */
    promisify(fn) {
        return async (...args) => {
            return new Promise((resolve, reject) => {
                fn(...args, (error, ...results) => {
                    if (error) {
                        reject(error);
                    } else {
                        resolve(results.length === 1 ? results[0] : results);
                    }
                });
            });
        };
    }
}

// Create global compatibility adapter
if (typeof window !== 'undefined') {
    window.compatAdapter = new CompatibilityAdapter();
    
    // Provide window.require compatibility (for gradual migration)
    if (!window.require) {
        window.require = (module) => {
            switch (module) {
                case 'fs':
                    return window.compatAdapter.fs;
                case 'path':
                    return window.compatAdapter.path;
                case 'child_process':
                    return window.compatAdapter.child_process;
                case 'electron':
                    return {
                        shell: window.compatAdapter.shell,
                        ipcRenderer: {
                            send: (channel, ...args) => {
                                if (channel === 'quit-app') {
                                    window.electronAPI?.quitApp();
                                } else if (channel === 'driver-status-changed') {
                                    window.electronAPI?.updateIcon(args[0]);
                                }
                            },
                            invoke: async (channel, ...args) => {
                                return await window.electronAPI?.[channel]?.(...args);
                            }
                        }
                    };
                default:
                    throw new Error(`Module ${module} not available in secure context`);
            }
        };
    }
}

