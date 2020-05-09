using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    // Necessary Managers
    [SerializeField] MusicManager musicManager;

    // Necessary UI Components
    [SerializeField] GameObject[] menuComponents;
    [SerializeField] Slider volumeSlider;
    
    // Start is called before the first frame update
    void Start()
    {
        musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
        volumeSlider.value = musicManager.Volume;
    }

    public void MusicVolumeChanged()
    {
        musicManager.Volume = volumeSlider.value;
    }

    public void ToggleMuteVolume()
    {
        musicManager.ToggleMute();
    }

    public void ToggleMenuItems(bool isShown)
    {
        foreach (GameObject g in menuComponents)
        {
            g.SetActive(isShown);
        }
    }
}
