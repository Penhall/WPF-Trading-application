using CommonFrontEnd.Global;
using CommonFrontEnd.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CommonFrontEnd.SharedMemories
{

    public static class SaveConfiguration
    {
        static string currentDir = Environment.CurrentDirectory;

        public static void SaveConfigurationAll(string Key)
        {
            //DirectoryInfo directory = new DirectoryInfo(
            //     Path.GetFullPath(Path.Combine(currentDir, @"UserSettings/Bolt1.xml")));
            //// string boltPath = @"E:\TWS_DotNetNewStructure\TWS_DOTNETT\CommonFrontEnd\xml\Bolt1.xml";
            //SaveConfigurationAll(CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict, directory.ToString(), "BoltConfig");

            DirectoryInfo appDirectory = new DirectoryInfo(Path.GetFullPath(Path.Combine(currentDir, @"xml/Users/AppSettings/AAAAAA.xml")));
            DirectoryInfo userDirectory = new DirectoryInfo(Path.GetFullPath(Path.Combine(currentDir, @"xml/Users/UserSettings/UUUUUU.xml")));

            switch (Key)
            {
                case "Bolt":
                    SaveConfigurationAll<BoltUserSettingsBolt>(ConfigurationMasterMemory.ConfigurationDict, userDirectory.ToString().Replace("UUUUUU", UtilityLoginDetails.GETInstance.FileName), "Bolt");
                    break;
                case "TWS":
                    SaveConfigurationAll<BoltUserSettingsTWSSettings>(ConfigurationMasterMemory.ConfigurationDict, userDirectory.ToString().Replace("UUUUUU", UtilityLoginDetails.GETInstance.FileName), "TWS");
                    break;
                case "WindowsPosition":
                    SaveConfigurationAll<BoltAppSettingsWindowsPosition>(ConfigurationMasterMemory.ConfigurationDict, appDirectory.ToString().Replace("AAAAAA", UtilityLoginDetails.GETInstance.FileName), "WindowsPosition");
                    break;
                default:
                    break;
            }
        }

        public static void SaveConfigurationAll(string[] Keys)
        {
            //DirectoryInfo directory = new DirectoryInfo(
            //     Path.GetFullPath(Path.Combine(currentDir, @"UserSettings/Bolt1.xml")));
            //// string boltPath = @"E:\TWS_DotNetNewStructure\TWS_DOTNETT\CommonFrontEnd\xml\Bolt1.xml";
            //SaveConfigurationAll(CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict, directory.ToString(), "BoltConfig");

            DirectoryInfo appDirectory = new DirectoryInfo(Path.GetFullPath(Path.Combine(currentDir, @"xml/Users/AppSettings/AAAAAA.xml")));
            DirectoryInfo userDirectory = new DirectoryInfo(Path.GetFullPath(Path.Combine(currentDir, @"xml/Users/UserSettings/UUUUUU.xml")));
            if (Keys != null && Keys.Length > 0)
            {
                foreach (string key in Keys)
                {
                    switch (key)
                    {
                        case "Bolt":
                            SaveConfigurationAll<BoltUserSettingsBolt>(ConfigurationMasterMemory.ConfigurationDict, userDirectory.ToString().Replace("UUUUUU", UtilityLoginDetails.GETInstance.FileName), "Bolt");
                            break;
                        case "TWS":
                            SaveConfigurationAll<BoltUserSettingsTWSSettings>(ConfigurationMasterMemory.ConfigurationDict, userDirectory.ToString().Replace("UUUUUU", UtilityLoginDetails.GETInstance.FileName), "TWS");
                            break;
                        case "WindowsPosition":
                            SaveConfigurationAll<BoltAppSettingsWindowsPosition>(ConfigurationMasterMemory.ConfigurationDict, appDirectory.ToString().Replace("AAAAAA", UtilityLoginDetails.GETInstance.FileName), "WindowsPosition");
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Serializes and save the xml document at provided path
        /// </summary>
        /// <typeparam name="T">The Type of parameter at runtime</typeparam>
        /// <param name="objects">Type parameter</param>
        /// <param name="xmlPath">Save file path</param>
        /// <param name="key">Provide key parameter to filter</param>
        public static void SaveConfigurationAll<T>(ConcurrentDictionary<string, T> objects, string xmlPath, string key)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextWriter writer = new StreamWriter(xmlPath))
            {
                serializer.Serialize(writer, objects.Where(x => x.Key == key).Select(x => x.Value).FirstOrDefault());
            }
        }

        /// <summary>
        /// Serializes and save the xml document at provided path
        /// </summary>
        /// <typeparam name="T">The Type of parameter at runtime</typeparam>
        /// <param name="objects">Type parameter</param>
        /// <param name="xmlPath">Save file path</param>
        /// <param name="key">Provide key parameter to filter</param>
        public static void SaveConfigurationAll<T>(ConcurrentDictionary<string, object> objects, string xmlPath, string key)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextWriter writer = new StreamWriter(xmlPath))
            {
                serializer.Serialize(writer, objects.Where(x => x.Key == key).Select(x => x.Value).FirstOrDefault());
            }
        }

        public static void SaveUserConfiguration(string xmlPath, string key)
        {
            if (xmlPath != null)
            {
                XDocument _XDocument = XDocument.Load(xmlPath);
                XmlSerializer ser = null;
                XDocument doc = new XDocument();
                XElement el;
                switch (key)
                {
                    case "Bolt":
                        BoltUserSettingsBolt oBoltUserSettings = new BoltUserSettingsBolt();
                        oBoltUserSettings = (BoltUserSettingsBolt)ConfigurationMasterMemory.ConfigurationDict[key];
                        ser = new XmlSerializer(typeof(BoltUserSettingsBolt));

                        using (XmlWriter xw = doc.CreateWriter())
                        {
                            ser.Serialize(xw, oBoltUserSettings);
                            xw.Close();
                        }
                        doc.Root.Name = "Bolt";

                        el = doc.Root;
                        _XDocument.Element("BoltUserSettings").Element("Bolt").ReplaceWith(el);

                        break;

                    case "WindowsPosition":
                        BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = new BoltAppSettingsWindowsPosition();
                        oBoltAppSettingsWindowsPosition = (BoltAppSettingsWindowsPosition)ConfigurationMasterMemory.ConfigurationDict[key];
                        ser = new XmlSerializer(typeof(BoltAppSettingsWindowsPosition));

                        using (XmlWriter xw = doc.CreateWriter())
                        {
                            ser.Serialize(xw, oBoltAppSettingsWindowsPosition);
                            xw.Close();
                        }
                        doc.Root.Name = "WindowsPosition";

                        el = doc.Root;
                        _XDocument.Element("BoltAppSettings").Element("WindowsPosition").ReplaceWith(el);
                        break;

                    case "TWS":

                        BoltUserSettingsTWSSettings oBoltUserSettingsTWSSettings = new BoltUserSettingsTWSSettings();
                        oBoltUserSettingsTWSSettings = (BoltUserSettingsTWSSettings)ConfigurationMasterMemory.ConfigurationDict[key];
                        ser = new XmlSerializer(typeof(BoltUserSettingsTWSSettings));

                        using (XmlWriter xw = doc.CreateWriter())
                        {
                            ser.Serialize(xw, oBoltUserSettingsTWSSettings);
                            xw.Close();
                        }
                        doc.Root.Name = "TWSSettings";

                        el = doc.Root;
                        _XDocument.Element("BoltUserSettings").Element("TWSSettings").ReplaceWith(el);
                        break;


                    default:

                        break;
                }

                _XDocument.Save(xmlPath);
            }
        }

    }

}
