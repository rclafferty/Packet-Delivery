using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Behind_The_Scenes;
using System.Text;

public class OfficeComputerManager : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] GameObject officeComputerCanvas;
    [SerializeField] Text screenText;

    [SerializeField] Button[] logisticsButtons;
    [SerializeField] Button taskTrackerButton;
    [SerializeField] Text taskTrackerPriceText;
    [SerializeField] Button exitMatrixButton;
    [SerializeField] Text exitMatrixPriceText;

    bool isComputerShown;
    GameplayManager gameplayManager;

    Dictionary<string, string> abbreviationLookupTable;

    const int TASK_TRACKER_COST = 10;
    const int EXIT_THE_MATRIX_COST = 30;

    bool isAtComputer;

    // Start is called before the first frame update
    void Start()
    {
        screenText.text = "";

        // Find the GameplayManager in scene
        GameObject object_gameplayManager = GameObject.Find("GameplayManager");
        if (object_gameplayManager != null)
        {
            gameplayManager = object_gameplayManager.GetComponent<GameplayManager>();
        }

        // Store known abbreviations passed from other scripts
        abbreviationLookupTable = new Dictionary<string, string>();
        abbreviationLookupTable.Add("CLA", "Central Lookup Agency");
        abbreviationLookupTable.Add("LLA SW", "Southwest Local Lookup Agency");
        abbreviationLookupTable.Add("LLA NE", "Northeast Local Lookup Agency");

        // Display upgrades price
        taskTrackerPriceText.text = "$" + TASK_TRACKER_COST;
        exitMatrixPriceText.text = "$" + EXIT_THE_MATRIX_COST;

        isAtComputer = false;

        // Hide computer and logistics buttons
        ToggleDisplayLogisticsButtons(false);
        ToggleComputerCanvas(false);
    }

    // Update is called once per frame
    void Update()
    {
        // If computer is hidden and player is within trigger
        if (!isComputerShown && isAtComputer)
        {
            // If pressing space bar
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Show computer
                ToggleComputerCanvas(true);
            }
        }
    }

    void ToggleComputerCanvas(bool isShown)
    {
        // Store isShown value for reference in other methods
        isComputerShown = isShown;

        // Display or hide the computer UI
        officeComputerCanvas.gameObject.SetActive(isShown);

        // Freeze player if at computer
        player.IsWalkingEnabled = !isShown;

        // Check if player already purchased the task tracker
        if (gameplayManager.HasTaskTracker)
        {
            taskTrackerPriceText.text = "Purchased";
            taskTrackerPriceText.fontStyle = FontStyle.Italic;
        }

        // Check if player already purchased the exit the matrix upgrade
        if (gameplayManager.HasExitedTheMatrix)
        {
            exitMatrixPriceText.text = "Purchased";
            exitMatrixPriceText.fontStyle = FontStyle.Italic;
        }

        gameplayManager.ForceUpdateHUD();
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        // If player is activating the trigger, they're at the computer
        isAtComputer = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player is activating the trigger, they're at the computer
        isAtComputer = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // If player is done activating the trigger, they left the computer
        isAtComputer = false;
    }

    public void TurnOffComputer()
    {
        ToggleComputerCanvas(false);
    }

    bool HasValidActiveDelivery()
    {
        // If Gameplay Manager is invalid
        if (gameplayManager == null)
            return false;

        gameplayManager.ForceUpdateHUD();

        return gameplayManager.HasCurrentTarget();
    }

    public void NewDeliveryRequest()
    {
        // If Gameplay Manager is valid
        if (gameplayManager != null)
        {
            string systemMessage = "";

            // If there is already an active delivery
            if (gameplayManager.HasCurrentTarget())
            {
                // Display error message reminding the user of the active delivery
                systemMessage = "Please complete your current delivery before initiating another.\n";
            }
            // If there is not an active delivery
            else
            {
                // Start a delivery and tell the user
                systemMessage = "Success! You've successfully setup the following delivery:\n";

                /*
                // If this is the first delivery
                if (LetterManager.isFirstLetter)
                {
                    // Get starting message
                    gameplayManager.GetStartingMessage();
                    LetterManager.isFirstLetter = false;
                }
                // If this is the 2nd+ delivery
                else
                {
                    // Get any message
                    gameplayManager.GetNextMessage();
                } */

                gameplayManager.GetNextMessage();
            }

            // Display the message details on screen with system message
            DisplayDetails(systemMessage);
        }

        // Hide the logistics screen
        ToggleDisplayLogisticsButtons(false);

        // Show the text
        screenText.gameObject.SetActive(true);

        gameplayManager.ForceUpdateHUD();
    }

    void DisplayDetails(in string systemMessage)
    {
        // Get the current message details
        // Letter currentMessage = gameplayManager.CurrentTargetMessage;
        Letter currentMessage = gameplayManager.CurrentMessage;
        // string sender = currentMessage.Sender;
        string sender = currentMessage.Sender.Name;
        // string recipient = currentMessage.Recipient;
        string recipient = currentMessage.Recipient.Name;

        // If the player has purchased the exit the matrix upgrade
        if (gameplayManager.HasExitedTheMatrix)
        {
            // Use the URLs instead of the names
            // sender = currentMessage.SenderURL;
            sender = currentMessage.Sender.URL;
            // recipient = currentMessage.RecipientURL;
            recipient = currentMessage.Recipient.URL;
        }
        
        // Format the text to be displayed on screen
        string senderLine = "Sender: " + sender;
        string receiverLine = "Recipient: " + recipient;
        // string bodyLine = "\n" + currentMessage.MessageBody;
        string bodyLine = "\n" + currentMessage.Body;

        string displayText = "";
        if (!string.IsNullOrEmpty(systemMessage))
        {
            displayText = systemMessage + "\n";
        }
        displayText += senderLine + "\n" + receiverLine + "\n" + bodyLine;

        // Show the text
        screenText.text = displayText;
        screenText.gameObject.SetActive(true);

        gameplayManager.ForceUpdateHUD();
    }

    void DisplayNoActiveDeliveryError()
    {
        // Display error message
        screenText.text = "You don't currently have an active delivery. Try starting a new request.";

        gameplayManager.ForceUpdateHUD();
    }

    public void ViewDeliveryDetails()
    {
        // If the player has an active delivery
        if (HasValidActiveDelivery())
        {
            // Display delivery details
            DisplayDetails("");
        }
        // If the player does not have a delivery
        else
        {
            // Display an error
            DisplayNoActiveDeliveryError();
        }
        
        // Hide logistics
        ToggleDisplayLogisticsButtons(false);

        // Show the text on-screen
        screenText.gameObject.SetActive(true);
    }

    public void NextDestination()
    {
        // Current display locations of the lookup agencies
        string[] streetNames = { "A St", "C St", "D St" }; // TODO: Add cross streets
        string[] locationName = { "Northeast Local Lookup Agency", "Central Lookup Agency", "Southwest Local Lookup Agency" };

        // If valid Gameplay Manager and if there is an active delivery
        if (HasValidActiveDelivery())
        {
            string nextDestination = "";

            // Attempt to lookup the abbreviation
            bool isValidAbbreviation = abbreviationLookupTable.TryGetValue(gameplayManager.NextDeliveryLocation.Trim(), out nextDestination);

            string nextDisplayDestination = "";

            // If no valid abbreviation is found
            if (isValidAbbreviation)
            {
                // Find the appropriate street name to display
                for (int i = 0; i < streetNames.Length; i++)
                {
                    if (locationName[i] == nextDestination)
                    {
                        // Format output to include street name
                        nextDisplayDestination = nextDestination + " on " + streetNames[i];
                        break;
                    }
                }
            }   
            else
            {
                // Display the stored location
                nextDestination = gameplayManager.NextDeliveryLocation;
                nextDisplayDestination = nextDestination;
            }
                
            // Display next location
            screenText.text = "You should try stopping by the " + nextDisplayDestination + " next";
        }
        else
        {
            // Display error
           DisplayNoActiveDeliveryError();
        }

        // Hide the logistics buttons
        ToggleDisplayLogisticsButtons(false);

        // Show the on-screen text
        screenText.gameObject.SetActive(true);

        gameplayManager.ForceUpdateHUD();
    }

    public void Logistics()
    {
        // If the Gameplay Manager is valid
        if (gameplayManager != null)
        {
            // If there is an active delivery
            if (gameplayManager.HasStartingLetter)
            {
                screenText.text = "Upgrades will be available after your first delivery is complete.";
            }
            // There is no active delivery
            else
            {
                ToggleDisplayLogisticsButtons(true);
            }
        }

        gameplayManager.ForceUpdateHUD();
    }

    void ToggleDisplayLogisticsButtons(bool isShown)
    {
        // Enable/Disable each logistics button
        foreach (Button b in logisticsButtons)
        {
            b.gameObject.SetActive(isShown);
        }

        // If showing the logistics screen
        if (isShown)
        {
            // Hide the message text
            screenText.gameObject.SetActive(false);
        }
    }

    public void PurchaseTaskTracker()
    {
        // Task Tracker instructions to display on purchase
        string instructions = "To use the Task Tracker, press TAB to display your current target and your next delivery location.";

        // Attempt to purchase the task tracker
        if (AttemptPurchaseUpgrade("Task Tracker", TASK_TRACKER_COST, instructions, gameplayManager.HasTaskTracker))
        {
            // Set task tracker flag in Gameplay Manager
            gameplayManager.HasTaskTracker = true;

            // Display purchased text
            taskTrackerPriceText.text = "Purchased";
            taskTrackerPriceText.fontStyle = FontStyle.Italic;

            // Activate the Notepad Manager's task tracker
            GameObject.FindObjectOfType<NotepadManager>().ToggleTaskTracker(true);
        }

        gameplayManager.ForceUpdateHUD();
    }

    public void PurchaseExitTheMatrix()
    {
        // Exit the Matrix instructions to display on purchase
        string instructions = "Now all addresses will be IP addresses and all lookup agencies will be based on real domain lookups.";

        // Attempt to purchase the exit the matrix upgrade
        if (AttemptPurchaseUpgrade("Exit the Matrix", EXIT_THE_MATRIX_COST, instructions, gameplayManager.HasExitedTheMatrix))
        {
            // Set flags in the Gameplay Manager
            gameplayManager.ExitTheMatrix();

            // Display purchased text
            exitMatrixPriceText.text = "Purchased";
            exitMatrixPriceText.fontStyle = FontStyle.Italic;
        }

        gameplayManager.ForceUpdateHUD();
    }

    bool AttemptPurchaseUpgrade(in string upgradeTitle, in int cost, in string instructions, bool hasPurchasedAlready)
    {
        bool success = false;

        // If the upgrade is already purchased
        if (hasPurchasedAlready)
        {
            // Set error message
            screenText.text = "You've already purchased the " + upgradeTitle + " upgrade.\n" + instructions; // TODO: Add some snarky "No need to buy the same thing twice, right?" comment

            // Show on-screen message
            screenText.gameObject.SetActive(true);

            // Failed to purchase
            success = false;
        }
        // If the player has enough money
        else if (gameplayManager.Money >= cost)
        {
            // Deduct the cost
            gameplayManager.Money -= cost;

            // Set success message
            screenText.text = "You successfully purchased the " + upgradeTitle + " upgrade.\n" + instructions;

            // Show on-screen message
            screenText.gameObject.SetActive(true);

            // Successful purchase
            success = true;
        }
        // If the player does NOT have enough money
        else
        {
            // Set error message
            screenText.text = "You need $" + cost + " to purchase the " + upgradeTitle + " upgrade.";

            // Failed purchase
            success = false;
        }

        gameplayManager.ForceUpdateHUD();

        return success;
    }
}
