using System.IO;
using System.Xml;
using System.Xml.Serialization;
#if GAMEGROWTH_UNITY_MONETIZATION
using UnityEditor.GameGrowth.Helpers.UnityMediation;
#endif
using UnityEngine;
using UnityEngine.GameGrowth;
#if GAMEGROWTH_FACEBOOK_SDK
using Facebook.Unity.Settings;
#endif

namespace UnityEditor.GameGrowth
{
    public static class GameGrowthAutomaticConfiguration
    {
        const string k_DeltaDnaConfigPath = "Assets/DeltaDNA/Resources/ddna_configuration.xml";
        
        static readonly string k_CannotAutoConfigureLog = L10n.Tr("Cannot auto-configure Game Growth Sdks.");
        
        [InitializeOnLoadMethod]
        static void InitializeConfiguration()
        {
            GameGrowthConfigurationAsset.onConfigurationUpdated -= InitializationCallback;
            GameGrowthConfigurationAsset.onConfigurationUpdated += InitializationCallback;
        }

        static void InitializationCallback(GameGrowthConfigurationAsset gameGrowthConfiguration)
        {
            ConfigureSdks(gameGrowthConfiguration);
        }
        
        public static void ConfigureSdks(GameGrowthConfigurationAsset gameGrowthConfiguration)
        {
            if (gameGrowthConfiguration == null)
            {
                Debug.LogWarning(k_CannotAutoConfigureLog);
                return;
            }

            var isFullIntegration = IntegrationLevelExtensions.IsFull(IntegrationLevelExtensions.GetParsedIntegrationLevel(gameGrowthConfiguration.projectSummary.integrationLevel));
            ApplyDeltaDnaConfiguration(gameGrowthConfiguration);

            if (isFullIntegration)
            {
#if GAMEGROWTH_ADMOB
                ApplyAdMobConfiguration(gameGrowthConfiguration);
#endif

#if GAMEGROWTH_UNITY_MONETIZATION
                EnableUnityMonetizationRequiredAdapters();
                ApplyAdMobConfigurationForUnityMonetization(gameGrowthConfiguration);
#endif

#if GAMEGROWTH_FACEBOOK_SDK
                ApplyFacebookConfiguration(gameGrowthConfiguration);
#endif
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        static void ApplyDeltaDnaConfiguration(GameGrowthConfigurationAsset gameGrowthConfiguration)
        {
            DeltaDNA.Configuration config = null;
            
            var ddnaConfigSerializer = new XmlSerializer(
                typeof(DeltaDNA.Configuration),
                new XmlRootAttribute("configuration"));

            if (File.Exists(k_DeltaDnaConfigPath))
            {
                using (var stringReader = new StringReader(File.ReadAllText(k_DeltaDnaConfigPath)))
                {
                    using (var xmlReader = XmlReader.Create(stringReader))
                    {
                        config = ddnaConfigSerializer.Deserialize(xmlReader) as DeltaDNA.Configuration;
                    }
                }
            }
            else
            {
                config = new DeltaDNA.Configuration();
            }

            config.environmentKeyDev = gameGrowthConfiguration.projectSummary.providers.ddna.devKey.value;
            config.environmentKeyLive = gameGrowthConfiguration.projectSummary.providers.ddna.liveKey.value;
            config.collectUrl = gameGrowthConfiguration.projectSummary.providers.ddna.collect;
            config.engageUrl = gameGrowthConfiguration.projectSummary.providers.ddna.engage;

            using (var stringWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings() {Indent = true}))
                {
                    ddnaConfigSerializer.Serialize(xmlWriter, config);
                    File.WriteAllText(k_DeltaDnaConfigPath, stringWriter.ToString());
                }
            }
        }

#if GAMEGROWTH_ADMOB
        static void ApplyAdMobConfiguration(GameGrowthConfigurationAsset gameGrowthConfiguration)
        {
            var config = AdMobConfiguration.LoadMainAsset();
            if (config != null)
            {
                config.androidAppId = gameGrowthConfiguration.projectSummary.providers.admob.appId.androidValue;
                config.iOSAppId = gameGrowthConfiguration.projectSummary.providers.admob.appId.iOSValue;

                EditorUtility.SetDirty(config);
            }
        }
#endif

#if GAMEGROWTH_UNITY_MONETIZATION
        static void EnableUnityMonetizationRequiredAdapters()
        {
            UnityMonetizationAdapterHelper.EnableRequiredAdNetworks();
        }

        static void ApplyAdMobConfigurationForUnityMonetization(GameGrowthConfigurationAsset gameGrowthConfiguration)
        {
            AdMobConfigurationHelper.SetAdMobConfigurationForUnityMonetization(
                gameGrowthConfiguration.projectSummary.providers.admob.appId.androidValue,
                gameGrowthConfiguration.projectSummary.providers.admob.appId.iOSValue);
        }
#endif        
        
#if GAMEGROWTH_FACEBOOK_SDK
        static void ApplyFacebookConfiguration(GameGrowthConfigurationAsset gameGrowthConfiguration)
        {
            if (FacebookSettings.AppIds != null && FacebookSettings.AppIds.Count > 0)
            {
                FacebookSettings.AppIds[0] = gameGrowthConfiguration.projectSummary.providers.facebook.appId;
            }
            
            EditorUtility.SetDirty(FacebookSettings.Instance);
        }
#endif
    }
}