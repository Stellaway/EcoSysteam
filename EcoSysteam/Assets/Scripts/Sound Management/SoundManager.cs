using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource _musicSource, _deathEffectSource, _spawnEffectSource;
    [SerializeField] private float Volume = 0.2f;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
        } else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        AudioListener.volume = Volume;
    }

    public void PlaySound(AudioClip clip)
    {
        _deathEffectSource.PlayOneShot(clip);
    }
}
