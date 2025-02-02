import tkinter as tk
from tkinter import scrolledtext, messagebox
import os
import time
import threading
import win32file, win32pipe

PIPE_NAME = r"\\.\pipe\MTTVirtualDisplayPipe"

class PipeClientGUI:
    def __init__(self, root):
        self.root = root
        self.root.title("Virtual Display Driver Control")
        
        self.frame = tk.Frame(root)
        self.frame.pack(padx=10, pady=10)
        
        self.label = tk.Label(self.frame, text="Command:")
        self.label.grid(row=0, column=0, sticky="w")
        
        self.command_entry = tk.Entry(self.frame, width=40)
        self.command_entry.grid(row=0, column=1, padx=5, pady=5)
        
        self.send_button = tk.Button(self.frame, text="Send Command", command=self.send_command)
        self.send_button.grid(row=0, column=2, padx=5, pady=5)
        
        self.reconnect_button = tk.Button(self.frame, text="Reconnect", command=self.reconnect_pipe)
        self.reconnect_button.grid(row=0, column=3, padx=5, pady=5)
        
        self.log = scrolledtext.ScrolledText(self.frame, width=60, height=15)
        self.log.grid(row=1, column=0, columnspan=4, pady=10)
        
        self.pipe = None
        self.connect_pipe()
        
    def connect_pipe(self):
        """Attempts to connect to the named pipe."""
        try:
            self.pipe = win32file.CreateFile(
                PIPE_NAME,
                win32file.GENERIC_READ | win32file.GENERIC_WRITE,
                0,
                None,
                win32file.OPEN_EXISTING,
                0,
                None
            )
            self.log.insert(tk.END, "Connected to the Virtual Display Driver pipe.\n")
        except Exception as e:
            self.log.insert(tk.END, f"Failed to connect to pipe: {e}\n")
            self.pipe = None
    
    def reconnect_pipe(self):
        """Reconnects to the named pipe."""
        self.log.insert(tk.END, "Attempting to reconnect...\n")
        self.connect_pipe()
        
    def send_command(self):
        """Sends a command to the driver."""
        if not self.pipe:
            messagebox.showerror("Error", "Not connected to pipe.")
            return
        
        command = self.command_entry.get().strip()
        if not command:
            messagebox.showwarning("Warning", "Command cannot be empty.")
            return
        
        try:
            win32file.WriteFile(self.pipe, command.encode('utf-16le'))
            self.log.insert(tk.END, f"Sent: {command}\n")
            self.receive_response()
        except Exception as e:
            self.log.insert(tk.END, f"Error sending command: {e}\n")
    
    def receive_response(self):
        """Reads and displays response from the driver."""
        try:
            _, data = win32file.ReadFile(self.pipe, 512)
            response = data.decode('utf-16le').strip()
            self.log.insert(tk.END, f"Response: {response}\n")
        except Exception as e:
            self.log.insert(tk.END, f"Error reading response: {e}\n")

if __name__ == "__main__":
    root = tk.Tk()
    app = PipeClientGUI(root)
    root.mainloop()
