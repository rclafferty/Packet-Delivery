using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameOptionsMenu : MonoBehaviour
{
    InGameOptionsMenu instance = null;

    // Necessary Managers
    [SerializeField] MusicManager musicManager;
    [SerializeField] LevelManager levelManager;
    [SerializeField] Transition fade;

    // Necessary UI Components
    [SerializeField] GameObject[] audioComponents;
    [SerializeField] GameObject[] instructionComponents;
    [SerializeField] GameObject[] quitComponents;
    [SerializeField] GameObject parentImage;

    [SerializeField] Slider volumeSlider;

    [SerializeField] Image fullScreenImage;

    string currentSceneName = "";

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        volumeSlider.value = musicManager.Volume;

        // Hide all except audio controls
        ToggleOptions(isAudio: false, isInstructions: false, isQuit: false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // If correct scene
            currentSceneName = SceneManager.GetActiveScene().name;

            if (currentSceneName != "loading" && currentSceneName != "title" && currentSceneName != "start_town" && currentSceneName != "credits")
            {
                if (parentImage.activeInHierarchy)
                {
                    // Disable
                    parentImage.SetActive(false);
                }
                else
                {
                    // Enable
                    parentImage.SetActive(true);

                    // Toggle -- Show audio by default
                    ToggleOptions(isAudio: true, isInstructions: false, isQuit: false);
                }
            }
        }
    }

    public void MusicVolumeChanged()
    {
        musicManager.Volume = volumeSlider.value;
    }

    public void ToggleMuteVolume()
    {
        musicManager.ToggleMute();
    }

    public void ToggleOptionsByString(string name)
    {
        if (name.ToLower() == "audio")
        {
            ToggleOptions(isAudio: true, isInstructions: false, isQuit: false);
        }
        else if (name.ToLower() == "instructions")
        {
            ToggleOptions(isAudio: false, isInstructions: true, isQuit: false);
        }
        else if (name.ToLower() == "quit")
        {
            ToggleOptions(isAudio: false, isInstructions: false, isQuit: true);
        }
        else if (name == "none")
        {
            parentImage.SetActive(false);
        }
    }

    void ToggleOptions(bool isAudio, bool isInstructions, bool isQuit)
    {
        if (isAudio == isInstructions == isQuit == false)
        {
            // hide
            parentImage.SetActive(false);
            return;
        }

        // If multiple are true, choose the first one
        if (isAudio && isInstructions)
        {
            isInstructions = false;
        }
        else if ((isAudio && isQuit) || (isInstructions && isQuit))
        {
            isQuit = false;
        }

        ToggleAudioControls(isAudio);
        ToggleInstructions(isInstructions);
        ToggleQuitMenu(isQuit);
    }

    void ToggleAudioControls(bool isShown)
    {
        // if isShown, show. Else, hide
        foreach (GameObject g in audioComponents)
        {
            g.SetActive(isShown);
        }
    }

    void ToggleInstructions(bool isShown)
    {
        // if isShown, show. Else, hide
        foreach (GameObject g in instructionComponents)
        {
            g.SetActive(isShown);
        }
    }

    void ToggleQuitMenu(bool isShown)
    {
        // if isShown, show. Else, hide
        foreach (GameObject g in quitComponents)
        {
            g.SetActive(isShown);
        }
    }

    public void InstructionImageClicked(Image image)
    {
        // Set fullscreen image to image
        fullScreenImage.sprite = image.sprite;

        // Show fullscreen image
        fullScreenImage.gameObject.SetActive(true);
    }

    public void HideFullScreenImage()
    {
        fullScreenImage.gameObject.SetActive(false);
    }

    public void QuitToTitleScreen()
    {
        GameObject.Find("Fade Canvas").GetComponent<Transition>().FadeMethod("title");
    }
}
