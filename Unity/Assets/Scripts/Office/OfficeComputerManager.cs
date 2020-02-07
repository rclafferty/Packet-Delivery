﻿using System.Collections;
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
    // [SerializeField] Text movementInstructions;

    [SerializeField] Button[] logisticsButtons;
    [SerializeField] Button taskTrackerButton;
    [SerializeField] Text taskTrackerPriceText;
    [SerializeField] Button exitMatrixButton;
    [SerializeField] Text exitMatrixPriceText;

    bool isComputerShown;
    GameplayManager gameplayManager;

    Dictionary<string, string> abbreviationLookupTable;

    const int TASK_TRACKER_COST = 10;
    const int EXIT_THE_MATRIX_COST = 40;

    // Start is called before the first frame update
    void Start()
    {
        screenText.text = "";

        ShowHideComputerCanvas(false);

        GameObject object_gameplayManager = GameObject.Find("GameplayManager");
        if (object_gameplayManager != null)
        {
            gameplayManager = object_gameplayManager.GetComponent<GameplayManager>();
        }

        abbreviationLookupTable = new Dictionary<string, string>();
        abbreviationLookupTable.Add("CLA", "Central Lookup Agency");
        abbreviationLookupTable.Add("LLA SW", "Southwest Local Lookup Agency");
        abbreviationLookupTable.Add("LLA NE", "Northeast Local Lookup Agency");

        ShowHideLogisticsButtons(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ShowHideComputerCanvas(bool isShown)
    {
        isComputerShown = isShown;
        officeComputerCanvas.gameObject.SetActive(isShown);
        // movementInstructions.gameObject.SetActive(!isShown);

        player.IsWalkingEnabled = !isShown;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        ShowHideComputerUI();
    }

    private void ShowHideComputerUI()
    {
        if (!isComputerShown)
        {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShowHideComputerCanvas(true);
            }
#else
            if (Input.touches.Length > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Stationary)
                {
                    ShowHideComputerCanvas(true);
                }
            }
#endif
        }
    }

    public void TurnOffComputer()
    {
        ShowHideComputerCanvas(false);
    }

    public void NewDeliveryRequest()
    {
        if (gameplayManager != null)
        {
            string systemMessage = "";
            if (gameplayManager.HasCurrentTarget())
            {
                systemMessage = "Please complete your current delivery before initiating another.\n";
                // Debug.Log("Already has current target: " + gameplayManager.CurrentTarget);
            }
            else
            {
                systemMessage = "Success! You've successfully setup the following delivery:\n";
                if (LetterManager.isFirstLetter)
                {
                    gameplayManager.GetStartingMessage();
                    LetterManager.isFirstLetter = false;
                    // Debug.Log("GameplayManager Current Target 3 ? " + (gameplayManager.CurrentTargetMessage != null));
                }
                else
                {
                    gameplayManager.GetNextMessage();
                    // Debug.Log("GameplayManager Current Target 1 ? " + (gameplayManager.CurrentTargetMessage != null));
                }
            }

            // Debug.Log("GameplayManager Current Target 2 ? " + (gameplayManager.CurrentTargetMessage != null));

            // TODO: Display the message details
            DisplayDetails(systemMessage);
        }

        ShowHideLogisticsButtons(false);
        screenText.gameObject.SetActive(true);
    }

    void DisplayDetails(in string systemMessage)
    {
        Letter currentMessage = gameplayManager.CurrentTargetMessage;
        // Debug.Log("Current Message ? " + (currentMessage != null));
        string senderLine = "Sender: " + currentMessage.Sender;
        string receiverLine = "Recipient: " + currentMessage.Recipient;
        string bodyLine = "\n" + currentMessage.MessageBody;

        string displayText = "";
        if (!string.IsNullOrEmpty(systemMessage))
        {
            displayText = systemMessage + "\n";
        }
        displayText += senderLine + "\n" + receiverLine + "\n" + bodyLine;
        screenText.text = displayText;
    }

    void DisplayDetails()
    {
        Letter currentMessage = gameplayManager.CurrentTargetMessage;
        string senderLine = "Sender: " + currentMessage.Sender;
        string receiverLine = "Recipient: " + currentMessage.Recipient;
        string bodyLine = "\n" + currentMessage.MessageBody;

        string displayText = senderLine + "\n" + receiverLine + "\n" + bodyLine;
        screenText.text = displayText;
    }

    void DisplayNoActiveDeliveryError()
    {
        screenText.text = "You don't currently have an active delivery. Try starting a new request.";
    }

    public void ViewDeliveryDetails()
    {
        if (gameplayManager.HasCurrentTarget())
        {
            DisplayDetails();
        }
        else
        {
            DisplayNoActiveDeliveryError();
        }
        
        ShowHideLogisticsButtons(false);
        screenText.gameObject.SetActive(true);
    }

    public void NextDestination()
    {
        if (gameplayManager != null)
        {
            if (gameplayManager.HasCurrentTarget())
            {
                string nextDestination = "";

                // Attempt to lookup the abbreviation
                if (!abbreviationLookupTable.TryGetValue(gameplayManager.NextDeliveryLocation, out nextDestination))
                {
                    // If not found, display the stored location
                    nextDestination = gameplayManager.NextDeliveryLocation;
                }
                
                screenText.text = "You should try stopping by the " + nextDestination + " next";
            }
            else
            {
                DisplayNoActiveDeliveryError();
            }

            // Debug.Log(gameplayManager.NextDeliveryLocation);
        }

        ShowHideLogisticsButtons(false);
        screenText.gameObject.SetActive(true);
    }

    public void Logistics()
    {
        screenText.text = "No logistics yet, sorry.";

        if (gameplayManager.HasStartingLetter)
        {
            screenText.text = "Upgrades will be available after your first delivery is complete.";
        }
        else
        {
            ShowHideLogisticsButtons(true);
        }
    }

    void ShowHideLogisticsButtons(bool isShown)
    {
        foreach (Button b in logisticsButtons)
        {
            b.gameObject.SetActive(isShown);
        }

        if (isShown)
        {
            screenText.gameObject.SetActive(false);
        }
    }

    public void PurchaseTaskTracker()
    {
        string instructions = "To use the Task Tracker, press TAB to display your current target and your next delivery location.";
        if (PurchaseUpgrade("Task Tracker", TASK_TRACKER_COST, instructions, gameplayManager.HasTaskTracker))
        {
            gameplayManager.HasTaskTracker = true;
            taskTrackerPriceText.text = "Purchased";
            taskTrackerPriceText.fontStyle = FontStyle.Italic;
        }
    }

    public void PurchaseExitTheMatrix()
    {
        string instructions = "Now all addresses will be IP addresses and all lookup agencies will be based on real domain lookups.";
        if (PurchaseUpgrade("Exit the Matrix", EXIT_THE_MATRIX_COST, instructions, gameplayManager.HasExitedTheMatrix))
        {
            gameplayManager.HasExitedTheMatrix = true;
            exitMatrixPriceText.text = "Purchased";
            exitMatrixPriceText.fontStyle = FontStyle.Italic;
        }
    }

    bool PurchaseUpgrade(in string upgradeTitle, in int cost, in string instructions, bool hasPurchasedAlready)
    {
        if (hasPurchasedAlready)
        {
            screenText.gameObject.SetActive(true);
            screenText.text = "You've already purchased the " + upgradeTitle + " upgrade.\n" + instructions; // TODO: Add some snarky "No need to buy the same thing twice, right?" comment
            return false;
        }
        else if (gameplayManager.Money >= cost)
        {
            gameplayManager.Money -= cost;
            screenText.gameObject.SetActive(true);
            screenText.text = "You successfully purchased the " + upgradeTitle + " upgrade.\n" + instructions;
            return true;
        }
        else
        {
            screenText.text = "You need $" + cost + " to purchase the " + upgradeTitle + " upgrade.";
            return false;
        }
    }
}
