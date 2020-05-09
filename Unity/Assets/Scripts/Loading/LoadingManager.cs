/* File: LoadingManager.cs
 * Author: Casey Lafferty
 * Project: Packet Delivery
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    // Necessary manager references
    [SerializeField] GameplayManager gameplayManager;
    [SerializeField] LetterManager letterManager;
    [SerializeField] LevelManager levelManager;
    [SerializeField] MusicManager musicManager;
    [SerializeField] StartSceneLoader startSceneLoader;
    [SerializeField] LookupAgencyManager lookupAgencyManager;
    [SerializeField] UpgradeManager upgradeManager;

    // Background music file
    [SerializeField] AudioClip music;

    // NPC and player sprites for starting dialogue
    [SerializeField] Sprite sprite_PlayerRight;
    [SerializeField] Sprite sprite_PlayerDown;
    [SerializeField] Sprite sprite_NPCLeft;
    
    void Start()
    {
        // Load and connect all necessary values and references
        LinkManagersAndInitializeValues();
        AssignInstantiatedObjectNames();

        // Once all is loaded, change to title scene
        levelManager.LoadLevel("title");
    }

    void AssignInstantiatedObjectNames()
    {
        // Change names of objects for consistency
        gameplayManager.name = "GameplayManager";
        letterManager.name = "LetterManager";
        levelManager.name = "LevelManager";
        musicManager.name = "MusicManager";
        startSceneLoader.name = "StartSceneLoader";
        lookupAgencyManager.name = "LookupAgencyManager";
    }

    void LinkManagersAndInitializeValues()
    {
        /* ORDER SENSITIVE */

        // Set music clip and play
        musicManager.SetAudioClip(music);
        musicManager.Play();

        // Set default delivery values
        gameplayManager.ResetDeliveryDetails();

        /* NOT order sensitive */
        
        // Set starting dialogue sprites
        startSceneLoader.sprite_PlayerDown = sprite_PlayerDown;
        startSceneLoader.sprite_PlayerRight = sprite_PlayerRight;
        startSceneLoader.sprite_NPCLeft = sprite_NPCLeft;

        // Populate upgrades and values
        upgradeManager.AddUpgrade("Task Tracker", 10, isRepeatable: false);
        upgradeManager.AddUpgrade("Company Running Shoes", 10, isRepeatable: false);
        upgradeManager.AddUpgrade("Address Book", 20, isRepeatable: false);
        upgradeManager.AddUpgrade("Address Book Slot", 10, isRepeatable: true);
        upgradeManager.AddUpgrade("Exit the Matrix", 30, isRepeatable: false);
        upgradeManager.AddUpgrade("Where Credit is Due", 20, isRepeatable: false);

        // Add Start Scene Loader's method to list of scene change events
        SceneManager.sceneLoaded += startSceneLoader.OnSceneWasLoaded;
    }
}