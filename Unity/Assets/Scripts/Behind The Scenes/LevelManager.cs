using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    static LevelManager instance = null;

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

    /// <summary>
    /// Load level by index
    /// </summary>
    /// <param name="index"></param>
    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }

    /// <summary>
    /// Load level by name
    /// </summary>
    /// <param name="name"></param>
    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
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