using System.Collections;
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
    UpgradeManager upgradeManager;

    GameObject prefab_CheatManager;
    GameObject prefab_GameplayManager;
    GameObject prefab_LevelManager;
    GameObject prefab_LetterManager;
    GameObject prefab_MusicManager;
    GameObject prefab_SpawnManager;
    GameObject prefab_StartSceneLoader;
    GameObject prefab_Timer;
    GameObject prefab_UpgradeManager;

    GameObject object_CheatManager;
    GameObject object_GameplayManager;
    GameObject object_LevelManager;
    GameObject object_LetterManager;
    GameObject object_MusicManager;
    GameObject object_SpawnManager;
    GameObject object_StartSceneLoader;
    GameObject object_Timer;
    GameObject object_UpgradeManager;

    [SerializeField]
    AudioClip music;

    [SerializeField]
    Sprite sprite_PlayerRight;
    [SerializeField]
    Sprite sprite_PlayerDown;
    [SerializeField]
    Sprite sprite_NPCLeft;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Loading game...");
        // LoadGame();
        // StartCoroutine(LoadGame());
        // Debug_LoadGame(false);

        // StartCoroutine(LoadGame_Coroutine());
        LoadGame_Method();
        Debug.Log("Done");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Debug_LoadGame(bool tf)
    {
        if (tf)
        {
            LoadGame_Method();
        }
        else
        {
            StartCoroutine(LoadGame_Coroutine());
        }
    }

    void LoadGame_Method()
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
        prefab_UpgradeManager = Resources.Load<GameObject>("Prefabs/UpgradeManager");

        // Instantiate prefabs
        object_CheatManager = Instantiate(prefab_CheatManager, Vector3.zero, Quaternion.identity);
        object_GameplayManager = Instantiate(prefab_GameplayManager, Vector3.zero, Quaternion.identity);
        object_LetterManager = Instantiate(prefab_LetterManager, Vector3.zero, Quaternion.identity);
        object_LevelManager = Instantiate(prefab_LevelManager, Vector3.zero, Quaternion.identity);
        object_MusicManager = Instantiate(prefab_MusicManager, Vector3.zero, Quaternion.identity);
        object_SpawnManager = Instantiate(prefab_SpawnManager, Vector3.zero, Quaternion.identity);
        object_StartSceneLoader = Instantiate(prefab_StartSceneLoader, Vector3.zero, Quaternion.identity);
        object_Timer = Instantiate(prefab_Timer, Vector3.zero, Quaternion.identity);
        object_UpgradeManager = Instantiate(prefab_UpgradeManager, Vector3.zero, Quaternion.identity);

        // Change names of objects for consistency
        object_CheatManager.name = "CheatManager";
        object_GameplayManager.name = "GameplayManager";
        object_LetterManager.name = "LetterManager";
        object_LevelManager.name = "LevelManager";
        object_MusicManager.name = "MusicManager";
        object_SpawnManager.name = "SpawnManager";
        object_StartSceneLoader.name = "StartSceneLoader";
        object_Timer.name = "Timer";
        object_UpgradeManager.name = "UpgradeManager";

        // Get the components
        cheatManager = object_CheatManager.GetComponent<CheatManager>();
        gameplayManager = object_GameplayManager.GetComponent<GameplayManager>();
        letterManager = object_LetterManager.GetComponent<LetterManager>();
        levelManager = object_LevelManager.GetComponent<LevelManager>();
        musicManager = object_MusicManager.GetComponent<MusicManager>();
        spawnManager = object_SpawnManager.GetComponent<RespawnManager>();
        startSceneLoader = object_StartSceneLoader.GetComponent<StartSceneLoader>();
        timer = object_Timer.GetComponent<Timer>();
        upgradeManager = object_UpgradeManager.GetComponent<UpgradeManager>();

        // Load Audio Clip from file
        // music = Resources.Load("Music/LaserGroove") as AudioClip;

        // Do something with them -- ORDER SENSITIVE
        musicManager.SetAudioClip(music);
        musicManager.Play();
        letterManager.LoadLetterFromFile();
        gameplayManager.SetTimer(timer);
        gameplayManager.CompleteTask();
        gameplayManager.SetLetterManager(letterManager);
        gameplayManager.CurrentTargetMessage = letterManager.GetStartingMessage();
        gameplayManager.SetUpgradeManager(upgradeManager);

        // Do something with them (cont) -- NOT order sensitive
        gameplayManager.CurrentSpawnLocation = spawnManager.GetSpawnPointByName("Office");
        cheatManager.SetLevelManager(levelManager);
        timer.SetDifficulty("Default");
        startSceneLoader.sprite_PlayerDown = sprite_PlayerDown;
        startSceneLoader.sprite_PlayerRight = sprite_PlayerRight;
        startSceneLoader.sprite_NPCLeft = sprite_NPCLeft;
        SceneManager.sceneLoaded += startSceneLoader.OnSceneWasLoaded;

        // DONE
        // yield return new WaitForSeconds(2);
        levelManager.LoadLevel("title");
    }

    IEnumerator LoadGame_Coroutine()
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
        prefab_UpgradeManager = Resources.Load<GameObject>("Prefabs/UpgradeManager");

        // Instantiate prefabs
        object_CheatManager = Instantiate(prefab_CheatManager, Vector3.zero, Quaternion.identity);
        object_GameplayManager = Instantiate(prefab_GameplayManager, Vector3.zero, Quaternion.identity);
        object_LetterManager = Instantiate(prefab_LetterManager, Vector3.zero, Quaternion.identity);
        object_LevelManager = Instantiate(prefab_LevelManager, Vector3.zero, Quaternion.identity);
        object_MusicManager = Instantiate(prefab_MusicManager, Vector3.zero, Quaternion.identity);
        object_SpawnManager = Instantiate(prefab_SpawnManager, Vector3.zero, Quaternion.identity);
        object_StartSceneLoader = Instantiate(prefab_StartSceneLoader, Vector3.zero, Quaternion.identity);
        object_Timer = Instantiate(prefab_Timer, Vector3.zero, Quaternion.identity);
        object_UpgradeManager = Instantiate(prefab_UpgradeManager, Vector3.zero, Quaternion.identity);

        // Change names of objects for consistency
        object_CheatManager.name = "CheatManager";
        object_GameplayManager.name = "GameplayManager";
        object_LetterManager.name = "LetterManager";
        object_LevelManager.name = "LevelManager";
        object_MusicManager.name = "MusicManager";
        object_SpawnManager.name = "SpawnManager";
        object_StartSceneLoader.name = "StartSceneLoader";
        object_Timer.name = "Timer";
        object_UpgradeManager.name = "UpgradeManager";

        // Get the components
        cheatManager = object_CheatManager.GetComponent<CheatManager>();
        gameplayManager = object_GameplayManager.GetComponent<GameplayManager>();
        letterManager = object_LetterManager.GetComponent<LetterManager>();
        levelManager = object_LevelManager.GetComponent<LevelManager>();
        musicManager = object_MusicManager.GetComponent<MusicManager>();
        spawnManager = object_SpawnManager.GetComponent<RespawnManager>();
        startSceneLoader = object_StartSceneLoader.GetComponent<StartSceneLoader>();
        timer = object_Timer.GetComponent<Timer>();
        upgradeManager = object_UpgradeManager.GetComponent<UpgradeManager>();

        // Load Audio Clip from file
        // music = Resources.Load("Music/LaserGroove") as AudioClip;

        // Do something with them -- ORDER SENSITIVE
        musicManager.SetAudioClip(music);
        musicManager.Play();
        letterManager.LoadLetterFromFile();
        gameplayManager.SetTimer(timer);
        gameplayManager.CompleteTask();
        gameplayManager.SetLetterManager(letterManager);
        gameplayManager.CurrentTargetMessage = letterManager.GetStartingMessage();
        gameplayManager.SetUpgradeManager(upgradeManager);

        // Do something with them (cont) -- NOT order sensitive
        gameplayManager.CurrentSpawnLocation = spawnManager.GetSpawnPointByName("Office");
        cheatManager.SetLevelManager(levelManager);
        timer.SetDifficulty("Default");
        startSceneLoader.sprite_PlayerDown = sprite_PlayerDown;
        startSceneLoader.sprite_PlayerRight = sprite_PlayerRight;
        startSceneLoader.sprite_NPCLeft = sprite_NPCLeft;
        SceneManager.sceneLoaded += startSceneLoader.OnSceneWasLoaded;
        
        // DONE
        // yield return new WaitForSeconds(2);
        levelManager.LoadLevel("title");

        yield return null;
    }
}