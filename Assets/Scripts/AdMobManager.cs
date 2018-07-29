using UnityEngine;
using System;
//using GoogleMobileAds.Api;

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

    public string android_banner_id;
    public string ios_banner_id;

    public string android_interstitial_id;
    public string ios_interstitial_id;

//    private BannerView bannerView;
//    private InterstitialAd interstitialAd;

//    public void Start()
//    {
//        MobileAds.Initialize(android_banner_id);
//        MobileAds.Initialize(ios_banner_id);

//        RequestBannerAd();
//        RequestInterstitialAd();

//        ShowBannerAd();
//    }

//    public void RequestBannerAd()
//    {
//        string adUnitId = string.Empty;

//#if UNITY_ANDROID
//        adUnitId = android_banner_id;
//#elif UNITY_IOS
//        adUnitId = ios_bannerAdUnitId;
//#endif

//        bannerView = new BannerView(adUnitId, new AdSize(576, 90), AdPosition.Bottom);
//        AdRequest request = new AdRequest.Builder().Build();

//        bannerView.LoadAd(request);
//    }

//    private void RequestInterstitialAd()
//    {
//        string adUnitId = string.Empty;

//#if UNITY_ANDROID
//        adUnitId = android_interstitial_id;
//#elif UNITY_IOS
//        adUnitId = ios_interstitialAdUnitId;
//#endif

//        interstitialAd = new InterstitialAd(adUnitId);
//        AdRequest request = new AdRequest.Builder().Build();

//        interstitialAd.LoadAd(request);

//        interstitialAd.OnAdClosed += HandleOnInterstitialAdClosed;
//    }

//    public void HandleOnInterstitialAdClosed(object sender, EventArgs args)
//    {
//        print("HandleOnInterstitialAdClosed event received.");

//        interstitialAd.Destroy();

//        RequestInterstitialAd();
//    }

//    public void ShowBannerAd()
//    {
//        bannerView.Show();
//    }

//    public void ShowInterstitialAd()
//    {
//        if (!interstitialAd.IsLoaded())
//        {
//            RequestInterstitialAd();
//            return;
//        }

//        interstitialAd.Show();
//    }

}


//Ref: http://minhyeokism.tistory.com/69 [programmer-dominic.kim]