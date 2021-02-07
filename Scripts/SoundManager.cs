using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField]
    private List<AudioClip> _impactAudios;
    [SerializeField]
    private List<AudioClip> _mergeAudios;

    private AudioSource _source;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        _source = GetComponent<AudioSource>();
    }

    public void PlayImpactSound()
    {
        _source.volume = 1;
        _source.PlayOneShot(_impactAudios[Random.Range(0, _impactAudios.Count)]);
    }

    public void PlayMergeSound()
    {
        _source.volume = 0.3f;
        _source.PlayOneShot(_mergeAudios[Random.Range(0, _mergeAudios.Count)]);
    }
}
