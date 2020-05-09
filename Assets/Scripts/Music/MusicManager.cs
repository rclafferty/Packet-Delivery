/* File: MusicManager.cs
 * Author: Casey Lafferty
 * Project: Packet Delivery
 */

using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Music Manager singleton reference
    static MusicManager instance = null;

    // Store the object playing the music in the scene(s)
    [SerializeField] AudioSource musicSource;

    // Store the .wav file being played in the background
    AudioClip musicClip;

    private void Awake()
    {
        // Only use one music manager at a time
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        musicSource = GameObject.Find("MusicSource").GetComponent<AudioSource>();
        musicSource.volume = 0.35f;
    }
    
    public void SetAudioClip(AudioClip ac)
    {
        // Save music clip
        musicClip = ac;

        // Set music clip in AudioSource
        musicSource.clip = musicClip;

        // Loop the clip
        musicSource.loop = true;
    }

    public void Play()
    {
        // Play via the audio source
        musicSource.Play();
    }

    public void Pause()
    {
        // Pause via the audio source
        musicSource.Pause();
    }

    public void Stop()
    {
        // Stop via the audio source
        musicSource.Stop();
    }

    public void ToggleMute()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void SetVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public float Volume
    {
        get
        {
            float volume = musicSource.volume;
            Debug.Log("Music at " + volume + "% volume");
            return volume;
        }
        set
        {
            musicSource.volume = value;
            Debug.Log("Music now set to " + value + "% volume");
        }
    }
}
