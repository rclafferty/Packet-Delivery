using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    GameplayManager gameplayManager;
    LevelManager levelManager;
    LetterManager letterManager;
    OfficeUIManager uiManager;
    UpgradeManager upgradeManager;
    // WorkGUIManager workUIManager;
    
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;

        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        levelManager = GameObject.Find("GameplayManager").GetComponent<LevelManager>();
        letterManager = GameObject.Find("GameplayManager").GetComponent<LetterManager>();

        gameplayManager.SetLetterManager(letterManager);
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode loadMode)
    {
        // Give the gameplay manager the current target (1st letter)
        gameplayManager.CurrentTargetMessage = letterManager.GetNextMessage();

        //if (scene.name == "Office")
        //uiManager = GameObject.Find("UI")
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
