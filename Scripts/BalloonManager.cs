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

    private float _shooterLerpTime = 0.1f;
    private Vector3 _oriShooterPos;
    private float _shooterLerpTimer = 0;
    private float _shootTime = 0.8f;
    private float _shootTimer = 0f;

    private List<Balloon> _bollons = new List<Balloon>();
    // Start is called before the first frame update
    void Start()
    {
        //EventUtil.AddListener(EventType.FinishLaunch, finishLaunch);
        EventUtil.AddListener(EventType.Destroy, balloonDestroy);
        EventUtil.AddListener(EventType.GameFinish, gameFinish);

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
        }
        if (Input.GetMouseButtonDown(0))
        {
            _shooterLerpTimer = 0;
            _oriShooterPos = _shooter.position;
        }
        if(Input.GetMouseButton(0))
        {
            _shooterLerpTimer += Time.deltaTime;
            float prograss = _shooterLerpTimer / _shooterLerpTime;
            var mousePos = Input.mousePosition;
            mousePos.x = Mathf.Clamp(mousePos.x, 0, Screen.width);
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            var targetPos = new Vector3(mousePos.x, _shooter.position.y, _shooter.position.z);
            _shooter.position = Vector3.Lerp(_oriShooterPos, targetPos, prograss);
        }
        if(_currentBalloon != null)
        {
            _currentBalloon.transform.position = _shooter.position;
        }

        if (Input.GetMouseButtonUp(0) && _currentBalloon)
        {
            _currentBalloon.Shoot();
            _lastBalloon = _currentBalloon;
            _currentBalloon = null;
            _shootTimer = 0f;
        }
    }
}
