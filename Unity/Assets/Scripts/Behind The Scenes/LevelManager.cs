using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    static LevelManager instance = null;
    static PlayerController playerInstance = null;

    readonly string[] LEVEL_NAMES = { "centralLookupAgency", "loading", "localLookupAgencyNE", "localLookupAgencySW", "office", "styleTitle", "title", "town" };

    readonly string[] INACTIVE_LEVELS = { "centralLookupAgency", "loading", "localLookupAgencyNE", "localLookupAgencySW", "office", "styleTitle", "title" };

    readonly string[] ACTIVE_LEVELS = { "town" };

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // Claim this object as the singleton
        instance = this;

        // Keep this object between scenes
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerController(PlayerController pc)
    {
        playerInstance = pc;
    }

    /// <summary>
    /// Load level by index
    /// </summary>
    /// <param name="index"></param>
    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);

        // if (index != 2), set player inactive
    }

    /// <summary>
    /// Load level by name
    /// </summary>
    /// <param name="name"></param>
    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);

        foreach (string s in ACTIVE_LEVELS)
        {
            if (s == name)
            {
                ShowPlayer(true);
                return;
            }
        }

        ShowPlayer(false);
    }

    public void ShowPlayer(bool tf)
    {
        playerInstance.gameObject.SetActive(tf);
    }
    
    /// <summary>
    /// Quit the game
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}