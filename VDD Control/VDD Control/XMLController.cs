using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

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

            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml(xmlContent);
                Console.WriteLine("[DEBUG] Successfully parsed XML document");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to parse XML: {ex.Message}");
                throw; // Re-throw to maintain original behavior
            }

            XmlNode countNode = xmlDoc.SelectSingleNode("//monitors/count");
            if (countNode != null)
                Count = int.Parse(countNode.InnerText);

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
                Resolution res = new Resolution
                {
                    Width = int.Parse(resNode.SelectSingleNode("width").InnerText),
                    Height = int.Parse(resNode.SelectSingleNode("height").InnerText),
                    Refresh_rate = double.Parse(resNode.SelectSingleNode("refresh_rate").InnerText)
                };
                Resolutions.Add(res);
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

            doc.Save(filePath);
        }

        private void AddOptionElement(XmlDocument doc, XmlElement parent, string name, bool value)
        {
            XmlElement element = doc.CreateElement(name);
            element.InnerText = value.ToString().ToLower();
            parent.AppendChild(element);
        }


    }
}
