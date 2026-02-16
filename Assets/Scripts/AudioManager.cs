using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Fontes de Áudio")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Clipe de Áudio")]
    public AudioClip background;
    public AudioClip death;
    public AudioClip addCombo;
    public AudioClip loseCombo;
    public AudioClip hitPunch;

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
    }

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (SFXSource != null && clip != null)
        {
            SFXSource.PlayOneShot(clip);
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = Mathf.Clamp01(volume);
        }
    }

    public void SetSFXVolume(float volume)
    {
        if (SFXSource != null)
        {
            SFXSource.volume = Mathf.Clamp01(volume);
        }
    }
}