using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public void OnYesButtonClicked()
    {
        RewardedAdsManager.Instanse.ShowRewardedVideo(OnAdsSuccess);
    }

    public void OnAdsSuccess()
    {
        EventUtil.SendMessage(BallonEventType.AdSuccess);
    }

    public void OnNoButtonClicked()
    {
        EventUtil.SendMessage(BallonEventType.BackToMainMenu);
    }
}
