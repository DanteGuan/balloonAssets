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
    private bool _shooting = false;

    public bool isFinishShooting = false;

    public float inFinishAreaTime = -1;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY;
        _canMerge = false; 
        _shooting = false;
    }

    public void Init(int id)
    {
        this.configID = id;
        var config = BalloonConfig.Instance.GetConfig(id);
        this._config = config;
        //_collider.radius = config.size;
        _image.sprite = LoadSourceSprite(config.imagePath);
        _rigidBody.gravityScale = -config.size;
        _rigidBody.mass = config.mass;
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
            inFinishAreaTime = Time.time;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!_shooting && collision.CompareTag("Finish") && inFinishAreaTime < 0)
        {
            inFinishAreaTime = Time.time;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish"))
        {
            inFinishAreaTime = -1;
        }
    }

    public void ResetInFinishTime()
    {
        inFinishAreaTime = -1;
    }

    private void processCollide(Collision2D collision)
    {
        if (_shooting && (collision.collider.CompareTag("Ceil") || collision.collider.CompareTag("Balloon")))
        {
            isFinishShooting = true;
            _shooting = false;
            EventUtil.SendMessage(BallonEventType.FinishLaunch, this);
            SoundManager.Instance.PlayImpactSound();
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
            if(this._config.nextID >= 1)
            {
                SoundManager.Instance.PlayMergeSound();
            }
            _mergeParticle.Stop();
            _mergeParticle.Play();
            upgrade();
            from._canMerge = false;
            _canMerge = false;
            StartCoroutine(mergeCoroutine(from._image));

            EventUtil.SendMessage(BallonEventType.Destroy, from);
            Destroy(from.gameObject);
        }
    }

    public ParticleSystem DestroySelf()
    {
        EventUtil.SendMessage(BallonEventType.Destroy, this);
        _mergeParticle.transform.SetParent(transform.parent);
        Destroy(this.gameObject);
        return _mergeParticle;
    }
    private void upgrade()
    {
        EventUtil.SendMessage(BallonEventType.AddScore, _config.score);
        _collider.enabled = false;
        this.Init(this._config.nextID);
        _collider.enabled = true;
        if(this._config.id > PlayerPrefs.GetInt("topBalloonID", 1))
        {
            PlayerPrefs.SetInt("topBalloonID", this._config.id);
        }
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
