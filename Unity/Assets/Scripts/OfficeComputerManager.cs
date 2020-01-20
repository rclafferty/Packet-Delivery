using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Behind_The_Scenes;
using System.Text;

public class OfficeComputerManager : MonoBehaviour
{
    [SerializeField] Canvas officeComputerCanvas;
    [SerializeField] Text screenText;

    bool isComputerShown;
    GameplayManager gameplayManager;

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
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ShowHideComputerCanvas(bool isShown)
    {
        isComputerShown = isShown;
        officeComputerCanvas.gameObject.SetActive(isShown);
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
            if (gameplayManager.HasCurrentTarget)
            {
                systemMessage = "Please complete your current delivery before initiating another.\n";
                Debug.Log("Already has current target: " + gameplayManager.CurrentTarget);
            }
            else
            {
                systemMessage = "Success! You've successfully initiated the following delivery:\n";
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

    void DisplayDetails(string systemMessage)
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
        if (gameplayManager.HasCurrentTarget)
        {
            // TODO: Visualize this
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
            if (gameplayManager.HasCurrentTarget)
            {
                string nextDestination = "";
                if (gameplayManager.NextDeliveryLocation == "CLA")
                {
                    nextDestination = "Central Lookup Agency";
                }
                else if (gameplayManager.NextDeliveryLocation == "LLA SW")
                {
                    nextDestination = "Southwest Local Lookup Agency";
                }
                else if (gameplayManager.NextDeliveryLocation == "LLA NE")
                {
                    nextDestination = "Northeast Local Lookup Agency";
                }
                else
                {
                    nextDestination = gameplayManager.NextDeliveryLocation;
                }

                // TODO: Visualize this
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
