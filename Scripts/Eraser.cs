using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Advertisements;
public class Eraser : MonoBehaviour
{
    private enum State
    {
        NotReady = 1,
        Ready,
    }
    private State _useState = State.NotReady;

    [SerializeField]
    private Text _countText;
    private RectTransform _rectTransform;
    public GameObject _button;
    public GameObject _AdChoosePannel;

    private Vector3 _oriPosition;

    private bool _isDraging = false;
    private int _totalEraseTime = 3;
    private int _eraseTime = 3;
    // Start is called before the first frame update
    void Start()
    {
        EventUtil.AddListener(BallonEventType.EraseSuccess, eraseSuccess);
        EventUtil.AddListener(BallonEventType.StartGame, startGame);
        _rectTransform = GetComponent<RectTransform>();
        _oriPosition = _rectTransform.position;
        _AdChoosePannel.SetActive(false);
        _countText.text = _eraseTime.ToString();
    }

    private void OnEnable()
    {
        _AdChoosePannel.SetActive(false);
    }

    public void OnButtonClick()
    {
        _AdChoosePannel.SetActive(true);
    }

    public void OnYesButtonClick()
    {
        if (_useState == State.NotReady)
        {
            RewardedAdsManager.Instanse.ShowRewardedVideo(OnAdsSuccess);
        }
        _AdChoosePannel.SetActive(false);
    }

    public void OnNoButtonClick()
    {
        _AdChoosePannel.SetActive(false);
    }

    private void OnAdsSuccess()
    {
        _eraseTime -= 1;
        _countText.text = _eraseTime.ToString();
        if (_eraseTime <= 0)
        {
            gameObject.SetActive(false);
        }
        _useState = State.Ready;
        _button.SetActive(false);
        EventUtil.SendMessage(BallonEventType.AdSuccess);
    }

    private void eraseSuccess(object args)
    {
        _useState = State.NotReady;
        _button.SetActive(true);
    }

    private void startGame(object args)
    {
        _eraseTime = _totalEraseTime;
        _countText.text = _eraseTime.ToString();
    }
}