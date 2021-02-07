using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _mainMenu;
    [SerializeField]
    private GameObject _erasePannel;
    [SerializeField]
    private GameObject _adPannel;
    [SerializeField]
    private GameObject _balloonInteractPanel;
    [SerializeField]
    private GameObject _gameOverPanel;
    [SerializeField]
    private Text _gameoverWarningText;

    [SerializeField]
    private Text _scoreText;
    // Start is called before the first frame update
    void Start()
    {
        EventUtil.AddListener(BallonEventType.ScoreChange, OnScoreChange);
        EventUtil.AddListener(BallonEventType.StartGame, StartGame);
        EventUtil.AddListener(BallonEventType.AdSuccess, adSuccess);
        EventUtil.AddListener(BallonEventType.EraseSuccess, eraseSuccess);
        EventUtil.AddListener(BallonEventType.BackToMainMenu, BackToMainMenu);
        EventUtil.AddListener(BallonEventType.ReadyToFinish, onGameFinish);
        EventUtil.AddListener(BallonEventType.GameoverWarning, gameoverWarning);
        EventUtil.AddListener(BallonEventType.CancleGameoverWarning, cancleGameoverWarning);
        BackToMainMenu();
    }

    private void onGameFinish(object args)
    {
        setAllActive(false);
        _gameOverPanel.SetActive(true);
    }

    private void OnScoreChange(object args)
    {
        _scoreText.text = string.Format("Score: {0}", (int)args);
    }

    private void setAllActive(bool isActive)
    {
        _mainMenu.SetActive(isActive);
        _erasePannel.SetActive(isActive);
        _adPannel.SetActive(isActive);
        _balloonInteractPanel.SetActive(isActive);
        _scoreText.gameObject.SetActive(isActive);
        _gameOverPanel.SetActive(isActive);
        _gameoverWarningText.gameObject.SetActive(isActive);
    }

    private void BackToMainMenu(object args = null)
    {
        setAllActive(false);
        _mainMenu.SetActive(true);
    }

    private void StartGame(object args)
    {
        setAllActive(false);
        _adPannel.SetActive(true);
        _balloonInteractPanel.SetActive(true);
        _scoreText.text = string.Format("Score: {0}", 0);
        _scoreText.gameObject.SetActive(true);
    }

    private void adSuccess(object args)
    {
        setAllActive(false);
        _erasePannel.SetActive(true);
    }

    private void eraseSuccess(object args)
    {
        setAllActive(false);
        _adPannel.SetActive(true);
        _balloonInteractPanel.SetActive(true);
        _scoreText.gameObject.SetActive(true);
    }

    private void gameoverWarning(object args)
    {
        _gameoverWarningText.text = string.Format("{0}", (int)args);
        _gameoverWarningText.gameObject.SetActive(true);
    }

    private void cancleGameoverWarning(object args)
    {
        if(_gameoverWarningText.gameObject.activeSelf)
        {
            _gameoverWarningText.gameObject.SetActive(false);
        }
    }
}
