using UnityEditor.Callbacks;
using UnityEngine.GameGrowth;

namespace UnityEditor.GameGrowth
{
    public static class GameGrowthPostBuild
    {
        [PostProcessBuild(100)]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string buildPath)
        {
            var gameGrowthConfiguration = GameGrowthConfiguration.LoadMainAsset();
            if (gameGrowthConfiguration == null)
            {
                return;
            }

            GameGrowthEnvironmentValidator.LogStatus(gameGrowthConfiguration.environment);
        }
    }
}