/**
 * Secure command execution utilities
 * Provides safe methods to execute system commands with input sanitization
 */

// Make available globally for browser context
if (typeof window !== 'undefined') {
    window.CommandExecutor = class CommandExecutor {
    /**
     * Execute a PowerShell command safely
     * @param {string} command - PowerShell command (without -Command flag)
     * @param {Array<string>} args - Additional arguments (will be sanitized)
     * @param {Object} options - Execution options
     * @returns {Promise<Object>} - { stdout: string, stderr: string, code: number }
     */
    static async executePowerShell(command, args = [], options = {}) {
        if (!window.electronAPI) {
            throw new Error('Electron API not available');
        }

        // Sanitize command and arguments
        const sanitizedCommand = this.sanitizePowerShellCommand(command);
        const sanitizedArgs = args.map(arg => this.sanitizePowerShellArg(arg));

        // Use IPC to execute command securely
        return await window.electronAPI.execCommand('powershell.exe', [
            '-NoProfile',
            '-NonInteractive',
            '-ExecutionPolicy', 'Bypass',
            '-Command', sanitizedCommand,
            ...sanitizedArgs
        ], options);
    }

    /**
     * Execute a command with array arguments (safer than string commands)
     * @param {string} command - Command to execute
     * @param {Array<string>} args - Command arguments
     * @param {Object} options - Execution options
     * @returns {Promise<Object>} - { stdout: string, stderr: string, code: number }
     */
    static async executeCommand(command, args = [], options = {}) {
        if (!window.electronAPI) {
            throw new Error('Electron API not available');
        }

        // Validate command is in whitelist
        const allowedCommands = ['powershell.exe', 'cmd.exe', 'wmic.exe'];
        if (!allowedCommands.includes(command.toLowerCase())) {
            throw new Error(`Command not allowed: ${command}`);
        }

        // Sanitize arguments
        const sanitizedArgs = args.map(arg => {
            if (typeof arg === 'string') {
                return this.sanitizeCommandArg(arg);
            }
            return String(arg);
        });

        return await window.electronAPI.execCommand(command, sanitizedArgs, options);
    }

    /**
     * Sanitize PowerShell command to prevent injection
     * @param {string} command - Command to sanitize
     * @returns {string} - Sanitized command
     */
    static sanitizePowerShellCommand(command) {
        if (typeof command !== 'string') {
            throw new Error('Command must be a string');
        }

        // Remove dangerous PowerShell operators and characters
        return command
            .replace(/`/g, '``')      // Escape backticks
            .replace(/\$/g, '`$')     // Escape dollar signs
            .replace(/"/g, '`"')      // Escape double quotes
            .replace(/'/g, "''")       // Escape single quotes
            .replace(/;/g, '')        // Remove semicolons
            .replace(/&/g, '')        // Remove ampersands
            .replace(/\|/g, '')       // Remove pipes
            .replace(/</g, '')        // Remove less than
            .replace(/>/g, '')        // Remove greater than
            .replace(/\(/g, '')       // Remove opening parentheses
            .replace(/\)/g, '')       // Remove closing parentheses
            .replace(/\{/g, '')       // Remove opening braces
            .replace(/\}/g, '')       // Remove closing braces
            .replace(/\[/g, '')       // Remove opening brackets
            .replace(/\]/g, '')       // Remove closing brackets
            .replace(/\n/g, '')       // Remove newlines
            .replace(/\r/g, '')       // Remove carriage returns
            .replace(/\t/g, ' ');     // Replace tabs with spaces
    }

    /**
     * Sanitize command argument
     * @param {string} arg - Argument to sanitize
     * @returns {string} - Sanitized argument
     */
    static sanitizeCommandArg(arg) {
        if (typeof arg !== 'string') {
            return String(arg);
        }

        // Remove shell metacharacters
        return arg
            .replace(/[;&|<>(){}[\]$`'"\\]/g, '')  // Remove shell special chars
            .replace(/\n/g, '')                     // Remove newlines
            .replace(/\r/g, '')                     // Remove carriage returns
            .replace(/\0/g, '');                    // Remove null bytes
    }

    /**
     * Sanitize PowerShell argument (for use in -Command)
     * @param {string} arg - Argument to sanitize
     * @returns {string} - Sanitized argument
     */
    static sanitizePowerShellArg(arg) {
        if (typeof arg !== 'string') {
            return String(arg);
        }

        // Escape PowerShell special characters
        return arg
            .replace(/`/g, '``')      // Escape backticks
            .replace(/\$/g, '`$')     // Escape dollar signs
            .replace(/"/g, '`"')      // Escape double quotes
            .replace(/'/g, "''");     // Escape single quotes
    }

    /**
     * Build a safe PowerShell command with parameters
     * @param {string} script - PowerShell script content
     * @param {Object} parameters - Parameters object
     * @returns {string} - Safe command string
     */
    static buildSafePowerShellCommand(script, parameters = {}) {
        // Validate script is a string
        if (typeof script !== 'string') {
            throw new Error('Script must be a string');
        }

        // Build parameter string
        const paramParts = [];
        for (const [key, value] of Object.entries(parameters)) {
            const sanitizedKey = this.sanitizePowerShellArg(key);
            const sanitizedValue = this.sanitizePowerShellArg(String(value));
            paramParts.push(`-${sanitizedKey} "${sanitizedValue}"`);
        }

        const paramString = paramParts.length > 0 ? ` ${paramParts.join(' ')}` : '';
        return `${this.sanitizePowerShellCommand(script)}${paramString}`;
    }
    };
}

// Also export for Node.js if needed
if (typeof module !== 'undefined' && module.exports) {
    module.exports = window.CommandExecutor || class CommandExecutor {};
}

