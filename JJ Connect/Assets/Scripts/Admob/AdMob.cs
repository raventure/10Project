using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using System;

public class AdMob : MonoBehaviour
{
    private static AdMob mAdMob;
    public static AdMob Instance
    {
        get
        {
            if (mAdMob == null)
            {
                mAdMob = FindObjectOfType<AdMob>();
            }
            return mAdMob;
        }
    }

#if UNITY_EDITOR
    string appId = "unused";
    string adUnitId = "unused";
    string adUnitInterstitialId = "unused";
    string adUnitVideoId = "unused";

#elif UNITY_ANDROID
    string appId                = "ca-app-pub-5962671645854399~5778397915";
    string adUnitId             = "ca-app-pub-5962671645854399/5203682845";
    string adUnitVideoId        = "ca-app-pub-5962671645854399/5203682845";

#elif UNITY_IPHONE
    string appId                = "ca-app-pub-5962671645854399~5778397915";
    string adUnitId             = "ca-app-pub-5962671645854399/5203682845";
    string adUnitVideoId        = "ca-app-pub-5962671645854399/5203682845";
#else
    string appId                = "unexpected_platform";
    string adUnitId             = "unexpected_platform";
    string adUnitVideoId        = "unexpected_platform";
#endif

    BannerView bannerView = null;
    InterstitialAd interstitialAd = null;
    RewardBasedVideoAd rewardBasedVideo = null;


    // Use this for initialization
    void Start()
    {
        MobileAds.Initialize(appId);
        this.rewardBasedVideo = RewardBasedVideoAd.Instance;

        this.RequestBanner();
        this.RequestRewardBasedVideo();
    }

    /* 배너 광고 함수 */
    private void RequestBanner()
    {
        bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);

        bannerView.OnAdLoaded += HandleOnAdLoaded; //성공 시
        bannerView.OnAdFailedToLoad += HandleOnAdFailedToLoad; //실패 시

        AdRequest.Builder builder = new AdRequest.Builder();
        AdRequest request = builder.AddTestDevice(AdRequest.TestDeviceSimulator).
            AddTestDevice("61140F27CD138576").
            Build();
        bannerView.LoadAd(request);
        bannerView.Show();
        //Debug.Log(SystemInfo.deviceUniqueIdentifier);
    }

    // 광고로드 완료 실행
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    // 광고 로드 실패 시
    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
        //실패 시 유니티 애즈로 처리
    }


    /* 보상형 리워드 광고 함수 */
    private void RequestRewardBasedVideo()
    {
        // Create an empty ad request.
        AdRequest.Builder builder = new AdRequest.Builder();
        AdRequest request = builder.AddTestDevice(AdRequest.TestDeviceSimulator).
            AddTestDevice("61140F27CD138576").
            Build();
        // Load the rewarded video ad with the request.
        this.rewardBasedVideo.LoadAd(request, adUnitVideoId);


        // Called when an ad request has successfully loaded.
        rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
        // Called when an ad request failed to load.
        rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        // Called when an ad is shown.
        rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
        // Called when the ad starts to play.
        rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
        // Called when the user should be rewarded for watching a video.
        rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded; //광고를 본 경우 처리
        // Called when the ad is closed.
        rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
        // Called when the ad click caused the user to leave the application.
        rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;
    }


    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardBasedVideoFailedToLoad event received with message: "
                             + args.Message);
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
        this.RequestRewardBasedVideo();
    }
    
    //광고를 본 경우 여기서 처리.
    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardBasedVideoRewarded event received for "
                        + amount.ToString() + " " + type);
    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
    }


    // 보상형 광고 보기
    private void UserOptToWatchAd()
    {
        if (rewardBasedVideo.IsLoaded())
        {
            rewardBasedVideo.Show();
        }
    }
}