using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCenter: MonoBehaviour
{
    private int _score;
    private void Awake()
    {
        Reset();
        EventUtil.AddListener(EventType.AddScore, onAddScore);
        EventUtil.AddListener(EventType.GameFinish, Reset);
    }
    public void Reset(object args = null)
    {
        _score = 0;
    }

    private void onAddScore(object args)
    {
        _score += (int)args;

        EventUtil.SendMessage(EventType.ScoreChange, _score);
    }
}
