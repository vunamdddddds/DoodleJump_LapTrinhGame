using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;
using UnityEngine;
public class Banner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private BannerView bannerView; 
    public static Banner instance;
    void Awake()
    {

         if (instance == null)
    {
        instance = this;
    }
    else
    {
        Destroy(gameObject);
    }
    }
    
    public void Start()
    {   


        // Initialize Google Mobile Ads Unity Plugin.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
              
           if (Manager.removeADsStatus)
           {
                   this.RemoveBanner();


           }else
           {
            this.RequestBanner();
           }
        });
    }
public void RemoveBanner()
{
    if (bannerView != null)
    {
        bannerView.Destroy();
        bannerView = null;
    }
}
    

    // Update is called once per frame
    void Update()
    {
        
    }
    private void RequestBanner()
    {
       string adUnitId ="ca-app-pub-3032893771344210/9616570563";
    this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
     bannerView.LoadAd(new AdRequest());
    }
}
