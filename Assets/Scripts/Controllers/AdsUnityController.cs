using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdsUnityController : MonoBehaviour, IUnityAdsListener
{
    private string bannerId = "bannerTop";
    private string rewardId = "healthReward";
    private string videoId = "video";
    #if UNITY_ANDROID
        private string gameId = "3900695";
    #elif UNITY_IPHONE
        private string gameId = "3900694";
    #endif
    private bool testMode = true;
    public bool loadBanner = false;
    public bool loadVideo = false;
    public Button btnReward;
    void Start () {
        Advertisement.AddListener(this);
        Advertisement.Initialize (gameId, testMode);

        var stateBanner = Advertisement.GetPlacementState(bannerId);
        var stateReward = Advertisement.GetPlacementState(rewardId);
        var stateVideo = Advertisement.GetPlacementState(videoId);
        
        if (stateBanner == PlacementState.Ready) {
            OnUnityAdsReady(bannerId);
        }
        if (stateReward == PlacementState.Ready) {
            OnUnityAdsReady(rewardId);
        }
        if (stateVideo == PlacementState.Ready) {
            OnUnityAdsReady(videoId);
        }
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish (string placementId, ShowResult showResult) {
        Advertisement.Banner.Show(bannerId);
        if (showResult == ShowResult.Finished && placementId == rewardId) {
            var controller = FindObjectOfType<GameController>();
            if (controller) {
                controller.AddPointsToHealth(20);
            }
        } else if (showResult == ShowResult.Skipped) {
            // Do not reward the user for skipping the ad.
        } else if (showResult == ShowResult.Failed) {
            Debug.LogWarning ("AD Error: " + placementId);
        }
    }

    public void OnUnityAdsReady (string placementId) {
        if (placementId == bannerId && loadBanner) {
            Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
            Advertisement.Banner.Show(bannerId);
        }
        if (placementId == rewardId && btnReward) {
            btnReward.interactable = true;
            btnReward.onClick.AddListener(delegate {
                Advertisement.Banner.Hide(false);
                Advertisement.Show(rewardId);
            });
        }
        if (placementId == rewardId && loadVideo) {
            Advertisement.Banner.Hide(false);
            Advertisement.Show(rewardId);
        }
    }

    public void OnUnityAdsDidError (string message) {
        Debug.Log(message);
    }

    public void OnUnityAdsDidStart (string placementId) {
        // Optional actions to take when the end-users triggers an ad.
    } 
    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy() {
        Advertisement.RemoveListener(this);
    }
}
