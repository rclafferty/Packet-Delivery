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
    string[] locations = { "office", "centralLookupAgency", "localLookupAgencyNE", "localLookupAgencySW" };
    string[] spawnpointNames = { "Office Spawnpoint", "CLA Spawnpoint", "LLA NE Spawnpoint", "LLA SW Spawnpoint" };

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

        SceneManager.sceneLoaded += OnSceneLoad;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnSceneLoad(Scene thisScene, LoadSceneMode loadSceneMode)
    {
        if (thisScene.name == "town")
        {
            GameObject[] obstacleTilemapObjects = GameObject.FindGameObjectsWithTag("ObstacleTilemap");

            foreach (GameObject tilemap in obstacleTilemapObjects)
            {
                if (tilemap.name.Contains(obstacleTilemapIndex.ToString()))
                {
                    tilemap.SetActive(true);
                }
                else
                {
                    tilemap.SetActive(false);
                }
            }

            int spawnIndex = -1;
            for (int i = 0; i < locations.Length; i++)
            {
                if (indoorLocation == locations[i])
                {
                    GameObject.Find("Player").transform.position = GameObject.Find(spawnpointNames[i]).transform.position;
                    spawnIndex = i;
                    break;
                }
            }

            // If entered an option not-yet accounted for (home, etc)
            if (spawnIndex == -1)
            {
                // Spawn outside of office
                int officeIndex = 0; 
                GameObject.Find("Player").transform.position = GameObject.Find(spawnpointNames[officeIndex]).transform.position;
            }
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

                Debug.Log("Current target: " + CurrentTarget);
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