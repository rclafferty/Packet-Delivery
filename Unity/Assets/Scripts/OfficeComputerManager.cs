using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Behind_The_Scenes;
using System.Text;

public class OfficeComputerManager : MonoBehaviour
{
    [SerializeField] GameObject officeComputerCanvas;
    [SerializeField] Text screenText;
    [SerializeField] Text movementInstructions;

    bool isComputerShown;
    GameplayManager gameplayManager;

    Dictionary<string, string> abbreviationLookupTable;

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
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ShowHideComputerCanvas(bool isShown)
    {
        isComputerShown = isShown;
        officeComputerCanvas.gameObject.SetActive(isShown);
        movementInstructions.gameObject.SetActive(!isShown);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isComputerShown)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShowHideComputerCanvas(true);
            }
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
                Debug.Log("Already has current target: " + gameplayManager.CurrentTarget);
            }
            else
            {
                systemMessage = "Success! You've successfully setup the following delivery:\n";
                if (LetterManager.isFirstLetter)
                {
                    gameplayManager.GetStartingMessage();
                    LetterManager.isFirstLetter = false;
                }
                else
                {
                    gameplayManager.GetNextMessage();
                }
            }

            // TODO: Display the message details
            DisplayDetails(systemMessage);
        }
    }

    void DisplayDetails(in string systemMessage)
    {
        Message currentMessage = gameplayManager.CurrentTargetMessage;
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
        Message currentMessage = gameplayManager.CurrentTargetMessage;
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

            Debug.Log(gameplayManager.NextDeliveryLocation);
        }
    }

    public void Logistics()
    {
        screenText.text = "No logistics yet, sorry.";
    }
}
