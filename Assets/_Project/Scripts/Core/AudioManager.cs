using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Clips de Inventario")]
    public AudioClip pickUpSound;
    public AudioClip dropSound;
    public AudioClip craftSuccessSound;
    public AudioClip errorSound;

    private AudioSource source;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            // PlayOneShot allow the sound to overlap without cutting
            source.PlayOneShot(clip);
        }
    }
}