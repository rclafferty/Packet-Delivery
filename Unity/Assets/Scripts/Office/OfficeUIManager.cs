using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OfficeUIManager : MonoBehaviour
{
    GameplayManager gameplayManager;
    Text statusText;

    // Start is called before the first frame update
    void Start()
    {
        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        statusText = GameObject.Find("Status Text").GetComponent<Text>();
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode loadMode)
    {
        string title = "Packet Delivery, Inc.";
        string recipientText = "Current Recipient\n" + gameplayManager.CurrentTarget;
        string upgradeText = "Upgrades\nNone";
    }
}
