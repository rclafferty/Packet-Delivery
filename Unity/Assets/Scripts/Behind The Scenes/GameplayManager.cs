// using System;
using Assets.Scripts.Behind_The_Scenes;
// using Assets.Scripts.Chat;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    static GameplayManager instance = null;

    public struct DeliveryDirections
    {
        public string building;
        public string color;
        public string mapDirection;
    }

    // Other necessary managers
    [SerializeField] LetterManager letterManager;

    [SerializeField] Letter currentTargetMessage;

    int obstacleTilemapIndex;

    [SerializeField] public string indoorLocation;
    string[] locations = { "office", "centralLookupAgency", "localLookupAgencyNE", "localLookupAgencySW", "home" };
    string[] spawnpointNames = { "Office Spawnpoint", "CLA Spawnpoint", "LLA NE Spawnpoint", "LLA SW Spawnpoint", "Home" };

    public Vector2 lastOutdoorPosition;

    public string currentAddress = "";
    public string deliveryAddress = "";

    // Start is called before the first frame update
    void Start()
    {
        obstacleTilemapIndex = -1;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        HasStartingLetter = true;
        currentTargetMessage = null;

        CurrentTarget = "";

        ResetDirectionsToOffice();

        GameplayTimer.StopTimerIfRunning();

        obstacleTilemapIndex++;

        lastOutdoorPosition = new Vector2(-14, 12);

        SceneManager.sceneLoaded += OnSceneLoad;
    }

    // Update is called once per frame
    void Update()
    {

    }

#if UNITY_EDITOR
    public void DebugChangePlayerPosition(int locationIndex)
    {
        if (SceneManager.GetActiveScene().name != "town")
            return;

        string spawnpointName = spawnpointNames[locationIndex];
        if (locationIndex == 4)
        {
            GameObject[] homeSpawnpoints = GameObject.FindGameObjectsWithTag("HomeSpawnpoint");
            if (homeSpawnpoints.Length == 0)
            {
                Debug.Log("No spawnpoints");
                return;
            }
            else if (homeSpawnpoints.Length == 1)
            {
                spawnpointName = homeSpawnpoints[0].name;
            }
            else
            {
                Debug.Log("OTI = " + obstacleTilemapIndex);
                spawnpointName = homeSpawnpoints[obstacleTilemapIndex].name;
            }
        }
        GameObject.Find("Player").transform.position = GameObject.Find(spawnpointName).transform.position;
    }
#endif

    public string GetLetterAddress()
    {
        if (CurrentTargetMessage == null)
            return "NONE";

        return CurrentTargetMessage.Address;
    }

    void OnSceneLoad(Scene thisScene, LoadSceneMode loadSceneMode)
    {
        if (thisScene.name == "town")
        {
            GameObject.Find("Player").transform.position = lastOutdoorPosition + (Vector2.down * 1);
        }
    }

    public void SetLetterManager(LetterManager lm)
    {
        if (lm == null)
        {
            return;
        }

        letterManager = lm;
        RemainingTasks = letterManager.RemainingLetterCount;
    }
    
    public Letter CurrentTargetMessage
    {
        get
        {
            return currentTargetMessage;
        }
        set
        {
            if (value == null)
            {
                currentTargetMessage = null;
                CurrentTarget = "";
            }
            else
            {
                currentTargetMessage = value;
                CurrentTarget = currentTargetMessage.Recipient;
            }

            string name = SceneManager.GetActiveScene().name.ToLower();
            if (name.Contains("cla2"))
                gameObject.GetComponent<CLA2GameplayManager>().SetTargetText();
        }
    }

    public bool HasCurrentTarget()
    {
        return (CurrentTarget != "");
    }

    public void CompleteTask()
    {
        if (HasCurrentTarget())
        {
            // Debug.Log("LetterManager ? " + (letterManager != null));
            // Debug.Log("Current Message ? " + (currentTargetMessage != null));
            letterManager.MarkMessageAsDelivered(currentTargetMessage.MessageID);
        }

        currentTargetMessage = null;
        CurrentTarget = "";

        ResetDirectionsToOffice();

        GameplayTimer.StopTimerIfRunning();

        obstacleTilemapIndex++;
    }

    DeliveryDirections ResetDirectionsToOffice()
    {
        NextDeliveryLocation = "Office";

        DeliveryDirections deliveryDirections;

        deliveryDirections.building = "office";
        deliveryDirections.color = "red";
        deliveryDirections.mapDirection = "northwest";

        NextStep = deliveryDirections;
        return deliveryDirections;
    }

    public void GetNextMessage()
    {
        this.CurrentTargetMessage = letterManager.GetNextMessage();
        NextDeliveryLocation = "CLA";
    }

    public void GetStartingMessage()
    {
        this.CurrentTargetMessage = letterManager.GetStartingMessage();
        NextDeliveryLocation = "CLA";
    }

    public bool HasRemainingTasks
    {
        get
        {
            RemainingTasks = letterManager.RemainingLetterCount;
            Debug.Log(RemainingTasks + " remaining letters");
            return (RemainingTasks > 0);
        }
    }

    // Auto property
    public Vector3 CurrentSpawnLocation { get; set; }
    public bool HasStartingLetter { get; set; }
    public string CurrentLocation { get; set; }
    public int RemainingTasks { get; set; }
    public string CurrentTarget { get; private set; }
    public DeliveryDirections NextStep { get; set; }
    public string NextDeliveryLocation { get; set; }
    public Timer GameplayTimer { get; set; }
}