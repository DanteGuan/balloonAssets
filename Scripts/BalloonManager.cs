using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonManager : MonoBehaviour
{
    [SerializeField]
    private Transform _balloonRoot;

    public Transform _shooter;
    private Balloon _currentBalloon;
    private Balloon _lastBalloon;

    [SerializeField]
    private Transform _finishTransform;

    private Vector3 _shooterTargetPos;

    private float _shooterLerpTime = 0.1f;
    private Vector3 _oriShooterPos;
    private float _shooterLerpTimer = 0;
    private float _shootTime = 0.8f;
    private float _shootTimer = 0f;
    private bool _isInUIInteract = false;

    private List<Balloon> _bollons = new List<Balloon>();
    // Start is called before the first frame update
    void Start()
    {
        //EventUtil.AddListener(EventType.FinishLaunch, finishLaunch);
        EventUtil.AddListener(EventType.Destroy, balloonDestroy);
        EventUtil.AddListener(EventType.GameFinish, gameFinish);
        EventUtil.AddListener(EventType.BeginUIInteract, beginUIInteract);
        EventUtil.AddListener(EventType.EndUIInteract, endUIInteract);
        EventUtil.AddListener(EventType.BalloonInteractBegin, balloonInteractBegin);
        EventUtil.AddListener(EventType.BalloonInteractDraging, balloonInteractDraging);
        EventUtil.AddListener(EventType.BalloonInteractEnd, balloonInteractEnd);

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

    void gameFinish(object args)
    {
        foreach(var balloon in _bollons)
        {
            Destroy(balloon.gameObject);
        }
        _bollons.Clear();
        _currentBalloon = randomGenerateBalloon();
        _lastBalloon = null;
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
        int id = Random.Range(1, 4);
        var balloon = BalloonFactory.Instance.GenerateBollon(id);
        _bollons.Add(balloon);
        balloon.transform.SetParent(_balloonRoot);
        return balloon;
    }

    // Update is called once per frame
    void Update()
    {
        _shootTimer += Time.deltaTime;
        if(_currentBalloon == null && _shootTime <= _shootTimer)
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
    }
}
