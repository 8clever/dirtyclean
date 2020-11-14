﻿using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;
public class AdsController : MonoBehaviour, IUnityAdsListener
{

    #if UNITY_IOS
    private string gameId = "3900694";
    #elif UNITY_ANDROID
    private string gameId = "3900695";
    #endif
    public GameController controller;
    public bool showBanner = false;
    private string rewardedHealthAd = "rewardedVideo";
    private string bannerAd = "bannerAd";
    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize (gameId, true);   
        if (showBanner) {
            Advertisement.Banner.SetPosition (BannerPosition.TOP_CENTER);
            StartCoroutine(ShowBannerWhenInitialized());
        }
        
    }
    public void ShowHealthAd() {
        if (Advertisement.IsReady(rewardedHealthAd)) {
            Advertisement.Show(rewardedHealthAd);
            return;
        }
        Debug.Log("Rewarded video not ready yet!");
    }
    IEnumerator ShowBannerWhenInitialized () {
        while (!Advertisement.isInitialized) {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.Show (bannerAd);
    }
    public void OnUnityAdsReady(string placementId)
    {
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log(message);
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Failed) return;
        if (controller && placementId == rewardedHealthAd) {
            controller.AddPointsToHealth(20);
        }
        Debug.Log("Health added!");
    }
    private void OnDestroy() {
        if (showBanner) {
            Advertisement.Banner.Hide();
        }    
        Advertisement.RemoveListener(this);
    }
}
