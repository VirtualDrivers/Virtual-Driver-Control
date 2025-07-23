using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using Microsoft.Win32;

namespace VDD_Control
{
    internal class XMLController
    {
        public int Count { get; set; }
        public string Friendlyname { get; set; }
        public List<string> G_refresh_rate { get; set; }
        public List<Resolution> Resolutions { get; set; }
        public bool CustomEdid { get; set; }
        public bool PreventSpoof { get; set; }
        public bool EdidCeaOverride { get; set; }
        public bool HardwareCursor { get; set; }
        public bool SDR10bit { get; set; }
        public bool HDRPlus { get; set; }
        public bool Logging { get; set; }
        public bool DebugLogging { get; set; }

        public class Resolution
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public double Refresh_rate { get; set; }
        }

        public XMLController(string FilePath)
        {
            // Handle null or empty file path by checking common locations
            if (string.IsNullOrEmpty(FilePath))
            {
                string[] commonLocations = 
                {
                    @"C:\VirtualDisplayDriver\vdd_settings.xml",
                    @"C:\IddSampleDriver\vdd_settings.xml",
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vdd_settings.xml")
                };
                
                foreach (string path in commonLocations)
                {
                    if (File.Exists(path))
                    {
                        FilePath = path;
                        break;
                    }
                }
                
                // If still null, default to the application directory
                if (string.IsNullOrEmpty(FilePath))
                {
                    FilePath = AppDomain.CurrentDomain.BaseDirectory;
                }
            }
            
            LoadFromXml(FilePath);
        }


        public void LoadFromXml(string filePath)
        {
            string xmlfile = filePath;
            
            // If the provided path doesn't end with the expected file name
            if (!string.IsNullOrEmpty(xmlfile) && !xmlfile.EndsWith("vdd_settings.xml", StringComparison.OrdinalIgnoreCase))
            {
                if (Directory.Exists(filePath))
                {
                    // It's a directory path, append the filename
                    xmlfile = Path.Combine(filePath, "vdd_settings.xml");
                }
                // Otherwise, assume it's a file path that just doesn't end with vdd_settings.xml
            }
            
            // Check if the file exists
            if (!File.Exists(xmlfile))
            {
                // Check fallback locations
                string[] fallbackPaths =
                {
                    @"C:\VirtualDisplayDriver\vdd_settings.xml",
                    @"C:\IddSampleDriver\vdd_settings.xml",
                    // Try the XML file in the project root directory
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vdd_settings.xml")
                };

                foreach (string path in fallbackPaths)
                {
                    if (File.Exists(path))
                    {
                        xmlfile = path;
                        break;
                    }
                }

                // If still not found after checking fallbacks, throw exception
                if (!File.Exists(xmlfile))
                {
                    throw new FileNotFoundException("XML file not found at specified path or fallback locations", xmlfile);
                }
            }

            // Log the file path we're trying to load
            Console.WriteLine($"[DEBUG] Attempting to load XML from: {xmlfile}");
            
            string xmlContent;
            using (StreamReader reader = new StreamReader(xmlfile))
            {
                xmlContent = reader.ReadToEnd();
            }
            
            // Log successful file read
            Console.WriteLine($"[DEBUG] Successfully read XML content, length: {xmlContent.Length}");

            // Create XML document with secure settings
            XmlDocument xmlDoc = new XmlDocument();
            
            // Create secure XML reader settings to prevent XXE attacks
            XmlReaderSettings secureSettings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,  // Prohibit DTD processing
                ValidationType = ValidationType.None,    // No validation
                XmlResolver = null,                      // No resolution of external entities
                MaxCharactersFromEntities = 1024,        // Limit entity expansion
                MaxCharactersInDocument = 1024 * 1024    // Limit document size to 1MB
            };
            
            try
            {
                // Use XmlReader with secure settings
                using (StringReader stringReader = new StringReader(xmlContent))
                using (XmlReader secureReader = XmlReader.Create(stringReader, secureSettings))
                {
                    xmlDoc.Load(secureReader);
                    Console.WriteLine("[DEBUG] Successfully parsed XML document securely");
                }
            }
            catch (XmlException xmlEx)
            {
                Console.WriteLine($"[ERROR] XML parsing error: {xmlEx.Message}");
                throw new XmlException($"The XML file appears to be malformed: {xmlEx.Message}", xmlEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to parse XML: {ex.Message}");
                throw; // Re-throw to maintain original behavior
            }

            XmlNode countNode = xmlDoc.SelectSingleNode("//monitors/count");
            if (countNode != null)
            {
                if (!int.TryParse(countNode.InnerText, out int countValue))
                {
                    Console.WriteLine($"[WARNING] Invalid monitor count value: {countNode.InnerText}. Defaulting to 1.");
                    Count = 1; // Default value
                }
                else
                {
                    Count = countValue;
                }
            }

            XmlNode friendlynameNode = xmlDoc.SelectSingleNode("//gpu/friendlyname");
            if (friendlynameNode != null)
                Friendlyname = friendlynameNode.InnerText;

            G_refresh_rate = new List<string>();
            XmlNodeList refreshRates = xmlDoc.SelectNodes("//global/g_refresh_rate");
            foreach (XmlNode rateNode in refreshRates)
            {
                G_refresh_rate.Add(rateNode.InnerText);
            }

            Resolutions = new List<Resolution>();
            XmlNodeList resolutionNodes = xmlDoc.SelectNodes("//resolutions/resolution");
            foreach (XmlNode resNode in resolutionNodes)
            {
                try
                {
                    XmlNode widthNode = resNode.SelectSingleNode("width");
                    XmlNode heightNode = resNode.SelectSingleNode("height");
                    XmlNode refreshNode = resNode.SelectSingleNode("refresh_rate");
                    
                    // Check if nodes exist
                    if (widthNode == null || heightNode == null || refreshNode == null)
                    {
                        Console.WriteLine("[WARNING] Skipping resolution node with missing width, height, or refresh_rate");
                        continue;
                    }
                    
                    // Use TryParse for safer parsing
                    bool validWidth = int.TryParse(widthNode.InnerText, out int width);
                    bool validHeight = int.TryParse(heightNode.InnerText, out int height);
                    bool validRefresh = double.TryParse(refreshNode.InnerText, out double refreshRate);
                    
                    // Skip invalid values
                    if (!validWidth || !validHeight || !validRefresh)
                    {
                        Console.WriteLine($"[WARNING] Skipping resolution with invalid values: Width={widthNode.InnerText}, Height={heightNode.InnerText}, RefreshRate={refreshNode.InnerText}");
                        continue;
                    }
                    
                    // Create and add valid resolution
                    Resolution res = new Resolution
                    {
                        Width = width,
                        Height = height,
                        Refresh_rate = refreshRate
                    };
                    Resolutions.Add(res);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[WARNING] Error parsing resolution: {ex.Message}");
                    // Continue to the next resolution node
                }
            }

            bool tempValue;
            ParseBooleanOption(xmlDoc, "options/CustomEdid", out tempValue);
            this.CustomEdid = tempValue;

            ParseBooleanOption(xmlDoc, "options/PreventSpoof", out tempValue);
            this.PreventSpoof = tempValue;

            ParseBooleanOption(xmlDoc, "options/EdidCeaOverride", out tempValue);
            this.EdidCeaOverride = tempValue;

            ParseBooleanOption(xmlDoc, "options/HardwareCursor", out tempValue);
            this.HardwareCursor = tempValue;

            ParseBooleanOption(xmlDoc, "options/SDR10bit", out tempValue);
            this.SDR10bit = tempValue;

            ParseBooleanOption(xmlDoc, "options/HDRPlus", out tempValue);
            this.HDRPlus = tempValue;

            ParseBooleanOption(xmlDoc, "options/logging", out tempValue);
            this.Logging = tempValue;

            ParseBooleanOption(xmlDoc, "options/debuglogging", out tempValue);
            this.DebugLogging = tempValue;

        }

        private void ParseBooleanOption(XmlDocument doc, string xpath, out bool value)
        {
            // Add the '//' prefix if it's missing
            string fullXpath = xpath.StartsWith("//") ? xpath : "//" + xpath;

            XmlNode node = doc.SelectSingleNode(fullXpath);
            if (node != null)
            {
                Console.WriteLine($"[DEBUG] Found node: {xpath} = {node.InnerText}");

                string nodeValue = node.InnerText.Trim();

                // Case-insensitive parsing of boolean values
                if (nodeValue.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    value = true;
                }
                else if (nodeValue.Equals("false", StringComparison.OrdinalIgnoreCase))
                {
                    value = false;
                }
                else
                {
                    // If the value is not "true" or "false", default to false
                    value = false;
                    Console.WriteLine($"[WARNING] Invalid boolean value for {xpath}: '{nodeValue}'. Defaulting to false.");
                }
            }
            else
            {
                value = false; // Default to false if node not found
                Console.WriteLine($"[DEBUG] Node not found: {fullXpath}");
            }
        }

        public List<string> GetResolutionsForDataGrid()
        {
            List<string> resolutionStrings = new List<string>();
            foreach (var resolution in Resolutions)
            {
                resolutionStrings.Add($"{resolution.Width},{resolution.Height},{resolution.Refresh_rate}");
            }
            return resolutionStrings;
        }


        public void SaveToXml(string filePath)
        {
            try
            {
                // Create secure XmlWriterSettings
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true,                 // Makes the output more readable
                    IndentChars = "  ",            // Two spaces for indentation
                    NewLineHandling = NewLineHandling.Replace,
                    Encoding = Encoding.UTF8,      // Use UTF-8 encoding
                    CheckCharacters = true         // Check for invalid XML characters
                };
                
                // Create a new XmlDocument
                XmlDocument doc = new XmlDocument();
                XmlElement root = doc.CreateElement("vdd_settings");
                doc.AppendChild(root);

            // Monitors
            XmlElement monitors = doc.CreateElement("monitors");
            XmlElement count = doc.CreateElement("count");
            count.InnerText = Count.ToString();
            monitors.AppendChild(count);
            root.AppendChild(monitors);

            // GPU
            XmlElement gpu = doc.CreateElement("gpu");
            XmlElement friendlyname = doc.CreateElement("friendlyname");
            friendlyname.InnerText = Friendlyname;
            gpu.AppendChild(friendlyname);
            root.AppendChild(gpu);

            // Global
            XmlElement global = doc.CreateElement("global");
            foreach (var rate in G_refresh_rate)
            {
                XmlElement rateElement = doc.CreateElement("g_refresh_rate");
                rateElement.InnerText = rate;
                global.AppendChild(rateElement);
            }
            root.AppendChild(global);

            // Resolutions
            XmlElement resolutions = doc.CreateElement("resolutions");
            foreach (var resolution in Resolutions)
            {
                XmlElement resolutionElement = doc.CreateElement("resolution");
                XmlElement width = doc.CreateElement("width");
                width.InnerText = resolution.Width.ToString();
                resolutionElement.AppendChild(width);

                XmlElement height = doc.CreateElement("height");
                height.InnerText = resolution.Height.ToString();
                resolutionElement.AppendChild(height);

                XmlElement refreshRate = doc.CreateElement("refresh_rate");
                refreshRate.InnerText = resolution.Refresh_rate.ToString();
                resolutionElement.AppendChild(refreshRate);

                resolutions.AppendChild(resolutionElement);
            }
            root.AppendChild(resolutions);

            // Options
            XmlElement options = doc.CreateElement("options");
            AddOptionElement(doc, options, "CustomEdid", CustomEdid);
            AddOptionElement(doc, options, "PreventSpoof", PreventSpoof);
            AddOptionElement(doc, options, "EdidCeaOverride", EdidCeaOverride);
            AddOptionElement(doc, options, "HardwareCursor", HardwareCursor);
            AddOptionElement(doc, options, "SDR10bit", SDR10bit);
            AddOptionElement(doc, options, "HDRPlus", HDRPlus);
            AddOptionElement(doc, options, "logging", Logging);
            AddOptionElement(doc, options, "debuglogging", DebugLogging);
            root.AppendChild(options);

                // Save the document using XmlWriter for security
                using (XmlWriter writer = XmlWriter.Create(filePath, settings))
                {
                    doc.WriteTo(writer);
                }
            }
            catch (Exception ex)
            {
                // Log and rethrow
                Console.WriteLine($"[ERROR] Failed to save XML file: {ex.Message}");
                throw;
            }
        }

        private void AddOptionElement(XmlDocument doc, XmlElement parent, string name, bool value)
        {
            XmlElement element = doc.CreateElement(name);
            element.InnerText = value.ToString().ToLower();
            parent.AppendChild(element);
        }

        // Static methods for managing application settings in the registry
        private const string RegistryKeyPath = @"SOFTWARE\MikeTheTech\VirtualDisplayDriverControl";

        public static bool GetDontShowDriverInstallPrompt()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, false))
                {
                    if (key != null)
                    {
                        object value = key.GetValue("DontShowDriverInstallPrompt");
                        if (value != null && bool.TryParse(value.ToString(), out bool result))
                        {
                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to read registry setting: {ex.Message}");
            }
            return false; // Default to showing the prompt
        }

        public static void SetDontShowDriverInstallPrompt(bool dontShow)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(RegistryKeyPath, true))
                {
                    if (key != null)
                    {
                        key.SetValue("DontShowDriverInstallPrompt", dontShow.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to write registry setting: {ex.Message}");
            }
        }

    }
}
