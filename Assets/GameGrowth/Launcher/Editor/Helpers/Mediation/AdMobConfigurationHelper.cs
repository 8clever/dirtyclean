#if GAMEGROWTH_UNITY_MONETIZATION
using Unity.Mediation.Settings.Editor;

namespace UnityEditor.GameGrowth.Helpers.UnityMediation
{
    public class AdMobConfigurationHelper
    {
        public static void SetAdMobConfigurationForUnityMonetization(string androidAppId, string iosAppId)
        {
            using (var adMobSettings = new AdMobSettings())
            {
                adMobSettings.AdMobAppIdAndroid = androidAppId;
                adMobSettings.AdMobAppIdIos = iosAppId;
                adMobSettings.Save();
            }        
        }
    }
}
#endif
