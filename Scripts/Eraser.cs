using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Advertisements;
public class Eraser : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    private enum State
    {
        NotReady = 1,
        Ready,
    }
    [SerializeField]
    private State _useState = State.NotReady;

    private RectTransform _rectTransform;

    private Vector3 _oriPosition;

    private bool _isDraging = false;
    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _oriPosition = _rectTransform.position;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(_useState == State.Ready)
        {
            _isDraging = true;
            Debug.Log("BeginUIInteract");
            EventUtil.SendMessage(EventType.BeginUIInteract, this);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(_isDraging)
        {
            Debug.Log("EndUIInteract");
            EventUtil.SendMessage(EventType.EndUIInteract, this);

            int layerMask = LayerMask.NameToLayer("Balloon");
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);
            foreach (var hit in hits)
            {
                var balloon = hit.collider.GetComponent<Balloon>();

                if (balloon)
                {
                    EventUtil.SendMessage(EventType.Destroy, balloon);
                    Destroy(balloon.gameObject);
                    _useState = State.NotReady;
                }
            }
            _rectTransform.position = _oriPosition;
        }
        _isDraging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isDraging)
        {
            Vector3 pos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(_rectTransform, eventData.position, eventData.enterEventCamera, out pos);
            _rectTransform.position = pos;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(_useState == State.NotReady)
        {

        }
    }
}