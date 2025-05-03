# Virtual Driver Control

A C# Windows Forms application for controlling and configuring the Virtual Display Driver by MikeTheTech.

![Screenshot](https://github.com/user-attachments/assets/5c915402-dece-49e5-acca-abf4e77cc7f8)

## Features

- Manage virtual display driver settings through XML configuration and direct pipeline functions
- Toggle specialized display features (HDR, SDR 10-bit, custom EDID, etc.)
- System tray integration for quick access to common functions

## Requirements

- Windows OS
- .NET 6.0 Runtime
- Compatible virtual display driver installed

## Instructions

1. Download the Built file(s) from the Releases page.
2. Extract
3. Run

## Building

1. Clone the repository
2. Open `VDD Control/VDD Control.sln` in Visual Studio
3. Build and run the solution

Alternatively, use the command line:

```
dotnet build "VDD Control/VDD Control.sln"
dotnet run --project "VDD Control/VDD Control/VDD Control.csproj"
```

## Configuration

The application searches for the configuration file `vdd_settings.xml` in:
- `C:\VirtualDisplayDriver\vdd_settings.xml`
