#if GAMEGROWTH_UNITY_MONETIZATION
using System.Collections.Generic;
using Unity.Mediation.Adapters.Editor;

namespace UnityEditor.GameGrowth.Helpers.UnityMediation
{
    public class UnityMonetizationAdapterHelper
    {
        static readonly string[] k_RequiredAdapterIds = {"admob-adapter", "facebook-adapter", "unityads-adapter"};
        
        public static void EnableRequiredAdNetworks()
        {
            List<AdapterInfo> installedAdapters = MediationAdapters.GetInstalledAdapters();
            
            foreach (var adapterId in k_RequiredAdapterIds)
            {
                if (!installedAdapters.Exists(adapter => adapter.Identifier.Equals(adapterId)))
                {
                    MediationAdapters.Install(adapterId);
                }
            }
            
            MediationAdapters.Apply();
        }
    }
}
#endif
