using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCenter: MonoBehaviour
{
    public static DataCenter Instanse = null;
    private int _topScore;
    private int _score;
    private void Awake()
    {
        Instanse = this;
        Reset();
        EventUtil.AddListener(BallonEventType.AddScore, onAddScore);
        EventUtil.AddListener(BallonEventType.GameFinish, Reset);
        EventUtil.AddListener(BallonEventType.StartGame, Reset);
    }
    public void Reset(object args = null)
    {
        _topScore = PlayerPrefs.GetInt("topScore", 0);
        _score = 0;
    }

    private void onAddScore(object args)
    {
        _score += (int)args;
        if(_score > _topScore)
        {
            _topScore = _score;
            PlayerPrefs.SetInt("topScore", _topScore);
        }

        EventUtil.SendMessage(BallonEventType.ScoreChange, _score);
    }
}
