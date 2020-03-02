// using System;
using Assets.Scripts.Behind_The_Scenes;
// using Assets.Scripts.Chat;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    static GameplayManager instance = null;

    const int DELIVERY_PAYMENT = 10; // Equivalent to 10 dollars

    public struct DeliveryDirections
    {
        public string building;
        public string color;
        public string mapDirection;
    }

    // Other necessary managers
    [SerializeField] LetterManager letterManager;
    [SerializeField] NotepadManager notepadManager;

    [SerializeField] GameObject moneyBackdrop;
    [SerializeField] Text moneyText;

    [SerializeField] Letter currentTargetMessage;

    int obstacleTilemapIndex;

    [SerializeField] public string indoorLocation;
    
    string[] locations = { "office", "centralLookupAgency", "localLookupAgencyNE", "localLookupAgencySW", "home" };

    [System.Obsolete("Should not be using spawnpoints anymore")]
    string[] spawnpointNames = { "Office Spawnpoint", "CLA Spawnpoint", "LLA NE Spawnpoint", "LLA SW Spawnpoint", "Home" };

    public Vector2 lastOutdoorPosition;

    public string currentAddress = "";
    public string deliveryAddress = "";

    int money = 0;

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

        GameplayTimer = GameObject.Find("Timer").GetComponent<Timer>();

        // lastOutdoorPosition = new Vector2(-14, 12);
        lastOutdoorPosition = new Vector2(265, -21.5f);

        Money = 0;
        moneyBackdrop.SetActive(false);

        ResetDeliveryDetails();

        HasExitedTheMatrix = false; // Set true for testing

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

            if (HasExitedTheMatrix)
            {
                GameObject addressManagerObject = GameObject.Find("AddressManager");
                if (addressManagerObject != null)
                {
                    // Exit the matrix
                    Debug.Log("Enabling exit the matrix mode");
                    AddressManager addressManager = addressManagerObject.GetComponent<AddressManager>();
                    addressManager.EnableExitTheMatrix();
                }
            }
        }
        else if (thisScene.name == "instructions")
        {
            moneyBackdrop.SetActive(true);
        }
    }

    public void ExitTheMatrix()
    {
        HasExitedTheMatrix = true;
        letterManager.ResetMessages();
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
                if (HasExitedTheMatrix)
                {
                    CurrentTarget = currentTargetMessage.RecipientURL;
                }
                else
                {
                    CurrentTarget = currentTargetMessage.Recipient;
                }
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

            if (HasStartingLetter)
            {
                HasStartingLetter = false;
            }

            Money += DELIVERY_PAYMENT;
        }

        ResetDeliveryDetails();
    }

    private void ResetDeliveryDetails()
    {
        currentTargetMessage = null;
        CurrentTarget = "";

        ResetDirectionsToOffice();

        GameplayTimer.StopTimerIfRunning();

        obstacleTilemapIndex++;

        notepadManager.ClearNotepad();
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
        deliveryAddress = currentTargetMessage.Address;

        notepadManager.AddItemToNotepad("Central Lookup Agency");
    }

    public void GetStartingMessage()
    {
        this.CurrentTargetMessage = letterManager.GetStartingMessage();
        NextDeliveryLocation = "CLA";
        deliveryAddress = currentTargetMessage.Address;

        notepadManager.AddItemToNotepad("Central Lookup Agency");
    }

    public void AddTodoItem(string item)
    {
        notepadManager.AddItemToNotepad(item);
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

    public int Money {
        get
        {
            return money;
        }
        set
        {
            money = value;
            moneyText.text = "$" + money;
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

    public bool HasTaskTracker { get; set; }
    public bool HasExitedTheMatrix { get; set; }
}