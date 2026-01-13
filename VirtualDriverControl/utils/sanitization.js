/**
 * HTML and input sanitization utilities
 * Provides functions to safely handle user-generated content
 */

// Make available globally for browser context
if (typeof window !== 'undefined') {
    window.Sanitizer = class Sanitizer {
    /**
     * Escape HTML special characters to prevent XSS
     * @param {string} text - Text to escape
     * @returns {string} - Escaped HTML string
     */
    static escapeHtml(text) {
        if (typeof text !== 'string') {
            return String(text);
        }

        const div = document.createElement('div');
        div.textContent = text;
        return div.innerHTML;
    }

    /**
     * Escape HTML attributes
     * @param {string} text - Text to escape
     * @returns {string} - Escaped string safe for HTML attributes
     */
    static escapeHtmlAttribute(text) {
        if (typeof text !== 'string') {
            text = String(text);
        }

        return text
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#x27;')
            .replace(/\//g, '&#x2F;');
    }

    /**
     * Sanitize text for use in innerHTML (basic sanitization)
     * For production, consider using DOMPurify library
     * @param {string} html - HTML string to sanitize
     * @returns {string} - Sanitized HTML string
     */
    static sanitizeHtml(html) {
        if (typeof html !== 'string') {
            return '';
        }

        // Basic sanitization - escape all HTML
        // For production use, integrate DOMPurify: https://github.com/cure53/DOMPurify
        return this.escapeHtml(html);
    }

    /**
     * Safely set text content of an element
     * @param {HTMLElement} element - DOM element
     * @param {string} text - Text content
     */
    static setTextContent(element, text) {
        if (!element) return;
        element.textContent = text || '';
    }

    /**
     * Safely create a text node
     * @param {string} text - Text content
     * @returns {Text} - Text node
     */
    static createTextNode(text) {
        return document.createTextNode(text || '');
    }

    /**
     * Safely set innerHTML with sanitization
     * @param {HTMLElement} element - DOM element
     * @param {string} html - HTML content (will be sanitized)
     */
    static setInnerHTML(element, html) {
        if (!element) return;
        // For now, escape all HTML. In production, use DOMPurify for selective sanitization
        element.innerHTML = this.sanitizeHtml(html);
    }

    /**
     * Sanitize PowerShell command arguments to prevent injection
     * @param {string} arg - Argument to sanitize
     * @returns {string} - Sanitized argument
     */
    static sanitizePowerShellArg(arg) {
        if (typeof arg !== 'string') {
            arg = String(arg);
        }

        // Remove potentially dangerous characters
        // PowerShell special characters: ` $ " ' ; & | < > ( ) { } [ ] \ @ # ? * ~
        // We'll escape quotes and backticks, and remove other dangerous characters
        return arg
            .replace(/`/g, '``')  // Escape backticks
            .replace(/\$/g, '`$') // Escape dollar signs
            .replace(/"/g, '`"')  // Escape double quotes
            .replace(/'/g, "''")  // Escape single quotes
            .replace(/;/g, '')    // Remove semicolons
            .replace(/&/g, '')    // Remove ampersands
            .replace(/\|/g, '')   // Remove pipes
            .replace(/</g, '')    // Remove less than
            .replace(/>/g, '')    // Remove greater than
            .replace(/\n/g, '')   // Remove newlines
            .replace(/\r/g, '');   // Remove carriage returns
    }

    /**
     * Sanitize file path to prevent path traversal
     * @param {string} filePath - File path to sanitize
     * @returns {string} - Sanitized path
     */
    static sanitizeFilePath(filePath) {
        if (typeof filePath !== 'string') {
            return '';
        }

        // Remove path traversal sequences
        let sanitized = filePath
            .replace(/\.\./g, '')      // Remove ..
            .replace(/\/\//g, '/')      // Remove double slashes
            .replace(/\\\\/g, '\\')     // Remove double backslashes
            .replace(/\0/g, '');       // Remove null bytes

        // Normalize path separators (Windows)
        sanitized = sanitized.replace(/\//g, '\\');

        return sanitized;
    }

    /**
     * Sanitize URL to prevent XSS and injection
     * @param {string} url - URL to sanitize
     * @returns {string} - Sanitized URL or empty string if invalid
     */
    static sanitizeUrl(url) {
        if (typeof url !== 'string') {
            return '';
        }

        try {
            const parsed = new URL(url);
            // Only allow http and https protocols
            if (parsed.protocol !== 'http:' && parsed.protocol !== 'https:') {
                return '';
            }
            return parsed.toString();
        } catch (e) {
            return '';
        }
    }

    /**
     * Create a safe DOM element with text content
     * @param {string} tagName - HTML tag name
     * @param {string} textContent - Text content
     * @param {Object} attributes - HTML attributes (will be escaped)
     * @returns {HTMLElement} - Created element
     */
    static createSafeElement(tagName, textContent = '', attributes = {}) {
        const element = document.createElement(tagName);
        element.textContent = textContent;

        for (const [key, value] of Object.entries(attributes)) {
            element.setAttribute(key, this.escapeHtmlAttribute(String(value)));
        }

        return element;
    }
    };
}

// Also export for Node.js if needed
if (typeof module !== 'undefined' && module.exports) {
    module.exports = window.Sanitizer || class Sanitizer {};
}

