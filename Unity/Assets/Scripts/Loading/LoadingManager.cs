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
    PlayerController playerController;

    GameObject prefab_CheatManager;
    GameObject prefab_GameplayManager;
    GameObject prefab_LevelManager;
    GameObject prefab_LetterManager;
    GameObject prefab_PlayerController;

    GameObject object_CheatManager;
    GameObject object_GameplayManager;
    GameObject object_LevelManager;
    GameObject object_LetterManager;
    GameObject object_PlayerController;

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
        prefab_PlayerController = Resources.Load<GameObject>("Prefabs/Player");

        // Instantiate prefabs
        object_CheatManager = Instantiate(prefab_CheatManager, Vector3.zero, Quaternion.identity);
        object_GameplayManager = Instantiate(prefab_GameplayManager, Vector3.zero, Quaternion.identity);
        object_LetterManager = Instantiate(prefab_LetterManager, Vector3.zero, Quaternion.identity);
        object_LevelManager = Instantiate(prefab_LevelManager, Vector3.zero, Quaternion.identity);
        object_PlayerController = Instantiate(prefab_PlayerController, Vector3.zero, Quaternion.identity);

        // Change names of objects for consistency
        object_CheatManager.name = "CheatManager";
        object_GameplayManager.name = "GameplayManager";
        object_LetterManager.name = "LetterManager";
        object_LevelManager.name = "LevelManager";
        object_PlayerController.name = "Player";

        // Get the components
        cheatManager = object_CheatManager.GetComponent<CheatManager>();
        gameplayManager = object_GameplayManager.GetComponent<GameplayManager>();
        letterManager = object_LetterManager.GetComponent<LetterManager>();
        levelManager = object_LevelManager.GetComponent<LevelManager>();
        playerController = object_PlayerController.GetComponent<PlayerController>();

        // Do something with them -- ORDER SENSITIVE
        letterManager.LoadLetterFromFile();
        gameplayManager.SetLetterManager(letterManager);

        // Do something with them (cont) -- NOT order sensitive
        levelManager.SetPlayerController(playerController);
        cheatManager.SetLevelManager(levelManager);

        // DONE
        levelManager.LoadLevel("title");

        yield return null;
    }
}