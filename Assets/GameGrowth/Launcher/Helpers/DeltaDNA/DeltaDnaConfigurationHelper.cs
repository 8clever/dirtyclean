using System.IO;
using System.Xml;
using System.Xml.Serialization;
using DeltaDNA;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.GameGrowth
{
    public static class DeltaDNAConfigurationHelper
    {
        const string k_ConfigurationFilePath = "Assets/DeltaDNA/Resources/ddna_configuration.xml";
        const string k_ConfigurationRootName = "configuration";
        
        static readonly XmlSerializer analyticsSerialiser = new XmlSerializer(
            typeof(DeltaDNA.Configuration),
            new XmlRootAttribute(k_ConfigurationRootName));

        public static void UpdateEnvironment(bool isDevelopment)
        {
            DeltaDNA.Configuration ddnaConfiguration = LoadAnalyticsConfig();
            ddnaConfiguration.environmentKey = isDevelopment ? 0 : 1;
            SaveAnalyticsConfig(ddnaConfiguration);
        }
        
        public static bool IsInDevelopmentMode()
        {
            DeltaDNA.Configuration ddnaConfiguration = LoadAnalyticsConfig();
            return ddnaConfiguration.environmentKey == 0;
        }

        private static DeltaDNA.Configuration LoadAnalyticsConfig() {
            if (File.Exists(k_ConfigurationFilePath)) {
                using (var stringReader = new StringReader(File.ReadAllText(k_ConfigurationFilePath))) {
                    using (var xmlReader = XmlReader.Create(stringReader)) {
                        return analyticsSerialiser.Deserialize(xmlReader) as DeltaDNA.Configuration;
                    }
                }
            } 
            
            return new DeltaDNA.Configuration();
        }
        
        private static void SaveAnalyticsConfig(DeltaDNA.Configuration analytics) {
            using (var stringWriter = new StringWriter()) {
                using (XmlWriter xmlWriter = XmlWriter.Create(
                    stringWriter, new XmlWriterSettings() { Indent = true })) {
                    analyticsSerialiser.Serialize(xmlWriter, analytics);
                    File.WriteAllText(k_ConfigurationFilePath, stringWriter.ToString());
#if UNITY_EDITOR
                    AssetDatabase.Refresh();
#endif
                }
            }
        }
    }
}