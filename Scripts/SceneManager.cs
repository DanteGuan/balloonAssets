using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField]
    private Transform _ceil;
    [SerializeField]
    private Transform _leftWall;
    [SerializeField]
    private Transform _rightWall;
    [SerializeField]
    private Transform _floor;

    // Start is called before the first frame update
    private void Awake()
    {
#if UNITY_IOS
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
#endif
    }
    void Start()
    {
        resetScene();
    }

    void resetScene()
    {
        var screenWidth = Screen.width;
        var screenHeight = Screen.height;
        _ceil.position = Camera.main.ScreenToWorldPoint(new Vector3(screenWidth * 0.5f, screenHeight, 0));
        _floor.position = Camera.main.ScreenToWorldPoint(new Vector3(screenWidth * 0.5f, 0, 0));
        _leftWall.position = Camera.main.ScreenToWorldPoint(new Vector3(0, screenHeight * 0.5f, 0));
        _rightWall.position = Camera.main.ScreenToWorldPoint(new Vector3(screenWidth, screenHeight * 0.5f, 0));
    }
}
