using UnityEngine;
using GoogleMobileAds.Api;


public class GoogleMobileAdsImplementation : MonoBehaviour
{
    // GoogleMobileAdsImplementation singelton
    public static GoogleMobileAdsImplementation instance;

    private BannerView bannerView; // Banner AD
    private InterstitialAd interstitial; // FullScreen AD
    private bool bannerState;

    void Start()
    {
        // Tnstantiating the GoogleMobileAdsImplementation singelton
        if (instance == null)
        {
            instance = this;
            bannerState = false;
        }
        // Setting the app id Based on the OS
        #if UNITY_ANDROID
            string appId = "yourAppId";
        #elif UNITY_IPHONE
            string appId = "yourAppId";
        #else
        string appId = "unexpected_platform";
        #endif

        // Initialize the app
        MobileAds.Initialize(appId);
    }

    public void RequestBanner()
    {
        if (!bannerState)
        {
            // Setting the AD Mob ID's Based on the OS

        #if UNITY_ANDROID
            string adUnitId = "yourAdUnitId";
        #elif UNITY_IPHONE
            string adUnitId = "yourAdUnitId";
        #else
            string adUnitId = "unexpected_platform";
        #endif
            // Creating the banner and posting it on the bottom of the screen
            bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

            // Request the AD to show on the banner
            AdRequest request = new AdRequest.Builder().Build();
            
            // Load the requested AD on the banner
            bannerView.LoadAd(request);
            bannerState = true;
        }
    }

    public void RequestInterstitial()
    {
    #if UNITY_ANDROID
        string adUnitId = " yourAdUnitId";
    #elif UNITY_IPHONE
        string adUnitId = "yourAdUnitId";
    #else
        string adUnitId = "unexpected_platform";
    #endif

        interstitial = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);
    }

    public void ShowInterstitial()
    {
        // Show the fullscrean AD
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
    }

    public void HideBanner()
    {
        // Hide the Banner
        try
        {
            bannerView.Hide();
            bannerState = false;
        }
        catch(System.Exception e)
        {
            e.GetBaseException();
        }
    }
}
