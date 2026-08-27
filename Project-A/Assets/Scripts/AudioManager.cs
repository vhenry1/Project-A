using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    


    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource musicSource;

    public GameObject musicObject; // Reference to the music GameObject
    public GameObject sfxObject; // Reference to the SFX GameObject

    [Header("Audio Clips")]
    public AudioClip damageSound;
    public AudioClip winSound;

    private void Awake()
    {
        if (musicObject != null)
        {
            DontDestroyOnLoad(musicObject);
        }
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}