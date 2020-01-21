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
    [SerializeField] RespawnManager spawnManager;
    [SerializeField] StartSceneLoader startSceneLoader;
    [SerializeField] LookupAgencyManager lookupAgencyManager;
    [SerializeField] Timer timer;

    [SerializeField] AudioClip music;

    [SerializeField] Sprite sprite_PlayerRight;
    [SerializeField] Sprite sprite_PlayerDown;
    [SerializeField] Sprite sprite_NPCLeft;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Loading game...");
        
        LoadGame_Method();
        Debug.Log("Done");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadGame_Method()
    {
        Debug.Log("Method, not coroutine");

        AssignInstantiatedObjectNames();
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
        spawnManager.name = "SpawnManager";
        startSceneLoader.name = "StartSceneLoader";
        lookupAgencyManager.name = "LookupAgencyManager";
        timer.name = "Timer";
    }

    void LinkManagersAndInitializeValues()
    {
        // ORDER SENSITIVE
        musicManager.SetAudioClip(music);
        musicManager.Play();
        gameplayManager.GameplayTimer = timer;
        gameplayManager.SetLetterManager(letterManager);
        gameplayManager.CompleteTask();
        // Disabled temporarily: Start with a message already
        // gameplayManager.CurrentTargetMessage = letterManager.GetStartingMessage();

        // Disabled temporarily: Set upgrade manager
        // gameplayManager.SetUpgradeManager(upgradeManager);

        // NOT order sensitive
        gameplayManager.CurrentSpawnLocation = spawnManager.GetSpawnPointByName("Office");
        cheatManager.SetLevelManager(levelManager);
        timer.SetDifficulty("Default");
        startSceneLoader.sprite_PlayerDown = sprite_PlayerDown;
        startSceneLoader.sprite_PlayerRight = sprite_PlayerRight;
        startSceneLoader.sprite_NPCLeft = sprite_NPCLeft;
        SceneManager.sceneLoaded += startSceneLoader.OnSceneWasLoaded;
    }
}