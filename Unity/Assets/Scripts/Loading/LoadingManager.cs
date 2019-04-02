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

    GameObject prefab_CheatManager;
    GameObject prefab_GameplayManager;
    GameObject prefab_LevelManager;
    GameObject prefab_LetterManager;

    GameObject object_CheatManager;
    GameObject object_GameplayManager;
    GameObject object_LevelManager;
    GameObject object_LetterManager;

    // Start is called before the first frame update
    void Start()
    {
        // Load prefabs
        prefab_CheatManager = Resources.Load<GameObject>("Prefabs/CheatManager");
        prefab_GameplayManager = Resources.Load<GameObject>("Prefabs/GameplayManager");
        prefab_LetterManager = Resources.Load<GameObject>("Prefabs/LetterManager");
        prefab_LevelManager = Resources.Load<GameObject>("Prefabs/LevelManager");

        // Instantiate prefabs
        object_CheatManager = Instantiate(prefab_CheatManager, Vector3.zero, Quaternion.identity);
        object_GameplayManager = Instantiate(prefab_GameplayManager, Vector3.zero, Quaternion.identity);
        object_LetterManager = Instantiate(prefab_LetterManager, Vector3.zero, Quaternion.identity);
        object_LevelManager = Instantiate(prefab_LevelManager, Vector3.zero, Quaternion.identity);

        // Change names of objects for consistency
        object_CheatManager.name = "CheatManager";
        object_GameplayManager.name = "GameplayManager";
        object_LetterManager.name = "LetterManager";
        object_LevelManager.name = "LevelManager";

        // Get the components
        cheatManager = object_CheatManager.GetComponent<CheatManager>();
        gameplayManager = object_GameplayManager.GetComponent<GameplayManager>();
        letterManager = object_LetterManager.GetComponent<LetterManager>();
        levelManager = object_LevelManager.GetComponent<LevelManager>();

        // Do something with them -- ORDER SENSITIVE
        letterManager.LoadLetterFromFile();
        gameplayManager.SetLetterManager(letterManager);
        gameplayManager.CurrentTargetMessage = letterManager.GetNextMessage();

        // DONE
        levelManager.LoadLevel("title");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}