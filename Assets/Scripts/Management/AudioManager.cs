using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SFX { 
    PlayerAddCombo,
    PlayerLoseCombo,
    PlayerDeath,
    PlayerHitPunch,
    PlayerBackground, 
}


[Serializable]
struct SFXConfig
{
    public SFX type;
    public AudioClip AudioClip;
    public float VolumeScale;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource SFXAudioSource;
    [SerializeField] private AudioSource EnvironmentAudioSource;
    [SerializeField] private SFXConfig[] SFXConfigs;

    private Dictionary<SFX, SFXConfig> SFXs;

    // Obsoleto
    //[Header("Fontes de Áudio")]
    //[SerializeField] AudioSource musicSource;
    //[SerializeField] AudioSource SFXSource;

    //[Header("Clipe de Áudio")]
    //public AudioClip background;
    //public AudioClip death;
    //public AudioClip addCombo;
    //public AudioClip loseCombo;
    //public AudioClip hitPunch;

    private void Awake()
    {
        // Implementação Singleton com proteção de duplicatas
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Persiste entre cenas

        SFXs = SFXConfigs.ToDictionary(sfxConfig => sfxConfig.type, sfxConfig => sfxConfig);
    }

    // Obsoleto
    //private void Start()
    //{
    //    musicSource.clip = background;
    //    musicSource.Play();
    //}

    // Obsoleto
    //public void PlaySFX(AudioClip clip)
    //{
    //    if (SFXSource != null && clip != null)
    //    {
    //        SFXSource.PlayOneShot(clip);
    //    }
    //}

    public void PlaySFXAudioMixer(SFX type)
    {
        if (SFXs.ContainsKey(type))
        {
            SFXConfig config = SFXs[type];
            SFXAudioSource.PlayOneShot(config.AudioClip, config.VolumeScale);
        }
    }

    // Obsoleto
    //public void StopMusic()
    //{
    //    if (musicSource != null)
    //    {
    //        musicSource.Stop();
    //    }
    //}

    // Obsoleto
    //public void SetMusicVolume(float volume)
    //{
    //    if (musicSource != null)
    //    {
    //        musicSource.volume = Mathf.Clamp01(volume);
    //    }
    //}

    // Obsoleto
    //public void SetSFXVolume(float volume)
    //{
    //    if (SFXSource != null)
    //    {
    //        SFXSource.volume = Mathf.Clamp01(volume);
    //    }
    //}
}