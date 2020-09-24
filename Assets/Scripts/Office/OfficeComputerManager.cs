/* File: OfficeComputerManager.cs
 * Author: Casey Lafferty
 * Project: Packet Delivery
 */

using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Behind_The_Scenes;

public class OfficeComputerManager : MonoBehaviour
{
    // Reference to the player object in the scene
    [SerializeField] PlayerController player;

    // Reference to the computer UI
    [SerializeField] GameObject officeComputerCanvas;

    // Reference to computer UI text for displaying various details
    [SerializeField] Text screenText;

    // Buttons on the logistic menu
    [SerializeField] Button[] logisticsButtons;

    // Price text for task tracker upgrade
    [SerializeField] Text taskTrackerPriceText;

    // Price text for company running shoes upgrade
    [SerializeField] Text companyRunningShoesPriceText;

    // Address Book upgrade text objects
    [SerializeField] Text addressBookNameText;
    [SerializeField] Text addressBookSlotNameText;
    [SerializeField] Text addressBookPriceText;
    [SerializeField] Text addressBookSlotPriceText;
    [SerializeField] Text addressBookSlotDescriptionText;

    // Price text for Exit the Matrix upgrade
    [SerializeField] Text exitMatrixPriceText;

    // Description text for Where Credit is Due
    [SerializeField] Text whereCreditIsDueDescriptionText;
    [SerializeField] Text whereCreditIsDuePriceText;

    // Fade Canvas -- for transitioning to the credits
    [SerializeField] Transition fadeManager;

    [SerializeField] Button addressBookLessonButton;
    [SerializeField] Button exitTheMatrixLessonButton;

    // Flag to indicate if the computer UI is shown
    bool isComputerShown;

    // Manager references
    GameplayManager gameplayManager;
    UpgradeManager upgradeManager;
    CacheManager cacheManager;
    HUDManager hudManager;

    // Flag to indicate if the player is close enough to the computer to interact with it
    bool isAtComputer;
    
    void Start()
    {
        // Remove the details from the text object
        screenText.text = "";

        // Set manager references
        FindManagersInScene();

        // Display upgrades price
        taskTrackerPriceText.text = "$" + upgradeManager.GetUpgradeCost("Task Tracker");
        companyRunningShoesPriceText.text = "$" + upgradeManager.GetUpgradeCost("Company Running Shoes");
        addressBookPriceText.text = "$" + upgradeManager.GetUpgradeCost("Address Book");
        addressBookSlotPriceText.text = "$" + upgradeManager.GetUpgradeCost("Address Book Slot");
        exitMatrixPriceText.text = "$" + upgradeManager.GetUpgradeCost("Exit the Matrix");
        whereCreditIsDuePriceText.text = "$" + upgradeManager.GetUpgradeCost("Where Credit is Due");

        // The player starts away from the computer
        isAtComputer = false;

        // Hide computer and logistics buttons
        ToggleDisplayLogisticsButtons(false);
        ToggleComputerCanvas(false);

        // If the player has already purchased the limit
        if (upgradeManager.GetQuantity("Address Book Slot") == 2)
        {
            // Make sure price is marked as "Maxed out" and italicized
            addressBookSlotPriceText.text = "Maxed Out";
            addressBookSlotPriceText.fontStyle = FontStyle.Italic;

            // Show on-screen message
            screenText.gameObject.SetActive(true);
        }
    }

    private void FindManagersInScene()
    {
        // Find the GameplayManager in scene
        GameObject gameplayManagerObject = GameObject.Find("GameplayManager");

        // If the gameplay manager object is valid (not null)
        if (gameplayManagerObject != null)
        {
            // Get the gameplay manager component from the object
            gameplayManager = gameplayManagerObject.GetComponent<GameplayManager>();

            // Adjust Address Book and Add Address Book Slot names
            if (gameplayManager.HasUpgrade("Exit the Matrix"))
            {
                addressBookNameText.text = "DNS Cache";
                addressBookSlotNameText.text = "Add DNS Cache Slot";

            }

            whereCreditIsDueDescriptionText.text = "End of the Game\nRequires: Task Tracker, " + addressBookNameText.text + ", Exit the Matrix";
        }

        // Find the UpgradeManager in scene
        GameObject upgradeManagerObject = GameObject.Find("UpgradeManager");

        // If the upgrade manager object is valid (not null)
        if (upgradeManagerObject != null)
        {
            // Get the upgrade manager component from the object
            upgradeManager = upgradeManagerObject.GetComponent<UpgradeManager>();
        }

        // Find the CacheManager in scene
        GameObject cacheManagerObject = GameObject.Find("CacheManager");

        // If the cache manager object is valid (not null)
        if (cacheManagerObject != null)
        {
            // Get the cache manager component from the object
            cacheManager = cacheManagerObject.GetComponent<CacheManager>();
        }

        // Find the HUDManager in scene
        GameObject hudManagerObject = GameObject.Find("HUD");

        // If the cache manager object is valid (not null)
        if (hudManagerObject != null)
        {
            // Get the cache manager component from the object
            hudManager = hudManagerObject.GetComponent<HUDManager>();
        }
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

        // Force the computer UI to update
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
        // Hide the canvas
        ToggleComputerCanvas(false);
    }

    bool HasValidActiveDelivery()
    {
        // If Gameplay Manager is invalid
        if (gameplayManager == null)
            return false;

        // Force update the HUD
        gameplayManager.ForceUpdateHUD();

        // Check if the player has an active delivery and return the result
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
                systemMessage = "Please <b>complete your current delivery</b> before initiating another.\n";
            }
            // If there is not an active delivery
            else
            {
                // Start a delivery and tell the user
                systemMessage = "Success! You've successfully setup a new delivery. Try heading towards the <b>Root Village Lookup Agency</b> for your next step.\n";

                gameplayManager.GetNextMessage();
            }

            // Display the message details on screen with system message
            DisplayDetails(systemMessage);
        }

        // Hide the logistics screen
        ToggleDisplayLogisticsButtons(false);

        // Show the text
        screenText.gameObject.SetActive(true);

        // Update the HUD
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
        
        // If there is a system message
        if (!string.IsNullOrEmpty(systemMessage))
        {
            // Put it before the message details
            displayText = systemMessage + "\n";
        }

        // Add the message details
        displayText += senderLine + "\n" + receiverLine + "\n" + bodyLine;

        // Show the text
        screenText.text = displayText;
        screenText.gameObject.SetActive(true);

        // Update the HUD
        gameplayManager.ForceUpdateHUD();
    }

    void DisplayNoActiveDeliveryError()
    {
        // Display error message
        screenText.text = "You don't currently have an active delivery. Try <b>starting a new request</b>.";

        // Update the HUD
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
            // Check if the lookup details are not null
            bool isValidAbbreviation = !string.IsNullOrEmpty(gameplayManager.NextStep.nextStep) && !string.IsNullOrEmpty(gameplayManager.NextStep.recipient);

            // Reference the stored next step
            string nextDisplayDestination = gameplayManager.NextStep.nextStep;
                
            // Display next location
            screenText.text = "You should try stopping by the <b>" + nextDisplayDestination + "</b> next";
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

        // Update the HUD
        gameplayManager.ForceUpdateHUD();
    }

    public void Logistics()
    {
        // If the Gameplay Manager is valid
        if (gameplayManager != null)
        {
            // Display the logistics menu
            ToggleDisplayLogisticsButtons(true);
        }

        // Update the HUD
        gameplayManager.ForceUpdateHUD();
    }

    void ToggleDisplayLogisticsButtons(bool isShown)
    {
        // Enable/Disable each logistics button
        foreach (Button b in logisticsButtons)
        {
            // Enable/disable all buttons
            b.gameObject.SetActive(isShown);
        }

        if (upgradeManager.HasPurchasedUpgrade("Exit The Matrix"))
        {
            exitTheMatrixLessonButton.gameObject.SetActive(isShown);
        }

        if (upgradeManager.HasPurchasedUpgrade("Address Book"))
        {
            addressBookLessonButton.gameObject.SetActive(isShown);
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
        string instructions = "To use the Task Tracker, <b>press " + HUDManager.TASK_TRACKER_KEY + "</b> to display your current target and your next delivery location.";

        // Attempt to purchase the task tracker
        string upgradeTitle = "Task Tracker";

        // If successful
        if (AttemptPurchaseUpgrade(upgradeTitle, instructions))
        {
            // Display the task tracker
            hudManager.ToggleTaskTracker(true);
        }
    }

    public void PurchaseCompanyRunningShoes()
    {
        // Exit the Matrix instructions to display on purchase
        string instructions = "Boss bought you some running shoes! Hold <b>Left Shift</b> to run faster.";

        string upgradeTitle = "Company Running Shoes";

        // Attempt to purchase the exit the matrix upgrade
        AttemptPurchaseUpgrade(upgradeTitle, instructions);
    }

    public void PurchaseExitTheMatrix()
    {
        // Exit the Matrix instructions to display on purchase
        string instructions = "Now all addresses will be IP addresses and all lookup agencies will be based on real domain lookups.";

        string upgradeTitle = "Exit the Matrix";

        // Attempt to purchase the exit the matrix upgrade
        if (AttemptPurchaseUpgrade(upgradeTitle, instructions))
        {
            // Adjust Address Book and Add Address Book Slot names
            addressBookNameText.text = "DNS Cache";
            addressBookSlotNameText.text = "Add DNS Cache Slot";
            
            // Change description text
            whereCreditIsDueDescriptionText.text = "End of the Game\nRequires: Task Tracker, " + addressBookNameText.text + ", Exit the Matrix";
        }
    }

    public void PurchaseAddressBook()
    {
        string addressBookDisplayTitle = "Address Book";
        if (gameplayManager.HasUpgrade("Exit the Matrix"))
        {
            addressBookDisplayTitle = "DNS Cache";
        }

        // Address Book instructions to display on purchase
        string instructions = "To use the " + addressBookDisplayTitle + ", <b>press " + HUDManager.ADDRESS_BOOK_KEY + "</b> to display the previous delivery locations.";

        // Attempt to purchase the task tracker
        string upgradeTitle = "Address Book";

        // If successful
        if (AttemptPurchaseUpgrade(upgradeTitle, instructions))
        {
            // Display the address book
            // hudManager.ToggleAddressBook(true);
        }
    }

    public void PurchaseAddressBookSlot()
    {
        string upgradeTitle = "Address Book Slot";
        const int BASE_SLOT_QUANTITY = 3;
        const int PURCHASE_QUANTITY_LIMIT = 2;

        bool hasExitedTheMatrix = gameplayManager.HasUpgrade("Exit the Matrix");
        string requiredUpgrade = "Address Book";
        if (hasExitedTheMatrix)
        {
            requiredUpgrade = "DNS Cache";
        }

        // If the player has already purchased the limit
        if (upgradeManager.GetQuantity(upgradeTitle) == PURCHASE_QUANTITY_LIMIT)
        {
            // Set error message
            screenText.text = "You cannot add any more slots to your " + requiredUpgrade + ".";

            // Make sure price is marked as "Maxed out" and italicized
            addressBookSlotPriceText.text = "Maxed Out";
            addressBookSlotPriceText.fontStyle = FontStyle.Italic;

            // Show on-screen message
            screenText.gameObject.SetActive(true);
        }
        // If the player has the address book upgrade and is under the limit
        else if (gameplayManager.HasUpgrade("Address Book"))
        {
            // Address Book instructions to display on purchase
            string instructions = "1 slot has been added to your " + requiredUpgrade;

            // If purchase is successful
            if (AttemptPurchaseUpgrade(upgradeTitle, instructions))
            {
                // Update the address book slot capacity for cacheManager
                cacheManager.maxCapacity = BASE_SLOT_QUANTITY + upgradeManager.GetQuantity(upgradeTitle);

                // If the player has purchased the limit
                if (upgradeManager.GetQuantity(upgradeTitle) == PURCHASE_QUANTITY_LIMIT)
                {
                    // Set price text as "Maxed out" and italicized
                    addressBookSlotPriceText.text = "Maxed Out";
                    addressBookSlotPriceText.fontStyle = FontStyle.Italic;

                    // show on-screen message
                    screenText.gameObject.SetActive(true);
                }

                // Update the HUD
                gameplayManager.ForceUpdateHUD();
            }
        }
        else
        {
            // Set error message
            screenText.text = "You need the <b>" + requiredUpgrade + "</b> upgrade first."; 

            // Show on-screen message
            screenText.gameObject.SetActive(true);
        }
    }

    public void PurchaseWhereCreditIsDue()
    {
        // Address Book instructions to display on purchase
        string instructions = "Congratulations!";

        // Attempt to purchase the task tracker
        string upgradeTitle = "Where Credit is Due";

        // Check if the player has the prereqs
        bool hasTaskTracker = upgradeManager.HasPurchasedUpgrade("Task Tracker");
        bool hasAddressBook = upgradeManager.HasPurchasedUpgrade("Address Book");
        bool hasExitTheMatrix = upgradeManager.HasPurchasedUpgrade("Exit the Matrix");
        if (hasTaskTracker && hasAddressBook && hasExitTheMatrix)
        {
            // Attempt to purchase Where Credit is Due
            if (AttemptPurchaseUpgrade(upgradeTitle, instructions))
            {
                // Navigate to credits then loop back to main menu
                gameplayManager.CompleteGameAndReset();
                fadeManager.FadeMethod("credits");
            }
        }
        else
        {
            string requiredCacheUpgradeName = "Address Book";
            if (gameplayManager.HasUpgrade("Exit the Matrix"))
            {
                requiredCacheUpgradeName = "DNS Cache";
            }

            screenText.text = "You must first purchase the <b>Task Tracker</b>, the <b>" + requiredCacheUpgradeName + "</b>, and the <b>Exit the Matrix</b> upgrades.";

            // Show on-screen message
            screenText.gameObject.SetActive(true);
        }
    }

    public bool AttemptPurchaseUpgrade(string upgradeTitle, string instructions)
    {
        bool isSuccessful = false;

        string upgradeName = "";

        if (upgradeTitle.ToLower() == "address book")
        {
            bool hasExitedTheMatrix = gameplayManager.HasUpgrade("Exit the Matrix");
            upgradeName = "Address Book";
            if (hasExitedTheMatrix)
            {
                upgradeName = "DNS Cache";
            }
        }
        else if (upgradeTitle.ToLower() == "address book slot")
        {
            bool hasExitedTheMatrix = gameplayManager.HasUpgrade("Exit the Matrix");
            upgradeName = "Address Book";
            if (hasExitedTheMatrix)
            {
                upgradeName = "DNS Cache Slot";
            }
        }
        else
        {
            upgradeName = upgradeTitle;
        }

        // If the requested upgrade can only be purchased once
        if (!upgradeManager.IsRepeatable(upgradeTitle))
        {
            // If the player has already purchased the upgrade
            if (upgradeManager.HasPurchasedUpgrade(upgradeTitle))
            {
                // Set error message
                screenText.text = "You've already purchased the " + upgradeName + " upgrade.\n" + instructions; // TODO: Add some snarky "No need to buy the same thing twice, right?" comment

                // Show on-screen message
                screenText.gameObject.SetActive(true);
                isSuccessful = false;
            }
            // If the player successfully purchases the upgrade
            else if(upgradeManager.AttemptPurchase(upgradeTitle))
            {
                // Set success message
                screenText.text = "You successfully purchased the " + upgradeName + " upgrade.\n" + instructions;

                // Show on-screen message
                screenText.gameObject.SetActive(true);

                isSuccessful = true;
            }
            // If the purchase was unsuccessful
            else
            {
                // Set error message
                screenText.text = "You need $" + upgradeManager.GetUpgradeCost(upgradeTitle) + " to purchase the " + upgradeName + " upgrade.";

                // Show on-screen message
                screenText.gameObject.SetActive(true);

                isSuccessful = false;
            }
        }
        // If the upgrade can be purchased multiple times
        else
        {
            // If successfully purchased again
            if (upgradeManager.AttemptPurchase(upgradeTitle))
            {
                // Set success message
                screenText.text = "You successfully purchased the " + upgradeName + " upgrade.\n" + instructions;

                // Show on-screen message
                screenText.gameObject.SetActive(true);

                isSuccessful = true;
            }
            // If purchase was unsuccessful
            else
            {
                // Set error message
                screenText.text = "You need $" + upgradeManager.GetUpgradeCost(upgradeTitle) + " to purchase the " + upgradeName + " upgrade.";

                // Show on-screen message
                screenText.gameObject.SetActive(true);

                isSuccessful = false;
            }
        }

        // Update computer UI
        ForceUpdateGUI();

        // Update HUD
        gameplayManager.ForceUpdateHUD();

        return isSuccessful;
    }

    void ForceUpdateGUI()
    {
        string requiredUpgrade = "Address Book";
        if (gameplayManager.HasUpgrade("Exit the Matrix"))
        {
            requiredUpgrade = "DNS Cache";
        }

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

        // Check if player already purchased the address book
        if (gameplayManager.HasUpgrade("Address Book"))
        {
            addressBookPriceText.text = "Purchased";
            addressBookPriceText.fontStyle = FontStyle.Italic;

            // Display current number of slot upgrades purchased
            addressBookSlotDescriptionText.text = "Adds 1 spot to " + requiredUpgrade + ".\n Current: " + upgradeManager.GetQuantity("Address Book Slot");
        }
        else
        {
            // Display note that the slot upgrades require the address book upgrade first
            addressBookSlotDescriptionText.text = "Adds 1 spot to " + requiredUpgrade + ".\n Requires " + requiredUpgrade + " upgrade.";
        }

        // Check if player already purchased the exit the matrix upgrade
        if (gameplayManager.HasUpgrade("Exit the Matrix"))
        {
            exitMatrixPriceText.text = "Purchased";
            exitMatrixPriceText.fontStyle = FontStyle.Italic;
        }

        // Update the HUD
        gameplayManager.ForceUpdateHUD();
    }
}
