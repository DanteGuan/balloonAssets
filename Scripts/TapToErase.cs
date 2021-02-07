using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TapToErase : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private bool _isInTap = false;

    private void OnEnable()
    {
        _isInTap = false;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isInTap)
        {
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);
            foreach (var hit in hits)
            {
                var balloon = hit.collider.GetComponent<Balloon>();

                if (balloon)
                {
                    var particle = balloon.DestroySelf();
                    particle.Play();
                    SoundManager.Instance.PlayMergeSound();
                    StartCoroutine("ParticleDestroyCroutine", particle.gameObject);
                    _isInTap = false;
                    EventUtil.SendMessage(BallonEventType.EraseSuccess);
                    break;
                }
            }
        }
    }

    private IEnumerable ParticleDestroyCroutine(GameObject particle)
    {
        yield return new WaitForSeconds(6f);
        Destroy(particle);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _isInTap = true;
    }
}
