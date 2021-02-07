using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonManager : MonoBehaviour
{
    private enum GameState
    {
        Running,
        ReadyToFinish,
        InAds,
        MainMenu,
    }
    private GameState _gameState = GameState.MainMenu;
    [SerializeField]
    private Transform _balloonRoot;

    public Transform _shooter;
    private Balloon _currentBalloon;
    private Balloon _lastBalloon;

    [SerializeField]
    private Transform _finishTransform;
    [SerializeField]
    private SpriteRenderer _finishImage;

    private Vector3 _shooterTargetPos;

    private float _shooterLerpTime = 0.1f;
    private Vector3 _oriShooterPos;
    private float _shooterLerpTimer = 0;
    private float _shootTime = 0.8f;
    private float _shootTimer = 0f;
    private bool _isInUIInteract = false;

    private List<Balloon> _bollons = new List<Balloon>();

    private bool _isGameStart = false;
    [SerializeField]
    private float _finishTime = 5.0f;
    [SerializeField]
    private int _maxGenerateID = 4;
    // Start is called before the first frame update
    void Start()
    {
        //EventUtil.AddListener(EventType.FinishLaunch, finishLaunch);
        EventUtil.AddListener(BallonEventType.StartGame, startGame);
        EventUtil.AddListener(BallonEventType.Destroy, balloonDestroy);
        EventUtil.AddListener(BallonEventType.BackToMainMenu, backToMainMenu);
        EventUtil.AddListener(BallonEventType.BeginUIInteract, beginUIInteract);
        EventUtil.AddListener(BallonEventType.EndUIInteract, endUIInteract);
        EventUtil.AddListener(BallonEventType.BalloonInteractBegin, balloonInteractBegin);
        EventUtil.AddListener(BallonEventType.BalloonInteractDraging, balloonInteractDraging);
        EventUtil.AddListener(BallonEventType.BalloonInteractEnd, balloonInteractEnd);
        EventUtil.AddListener(BallonEventType.EraseSuccess, eraseSuccess); 
        EventUtil.AddListener(BallonEventType.AdSuccess, adSuccess); 

        _finishTransform.gameObject.SetActive(false); 
        _gameState = GameState.MainMenu;
    }

    void startGame(object args)
    {
        _isGameStart = true;
        _lastBalloon = null;
        _currentBalloon = randomGenerateBalloon();

        var screenWidth = Screen.width;
        var screenHeight = Screen.height;
        var finishPos = Camera.main.ScreenToWorldPoint(
            new Vector3(screenWidth * 0.5f, screenHeight * 0.15f, 0));
        finishPos.z = 0;
        _finishTransform.position = finishPos;
        var shootPos = Camera.main.ScreenToWorldPoint(
            new Vector3(screenWidth * 0.5f, screenHeight * 0.1f, 0));
        shootPos.z = 0;
        _shooter.position = shootPos;
        _shooterTargetPos = shootPos;
        _finishTransform.gameObject.SetActive(true);
        _gameState = GameState.Running;
    }

    void adSuccess(object args)
    {
        _gameState = GameState.InAds;
    }

    void eraseSuccess(object args)
    {
        _gameState = GameState.Running;
        foreach (var balloon in _bollons)
        {
            balloon.ResetInFinishTime();
        }
    }

    void balloonInteractBegin(object args)
    {
        _shooterLerpTimer = 0;
        _oriShooterPos = _shooter.position;
        balloonInteractDraging(args);
    }

    void balloonInteractDraging(object args)
    {
        var mousePos = Input.mousePosition;
        mousePos.x = Mathf.Clamp(mousePos.x, 0, Screen.width);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        _shooterTargetPos = new Vector3(mousePos.x, _shooter.position.y, _shooter.position.z);
    }

    void balloonInteractEnd(object args)
    {
        if(_currentBalloon)
        {
            _currentBalloon.Shoot();
            _lastBalloon = _currentBalloon;
            _currentBalloon = null;
            _shootTimer = 0f;
        }
    }

    void beginUIInteract(object args)
    {
        _isInUIInteract = true;
    }

    void endUIInteract(object args)
    {
        _isInUIInteract = false;
    }

    void backToMainMenu(object args)
    {
        _gameState = GameState.MainMenu;
        foreach (var balloon in _bollons)
        {
            Destroy(balloon.gameObject);
        }
        _bollons.Clear();
        _isGameStart = false;
        _finishTransform.gameObject.SetActive(false);
    }

    void balloonDestroy(object args)
    {
        Balloon balloon = (Balloon)args;
        _bollons.Remove(balloon);
    }

    void finishLaunch(object args)
    {
        if(_currentBalloon == null && _lastBalloon == (Balloon)args)
        {
            _currentBalloon = randomGenerateBalloon();
        }
    }

    Balloon randomGenerateBalloon()
    {
        int id = Random.Range(1, _maxGenerateID);
        var balloon = BalloonFactory.Instance.GenerateBollon(id);
        _bollons.Add(balloon);
        balloon.transform.SetParent(_balloonRoot);
        return balloon;
    }

    bool shouldCheckFinish()
    {
        if (_gameState == GameState.InAds)
            return false;
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isGameStart)
            return;
        _shootTimer += Time.deltaTime;
        if (_currentBalloon == null && _shootTime <= _shootTimer)
        {
            _currentBalloon = randomGenerateBalloon();
            _currentBalloon.transform.position = _shooter.position;
        }
        _shooterLerpTimer += Time.deltaTime;
        float prograss = _shooterLerpTimer / _shooterLerpTime;
        _shooter.position = Vector3.Lerp(_oriShooterPos, _shooterTargetPos, prograss);

        if (_currentBalloon != null)
        {
            _currentBalloon.transform.position = _shooter.position;
        }
        var currentTime = Time.time;
        float nearestTime = -1;
        var shouldWarning = false;
        _finishImage.enabled = false;
        foreach (var balloon in _bollons)
        {
            if (balloon.isFinishShooting && balloon.inFinishAreaTime > 0)
            {
                _finishImage.enabled = true;
                shouldWarning = true;
                var diff = currentTime - balloon.inFinishAreaTime;
                if (diff > nearestTime)
                    nearestTime = diff;
            }
            if(balloon.isFinishShooting && !_finishImage.enabled)
            {
                if(balloon.transform.position.y - _finishTransform.position.y < 2)
                {
                    _finishImage.enabled = true;
                }
            }
        }
        if (shouldCheckFinish())
        {
            if (shouldWarning)
            {
                float leftTime = _finishTime - nearestTime;
                if (leftTime > 0f)
                {
                    EventUtil.SendMessage(BallonEventType.GameoverWarning, Mathf.CeilToInt(leftTime));
                }
                else if (_gameState != GameState.ReadyToFinish)
                {
                    _gameState = GameState.ReadyToFinish;
                    EventUtil.SendMessage(BallonEventType.ReadyToFinish);
                }
            }
            else
            {
                EventUtil.SendMessage(BallonEventType.CancleGameoverWarning);
            }
        }
    }
}
