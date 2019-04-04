using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    static MusicManager instance = null;

    [SerializeField]
    AudioSource musicSource;
    AudioClip musicClip;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource = GameObject.Find("MusicSource").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        musicSource.Play();
    }

    public void Pause()
    {
        musicSource.Pause();
    }

    public void Stop()
    {
        musicSource.Stop();
    }

    public void ResetMusic()
    {
        musicSource.Stop();
        musicSource.Play();
    }
}
