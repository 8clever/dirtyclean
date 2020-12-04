using System;
using com.adjust.sdk;
#if GAMEGROWTH_PURCHASE_VERIFICATION
using com.adjust.sdk.purchase;
#endif

namespace UnityEngine.GameGrowth
{
    public static class AdjustHelper
    {
        const string k_UnknownValue = "unknown";
        const string k_UnknownLogLevelMessage = "Adjust log level is unknown. Defaulting to Info.";
        
        public static AdjustEnvironment GetAdjustEnvironment(GameGrowthEnvironment gameGrowthEnvironment)
        {
            return (gameGrowthEnvironment == GameGrowthEnvironment.Sandbox)
                ? AdjustEnvironment.Sandbox
                : AdjustEnvironment.Production;
        }
#if GAMEGROWTH_PURCHASE_VERIFICATION        
        public static ADJPEnvironment GetAdjustPurchaseEnvironment(GameGrowthEnvironment gameGrowthEnvironment)
        {
            return (gameGrowthEnvironment == GameGrowthEnvironment.Sandbox)
                ? ADJPEnvironment.Sandbox
                : ADJPEnvironment.Production;
        }
        
        public static ADJPLogLevel GetPurchaseLogLevel(AdjustLogLevel logLevel)
        {
            var logLevelString = logLevel.ToLowercaseString();
            if (logLevelString == k_UnknownValue)
            {
                Debug.LogError(k_UnknownLogLevelMessage);
                return ADJPLogLevel.Info; //Use the default value;
            }

            if (logLevel == AdjustLogLevel.Suppress)
            {
                //Can't suppress adjustPurchase logs, so make it as silent as possible
                logLevelString = AdjustLogLevel.Error.ToLowercaseString();
            }

            if (Enum.TryParse<ADJPLogLevel>(logLevelString, true, out var purchaseLogLevel))
            {
                return purchaseLogLevel;
            }

            Debug.LogError(k_UnknownLogLevelMessage);
            return ADJPLogLevel.Info; //Use the default value;
        }
#endif
    }
}