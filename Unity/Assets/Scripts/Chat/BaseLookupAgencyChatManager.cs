using Assets.Scripts.Lookup_Agencies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BaseLookupAgencyChatManager : ChatManager
{
    protected GameplayManager gameplayManager;
    [SerializeField] protected LookupAgencyManager lookupAgencyManager; // Revisit???

    [SerializeField] protected EventSystem eventSystem;
    [SerializeField] protected Text chatText;
    [SerializeField] protected Text option1Text;
    [SerializeField] protected Text option2Text;
    [SerializeField] protected Button option1Button;
    [SerializeField] protected Button option2Button;
    [SerializeField] protected InputField inputField;

    protected string chatString;
    protected string option1String;
    protected string option2String;

    protected UnityAction option1Action;
    protected UnityAction option2Action;

    protected Coroutine currentCoroutine;

    protected List<Person> listOfPeople;

    protected string thisLocation;

    // Start is called before the first frame update
    void Start()
    {
        // Dynamic objects -- must look up at runtime
        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        lookupAgencyManager = GameObject.Find("LookupAgencyManager").GetComponent<LookupAgencyManager>();

        // Set scene objects using the parent class
        SetSceneObjects(eventSystem, chatText, option1Text, option1Button, option2Text, option2Button);

        thisLocation = "";

        StartText();
        ShowInputField(false);
        ShowText();
    }

    protected void StartText()
    {
        chatString = "How may we assist you?";
        option1String = "I'm looking for someone.";
        option2String = "I can't figure out where to go from here.";

        option1Action = delegate
        {
            ShowInputField(true);
            ShowText();
        };
        option2Action = delegate
        {
            WhereToGo();
            ShowText();
        };
    }

    protected void WhereToGo()
    {
        if (IsInCorrectLocation())
        {
            CorrectCurrentLocation();
        }
        else
        {
            IncorrectCurrentLocation();
        }
    }

    protected void IncorrectCurrentLocation()
    {
        string nextDeliveryLocation = gameplayManager.NextDeliveryLocation;

        if (nextDeliveryLocation == "CLA")
        {
            chatString = FormatChatMessage("Central Lookup Agency", "center");
        }
        else if (nextDeliveryLocation == "LLA NE")
        {
            chatString = FormatChatMessage("Local Lookup Agency", "northeast");
        }
        else if (nextDeliveryLocation == "LLA NE")
        {
            chatString = FormatChatMessage("Local Lookup Agency", "southwest");
        }
        else
        {
            GameplayManager.DeliveryDirections nextStep = gameplayManager.NextStep;
            chatString = FormatChatMessage(nextStep.color + " " + nextStep.building, nextStep.mapDirection);
        }

        option1String = "Thank you for your help.";
        option2String = "";

        option1Action = delegate
        {
            Depart();
            ShowText();
        };
        option2Action = null;
    }

    protected void CorrectCurrentLocation()
    {
        chatString = "You're in the right spot! I can help you.";
        option1String = "Okay!";
        option2String = "On second though, I'll come back later.";

        option1Action = delegate
        {
            StartText();
            ShowText();
        };

        option2Action = delegate
        {
            Depart();
            ShowText();
        };
    }

    protected virtual bool IsInCorrectLocation()
    {
        string nextDeliveryLocation = gameplayManager.NextDeliveryLocation;
        return (nextDeliveryLocation == "CLA" && thisLocation == "Central") ||
            (nextDeliveryLocation == "LLA NE" && thisLocation == "Northeast") ||
            (nextDeliveryLocation == "LLA SW" && thisLocation == "Southwest");
    }

    void AttemptStartLookup()
    {
        if (gameplayManager.CurrentTargetMessage == null)
        {
            chatString = "You don't have a package with you to deliver.";
            option1String = "Whoops. I'll come back when I have one.";
            option2String = "";

            option1Action = delegate
            {
                Depart();
                ShowText();
            };
            option2Action = null;
        }
        else if (!IsInCorrectLocation())
        {
            chatString = "Hmm... I'm not sure you're in the right spot. Try visiting the Central Lookup Agency to get started with your delivery.";
            option1String = "I'll do that.";
            option2String = "";

            option1Action = delegate
            {
                Depart();
                ShowText();
            };
            option2Action = null;
        }
        else
        {
            ShowInputField(true);
        }
    }

    protected void ShowText()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        // Write text using parent class
        currentCoroutine = StartCoroutine(WriteText(speaker: "Agent", chatString, option1String, option2String));

        // Add event listeners using parent class
        AddEventListeners(option1Action, option2Action);
    }

    protected void ShowInputField(bool isActive)
    {
        inputField.gameObject.SetActive(isActive);
        option1Button.gameObject.SetActive(!isActive);
        option2Button.gameObject.SetActive(true); // Always show this one

        if (isActive)
        {
            chatString = "Who are you looking for?";
            option1String = "";
            option2String = "This person.";

            option1Action = null;
            option2Action = delegate
            {
                // Lookup person
                ShowText();
            };
        }
    }

    string FormatChatMessage(string building, string direction)
    {
        string message = "";
        if (building == "Central Lookup Agency")
        {
            message = "Please visit the Central Lookup Agency for your next step.";
        }
        else
        {
            const string chatTemplate = "Please visit the # on the ## side of town for your next step.";
            message = chatTemplate;
            message = message.Replace("##", direction.ToLower());
            message = message.Replace("#", building);
        }
        return message;
    }

    protected void WrongPerson()
    {
        chatString = "Hmm... That's not the person written on your package.";
        option1String = "Whoops. Let me try again.";
        option2String = "I'll try again later.";

        option1Action = delegate
        {
            ShowInputField(true);
            ShowText();
        };

        option2Action = delegate
        {
            Depart();
            ShowText();
        };
    }

    protected void Depart()
    {
        chatString = "Have a good day!";
        option1String = "Bye.";
        option2String = "";

        option1Action = delegate { SceneManager.LoadScene("town"); };
        option2Action = null;
    }
}
