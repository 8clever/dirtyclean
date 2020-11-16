using UnityEngine;
using GoogleMobileAds.Api;

public class AdsController : MonoBehaviour
{
    #if UNITY_ANDROID
        private string bannerId = "ca-app-pub-7579927697787840/9033894090";
        private string rewardId = "ca-app-pub-7579927697787840/7090257032";
    #elif UNITY_IPHONE
        private string bannerId = "not-have-yet";
        private string rewardId = "not-have-yet";
    #else
        private string bannerId = "unexpected_platform";
        private string rewardId = "unexpected_platform";
    #endif

    public bool loadBanner = false;
    public bool loadReward = false;
    private BannerView banner;
    private RewardedAd reward;
    void Start()
    {
        MobileAds.Initialize(initStatus => {});
        banner = new BannerView(bannerId, AdSize.Banner, AdPosition.Top);
        reward = new RewardedAd(rewardId);
        reward.OnUserEarnedReward += HandlerUserEarnedReward;
        reward.OnAdClosed += HandleRewardClosed;
        if (loadBanner) {
            banner.LoadAd(GetAdRequest());
        }
        if (loadReward) {
            reward.LoadAd(GetAdRequest());
        }
    }
    private AdRequest GetAdRequest () {
        return new AdRequest.Builder().Build();
    }
    private void OnDestroy() {
        banner.Destroy();
    }
    public void ShowHealthAd() {
        if (reward.IsLoaded()) {
            reward.Show();
        }
    }
    public void HandlerUserEarnedReward(object sender, Reward args) {
        GameController controller = GameObject.FindObjectOfType<GameController>();
        controller.AddPointsToHealth(System.Convert.ToInt32(args.Amount));
        Debug.Log($"ReceivedPoints: {args.Amount}");
    }
    public void HandleRewardClosed(object sender, System.EventArgs args) {
        reward.LoadAd(GetAdRequest());
    }
}
