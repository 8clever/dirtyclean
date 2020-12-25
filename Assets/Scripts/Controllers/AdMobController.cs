using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using GoogleMobileAdsMediationTestSuite.Api;
using GoogleMobileAds.Api;

public class AdMobController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool LoadBanner = false;
    public bool LoadInterstitial = false;
    public Button RewardedButton;
    private AdRequest request;
    private RewardedAd rewarded;
    private BannerView banner;
    void Start()
    {
        MobileAds.Initialize((initStatus) =>
        {
            Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
            foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
            {
                string className = keyValuePair.Key;
                AdapterStatus status = keyValuePair.Value;
                switch (status.InitializationState)
                {
                case AdapterState.NotReady:
                    // The adapter initialization did not complete.
                    MonoBehaviour.print("Adapter: " + className + " not ready.");
                    break;
                case AdapterState.Ready:
                    // The adapter was successfully initialized.
                    MonoBehaviour.print("Adapter: " + className + " is initialized.");
                    break;
                }
            }
            OnAdapterReady();
        });
    }

    public void ShowMediationTestSuite () {
        MediationTestSuite.Show();
    }

    private void OnAdapterReady () {
        request = new AdRequest.Builder().Build();
        if (LoadBanner) {
            loadBanner();
        }
        if (RewardedButton) {
            loadRewardedVideo();
            RewardedButton.onClick.AddListener(showRewardedVideo);
        }
        if (LoadInterstitial) {
            loadRewardedVideo();
        }
    }

    private void loadBanner () {
        #if UNITY_ANDROID
            var adUnitId = "ca-app-pub-7579927697787840/9033894090";
        #elif UNITY_IPHONE
            var adUnitId = "ca-app-pub-7579927697787840/7409875131";
        #endif
        banner = new BannerView(adUnitId, AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth), AdPosition.Top);
        banner.OnAdLoaded += HandleBannerAdLoaded;
        banner.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        banner.LoadAd(request);
    }

    private void loadRewardedVideo () {
        #if UNITY_ANDROID
            var adUnitId = "ca-app-pub-7579927697787840/7090257032";
        #elif UNITY_IPHONE
            var adUnitId = "ca-app-pub-7579927697787840/6621221850";
        #endif
        rewarded = new RewardedAd(adUnitId);
        rewarded.OnAdLoaded += HandleRewardedAdLoaded;
        rewarded.OnUserEarnedReward += HandleUserEarnedReward;
        rewarded.LoadAd(request);
    }

    private void showRewardedVideo () {
        rewarded.Show();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        if (RewardedButton) {
            RewardedButton.interactable = true;
        }
        if (LoadInterstitial) {
            rewarded.Show();
        }
    }

    public void HandleBannerAdLoaded(object sender, EventArgs args)
    {
        banner.Show();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);
        var game = GameObject.FindObjectOfType<GameController>();
        if (game) {
            game.AddPointsToHealth(Convert.ToInt32(amount));
        }
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }
}
