using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class AudioManager : MonoBehaviour
{

    [SerializeField] private AudioSource SFXAudioSource;
    [SerializeField] private AudioSource EnvironmentAudioSource;
    [SerializeField] private SFXConfig[] SFXConfigs;

    private Dictionary<SFX, SFXConfig> SFXs;

    private void Awake()
    {
        SFXs = SFXConfigs.ToDictionary(sfxc => sfxc.Type, sfxc => sfxc);
    }

    public void PlaySFX(SFX type)
    {
        if (SFXs.ContainsKey(type))
        {
            SFXConfig config = SFXs[type];
            SFXAudioSource.PlayOneShot(config.AudioClip, config.VolumeScale);
        }
    }

    
    [Header("Fontes de Áudio")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Clipe de Áudio")]
    public AudioClip background;
    public AudioClip death;
    public AudioClip addCombo;
    public AudioClip loseCombo;
    public AudioClip hitPunch;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    } 
    
}

public enum SFX
{
    playerDeath,
    playerAddCombo,
    playerLoseCombo,
    playerHitPunch
}

[SerializeField]
struct SFXConfig
{
    public SFX Type;
    public AudioClip AudioClip;
    public float VolumeScale;
}
