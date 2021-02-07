using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class RewardedAdsManager : MonoBehaviour, UnityEngine.Advertisements.IUnityAdsListener
{
    public static RewardedAdsManager Instanse;
#if UNITY_IOS
    private string gameId = "4001372";
#elif UNITY_ANDROID
    private string gameId = "4001373";
#endif

    public string myPlacementId = "rewardedErase";

    private System.Action _successAction;

    void Start()
    {
        Instanse = this;
        // Initialize the Ads listener and service:
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, true);
    }

    // Implement a function for showing a rewarded video ad:
    public void ShowRewardedVideo(System.Action successAction)
    {
        if(Advertisement.IsReady(myPlacementId))
        {
            _successAction = successAction;
            Advertisement.Show(myPlacementId);
        }
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsReady(string placementId)
    {
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            if(_successAction != null)
            {
                _successAction();
            }
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }
}