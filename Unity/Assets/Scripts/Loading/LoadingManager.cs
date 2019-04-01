using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    GameplayManager gameplayManager;
    LevelManager levelManager;
    LetterManager letterManager;

    GameObject prefab_GameplayManager;
    GameObject prefab_LevelManager;
    GameObject prefab_LetterManager;

    GameObject object_GameplayManager;
    GameObject object_LevelManager;
    GameObject object_LetterManager;

    // Start is called before the first frame update
    void Start()
    {
        // Load prefabs
        prefab_GameplayManager = Resources.Load<GameObject>("Prefabs/GameplayManager");
        prefab_LevelManager = Resources.Load<GameObject>("Prefabs/LevelManager");
        prefab_LetterManager = Resources.Load<GameObject>("Prefabs/LetterManager");

        // Instantiate prefabs
        object_GameplayManager = Instantiate(prefab_GameplayManager, Vector3.zero, Quaternion.identity);
        object_LevelManager = Instantiate(prefab_LevelManager, Vector3.zero, Quaternion.identity);
        object_LetterManager = Instantiate(prefab_LetterManager, Vector3.zero, Quaternion.identity);

        // Change names of objects for consistency
        object_GameplayManager.name = "GameplayManager";
        object_LevelManager.name = "LevelManager";
        object_LetterManager.name = "LetterManager";

        // Get the components
        gameplayManager = object_GameplayManager.GetComponent<GameplayManager>();
        levelManager = object_LevelManager.GetComponent<LevelManager>();
        letterManager = object_LetterManager.GetComponent<LetterManager>();

        // Do something with them
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