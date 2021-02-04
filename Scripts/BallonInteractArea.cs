using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BallonInteractArea : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private bool _isInteract = false;
    public void OnDrag(PointerEventData eventData)
    {
        if(_isInteract)
        {
            EventUtil.SendMessage(EventType.BalloonInteractDraging, eventData.position);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isInteract = true;
        EventUtil.SendMessage(EventType.BalloonInteractBegin, eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isInteract = false;
        EventUtil.SendMessage(EventType.BalloonInteractEnd, eventData.position);
    }
}
