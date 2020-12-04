using System;
using System.Net;
using com.adjust.sdk;
using DeltaDNA;

#if GAMEGROWTH_UNITY_MEDIATION
using Unity.Mediation;
#endif

#if GAMEGROWTH_PURCHASE_VERIFICATION
using com.adjust.sdk.purchase;
#endif

#if GAMEGROWTH_FACEBOOK_SDK
using Facebook.Unity;
#endif

namespace UnityEngine.GameGrowth
{
    public class GameGrowthLauncher : MonoBehaviour
    {
        public const string appTokenDefaultText = "{Your App Token}";
        public const string androidAppTokenDefaultText = "{Your Android App Token}";
        public const string iosAppTokenDefaultText = "{Your iOS App Token}";
        
        public const string purchaseEventTokenDefaultText = "{Your Purchase Event Token}";
        public const string androidPurchaseEventTokenDefaultText = "{Your Android Purchase Event Token}";
        public const string iosPurchaseEventTokenDefaultText = "{Your iOS Purchase Event Token}";

        const string k_MissingAdjustAttributionMessage = "Missing adjustAttribution, cannot record adjustAttribution event with Delta DNA";
        const string k_MissingAdImpressionDataMessage = "Missing adImpressionData, cannot record adImpression event with Delta DNA";
        const string k_MissingTransactionDataMessage = "Missing transactionData, cannot record transaction event with Delta DNA";

        static IDataPlatformAccessLayer s_DataPlatformAccessLayer;
        static DeltaDNA.GameEvent s_AdjustAttributionEvent;

        //Game Growth
        [SerializeField] 
        GameGrowthConfigurationAsset m_DefaultConfiguration = null;
        [SerializeField]
        GameGrowthConfiguration m_Configuration;

        //Adjust
        [SerializeField]
        bool m_StartAdjustManually;
        [SerializeField]
        bool m_AdjustEventBuffering;
        [SerializeField]
        bool m_AdjustSendInBackground;
        [SerializeField]
        bool m_AdjustLaunchDeferredDeeplink = true;
        [SerializeField]
        string m_AdjustAndroidAppToken = androidAppTokenDefaultText;
        [SerializeField]
        string m_AdjustAndroidPurchaseEventToken = androidPurchaseEventTokenDefaultText;
        [SerializeField]
        string m_AdjustIosAppToken = iosAppTokenDefaultText;
        [SerializeField]
        string m_AdjustIosPurchaseEventToken = iosPurchaseEventTokenDefaultText;
        
        [SerializeField]
        string m_AdjustAppToken = appTokenDefaultText;
        [SerializeField]
        string m_AdjustPurchaseEventToken = purchaseEventTokenDefaultText;
        
        [SerializeField]
        bool m_AdjustUseSameToken = true;
        [SerializeField] 
        bool m_AdjustOverrideTokens;
        
        [SerializeField]
        AdjustLogLevel m_AdjustLogLevel = AdjustLogLevel.Info;

#if UNITY_ANDROID       
        UnityThreadUtil m_MainThreadCaller;
#endif

#if UNITY_ANDROID || UNITY_IPHONE
        string m_AdjustPlatformAppToken;
        string m_AdjustPlatformPurchaseEventToken;
#endif

        public GameGrowthConfigurationAsset DefaultConfiguration
        {
            get { return m_DefaultConfiguration; }
        }
        
        public GameGrowthConfiguration configuration
	    {
            get => m_Configuration;
            set => m_Configuration = value;
        }
        
        public bool startAdjustManually
        {
            get => m_StartAdjustManually;
            set => m_StartAdjustManually = value;
        }
        public bool adjustEventBuffering
        {
            get => m_AdjustEventBuffering;
            set => m_AdjustEventBuffering = value;
        }
        public bool adjustSendInBackground
        {
            get => m_AdjustSendInBackground;
            set => m_AdjustSendInBackground = value;
        }
        public bool adjustLaunchDeferredDeepLink
        {
            get => m_AdjustLaunchDeferredDeeplink;
            set => m_AdjustLaunchDeferredDeeplink = value;
        }
        public string adjustAndroidAppToken
        {
            get => m_AdjustAndroidAppToken;
            set => m_AdjustAndroidAppToken = value;
        }

        public string adjustAndroidPurchaseEventToken
        {
            get => m_AdjustAndroidPurchaseEventToken;
            set => m_AdjustAndroidPurchaseEventToken = value;
        }

        public string adjustIosAppToken
        {
            get => m_AdjustIosAppToken;
            set => m_AdjustIosAppToken = value;
        }

        public string adjustIosPurchaseEventToken
        {
            get => m_AdjustIosPurchaseEventToken;
            set => m_AdjustIosPurchaseEventToken = value;
        }

        public bool adjustUseSameToken
        {
            get => m_AdjustUseSameToken;
            set => m_AdjustUseSameToken = value;
        }
                
        public bool adjustOverrideTokens
        {
            get => m_AdjustOverrideTokens;
            set => m_AdjustOverrideTokens = value;
        }
        
        public string adjustAppToken
        {
            get => m_AdjustAppToken;
            set => m_AdjustAppToken = value;
        }

        public string adjustPurchaseEventToken
        {
            get => m_AdjustPurchaseEventToken;
            set => m_AdjustPurchaseEventToken = value;
        }

        public AdjustLogLevel adjustLogLevel
        {
            get => m_AdjustLogLevel;
            set => m_AdjustLogLevel = value;
        }

        //Delta DNA
        bool m_DidSentDdnaClientDeviceEvent;
        
        [SerializeField]
        bool m_StartDeltaDnaManually;
        
        [SerializeField]
        AttributionCallbackHandler m_AttributionChangedCallbackHandler = new AttributionCallbackHandler();
        [SerializeField]
        AdImpressionHandler m_AdImpressionHandler = new AdImpressionHandler();
        [SerializeField]
        TransactionHandler m_TransactionHandler = new TransactionHandler();

        public bool startDeltaDnaManually
        {
            get => m_StartDeltaDnaManually;
            set => m_StartDeltaDnaManually = value;
        }

        public AttributionCallbackHandler attributionChangedCallbackHandler
        {
            get => m_AttributionChangedCallbackHandler;
            set => m_AttributionChangedCallbackHandler = value;
        }

        public AdImpressionHandler adImpressionHandler
        {
            get => m_AdImpressionHandler;
            set => m_AdImpressionHandler = value;
        }

        public TransactionHandler transactionHandler
        {
            get => m_TransactionHandler;
            set => m_TransactionHandler = value;
        }
        
#if GAMEGROWTH_UNITY_MEDIATION || GAMEGROWTH_MOPUB
        //Mediation
        [SerializeField]
        bool m_UseDefaultImpressionTrackedHandler = true;
        
        public bool useDefaultImpressionTrackedHandler
        {
            get => m_UseDefaultImpressionTrackedHandler;
            set => m_UseDefaultImpressionTrackedHandler = value;
        }

#endif

#if GAMEGROWTH_FACEBOOK_SDK
        //FacebookSdk
        [SerializeField]
        bool m_StartFacebookSdkManually;
        
        public bool startFacebookSdkManually
        {
            get => m_StartFacebookSdkManually;
            set => m_StartFacebookSdkManually = value;
        }

#endif

        private void Start()
        {
            GameGrowthEnvironmentValidator.LogStatus(m_Configuration.environment);
        }

        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);

#if UNITY_ANDROID
            m_MainThreadCaller = gameObject.AddComponent<UnityThreadUtil>();
            m_AdjustPlatformAppToken = adjustAndroidAppToken;
            m_AdjustPlatformPurchaseEventToken = adjustAndroidPurchaseEventToken;
#elif UNITY_IPHONE
            m_AdjustPlatformAppToken = adjustIosAppToken;
            m_AdjustPlatformPurchaseEventToken = adjustIosPurchaseEventToken;
#endif
            SetupDeltaDna();
            SetupAdjust();
#if GAMEGROWTH_PURCHASE_VERIFICATION            
            SetupAdjustPurchase();
#endif
#if GAMEGROWTH_UNITY_MEDIATION
            SetupUnityMeditation();         
#elif GAMEGROWTH_MOPUB
            SetupMoPub();
#endif
#if GAMEGROWTH_FACEBOOK_SDK
            SetupFacebookSdk();
#endif
            s_DataPlatformAccessLayer = new DataPlatformAccessLayer();
            
            if (!startAdjustManually && !startDeltaDnaManually)
            {
                CollectIds();
            }
        }
        
        void OnApplicationPause (bool pauseStatus)
        {
#if GAMEGROWTH_FACEBOOK_SDK
            if (!pauseStatus) {
                SetupFacebookSdk();
            }
#endif
        }

        void SetupDeltaDna()
        {
            DDNA.Instance.Settings.OnInitSendClientDeviceEvent = false;
            DDNA.Instance.OnNewSession += OnDdnaNewSession;

            if (!startDeltaDnaManually)
            {
                DDNA.Instance.StartSDK();
            }
        }

        void OnDdnaNewSession()
        {
            if (m_DidSentDdnaClientDeviceEvent)
            {
                return;
            }

            DeltaDNA.Logger.LogDebug("Sending 'clientDevice' event");

            var clientDeviceEvent = new GameEvent("clientDevice")
                .AddParam("deviceName", ClientInfo.DeviceName)
                .AddParam("deviceType", ClientInfo.DeviceType)
                .AddParam("hardwareVersion", ClientInfo.DeviceModel)
                .AddParam("operatingSystem", ClientInfo.OperatingSystem)
                .AddParam("operatingSystemVersion", ClientInfo.OperatingSystemVersion)
                .AddParam("timezoneOffset", ClientInfo.TimezoneOffset)
                .AddParam("userLanguage", ClientInfo.LanguageCode)
                .AddParam("cpuType", SystemInfo.processorType)
                .AddParam("networkType", Application.internetReachability);

            if (ClientInfo.Manufacturer != null) {
                clientDeviceEvent.AddParam("manufacturer", ClientInfo.Manufacturer);
            }

            DDNA.Instance.RecordEvent(clientDeviceEvent).Run();
            m_DidSentDdnaClientDeviceEvent = true;
        }

        void SetupAdjust()
        {
#if UNITY_ANDROID || UNITY_IPHONE                
            if (!startAdjustManually)
            {
                var adjustPlatformAppTokens = "";

                if (!m_AdjustOverrideTokens)
                {
#if UNITY_ANDROID
                    adjustPlatformAppTokens = m_DefaultConfiguration.projectSummary.providers.adjust.appToken.androidValue;
#elif UNITY_IPHONE
                    adjustPlatformAppTokens = m_DefaultConfiguration.projectSummary.providers.adjust.appToken.iOSValue;
#endif
                }
                else
                {
                    adjustPlatformAppTokens = m_AdjustPlatformAppToken;
                }
                
                var adjustConfig = new AdjustConfig(adjustPlatformAppTokens, AdjustHelper.GetAdjustEnvironment(m_Configuration.environment))
                {
                    logLevel = adjustLogLevel,
                    eventBufferingEnabled = adjustEventBuffering,
                    sendInBackground = adjustSendInBackground,
                    launchDeferredDeeplink = adjustLaunchDeferredDeepLink,
                };
                
                adjustConfig.setAttributionChangedDelegate(attribution =>
                    {
                        attributionChangedCallbackHandler.Invoke(attribution);
                    }, name);

                Adjust.start(adjustConfig);
            }
#endif                
        }

#if UNITY_IOS
        public void GetNativeAttribution(string attributionData)
        {
            Adjust.GetNativeAttribution(attributionData);
        }

        public void GetNativeEventSuccess(string eventSuccessData)
        {
            Adjust.GetNativeEventSuccess(eventSuccessData);
        }

        public void GetNativeEventFailure(string eventFailureData)
        {
            Adjust.GetNativeEventFailure(eventFailureData);
        }

        public void GetNativeSessionSuccess(string sessionSuccessData)
        {
            Adjust.GetNativeSessionSuccess(sessionSuccessData);
        }

        public void GetNativeSessionFailure(string sessionFailureData)
        {
            Adjust.GetNativeSessionFailure(sessionFailureData);
        }

        public void GetNativeDeferredDeeplink(string deeplinkURL)
        {
            Adjust.GetNativeDeferredDeeplink(deeplinkURL);
        }
        
        public void GetAuthorizationStatus(string authorizationStatus)
        {
            Adjust.GetAuthorizationStatus(authorizationStatus);
        }
        
#endif

#if GAMEGROWTH_PURCHASE_VERIFICATION
        void SetupAdjustPurchase()
        {
#if UNITY_ANDROID || UNITY_IPHONE                
            var purchaseLogLevel = AdjustHelper.GetPurchaseLogLevel(adjustLogLevel);
            var purchaseEnvironment = AdjustHelper.GetAdjustPurchaseEnvironment(m_Configuration.environment);
            
            if (!startAdjustManually)
            {
                AdjustPurchase.Init(new ADJPConfig(m_AdjustPlatformAppToken, purchaseEnvironment)
                {
                    logLevel = purchaseLogLevel,
                });
            }
#endif                
        }
#endif        
        
        public static void CollectIds()
        {
            if(!Analytics.Analytics.playerOptedOut) 
            {
#if UNITY_ANDROID
                Adjust.getGoogleAdId(googleAdId =>
                {
                    SendIds(
                        string.Empty, 
                        googleAdId, 
                        Adjust.getAdid(), 
                        DDNA.Instance.UserID, 
                        DDNA.Instance.EnvironmentKey, 
                        SystemInfo.deviceUniqueIdentifier,
                        Analytics.AnalyticsSessionInfo.userId,
                        Application.platform.ToString(),
                        Application.cloudProjectId);
                });
#elif UNITY_IPHONE
                SendIds(
                    Adjust.getIdfa(),
                    string.Empty, 
                    Adjust.getAdid(), 
                    DDNA.Instance.UserID, 
                    DDNA.Instance.EnvironmentKey, 
                    SystemInfo.deviceUniqueIdentifier,
                    Analytics.AnalyticsSessionInfo.userId,
                    Application.platform.ToString(),
                    Application.cloudProjectId);
#endif
            }
        }
        
        static void SendIds(string idfa, string googleAdId, string adid, string ddnaUserId, string ddnaEnvironmentKey, string deviceId, string unityAnalyticsUserId, string platform, string unityProjectId)
        {
            s_DataPlatformAccessLayer.SendIds(
                idfa,
                googleAdId,
                adid,
                ddnaUserId,
                ddnaEnvironmentKey,
                deviceId,
                platform,
                unityAnalyticsUserId,
                unityProjectId,
                data =>
                {
                    if (data.responseCode != Convert.ToInt64(HttpStatusCode.OK))
                    {
                        //Do a single retry if the previous attempt failed
                        s_DataPlatformAccessLayer.SendIds(
                            idfa,
                            googleAdId,
                            adid,
                            ddnaUserId,
                            ddnaEnvironmentKey,
                            deviceId,
                            platform,
                            unityAnalyticsUserId,
                            unityProjectId,
                            null);
                    }
                });
        }

#if GAMEGROWTH_PURCHASE_VERIFICATION
        public void GetNativeVerificationInfo(string stringVerificationInfo)
        {
            AdjustPurchase.GetNativeVerificationInfo(stringVerificationInfo);
        }
#endif

#if GAMEGROWTH_UNITY_MEDIATION
        void SetupUnityMeditation()
        {
            if(useDefaultImpressionTrackedHandler)
            {
                ImpressionEventPublisher.OnImpression += DefaultMediationImpressionTrackedEventHandler;
            }
        }

        public void DefaultMediationImpressionTrackedEventHandler(object sender, ImpressionEventArgs e)
        {
            AdImpressionData gameGrowthAdImpressionData;

            if(e?.ImpressionData == null)
            {
                gameGrowthAdImpressionData = new AdImpressionData(AdjustConfig.AdjustAdRevenueSourceUnity, null, AdCompletionStatus.Completed);
            }
            else
            {
                var adImpressionData = e.ImpressionData;
                var impressionDataJson = JsonUtility.ToJson(e.ImpressionData, true);
                
                float.TryParse(adImpressionData.PublisherRevenue, out var adEcpmUsd);
    
                gameGrowthAdImpressionData = new AdImpressionData(AdjustConfig.AdjustAdRevenueSourceUnity, impressionDataJson, AdCompletionStatus.Completed) 
                {
                    adEcpmUsd = adEcpmUsd * 1000,
                    adProvider = adImpressionData.AdSourceName ?? "N/A",
                    placementId = adImpressionData.AdUnitId ?? "N/A",
                    placementName = adImpressionData.AdUnitName,
                    placementType = adImpressionData.AdUnitFormat
                };
            }
            
            adImpressionHandler.Invoke(gameGrowthAdImpressionData);
        }
        
#elif GAMEGROWTH_MOPUB
        void SetupMoPub()
        {
            if(useDefaultImpressionTrackedHandler) 
            {
                MoPubManager.OnImpressionTrackedEvent += DefaultMoPubImpressionTrackedEventHandler;
            }
        }
        
        public void DefaultMoPubImpressionTrackedEventHandler(string adUnitId, MoPub.ImpressionData adImpressionData)
        {
            var gameGrowthAdImpressionData = new AdImpressionData(AdjustConfig.AdjustAdRevenueSourceMopub, adImpressionData.JsonRepresentation, AdCompletionStatus.Completed) {
                    adEcpmUsd = Convert.ToSingle(adImpressionData.PublisherRevenue ?? 0) * 1000,
                    adProvider = adImpressionData.NetworkName ?? "N/A",
                    placementId = adImpressionData.AdUnitId ?? "N/A",
                    placementName = adImpressionData.AdUnitName,
                    placementType = adImpressionData.AdUnitFormat,
                };
            adImpressionHandler.Invoke(gameGrowthAdImpressionData);
        }
#endif

#if GAMEGROWTH_FACEBOOK_SDK
        void SetupFacebookSdk()
        {
            if (!startFacebookSdkManually)
            {
                if (FB.IsInitialized)
                {
                    FB.ActivateApp();
                }
                else
                {
                    FB.Init(FB.ActivateApp);
                }
            }
        }
#endif
        public void DefaultAdjustAttributionCallback(AdjustAttribution adjustAttribution)
        {
            if (adjustAttribution == null)
            {
                Debug.LogError(k_MissingAdjustAttributionMessage);
                return;
            }
            
            //Track attribution with Delta DNA.
            var adjustAttributionEvent = new GameEvent(AdjustAttributionExtensions.adjustAttributionEventName);
            AddStringParam(adjustAttributionEvent, AdjustAttributionExtensions.acquisitionChannelParam, adjustAttribution.network + "::" + adjustAttribution.campaign);
            AddStringParam(adjustAttributionEvent, AdjustAttributionExtensions.adGroupParam, adjustAttribution.adgroup);
            AddStringParam(adjustAttributionEvent, AdjustAttributionExtensions.campaignParam, adjustAttribution.campaign);
            AddStringParam(adjustAttributionEvent, AdjustAttributionExtensions.creativeParam, adjustAttribution.creative);
            AddStringParam(adjustAttributionEvent, AdjustAttributionExtensions.networkParam, adjustAttribution.network);
            AddStringParam(adjustAttributionEvent, AdjustAttributionExtensions.trackerNameParam, adjustAttribution.trackerName);
            AddStringParam(adjustAttributionEvent, AdjustAttributionExtensions.trackerTokenParam, adjustAttribution.trackerToken);
            // No value assigned for activityKind.
            AddStringParam(adjustAttributionEvent, AdjustAttributionExtensions.activityKindParam, string.Empty);
            s_AdjustAttributionEvent = adjustAttributionEvent;
            
#if UNITY_ANDROID            
            m_MainThreadCaller.RunOnMainThread(RecordDeltaDnaAttributionEvent);
#else
            RecordDeltaDnaAttributionEvent();
#endif

            //Since this is a callback from Adjust, we don't need to track anything with Adjust
        }

        void RecordDeltaDnaAttributionEvent()
        {
            DDNA.Instance.RecordEvent(s_AdjustAttributionEvent).Run();
            //For attribution, for the upload of the event now to prevent missed attribution on game closing too soon.
            DDNA.Instance.Upload();
            s_AdjustAttributionEvent = null;
        }
        public void DefaultRecordAdImpression(AdImpressionData adImpressionData)
        {
            if (adImpressionData == null)
            {
                Debug.LogError(k_MissingAdImpressionDataMessage);
                return;
            }
            //Track impression with Delta DNA
            var adImpressionEvent = new GameEvent(AdImpressionData.adImpressionEventName)
                .AddParam(AdImpressionData.adCompletionStatusParamName, adImpressionData.adCompletionStatus.ExportForEvent());
            AddOptionalFloatParam(adImpressionEvent, AdImpressionData.adEcpmUsdParamName, adImpressionData.adEcpmUsd);
            AddOptionalStringParam(adImpressionEvent, AdImpressionData.adProviderParamName, adImpressionData.adProvider);
            AddOptionalStringParam(adImpressionEvent, AdImpressionData.placementIdParamName, adImpressionData.placementId);
            AddOptionalStringParam(adImpressionEvent, AdImpressionData.placementNameParamName, adImpressionData.placementName);
            AddOptionalStringParam(adImpressionEvent, AdImpressionData.placementTypeParamName, adImpressionData.placementType);
            DDNA.Instance.RecordEvent(adImpressionEvent).Run();
            
            //Track revenue with Adjust
            Adjust.trackAdRevenue(adImpressionData.adRevenueSource, adImpressionData.impressionJsonData ?? "{}");
        }
        
        public void DefaultRecordTransaction(TransactionData transactionData)
        {
            if (transactionData == null)
            {
                Debug.LogError(k_MissingTransactionDataMessage);
                return;
            }

            //Track purchase with Delta DNA
            var transactionEvent = new Transaction(
                transactionData.transactionName, 
                transactionData.transactionType.ExportForEvent(),
                transactionData.productsReceived,
                transactionData.productsSpent);
            
            AddOptionalStringParam(transactionEvent, TransactionData.amazonPurchaseTokenParamName, transactionData.amazonPurchaseToken);
            AddOptionalStringParam(transactionEvent, TransactionData.amazonUserIdParamName, transactionData.amazonUserId);
            AddOptionalIntParam(transactionEvent, TransactionData.engagementIdParamName, transactionData.engagementId);
            AddOptionalBoolParam(transactionEvent, TransactionData.isInitiatorParamName, transactionData.isInitiator);
            AddOptionalPaymentCountryParam(transactionEvent, TransactionData.paymentCountryParamName, transactionData.paymentCountry);
            AddOptionalStringParam(transactionEvent, TransactionData.productIdParamName, transactionData.productId);
            AddOptionalIntParam(transactionEvent, TransactionData.revenueValidatedParamName, transactionData.revenueValidated);
            AddOptionalStringParam(transactionEvent, TransactionData.sdkVersionParamName, transactionData.sdkVersion);
            AddOptionalStringParam(transactionEvent, TransactionData.transactionIdParamName, transactionData.transactionId);
            AddOptionalStringParam(transactionEvent, TransactionData.transactionReceiptParamName, transactionData.transactionReceipt);
            AddOptionalStringParam(transactionEvent, TransactionData.transactionReceiptSignatureParamName, transactionData.transactionReceiptSignature);
            AddOptionalTransactionServerParam(transactionEvent, TransactionData.transactionServerParamName, transactionData.transactionServer);
            AddOptionalStringParam(transactionEvent, TransactionData.transactorIdParamName, transactionData.transactorId);
            AddOptionalIntParam(transactionEvent, TransactionData.userLevelParamName, transactionData.userLevel);
            AddOptionalIntParam(transactionEvent, TransactionData.userScoreParamName, transactionData.userScore);
            AddOptionalIntParam(transactionEvent, TransactionData.userXpParamName, transactionData.userXp);

            DDNA.Instance.RecordEvent(transactionEvent).Run();
            
#if UNITY_ANDROID || UNITY_IPHONE            
            //Track purchase with Adjust
            var adjustEvent = new AdjustEvent(m_AdjustPlatformPurchaseEventToken);
            adjustEvent.setRevenue(transactionData.localizedPrice, transactionData.isoCurrencyCode);
            adjustEvent.setTransactionId(transactionData.transactionId);
            adjustEvent.receipt = transactionData.transactionReceipt;
            adjustEvent.isReceiptSet = true;
            Adjust.trackEvent(adjustEvent);
#endif            
        }

        static void AddOptionalStringParam(Transaction transaction, string paramName, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                transaction.AddParam(paramName, value);
            }
        }
        
        static void AddOptionalIntParam(Transaction transaction, string paramName, int? value)
        {
            if (value.HasValue)
            {
                transaction.AddParam(paramName, value.Value);
            }
        }
        
        static void AddOptionalBoolParam(Transaction transaction, string paramName, bool? value)
        {
            if (value.HasValue)
            {
                transaction.AddParam(paramName, value.Value);
            }
        }
        
        static void AddOptionalPaymentCountryParam(Transaction transaction, string paramName, PaymentCountry? value)
        {
            if (value.HasValue)
            {
                transaction.AddParam(paramName, value.Value.ToString());
            }
        }
        
        static void AddOptionalTransactionServerParam(Transaction transaction, string paramName, TransactionServer? value)
        {
            if (value.HasValue)
            {
                transaction.AddParam(paramName, value.Value.ExportForEvent());
            }
        }

        static void AddOptionalFloatParam(GameEvent gameEvent, string paramName, float? value)
        {
            if (value.HasValue)
            {
                gameEvent.AddParam(paramName, value.Value);
            }
        }

        static void AddOptionalStringParam(GameEvent gameEvent, string paramName, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                gameEvent.AddParam(paramName, value);
            }
        }
        
        static void AddStringParam(GameEvent gameEvent, DeltaDnaEventParam param, string value)
        {
            // Unavailable data should be sent as empty string parameters
            gameEvent.AddParam(param.name, param.GetStringValue(value));
        }

        void OnDestroy()
        {
            if (DDNA.Instance != null)
            {
                DDNA.Instance.OnNewSession -= OnDdnaNewSession;
            }            
        }
    }
}
