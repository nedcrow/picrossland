using UnityEngine;
using System;
using GoogleMobileAds.Api;

public class AdMobManager : MonoBehaviour
{
    #region Singleton
    private static AdMobManager _instance;
    public static AdMobManager instance
    {
        get
        {
            if (!_instance)
            {
                GameObject obj = GameObject.Find("AdMobManager");
                if (obj == null)
                {
                    obj = new GameObject("AdMobManager");
                    obj.AddComponent<AdMobManager>();
                }
                return obj.GetComponent<AdMobManager>();
            }
            return _instance;
        }
    }
    #endregion
    // app ID : banner : ca-app-pub-1894471406717971/1502663830   Interstitial : ca-app-pub-1894471406717971/7714200031

    public string android_banner_id = "ca-app-pub-3940256099942544/6300978111";
    public string ios_banner_id = "a-app-pub-3940256099942544/2934735716";

    public string android_interstitial_id = "ca-app-pub-3940256099942544/1033173712";
    public string ios_interstitial_id = "ca-app-pub-3940256099942544/4411468910";

    private BannerView bannerView;
    private InterstitialAd interstitialAd;

    public void Start()
    {
        MobileAds.Initialize(android_banner_id);
        MobileAds.Initialize(ios_banner_id);

        RequestBannerAd();
        RequestInterstitialAd();

        DontDestroyOnLoad(transform.gameObject);
        //ShowBannerAd();
    }

    public void RequestBannerAd()
    {
        string adUnitId = string.Empty;

#if UNITY_ANDROID
        adUnitId = android_banner_id;
#elif UNITY_IOS
            adUnitId = ios_bannerAdUnitId;
#endif

        bannerView = new BannerView(adUnitId, new AdSize(320, 50), AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();

        bannerView.LoadAd(request);
    }

    private void RequestInterstitialAd()
    {
        string adUnitId = string.Empty;

#if UNITY_ANDROID
        adUnitId = android_interstitial_id;
#elif UNITY_IOS
            adUnitId = ios_interstitialAdUnitId;
#endif

        interstitialAd = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();

        interstitialAd.LoadAd(request);

        interstitialAd.OnAdClosed += HandleOnInterstitialAdClosed;
    }

    public void HandleOnInterstitialAdClosed(object sender, EventArgs args)
    {
        print("HandleOnInterstitialAdClosed event received.");

        interstitialAd.Destroy();

        RequestInterstitialAd();
    }

    public void ShowBannerAd()
    {
        bannerView.Show();
    }

    public void ShowInterstitialAd()
    {
        if (!interstitialAd.IsLoaded())
        {
            RequestInterstitialAd();
            return;
        }

        interstitialAd.Show();
    }

}


//Ref: http://minhyeokism.tistory.com/69 [programmer-dominic.kim]