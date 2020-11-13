using UnityEngine;
using UnityEngine.Advertisements;
public class AdsController : MonoBehaviour
{
    private string[] gameIds = {
        "3900694",
        "3900695"
    };
    private System.DateTime nextTimeAd;
    private int ShowAdEachMinute = 10;
    private void Awake() {
        SetNextTimeAd();
    }
    void Start()
    {
        foreach (var id in gameIds) {
            Advertisement.Initialize (id);   
        }
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
