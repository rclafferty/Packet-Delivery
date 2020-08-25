/* File: GameplayManager.cs
 * Author: Casey Lafferty
 * Project: Packet Delivery
 */

using Assets.Scripts.Behind_The_Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    // Gameplay Manager singleton reference
    static GameplayManager instance = null;

    // Payment for delivery --> Equivalent to 10 dollars
    const int DELIVERY_PAYMENT = 10;

    // Instructions for the next step in the delivery process
    public struct DeliveryInstructions
    {
        public string recipient;
        public char neighborhoodID;
        public string nextStep;
    };

    // Necessary managers to store
    [SerializeField] LookupAgencyManager lookupAgencyManager;
    [SerializeField] LetterManager letterManager;
    [SerializeField] HUDManager hudManager;
    [SerializeField] UpgradeManager upgradeManager;
    [SerializeField] CacheManager cacheManager;

    // Store the current indoor location (if applicable)
    [SerializeField] public string indoorLocation;

    // Store the last outdoor position -- used for spawning in the right spot in town
    public Vector2 lastOutdoorPosition;

    // Address the player is currently attempting to visit
    public string currentAddress = "";

    // Address the player is delivering to
    public string deliveryAddress = "";

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        HasStartingLetter = false;

        // Start with no money -- only get money from deliveries
        Money = 0;

        // First outdoor spawn point is outside the office
        lastOutdoorPosition = new Vector2(265, -21.5f);
    }

    void Start()
    {
        ResetDeliveryDetails();

        // Add event to call OnSceneLoad() every time a scene is changed
        SceneManager.sceneLoaded += OnSceneLoad;

#if UNITY_EDITOR
        Money = 200;
#endif
    }

    void OnSceneLoad(Scene thisScene, LoadSceneMode loadSceneMode)
    {
        // If the player is outdoor in the town
        if (thisScene.name == "town")
        {
            // Find the player object
            GameObject player = GameObject.Find("Player");

            // Set the player's position and adjust to avoid trigger
            player.transform.position = lastOutdoorPosition + (Vector2.down * 1);

            // If the player has purchased the "Exit the Matrix" upgrade
            if (upgradeManager.HasPurchasedUpgrade("Exit the Matrix"))
            {
                // Find the address manager object
                GameObject addressManagerObject = GameObject.Find("AddressManager");

                // If the address manager exists
                if (addressManagerObject != null)
                {
                    // Exit the matrix
                    Debug.Log("Enabling exit the matrix mode");
                    AddressManager addressManager = addressManagerObject.GetComponent<AddressManager>();
                    // addressManager.EnableExitTheMatrix();
                }
            }
        }
        // If the current scene is the instructions screen
        else if (thisScene.name == "instructions")
        {
            // Hide HUD
            hudManager.ToggleDisplay(true);
        }
    }

    public void ExitTheMatrix()
    {
        // Purchase the "Exit the Matrix" upgrade
        upgradeManager.AttemptPurchase("Exit the Matrix");

        // Update the HUD
        hudManager.DisplayText();
    }

    public bool HasUpgrade(string title)
    {
        // If the string is valid, check if the player has purchased the upgrade.
        // If it's invalid, unable to check -- automatic "no"
        return !string.IsNullOrEmpty(title) ? upgradeManager.HasPurchasedUpgrade(title) : false;
    }

    public bool HasCurrentTarget()
    {
        // If the current message is null, there's no target. Else, there is a target
        return CurrentMessage != null;
    }

    public void CompleteTask()
    {
        // If there is a current delivery
        if (HasCurrentTarget())
        {
            // Mark current delivery as completed
            letterManager.MarkDelivered(CurrentMessage.ID);

            // If this is the first delivery
            if (HasStartingLetter)
            {
                // Mark the first-letter flag as false
                HasStartingLetter = false;
            }

            // Give the player their reward
            Money += DELIVERY_PAYMENT;

            // If the player hass the address book upgrade
            if (HasUpgrade("Address Book"))
            {
                // Cache the completed delivery
                cacheManager.AddAddress(CurrentMessage.Recipient);
            }
        }

        // Set all delivery instructions to starting instructions
        ResetDeliveryDetails();
    }

    public void ResetDeliveryDetails()
    {
        // Remove the current message reference
        CurrentMessage = null;

        // Reset the current recipient reference name
        CurrentTarget = "";

        // Set the next step instructions
        DeliveryInstructions instructions;
        instructions.recipient = "None";
        instructions.neighborhoodID = 'X';
        instructions.nextStep = "Packet Delivery Office";
        NextStep = instructions;

        // Reset and update the HUD
        hudManager.ClearCurrentTask();
        hudManager.DisplayText();
    }

    public void GetNextMessage()
    {
        // Get the next delivery letter
        CurrentMessage = letterManager.GetNextLetter();

        // Lookup the next lookup agency name
        string nextLocation = lookupAgencyManager.GetNeighborhoodNameFromID('X') + " Lookup Agency";

        DeliveryInstructions instructions;

        // If the player has unlocked "Exit the Matrix"
        if (HasUpgrade("Exit the Matrix"))
        {
            // Reference the recipient's URL
            instructions.recipient = CurrentMessage.Recipient.URL;
        }
        // If the player has not unlocked it
        else
        {
            // Reference the recipient's name
            instructions.recipient = CurrentMessage.Recipient.Name;
        }

        instructions.neighborhoodID = 'X';
        instructions.nextStep = nextLocation;

        // Set instructions and update appropriate components
        SetNextSteps(instructions);
    }

    public void SetNextSteps(DeliveryInstructions instructions)
    {
        // Store the instructions globaly
        NextStep = instructions;

        // Update the HUD with the instructions
        hudManager.DisplayText();
    }

    public void ForceUpdateHUD()
    {
        // If there is an active delivery
        if (CurrentMessage != null)
        {
            DeliveryInstructions instructions = NextStep;

            // If the player has unlocked "Exit the Matrix"
            if (upgradeManager.HasPurchasedUpgrade("Exit the Matrix"))
            {
                // Reference the recipient's URL
                instructions.recipient = CurrentMessage.Recipient.URL;
            }
            else
            {
                // Reference the recipient's name
                instructions.recipient = CurrentMessage.Recipient.Name;
            }

            instructions.nextStep = lookupAgencyManager.GetNeighborhoodNameFromID(instructions.neighborhoodID);

            // Check if the next step is a lookup agency
            bool isNextStepLookupAgency = !(instructions.nextStep.Contains("Office") || instructions.nextStep.Contains("Residence"));

            // If it is a lookup agency
            if (isNextStepLookupAgency)
            {
                // Adjust the displayed next step text 
                instructions.nextStep += " Lookup Agency";
            }

            // Store the next delivery steps globaly
            NextStep = instructions;
        }

        // Update the HUD
        hudManager.DisplayText();
    }

    public void CompleteGameAndReset()
    {
        // Reset Money
        Money = 0;

        // Reset Upgrades
        upgradeManager.ResetUpgrades();

        // Hide HUD
        hudManager.ToggleDisplay(false);
    }

    public bool HasRemainingTasks
    {
        get
        {
            // Count the number of remaining letters to deliver
            RemainingTasks = letterManager.RemainingLetterCount;

            // If the number of letters is nonzero, return true. Else, false
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

    public Letter CurrentMessage { get; private set; }
    public char CurrentNeighborhoodID { get; set; }
}