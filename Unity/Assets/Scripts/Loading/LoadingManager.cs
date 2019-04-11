﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    CheatManager cheatManager;
    GameplayManager gameplayManager;
    LetterManager letterManager;
    LevelManager levelManager;
    MusicManager musicManager;
    PlayerController playerController;
    RespawnManager spawnManager;
    StartSceneLoader startSceneLoader;
    Timer timer;

    GameObject prefab_CheatManager;
    GameObject prefab_GameplayManager;
    GameObject prefab_LevelManager;
    GameObject prefab_LetterManager;
    GameObject prefab_MusicManager;
    // GameObject prefab_PlayerController;
    GameObject prefab_SpawnManager;
    GameObject prefab_StartSceneLoader;
    GameObject prefab_Timer;

    GameObject object_CheatManager;
    GameObject object_GameplayManager;
    GameObject object_LevelManager;
    GameObject object_LetterManager;
    GameObject object_MusicManager;
    // GameObject object_PlayerController;
    GameObject object_SpawnManager;
    GameObject object_StartSceneLoader;
    GameObject object_Timer;

    [SerializeField]
    AudioClip music;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadGame());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadGame()
    {
        // Load prefabs
        prefab_CheatManager = Resources.Load<GameObject>("Prefabs/CheatManager");
        prefab_GameplayManager = Resources.Load<GameObject>("Prefabs/GameplayManager");
        prefab_LetterManager = Resources.Load<GameObject>("Prefabs/LetterManager");
        prefab_LevelManager = Resources.Load<GameObject>("Prefabs/LevelManager");
        prefab_MusicManager = Resources.Load<GameObject>("Prefabs/MusicManager");
        prefab_SpawnManager = Resources.Load<GameObject>("Prefabs/SpawnManager");
        prefab_StartSceneLoader = Resources.Load<GameObject>("Prefabs/StartSceneLoader");
        prefab_Timer = Resources.Load<GameObject>("Prefabs/Timer");

        // Instantiate prefabs
        object_CheatManager = Instantiate(prefab_CheatManager, Vector3.zero, Quaternion.identity);
        object_GameplayManager = Instantiate(prefab_GameplayManager, Vector3.zero, Quaternion.identity);
        object_LetterManager = Instantiate(prefab_LetterManager, Vector3.zero, Quaternion.identity);
        object_LevelManager = Instantiate(prefab_LevelManager, Vector3.zero, Quaternion.identity);
        object_MusicManager = Instantiate(prefab_MusicManager, Vector3.zero, Quaternion.identity);
        // object_PlayerController = Instantiate(prefab_PlayerController, Vector3.zero, Quaternion.identity);
        object_SpawnManager = Instantiate(prefab_SpawnManager, Vector3.zero, Quaternion.identity);
        object_StartSceneLoader = Instantiate(prefab_StartSceneLoader, Vector3.zero, Quaternion.identity);
        object_Timer = Instantiate(prefab_Timer, Vector3.zero, Quaternion.identity);

        // Change names of objects for consistency
        object_CheatManager.name = "CheatManager";
        object_GameplayManager.name = "GameplayManager";
        object_LetterManager.name = "LetterManager";
        object_LevelManager.name = "LevelManager";
        object_MusicManager.name = "MusicManager";
        // object_PlayerController.name = "Player";
        object_SpawnManager.name = "SpawnManager";
        object_StartSceneLoader.name = "StartSceneLoader";
        object_Timer.name = "Timer";

        // Get the components
        cheatManager = object_CheatManager.GetComponent<CheatManager>();
        gameplayManager = object_GameplayManager.GetComponent<GameplayManager>();
        letterManager = object_LetterManager.GetComponent<LetterManager>();
        levelManager = object_LevelManager.GetComponent<LevelManager>();
        musicManager = object_MusicManager.GetComponent<MusicManager>();
        // playerController = object_PlayerController.GetComponent<PlayerController>();
        spawnManager = object_SpawnManager.GetComponent<RespawnManager>();
        startSceneLoader = object_StartSceneLoader.GetComponent<StartSceneLoader>();
        timer = object_Timer.GetComponent<Timer>();

        // Load Audio Clip from file
        // music = Resources.Load("Music/LaserGroove") as AudioClip;

        // Do something with them -- ORDER SENSITIVE
        letterManager.LoadLetterFromFile();
        gameplayManager.SetTimer(timer);
        gameplayManager.CompleteTask();
        gameplayManager.SetLetterManager(letterManager);
        gameplayManager.CurrentTargetMessage = letterManager.GetStartingMessage();

        // Do something with them (cont) -- NOT order sensitive
        gameplayManager.CurrentSpawnLocation = spawnManager.GetSpawnPointByName("Office");
        musicManager.SetAudioClip(music);
        musicManager.Play();
        // levelManager.SetPlayerController(playerController);
        cheatManager.SetLevelManager(levelManager);
        timer.SetDifficulty("Hard");
        SceneManager.sceneLoaded += startSceneLoader.OnSceneWasLoaded;
        
        // DONE
        // yield return new WaitForSeconds(2);
        levelManager.LoadLevel("title");

        yield return null;
    }
}