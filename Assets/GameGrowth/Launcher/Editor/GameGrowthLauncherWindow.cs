using DeltaDNA.Editor;
using UnityEditor;

#if GAMEGROWTH_FACEBOOK_SDK
    using Facebook.Unity.Editor;
    using Facebook.Unity.Settings;
#endif

namespace UnityEngine.GameGrowth
{
    [CustomEditor(typeof(GameGrowthLauncher))]
    public class GameGrowthWindow : UnityEditor.Editor
    {
        const string k_CollectIdsMsg = "If you decide to launch Adjust or delta DNA manually, do not forget to call" +
            " method \"GameGrowthLauncher.CollectIds();\" when both Adjust and delta DNA are started.";

        GameGrowthLauncher m_Launcher;

        SerializedProperty m_DefaultConfiguration;
        GUIContent m_DefaultConfigurationContent = new GUIContent(L10n.Tr("Default Configuration"));
        
        string m_GameGrowthEnvironmentWarning;
        GameGrowthConfiguration m_Configuration;
        GUIContent m_EnvironmentLabel = new GUIContent(L10n.Tr("Environment"));

        bool m_AdjustFold;
        GUIContent m_AdjustLabel = new GUIContent(L10n.Tr("Adjust"));

        SerializedProperty m_StartAdjustManually;
        GUIContent m_StartAdjustManuallyLabel = new GUIContent(L10n.Tr("Start Manually"));

        SerializedProperty m_AdjustAndroidAppToken;
        GUIContent m_AdjustAndroidAppTokenLabel = new GUIContent(L10n.Tr("Android App Token"));
        
        SerializedProperty m_AdjustEventBuffering;
        GUIContent m_AdjustEventBufferingLabel = new GUIContent(L10n.Tr("Event Buffering"));

        SerializedProperty m_AdjustSendInBackground;
        GUIContent m_AdjustSendInBackgroundLabel = new GUIContent(L10n.Tr("Send In Background"));
        
        SerializedProperty m_AdjustLaunchDeferredDeeplink;
        GUIContent m_AdjustLaunchDeferredDeeplinkLabel = new GUIContent(L10n.Tr("Launch Deferred Deeplink"));
        
        SerializedProperty m_AdjustAndroidPurchaseEventToken;
        GUIContent m_AdjustAndroidPurchaseEventTokenLabel = new GUIContent(L10n.Tr("Android Purchase Event Token"));

        SerializedProperty m_AdjustOverrideTokens;
        GUIContent m_AdjustOverrideTokensLabel = new GUIContent(L10n.Tr("Override Adjust Default Tokens"));
        
        SerializedProperty m_AdjustUseSameToken;
        GUIContent m_AdjustUseSameTokenLabel = new GUIContent(L10n.Tr("Use Same Configuration For All"));
        
        SerializedProperty m_AdjustIosAppToken;
        GUIContent m_AdjustIosAppTokenLabel = new GUIContent(L10n.Tr("iOS App Token"));

        SerializedProperty m_AdjustIosPurchaseEventToken;
        GUIContent m_AdjustIosPurchaseEventTokenLabel = new GUIContent(L10n.Tr("iOS Purchase Event Token"));

        SerializedProperty m_AdjustAppToken;
        GUIContent m_AdjustAppTokenLabel = new GUIContent(L10n.Tr("App Token"));

        SerializedProperty m_AdjustPurchaseEventToken;
        GUIContent m_AdjustPurchaseEventTokenLabel = new GUIContent(L10n.Tr("Purchase Event Token"));

        SerializedProperty m_AdjustLogLevel;
        GUIContent m_AdjustLogLevelLabel = new GUIContent(L10n.Tr("Log Level"));

        SerializedProperty m_AdjustEnvironment;
        GUIContent m_AdjustEnvironmentLabel = new GUIContent(L10n.Tr("Environment"));

        SerializedProperty m_AttributionChangedCallbackHandler;
        GUIContent m_AttributionChangedCallbackHandlerLabel = new GUIContent(L10n.Tr("Attribution Callback Handler"));
        
        SerializedProperty m_AdImpressionHandler;
        GUIContent m_AdImpressionHandlerLabel = new GUIContent(L10n.Tr("Ad Impression Handler"));
        
        SerializedProperty m_TransactionHandler;
        GUIContent m_TransactionHandlerLabel = new GUIContent(L10n.Tr("Transaction Handler"));

        bool m_DdnaFold;
        GUIContent m_DdnaLabel = new GUIContent(L10n.Tr("deltaDNA"));

        SerializedProperty m_StartDeltaDnaManually;
        GUIContent m_StartDeltaDnaManuallyLabel = new GUIContent(L10n.Tr("Start Manually"));

        GUIContent m_ConfigureDeltaDnaButtonLabel = new GUIContent(L10n.Tr("Configure deltaDNA"));

#if GAMEGROWTH_UNITY_MEDIATION || GAMEGROWTH_MOPUB
        bool m_MediationFold;
        GUIContent m_MediationLabel = new GUIContent(L10n.Tr("Mediation"));
        
        SerializedProperty m_UseDefaultImpressionTrackedHandler;
        GUIContent m_UseDefaultImpressionTrackedHandlerLabel = new GUIContent(L10n.Tr("Default Impression Handler"), L10n.Tr("Will automatically set an AdImpressionTrackedHandler for mediation to integrate with Adjust and Delta DNA."));
#endif

#if GAMEGROWTH_ADMOB
        private UnityEditor.Editor m_AdMobConfigurationEditor;
        
        bool m_AdMobFold;
        GUIContent m_AdMobLabel = new GUIContent(L10n.Tr("AdMob"));
        AdMobConfiguration m_AdMobConfiguration;
#endif        
        
#if GAMEGROWTH_FACEBOOK_SDK
        bool m_FacebookSdkFold;
        GUIContent m_FacebookSdkLabel = new GUIContent(L10n.Tr("Facebook SDK"));
        
        SerializedProperty m_StartFacebookSdkManually;
        GUIContent m_StartFacebookSdkManuallyLabel = new GUIContent(L10n.Tr("Start Manually"));
#endif

        string m_NoDefaultConfigurationMessage = L10n.Tr("No default configuration found. Open the Game Growth Configuration window from the Game Growth menu to get your default configuration.");

        void OnEnable()
        {
            m_Launcher = (GameGrowthLauncher) target;

            m_Configuration = m_Launcher.configuration;
            m_GameGrowthEnvironmentWarning = GameGrowthEnvironmentValidator.GetVerboseWarning(m_Configuration.environment);

            m_DefaultConfiguration = serializedObject.FindProperty("m_DefaultConfiguration");
          
            m_StartAdjustManually = serializedObject.FindProperty("m_StartAdjustManually");
            m_AdjustEventBuffering = serializedObject.FindProperty("m_AdjustEventBuffering");
            m_AdjustSendInBackground = serializedObject.FindProperty("m_AdjustSendInBackground");
            m_AdjustLaunchDeferredDeeplink = serializedObject.FindProperty("m_AdjustLaunchDeferredDeeplink");

            m_AdjustAndroidAppToken = serializedObject.FindProperty("m_AdjustAndroidAppToken");
            m_AdjustAndroidPurchaseEventToken = serializedObject.FindProperty("m_AdjustAndroidPurchaseEventToken");
            m_AdjustIosAppToken = serializedObject.FindProperty("m_AdjustIosAppToken");
            m_AdjustIosPurchaseEventToken = serializedObject.FindProperty("m_AdjustIosPurchaseEventToken");

            m_AdjustOverrideTokens = serializedObject.FindProperty("m_AdjustOverrideTokens");
            m_AdjustUseSameToken = serializedObject.FindProperty("m_AdjustUseSameToken");
            m_AdjustAppToken = serializedObject.FindProperty("m_AdjustAppToken");
            m_AdjustPurchaseEventToken = serializedObject.FindProperty("m_AdjustPurchaseEventToken");
            
            m_AdjustLogLevel = serializedObject.FindProperty("m_AdjustLogLevel");

            m_AttributionChangedCallbackHandler = serializedObject.FindProperty("m_AttributionChangedCallbackHandler");
            m_AdImpressionHandler = serializedObject.FindProperty("m_AdImpressionHandler");
            m_TransactionHandler = serializedObject.FindProperty("m_TransactionHandler");

            m_StartDeltaDnaManually = serializedObject.FindProperty("m_StartDeltaDnaManually");
            m_AdjustFold = true;
            m_DdnaFold = true;
            
#if GAMEGROWTH_UNITY_MEDIATION ||GAMEGROWTH_MOPUB
            m_MediationFold = true;
            m_UseDefaultImpressionTrackedHandler = serializedObject.FindProperty("m_UseDefaultImpressionTrackedHandler");
#endif
            
#if GAMEGROWTH_ADMOB
            m_AdMobFold = true;
            m_AdMobConfiguration = AdMobConfiguration.LoadMainAsset();
#endif
            
#if GAMEGROWTH_FACEBOOK_SDK
            m_FacebookSdkFold = true;
            m_StartFacebookSdkManually = serializedObject.FindProperty("m_StartFacebookSdkManually");
#endif

            // Synchronize the object and verify if the actual app token or purchase token are different (may be indicating an upgrade from older version)
            serializedObject.Update();
            if (IsAppTokenDifferent() || IsPurchaseEventTokenDifferent())
            {
                m_AdjustUseSameToken.boolValue = false;
                serializedObject.ApplyModifiedProperties();
            }
        }
        /// <summary>
        /// Check if the App Tokens are defined and not set to the same value
        /// </summary>
        /// <returns>true if yes, false otherwise</returns>
        bool IsAppTokenDifferent()
        {
            // Check first if the app tokens are defined (not the default values)
            var appTokenDefined = 
                m_AdjustAndroidAppToken.stringValue != GameGrowthLauncher.androidAppTokenDefaultText 
                || m_AdjustIosAppToken.stringValue != GameGrowthLauncher.iosAppTokenDefaultText;

            // Check if the app tokens are different
            var appTokenDifferent = appTokenDefined && (m_AdjustAndroidAppToken.stringValue != m_AdjustIosAppToken.stringValue);

            return appTokenDifferent;
        }

        /// <summary>
        /// Check if the Purchse Event Tokens are defined and not set to the same value
        /// </summary>
        /// <returns>true if yes, false otherwise</returns>
        bool IsPurchaseEventTokenDifferent() 
        { 
            // Check first if the purchase event tokens are defined (not the default values)
            var purchaseEventTokenDefined = 
                m_AdjustAndroidPurchaseEventToken.stringValue != GameGrowthLauncher.androidPurchaseEventTokenDefaultText 
                || m_AdjustIosPurchaseEventToken.stringValue != GameGrowthLauncher.iosPurchaseEventTokenDefaultText;

            // Check if the purchase event tokens are different
            var purchaseEventTokenDifferent = purchaseEventTokenDefined && (m_AdjustAndroidPurchaseEventToken.stringValue != m_AdjustIosPurchaseEventToken.stringValue);
            return purchaseEventTokenDifferent;
        }

        public override void OnInspectorGUI()
        {
            // Make sure the serialized Object is up-to-date before using it.
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_DefaultConfiguration, m_DefaultConfigurationContent);
            EditorGUILayout.Separator();

            if (!string.IsNullOrEmpty(m_GameGrowthEnvironmentWarning))
            {
                EditorGUILayout.HelpBox(m_GameGrowthEnvironmentWarning, MessageType.Info);
            }
            
            if (m_StartAdjustManually.boolValue || m_StartDeltaDnaManually.boolValue)
            {
                EditorGUILayout.HelpBox(k_CollectIdsMsg, MessageType.Info);
            }
            
            EditorGUI.BeginChangeCheck();
            m_Configuration.environment = (GameGrowthEnvironment)EditorGUILayout.EnumPopup(m_EnvironmentLabel, m_Configuration.environment);
            if (EditorGUI.EndChangeCheck())
            {
                var isSandbox = m_Configuration.environment == GameGrowthEnvironment.Sandbox;
                DeltaDNAConfigurationHelper.UpdateEnvironment(isSandbox);

                m_GameGrowthEnvironmentWarning = GameGrowthEnvironmentValidator.GetVerboseWarning(m_Configuration.environment);
            }
                        
            m_AdjustFold = EditorGUILayout.Foldout(m_AdjustFold, m_AdjustLabel);
            if (m_AdjustFold)
            {
                EditorGUILayout.PropertyField(m_StartAdjustManually, m_StartAdjustManuallyLabel);
                EditorGUILayout.PropertyField(m_AdjustEventBuffering, m_AdjustEventBufferingLabel);
                EditorGUILayout.PropertyField(m_AdjustSendInBackground, m_AdjustSendInBackgroundLabel);
                EditorGUILayout.PropertyField(m_AdjustLaunchDeferredDeeplink, m_AdjustLaunchDeferredDeeplinkLabel);

                GUILayout.Space(2f);
                EditorGUILayout.PropertyField(m_AdjustOverrideTokens, m_AdjustOverrideTokensLabel);
                if (m_AdjustOverrideTokens.boolValue)
                {
                    EditorGUILayout.PropertyField(m_AdjustUseSameToken, m_AdjustUseSameTokenLabel);

                    if (m_AdjustUseSameToken.boolValue)
                    {
                        EditorGUILayout.PropertyField(m_AdjustAppToken, m_AdjustAppTokenLabel);
                        EditorGUILayout.PropertyField(m_AdjustPurchaseEventToken, m_AdjustPurchaseEventTokenLabel);

                        var appToken = m_AdjustAppToken.stringValue;
                        if (appToken != GameGrowthLauncher.appTokenDefaultText)
                        {
                            m_AdjustAndroidAppToken.stringValue = appToken;
                            m_AdjustIosAppToken.stringValue = appToken;
                        }
                        else
                        {
                            m_AdjustAndroidAppToken.stringValue = GameGrowthLauncher.androidAppTokenDefaultText;
                            m_AdjustIosAppToken.stringValue = GameGrowthLauncher.iosAppTokenDefaultText;
                        }

                        var purchaseEventToken = m_AdjustPurchaseEventToken.stringValue;
                        if (purchaseEventToken != GameGrowthLauncher.purchaseEventTokenDefaultText)
                        {
                            m_AdjustAndroidPurchaseEventToken.stringValue = purchaseEventToken;
                            m_AdjustIosPurchaseEventToken.stringValue = purchaseEventToken;
                        }
                        else
                        {
                            m_AdjustAndroidPurchaseEventToken.stringValue = GameGrowthLauncher.androidPurchaseEventTokenDefaultText;
                            m_AdjustIosPurchaseEventToken.stringValue = GameGrowthLauncher.iosPurchaseEventTokenDefaultText;
                        }
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(m_AdjustAndroidAppToken, m_AdjustAndroidAppTokenLabel);
                        EditorGUILayout.PropertyField(m_AdjustAndroidPurchaseEventToken, m_AdjustAndroidPurchaseEventTokenLabel);
                        EditorGUILayout.PropertyField(m_AdjustIosAppToken, m_AdjustIosAppTokenLabel);
                        EditorGUILayout.PropertyField(m_AdjustIosPurchaseEventToken, m_AdjustIosPurchaseEventTokenLabel);
                    }
                }

                EditorGUILayout.PropertyField(m_AdjustLogLevel, m_AdjustLogLevelLabel);
            }
            
            EditorGUILayout.Separator();

            m_DdnaFold = EditorGUILayout.Foldout(m_DdnaFold, m_DdnaLabel);
            if (m_DdnaFold)
            {
                EditorGUILayout.PropertyField(m_StartDeltaDnaManually, m_StartDeltaDnaManuallyLabel);
                EditorGUILayout.PropertyField(m_AttributionChangedCallbackHandler, m_AttributionChangedCallbackHandlerLabel, true);
                EditorGUILayout.PropertyField(m_AdImpressionHandler, m_AdImpressionHandlerLabel, true);
                EditorGUILayout.PropertyField(m_TransactionHandler, m_TransactionHandlerLabel, true);
                if (GUILayout.Button(m_ConfigureDeltaDnaButtonLabel))
                {
                    var window = EditorWindow.GetWindow<ConfigurationWindow>();
                    window.Show();
                }
            }
            
#if GAMEGROWTH_FACEBOOK_SDK
            m_FacebookSdkFold = EditorGUILayout.Foldout(m_FacebookSdkFold, m_FacebookSdkLabel);
            if (m_FacebookSdkFold)
            {
                EditorGUILayout.PropertyField(m_StartFacebookSdkManually, m_StartFacebookSdkManuallyLabel);
            }
#endif  
            
#if GAMEGROWTH_UNITY_MEDIATION ||GAMEGROWTH_MOPUB
            m_MediationFold = EditorGUILayout.Foldout(m_MediationFold, m_MediationLabel);
            if (m_MediationFold)
            {
                EditorGUILayout.PropertyField(m_UseDefaultImpressionTrackedHandler, m_UseDefaultImpressionTrackedHandlerLabel);
            }
#endif
            
#if GAMEGROWTH_ADMOB
            m_AdMobFold = EditorGUILayout.Foldout(m_AdMobFold, m_AdMobLabel);
            if (m_AdMobFold)
            {
                if (m_AdMobConfiguration != null)
                {
                    CreateCachedEditor(m_AdMobConfiguration, null, ref m_AdMobConfigurationEditor);
                    m_AdMobConfigurationEditor.OnInspectorGUI();
                }
                else
                {
                    Debug.LogWarning("Could not find AdMobConfigurationAsset.");
                }
            }
#endif

            
            EditorGUILayout.BeginVertical();
            try
            {
                GUILayout.Space(5f);
                
                if (m_Launcher.DefaultConfiguration != null 
                    && m_Launcher.DefaultConfiguration.projectSummary != null
                    && !string.IsNullOrEmpty(m_Launcher.DefaultConfiguration.projectSummary.projectId))
                {
                    m_Launcher.DefaultConfiguration.DrawInspector();
                }
                else
                {
                    EditorGUILayout.HelpBox(m_NoDefaultConfigurationMessage, MessageType.Warning);
                }
            }
            finally
            {
                EditorGUILayout.EndVertical();   
            }
            
            serializedObject.ApplyModifiedProperties();
        }
   }
}