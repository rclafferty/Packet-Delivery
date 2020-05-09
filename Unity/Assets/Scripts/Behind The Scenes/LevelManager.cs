/* File: LevelManager.cs
 * Author: Casey Lafferty
 * Project: Packet Delivery
 */

using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    // Level Manager singleton reference
    static LevelManager instance = null;
    
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

        // Call OnSceneLoaded() every time a new scene loads
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene newScene, LoadSceneMode loadSceneMode)
    {
        if (newScene.name == "instructions")
        {
            // No longer need this method being called for every scene change
            SceneManager.sceneLoaded -= OnSceneLoaded;

            /* // Find the "continue" button
            Button continueButton = GameObject.Find("Continue Button").GetComponent<Button>();

            // Remove any unnecessary listeners
            continueButton.onClick.RemoveAllListeners();

            // Get the Fade Canvas object and components
            Transition fadeTransition = GameObject.Find("Fade Canvas").GetComponent<Transition>();

            // Construct the UnityAction to be called on button press
            UnityAction continueToOfficeEvent = delegate { fadeTransition.FadeMethod("office"); };

            // Add the UnityAction to the "continue" button press event
            continueButton.onClick.AddListener(continueToOfficeEvent); */
        }
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