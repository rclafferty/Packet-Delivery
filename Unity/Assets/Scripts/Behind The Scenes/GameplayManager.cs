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
    
    public struct DeliveryInstructions
    {
        public string recipient;
        public char neighborhoodID;
        public string nextStep;
    };

    // Other necessary managers
    [SerializeField] LookupAgencyManager lookupAgencyManager;
    [SerializeField] LetterManager letterManager;
    [SerializeField] HUDManager hudManager;
    [SerializeField] UpgradeManager upgradeManager;

    [SerializeField] public string indoorLocation;

    public Vector2 lastOutdoorPosition;

    public string currentAddress = "";
    public string deliveryAddress = "";

    [SerializeField] string target;

    // Start is called before the first frame update
    void Start()
    {
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
        
        HasStartingLetter = false;
        
        lastOutdoorPosition = new Vector2(265, -21.5f);
        Money = 0;
        
        ResetDeliveryDetails();

#if UNITY_EDITOR
        // upgradeManager.ForcePurchaseUpgrade("Exit the Matrix");
#endif

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
            GameObject.Find("Player").transform.position = lastOutdoorPosition + (Vector2.down * 1);

            if (upgradeManager.HasPurchasedUpgrade("Exit the Matrix"))
            {
                GameObject addressManagerObject = GameObject.Find("AddressManager");
                if (addressManagerObject != null)
                {
                    // Exit the matrix
                    Debug.Log("Enabling exit the matrix mode");
                    AddressManager addressManager = addressManagerObject.GetComponent<AddressManager>();
                    // addressManager.EnableExitTheMatrix();
                }
            }
        }
        else if (thisScene.name == "instructions")
        {
            // Hide HUD
            hudManager.ToggleDisplay(true);
        }
    }

    public void ExitTheMatrix()
    {
        upgradeManager.AttemptPurchase("Exit the Matrix");
        hudManager.DisplayText();
    }

    public bool HasUpgrade(string title)
    {
        return upgradeManager.HasPurchasedUpgrade(title);
    }

    public Letter CurrentMessage { get; private set; }

    public bool HasCurrentTarget()
    {
        return CurrentMessage != null;
    }

    public void CompleteTask()
    {
        if (HasCurrentTarget())
        {
            Debug.Log("Task completed for " + CurrentMessage.Recipient.Name);
            letterManager.MarkDelivered(CurrentMessage.ID);

            if (HasStartingLetter)
            {
                HasStartingLetter = false;
            }

            Money += DELIVERY_PAYMENT;
        }
        
        ResetDeliveryDetails();
    }

    public void ResetDeliveryDetails()
    {
        CurrentMessage = null;
        CurrentTarget = "";

        DeliveryInstructions instructions;
        instructions.recipient = "None";
        instructions.neighborhoodID = 'X';
        instructions.nextStep = "Packet Delivery Office";
        NextStep = instructions;

        hudManager.ClearCurrentTask();
        hudManager.DisplayText();
    }

    public void GetNextMessage()
    {
        CurrentMessage = letterManager.GetNextLetter();

        string nextLocation = lookupAgencyManager.GetNeighborhoodNameFromID('X') + " Lookup Agency";

        DeliveryInstructions instructions;
        if (upgradeManager.HasPurchasedUpgrade("Exit the Matrix"))
        {
            instructions.recipient = CurrentMessage.Recipient.URL;
        }
        else
        {
            instructions.recipient = CurrentMessage.Recipient.Name;
        }
        instructions.neighborhoodID = 'X';
        instructions.nextStep = nextLocation;

        SetNextSteps(instructions);

        target = instructions.recipient;

        // notepadManager.AddItemToNotepad("Central Lookup Agency");
    }

    public void SetNextSteps(DeliveryInstructions instructions)
    {
        NextStep = instructions;
        hudManager.DisplayText();
    }

    public void ForceUpdateHUD()
    {
        if (CurrentMessage != null)
        {
            DeliveryInstructions instructions = NextStep;
            if (upgradeManager.HasPurchasedUpgrade("Exit the Matrix"))
            {
                instructions.recipient = CurrentMessage.Recipient.URL;
            }
            else
            {
                instructions.recipient = CurrentMessage.Recipient.Name;
            }
            instructions.nextStep = lookupAgencyManager.GetNeighborhoodNameFromID(instructions.neighborhoodID);

            if (!(instructions.nextStep.Contains("Office") || instructions.nextStep.Contains("Residence")))
            {
                instructions.nextStep += " Lookup Agency";
            }
            NextStep = instructions;
        }

        hudManager.DisplayText();
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
    public DeliveryInstructions NextStep { get; set; }
    public string NextDeliveryLocation { get; set; }
    public int Money { get; set; }

    public char CurrentNeighborhoodID { get; set; }
}