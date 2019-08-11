using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdMobManager : MonoBehaviour
{
    private BannerView bannerView;
    private RewardedAd rewardedAd;

    [SerializeField] private string appID = "ca-app-pub-1406585743046836~2466457723";
    [SerializeField] private string bannerID = "ca-app-pub-1406585743046836/7335641022";
    [SerializeField] private string rewardID = "ca-app-pub-1406585743046836/9271307712";
        //"ca-app-pub-1406585743046836/9271307712";

    void Start()
    {
        MobileAds.Initialize(appID);
        


        this.RequestReward();
    }

    // Returns an ad request with custom ad targeting.
    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

    private void RequestReward()
    {
        // Create new rewarded ad instance.
        this.rewardedAd = new RewardedAd(rewardID);

        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the ad is done
        this.rewardedAd.OnUserEarnedReward += HandleRewardBasedVideoRewarded;

        // Create an empty ad request.
        AdRequest request = this.CreateAdRequest();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    public void UserOptToWatchAd()
    {
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
        else
        {
            MonoBehaviour.print("Rewarded ad is not ready yet");
        }
    }

    public void OnClickShowBanner()
    {
        this.RequestBanner();
    }

    public void DeleteBanner()
    {
        bannerView.Destroy();
    }

    private void RequestBanner()
    {
        bannerView = new BannerView(bannerID, AdSize.Banner, AdPosition.Bottom);

        AdRequest request = new AdRequest.Builder().Build();

        bannerView.LoadAd(request);
    }


    #region RewardedAd callback handlers

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: " + args.Message);
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: " + args.Message);
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        int amount = (int)args.Amount;
        PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore", 0) + amount);
    }

    #endregion
}
