using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    // Start is called before the first frame update
    void Start()
    {
        EventUtil.AddListener(EventType.ScoreChange, OnScoreChange);
    }

    private void OnScoreChange(object args)
    {
        _scoreText.text = string.Format("Score: {0}", (int)args);
    }
}
