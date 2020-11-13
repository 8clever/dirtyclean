using UnityEngine;
using UnityEngine.Advertisements;
public class AdsController : MonoBehaviour
{

    #if UNITY_IOS
    private string gameId = "3900694";

    #elif UNITY_ANDROID
    private string gameId = "3900695";
    #endif
    private System.DateTime nextTimeAd;
    private int ShowAdEachMinute = 10;
    private void Awake() {
        SetNextTimeAd();
    }
    void Start()
    {
        Advertisement.Initialize (gameId);   
    }
    private void SetNextTimeAd () {
        nextTimeAd = System.DateTime.UtcNow.Add(System.TimeSpan.FromMinutes(ShowAdEachMinute));
    }
    public void ShowInterstitialAd() {
        if (Advertisement.IsReady()) {
            Advertisement.Show();
            return;
        } 

        Debug.Log("Interstitial ad not ready at the moment! Please try again later!");
    }

    void Update()
    {
        if (nextTimeAd < System.DateTime.UtcNow) {
            SetNextTimeAd();
            ShowInterstitialAd();
        }
    }
}
