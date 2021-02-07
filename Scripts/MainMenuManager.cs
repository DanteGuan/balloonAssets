using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private Text _topScoreText;
    [SerializeField]
    private Image _balloon;
    private void OnEnable()
    {
        int topScore = PlayerPrefs.GetInt("topScore", 0);
        setTopScoreText(topScore);
        var topBalloon = PlayerPrefs.GetInt("topBalloonID", 1);
        setTopBalloon(topBalloon);
    }

    public void OnStartButtonClicked()
    {
        EventUtil.SendMessage(BallonEventType.StartGame);
    }

    private void setTopScoreText(int score)
    {
        _topScoreText.text = string.Format("Top Score: {0}", score);
    }

    private void setTopBalloon(int balloonConfigId)
    {
        var config = BalloonConfig.Instance.GetConfig(balloonConfigId);
        _balloon.sprite = LoadSourceSprite(config.imagePath);
    }
    private Sprite LoadSourceSprite(string relativePath)
    {
        Object Preb = Resources.Load(relativePath, typeof(Sprite));
        Sprite tmpsprite = null;
        try
        {
            tmpsprite = Instantiate(Preb) as Sprite;
        }
        catch (System.Exception ex)
        {

        }

        return tmpsprite;
    }
}
