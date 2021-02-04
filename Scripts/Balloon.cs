using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    public int configID = 1;
    [SerializeField]
    private CircleCollider2D _collider;
    [SerializeField]
    private SpriteRenderer _image;
    [SerializeField]
    private ParticleSystem _mergeParticle;

    private Rigidbody2D _rigidBody;
    private ConfigNode _config;

    private float _mergeTime = 0.3f;
    private bool _canMerge = true;
    private bool _shooting = true;

    private float _finishTimer = -1;
    private float _finishTime = 5;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY;
        _canMerge = false;
    }

    public void Init(int id)
    {
        this.configID = id;
        var config = BalloonConfig.Instance.GetConfig(id);
        this._config = config;
        //_collider.radius = config.size;
        _image.sprite = LoadSourceSprite(config.imagePath);
        _rigidBody.gravityScale = -config.size;
    }

    public void Shoot()
    {
        _rigidBody.constraints = RigidbodyConstraints2D.None;
        GetComponent<ConstantForce2D>().force = new Vector2(0, 150);
        _rigidBody.WakeUp();
        _canMerge = true;
        _shooting = true;
    }

    public void setScale()
    {
        float scale = _config.size;
        transform.localScale = new Vector3(scale, scale, scale);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        processCollide(collision);
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        processCollide(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_shooting && collision.CompareTag("Finish"))
        {
            _finishTimer = 0.01f;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_finishTimer >= 0)
        {
            _finishTimer += Time.fixedDeltaTime;
            if (_finishTimer >= _finishTime)
            {
                EventUtil.SendMessage(EventType.GameFinish, this);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish"))
        {
            _finishTimer = -1;
        }
    }

    private void processCollide(Collision2D collision)
    {
        if (_shooting && (collision.collider.CompareTag("Ceil") || collision.collider.CompareTag("Balloon")))
        {
            _shooting = false;
            EventUtil.SendMessage(EventType.FinishLaunch, this);
        }
        if(!_shooting)
        {
            var other = collision.collider.GetComponent<Balloon>();
            if (other && other._config.size == this._config.size)
            {
                merge(this, other);
            }
        }
    }

    private void merge(Balloon one, Balloon two)
    {
        if(!one._canMerge || !two._canMerge)
        {
            return;
        }
        var compareValueOne = one.transform.position.y;
        var compareValueTwo = two.transform.position.y;
        if(compareValueOne >= compareValueTwo)
        {
            one.mergeFrom(two);
        }
        else
        {
            two.mergeFrom(one);
        }
    }

    private void mergeFrom(Balloon from)
    {
        if (this._config.nextID > 0)
        {
            _mergeParticle.Stop();
            _mergeParticle.Play();
            upgrade();
            from._canMerge = false;
            _canMerge = false;
            StartCoroutine(mergeCoroutine(from._image));

            EventUtil.SendMessage(EventType.Destroy, from);
            Destroy(from.gameObject);
        }
    }
    private void upgrade()
    {
        EventUtil.SendMessage(EventType.AddScore, _config.score);
        _collider.enabled = false;
        this.Init(this._config.nextID);
        _collider.enabled = true;
    }

    IEnumerator mergeCoroutine(SpriteRenderer other)
    {
        float mergeTimer = 0;
        other.transform.SetParent(transform);
        var oriLocalPos = other.transform.localPosition;
        Vector3 targetScale = new Vector3(_config.size, _config.size, _config.size);
        Vector3 oriScale = transform.localScale;
        while (mergeTimer < _mergeTime)
        {
            mergeTimer += Time.deltaTime;
            mergeTimer = Mathf.Clamp(mergeTimer, 0, _mergeTime);
            var scale = _mergeTime - mergeTimer;
            var prograss = mergeTimer / _mergeTime;
            other.transform.localPosition = Vector3.Lerp(oriLocalPos, _image.transform.localPosition,
                prograss);
            other.transform.localScale = new Vector3(scale, scale, scale);
            transform.localScale = Vector3.Lerp(oriScale, targetScale, prograss);
            yield return null;
        }
        transform.localScale = targetScale;
        Destroy(other.gameObject);
        _canMerge = true;
    }

    public Sprite LoadSourceSprite(string relativePath)
    {
        Object Preb = Resources.Load(relativePath, typeof(Sprite));
        Sprite tmpsprite = null;
        try
        {
            tmpsprite = Instantiate(Preb) as Sprite;
        }
        catch (System.Exception ex)
        {

        }

        return tmpsprite;
    }

}
