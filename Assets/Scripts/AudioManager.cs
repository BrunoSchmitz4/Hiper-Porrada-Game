using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Fontes de Áudio")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Clipe de Áudio")]
    public AudioClip background;
    public AudioClip death;
    public AudioClip addCombo;
    public AudioClip loseCombo;
    public AudioClip hitPunch;
    //public AudioClip missPunch;

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
