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
    // [SerializeField] Button taskTrackerButton;
    [SerializeField] Text taskTrackerPriceText;
    // [SerializeField] Button companyRunningShoesButton;
    [SerializeField] Text companyRunningShoesPriceText;
    // [SerializeField] Button addressBookButton;
    [SerializeField] Text addressBookPriceText;
    // [SerializeField] Button addressBookSlotButton;
    [SerializeField] Text addressBookSlotPriceText;
    // [SerializeField] Button exitMatrixButton;
    [SerializeField] Text exitMatrixPriceText;

    bool isComputerShown;
    GameplayManager gameplayManager;
    UpgradeManager upgradeManager;

    bool isAtComputer;

    // Start is called before the first frame update
    void Start()
    {
        screenText.text = "";

        // Find the GameplayManager in scene
        GameObject gameplayManagerObject = GameObject.Find("GameplayManager");
        if (gameplayManagerObject != null)
        {
            gameplayManager = gameplayManagerObject.GetComponent<GameplayManager>();
        }

        // Find the UpgradeManager in scene
        GameObject upgradeManagerObject = GameObject.Find("UpgradeManager");
        if (upgradeManagerObject != null)
        {
            upgradeManager = upgradeManagerObject.GetComponent<UpgradeManager>();
        }

        // Display upgrades price
        taskTrackerPriceText.text = "$" + upgradeManager.GetUpgradeCost("Task Tracker");
        companyRunningShoesPriceText.text = "$" + upgradeManager.GetUpgradeCost("Company Running Shoes");
        exitMatrixPriceText.text = "$" + upgradeManager.GetUpgradeCost("Exit the Matrix");

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
        // Set default text as empty
        screenText.text = "";

        // Store isShown value for reference in other methods
        isComputerShown = isShown;

        // Display or hide the computer UI
        officeComputerCanvas.gameObject.SetActive(isShown);

        // Freeze player if at computer
        player.IsWalkingEnabled = !isShown;

        ForceUpdateGUI();
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
        Letter currentMessage = gameplayManager.CurrentMessage;
        string sender = currentMessage.Sender.Name;
        string recipient = currentMessage.Recipient.Name;

        // If the player has purchased the exit the matrix upgrade
        if (gameplayManager.HasUpgrade("Exit the Matrix"))
        {
            // Use the URLs instead of the names
            sender = currentMessage.Sender.URL;
            recipient = currentMessage.Recipient.URL;
        }
        
        // Format the text to be displayed on screen
        string senderLine = "Sender: " + sender;
        string receiverLine = "Recipient: " + recipient;
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
        // If valid Gameplay Manager and if there is an active delivery
        if (HasValidActiveDelivery())
        {
            // Attempt to lookup the abbreviation
            bool isValidAbbreviation = gameplayManager.NextStep.nextStep != null && gameplayManager.NextStep.recipient != null;

            string nextDisplayDestination = gameplayManager.NextStep.nextStep;
                
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
        string upgradeTitle = "Task Tracker";

        if (AttemptPurchaseUpgrade(upgradeTitle, instructions))
        {
            GameObject.Find("HUD").GetComponent<HUDManager>().ToggleDisplay(true);
        }
    }

    public void PurchaseCompanyRunningShoes()
    {
        // Exit the Matrix instructions to display on purchase
        string instructions = "Boss bought you some running shoes! Hold Left Shift to run faster.";

        // Attempt to purchase the exit the matrix upgrade
        string upgradeTitle = "Company Running Shoes";

        AttemptPurchaseUpgrade(upgradeTitle, instructions);
    }

    public void PurchaseExitTheMatrix()
    {
        // Exit the Matrix instructions to display on purchase
        string instructions = "Now all addresses will be IP addresses and all lookup agencies will be based on real domain lookups.";

        // Attempt to purchase the exit the matrix upgrade
        string upgradeTitle = "Exit the Matrix";

        AttemptPurchaseUpgrade(upgradeTitle, instructions);
    }

    public void PurchaseAddressBook()
    {
        // Address Book instructions to display on purchase
        string instructions = "To use the Address Book, press L to display the previous delivery locations.";

        // Attempt to purchase the task tracker
        string upgradeTitle = "Address Book";

        AttemptPurchaseUpgrade(upgradeTitle, instructions);
    }

    public void PurchaseAddressBookSlot()
    {
        if (GameObject.Find("GameplayManager").GetComponent<GameplayManager>().HasUpgrade("Address Book"))
        {
            // Address Book instructions to display on purchase
            string instructions = "1 slot has been added to your Address Book.";

            // Attempt to purchase the task tracker
            string upgradeTitle = "Address Book Slot";

            AttemptPurchaseUpgrade(upgradeTitle, instructions);
        }
        else
        {
            // Set error message
            screenText.text = "You need the Address Book upgrade first."; 

            // Show on-screen message
            screenText.gameObject.SetActive(true);
        }
    }

    public bool AttemptPurchaseUpgrade(string upgradeTitle, string instructions)
    {
        bool isSuccessful = false;
        
        if (upgradeManager.HasPurchasedUpgrade(upgradeTitle))
        {
            // Set error message
            screenText.text = "You've already purchased the " + upgradeTitle + " upgrade.\n" + instructions; // TODO: Add some snarky "No need to buy the same thing twice, right?" comment

            // Show on-screen message
            screenText.gameObject.SetActive(true);
            isSuccessful = false;
        }
        else if (upgradeManager.AttemptPurchase(upgradeTitle))
        {
            // Set success message
            screenText.text = "You successfully purchased the " + upgradeTitle + " upgrade.\n" + instructions;

            // Show on-screen message
            screenText.gameObject.SetActive(true);

            isSuccessful = true;
        }
        else
        {
            // Set error message
            screenText.text = "You need $" + upgradeManager.GetUpgradeCost(upgradeTitle) + " to purchase the " + upgradeTitle + " upgrade.";

            // Show on-screen message
            screenText.gameObject.SetActive(true);

            isSuccessful = false;
        }

        ForceUpdateGUI();
        gameplayManager.ForceUpdateHUD();

        return isSuccessful;
    }

    void ForceUpdateGUI()
    {
        // Check if player already purchased the task tracker
        if (gameplayManager.HasUpgrade("Task Tracker"))
        {
            taskTrackerPriceText.text = "Purchased";
            taskTrackerPriceText.fontStyle = FontStyle.Italic;
        }

        // Check if player already purchased the company running shoes
        if (gameplayManager.HasUpgrade("Company Running Shoes"))
        {
            companyRunningShoesPriceText.text = "Purchased";
            companyRunningShoesPriceText.fontStyle = FontStyle.Italic;
        }

        // Check if player already purchased the exit the matrix upgrade
        if (gameplayManager.HasUpgrade("Exit the Matrix"))
        {
            exitMatrixPriceText.text = "Purchased";
            exitMatrixPriceText.fontStyle = FontStyle.Italic;
        }

        gameplayManager.ForceUpdateHUD();
    }
}
