using UnityEngine;
using UnityEngine.GameGrowth;

public static class GameGrowthEnvironmentValidator
{
    const string k_GameGrowthInSandboxMessage = "Game Growth - Sandbox";
    const string k_GameGrowthInStoreMessage = "Game Growth - Store";
    const string k_DeltaDnaInDevMessage = "DeltaDNA - Development";

    const string k_GameGrowthInSandboxWarning = "Game Growth is currently in Sandbox mode. Make sure to switch environment to Store for your release.";
    const string k_DeltaDnaInDevWarning = "Game Growth is currently in Store mode, but DeltaDNA configuration is set to Dev Make sure to switch environment to Live for your release.";

    public static string GetVerboseWarning(GameGrowthEnvironment environment)
    {
        if (environment == GameGrowthEnvironment.Sandbox)
        {
            return k_GameGrowthInSandboxWarning;
        }
        else if (DeltaDNAConfigurationHelper.IsInDevelopmentMode())
        {
            return k_DeltaDnaInDevWarning;
        }
        
        return null;
    }
    
    public static void LogStatus(GameGrowthEnvironment environment, bool warningOnly = false)
    {
        if (environment == GameGrowthEnvironment.Sandbox)
        {
            Debug.LogWarning(k_GameGrowthInSandboxMessage);
        }
        else
        {
            if (DeltaDNAConfigurationHelper.IsInDevelopmentMode())
            {
                Debug.LogWarning(k_DeltaDnaInDevMessage);
            }
            else if (!warningOnly)
            {
                Debug.Log(k_GameGrowthInStoreMessage);
            }
        }
    }
}
