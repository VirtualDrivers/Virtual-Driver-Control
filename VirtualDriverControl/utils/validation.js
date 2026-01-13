/**
 * Input validation utilities
 * Provides functions to validate and sanitize user inputs
 */

// Make available globally for browser context
if (typeof window !== 'undefined') {
    window.InputValidator = class InputValidator {
    /**
     * Validate and sanitize a string input
     * @param {string} input - The input string to validate
     * @param {Object} options - Validation options
     * @returns {Object} - { valid: boolean, value: string, error: string }
     */
    static validateString(input, options = {}) {
        const {
            minLength = 0,
            maxLength = Infinity,
            allowEmpty = false,
            pattern = null,
            trim = true
        } = options;

        if (typeof input !== 'string') {
            return { valid: false, value: '', error: 'Input must be a string' };
        }

        let value = trim ? input.trim() : input;

        if (!allowEmpty && value.length === 0) {
            return { valid: false, value: '', error: 'Input cannot be empty' };
        }

        if (value.length < minLength) {
            return { valid: false, value: '', error: `Input must be at least ${minLength} characters` };
        }

        if (value.length > maxLength) {
            return { valid: false, value: '', error: `Input must be no more than ${maxLength} characters` };
        }

        if (pattern && !pattern.test(value)) {
            return { valid: false, value: '', error: 'Input does not match required pattern' };
        }

        return { valid: true, value, error: null };
    }

    /**
     * Validate a number input
     * @param {*} input - The input to validate
     * @param {Object} options - Validation options
     * @returns {Object} - { valid: boolean, value: number, error: string }
     */
    static validateNumber(input, options = {}) {
        const {
            min = -Infinity,
            max = Infinity,
            integer = false,
            allowNaN = false
        } = options;

        const num = Number(input);

        if (!allowNaN && (isNaN(num) || !isFinite(num))) {
            return { valid: false, value: null, error: 'Input must be a valid number' };
        }

        if (integer && !Number.isInteger(num)) {
            return { valid: false, value: null, error: 'Input must be an integer' };
        }

        if (num < min) {
            return { valid: false, value: null, error: `Input must be at least ${min}` };
        }

        if (num > max) {
            return { valid: false, value: null, error: `Input must be no more than ${max}` };
        }

        return { valid: true, value: num, error: null };
    }

    /**
     * Validate a file path
     * @param {string} path - The path to validate
     * @returns {Object} - { valid: boolean, value: string, error: string }
     */
    static validateFilePath(path) {
        const result = this.validateString(path, {
            minLength: 1,
            maxLength: 260, // Windows MAX_PATH
            allowEmpty: false
        });

        if (!result.valid) {
            return result;
        }

        // Check for path traversal attempts
        if (path.includes('..') || path.includes('//') || path.includes('\\\\')) {
            return { valid: false, value: '', error: 'Invalid path: path traversal detected' };
        }

        // Check for null bytes
        if (path.includes('\0')) {
            return { valid: false, value: '', error: 'Invalid path: null byte detected' };
        }

        return { valid: true, value: path, error: null };
    }

    /**
     * Validate monitor count
     * @param {*} input - The input to validate
     * @returns {Object} - { valid: boolean, value: number, error: string }
     */
    static validateMonitorCount(input) {
        return this.validateNumber(input, {
            min: 1,
            max: 16,
            integer: true
        });
    }

    /**
     * Validate resolution dimensions
     * @param {*} width - Width value
     * @param {*} height - Height value
     * @returns {Object} - { valid: boolean, width: number, height: number, error: string }
     */
    static validateResolution(width, height) {
        const widthResult = this.validateNumber(width, {
            min: 640,
            max: 7680,
            integer: true
        });

        if (!widthResult.valid) {
            return { valid: false, width: null, height: null, error: `Width: ${widthResult.error}` };
        }

        const heightResult = this.validateNumber(height, {
            min: 480,
            max: 4320,
            integer: true
        });

        if (!heightResult.valid) {
            return { valid: false, width: null, height: null, error: `Height: ${heightResult.error}` };
        }

        return {
            valid: true,
            width: widthResult.value,
            height: heightResult.value,
            error: null
        };
    }

    /**
     * Validate refresh rate
     * @param {*} input - The input to validate
     * @returns {Object} - { valid: boolean, value: number, error: string }
     */
    static validateRefreshRate(input) {
        return this.validateNumber(input, {
            min: 1,
            max: 1000,
            integer: false
        });
    }

    /**
     * Validate GPU name
     * @param {string} input - The GPU name to validate
     * @returns {Object} - { valid: boolean, value: string, error: string }
     */
    static validateGPUName(input) {
        return this.validateString(input, {
            minLength: 1,
            maxLength: 256,
            allowEmpty: false,
            pattern: /^[a-zA-Z0-9\s\-_]+$/ // Alphanumeric, spaces, hyphens, underscores
        });
    }

    /**
     * Validate color format
     * @param {string} input - The color format to validate
     * @returns {Object} - { valid: boolean, value: string, error: string }
     */
    static validateColorFormat(input) {
        const validFormats = ['RGB', 'YCbCr444', 'YCbCr422', 'YCbCr420'];
        if (!validFormats.includes(input)) {
            return { valid: false, value: '', error: `Color format must be one of: ${validFormats.join(', ')}` };
        }
        return { valid: true, value: input, error: null };
    }

    /**
     * Validate hex color code
     * @param {string} input - The hex color to validate
     * @returns {Object} - { valid: boolean, value: string, error: string }
     */
    static validateHexColor(input) {
        const hexPattern = /^#[0-9A-Fa-f]{6}$/;
        const result = this.validateString(input, {
            minLength: 7,
            maxLength: 7,
            allowEmpty: false,
            pattern: hexPattern
        });
        return result;
    }
    };
}

// Also export for Node.js if needed
if (typeof module !== 'undefined' && module.exports) {
    module.exports = window.InputValidator || class InputValidator {};
}

