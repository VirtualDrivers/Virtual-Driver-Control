# Virtual Driver Control

WinUI3-style control panel for Virtual Display Driver.

## Prerequisites

- **Node.js** (v14 or higher recommended)
- **Windows** (Windows 10/11) - This application is Windows-specific
- **Administrator privileges** - The application will automatically request elevation when needed

## Installation

1. Install dependencies:
```bash
npm install
```

## Running the Application

### Development Mode

To run the application in development mode:

```bash
npm start
```

Or with the dev flag:

```bash
npm run dev
```

**Note:** The application will automatically request administrator privileges when launched, as it needs elevated permissions to manage virtual display drivers.

### Building the Application

To build a portable Windows executable:

```bash
npm run build-portable
```

To build for distribution:

```bash
npm run build
```

Built files will be in the `dist` directory.

## Project Structure

- `main.js` - Electron main process (handles window creation and IPC)
- `app.js` - Renderer process logic (UI interactions and driver management)
- `index.html` - Application UI
- `styles.css` - Application styling
- `package.json` - Project configuration and dependencies

## Features

- Configure virtual display driver settings
- Manage display resolutions and refresh rates
- HDR and color configuration
- EDID configuration and analysis
- Driver status monitoring
- Logging and debug tools

## Important Notes

- **Administrator Rights Required**: This application requires administrator privileges to manage Windows drivers
- **Windows Only**: This application is designed specifically for Windows and uses Windows-specific APIs
- **Driver Installation**: Ensure the Virtual Display Driver is installed before using this control panel

