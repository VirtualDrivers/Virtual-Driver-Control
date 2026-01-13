/**
 * Safe DOM manipulation utilities
 * Provides secure alternatives to innerHTML
 */

// Make available globally for browser context
if (typeof window !== 'undefined') {
    window.DOMUtils = class DOMUtils {
    /**
     * Safely set text content of an element
     * @param {HTMLElement|string} element - DOM element or selector
     * @param {string} text - Text content
     */
    static setText(element, text) {
        const el = typeof element === 'string' ? document.querySelector(element) : element;
        if (el) {
            el.textContent = text || '';
        }
    }

    /**
     * Safely clear element content
     * @param {HTMLElement|string} element - DOM element or selector
     */
    static clear(element) {
        const el = typeof element === 'string' ? document.querySelector(element) : element;
        if (el) {
            el.textContent = '';
            // Remove all child nodes
            while (el.firstChild) {
                el.removeChild(el.firstChild);
            }
        }
    }

    /**
     * Safely append a text node
     * @param {HTMLElement|string} element - DOM element or selector
     * @param {string} text - Text content
     */
    static appendText(element, text) {
        const el = typeof element === 'string' ? document.querySelector(element) : element;
        if (el && text) {
            el.appendChild(document.createTextNode(text));
        }
    }

    /**
     * Safely create and append an element
     * @param {HTMLElement|string} parent - Parent element or selector
     * @param {string} tagName - HTML tag name
     * @param {Object} options - Element options
     * @returns {HTMLElement} - Created element
     */
    static createAndAppend(parent, tagName, options = {}) {
        const parentEl = typeof parent === 'string' ? document.querySelector(parent) : parent;
        if (!parentEl) return null;

        const element = document.createElement(tagName);
        
        if (options.text) {
            element.textContent = options.text;
        }
        
        if (options.className) {
            element.className = options.className;
        }
        
        if (options.id) {
            element.id = options.id;
        }
        
        if (options.attributes) {
            for (const [key, value] of Object.entries(options.attributes)) {
                element.setAttribute(key, value);
            }
        }
        
        if (options.children) {
            options.children.forEach(child => {
                if (typeof child === 'string') {
                    element.appendChild(document.createTextNode(child));
                } else if (child instanceof Node) {
                    element.appendChild(child);
                }
            });
        }
        
        parentEl.appendChild(element);
        return element;
    }

    /**
     * Safely set HTML content (with sanitization)
     * WARNING: Only use when absolutely necessary, prefer DOM manipulation
     * @param {HTMLElement|string} element - DOM element or selector
     * @param {string} html - HTML content (will be sanitized)
     */
    static setHTML(element, html) {
        const el = typeof element === 'string' ? document.querySelector(element) : element;
        if (!el) return;
        
        // For now, escape all HTML. In production, use DOMPurify for selective sanitization
        if (window.Sanitizer) {
            el.innerHTML = window.Sanitizer.sanitizeHtml(html);
        } else {
            // Fallback: escape HTML
            const div = document.createElement('div');
            div.textContent = html;
            el.innerHTML = div.innerHTML;
        }
    }

    /**
     * Safely create element from HTML string (with sanitization)
     * @param {string} html - HTML string
     * @returns {HTMLElement|null} - Created element or null
     */
    static createFromHTML(html) {
        let sanitized = html;
        if (window.Sanitizer) {
            sanitized = window.Sanitizer.sanitizeHtml(html);
        } else {
            // Fallback: escape HTML
            const div = document.createElement('div');
            div.textContent = html;
            sanitized = div.innerHTML;
        }
        
        const temp = document.createElement('div');
        temp.innerHTML = sanitized;
        
        return temp.firstElementChild;
    }
    };
}

// Also export for Node.js if needed
if (typeof module !== 'undefined' && module.exports) {
    module.exports = window.DOMUtils || class DOMUtils {};
}

