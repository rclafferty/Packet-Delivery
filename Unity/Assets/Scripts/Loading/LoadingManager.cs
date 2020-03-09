using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] CheatManager cheatManager;
    [SerializeField] GameplayManager gameplayManager;
    [SerializeField] LetterManager letterManager;
    [SerializeField] LevelManager levelManager;
    [SerializeField] MusicManager musicManager;
    [SerializeField] StartSceneLoader startSceneLoader;
    [SerializeField] LookupAgencyManager lookupAgencyManager;
    [SerializeField] UpgradeManager upgradeManager;
    // [SerializeField] Timer timer;

    [SerializeField] AudioClip music;

    [SerializeField] Sprite sprite_PlayerRight;
    [SerializeField] Sprite sprite_PlayerDown;
    [SerializeField] Sprite sprite_NPCLeft;

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("Loading game...");
        
        LoadGame_Method();
        // Debug.Log("Done");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadGame_Method()
    {
        // Debug.Log("Method, not coroutine");

        // AssignInstantiatedObjectNames();
        LinkManagersAndInitializeValues();
        
        levelManager.LoadLevel("title");
    }

    void AssignInstantiatedObjectNames()
    {
        // Change names of objects for consistency
        cheatManager.name = "CheatManager";
        gameplayManager.name = "GameplayManager";
        letterManager.name = "LetterManager";
        levelManager.name = "LevelManager";
        musicManager.name = "MusicManager";
        startSceneLoader.name = "StartSceneLoader";
        lookupAgencyManager.name = "LookupAgencyManager";
        // timer.name = "Timer";
    }

    void LinkManagersAndInitializeValues()
    {
        // ORDER SENSITIVE
        musicManager.SetAudioClip(music);
        musicManager.Play();
        gameplayManager.ResetDeliveryDetails();

        // NOT order sensitive
        cheatManager.SetLevelManager(levelManager);
        startSceneLoader.sprite_PlayerDown = sprite_PlayerDown;
        startSceneLoader.sprite_PlayerRight = sprite_PlayerRight;
        startSceneLoader.sprite_NPCLeft = sprite_NPCLeft;
        upgradeManager.AddUpgrade("Task Tracker", 10);
        upgradeManager.AddUpgrade("Company Running Shoes", 10);
        upgradeManager.AddUpgrade("Exit the Matrix", 30);

        SceneManager.sceneLoaded += startSceneLoader.OnSceneWasLoaded;
    }
}