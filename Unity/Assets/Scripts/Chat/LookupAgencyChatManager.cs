using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Assets.Scripts.Lookup_Agencies;

public class LookupAgencyChatManager : ChatManager
{
    GameplayManager gameplayManager;
    [SerializeField] LookupAgencyManager lookupAgencyManager; // Revisit???

    [SerializeField] EventSystem eventSystem;
    [SerializeField] Text chatText;
    [SerializeField] Text option1Text;
    [SerializeField] Text option2Text;
    [SerializeField] Button option1Button;
    [SerializeField] Button option2Button;
    [SerializeField] InputField inputField;

    string chatString;
    string option1String;
    string option2String;

    UnityAction option1Action;
    UnityAction option2Action;

    Coroutine currentCoroutine;

    List<Person> listOfPeople;

    string thisLocation;

    // Start is called before the first frame update
    void Start()
    {
        // Dynamic objects -- must look up at runtime
        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        lookupAgencyManager = GameObject.Find("LookupAgencyManager").GetComponent<LookupAgencyManager>();

        // Set scene objects using the parent class
        SetSceneObjects(eventSystem, chatText, option1Text, option1Button, option2Text, option2Button);

        thisLocation = "";
        SetupBySceneName();

        inputField.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Changed from original -- change back if issues
        if (inputField.gameObject.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                option2Button.onClick.Invoke();
            }
        }
    }

    void SetupBySceneName()
    {
        string sceneName = SceneManager.GetActiveScene().name.ToLower();
        string location = "";

        string[] sceneNameSegments = { "central", "locallookupagencyne", "locallookupagencysw" };
        string[] locationNames = { "Central", "Northeast", "Southwest" };

        for (int i = 0; i < sceneNameSegments.Length; i++)
        {
            if (sceneName == sceneNameSegments[i])
            {
                location = locationNames[i];
            }
        }

        listOfPeople = lookupAgencyManager.GetNamesByLocation(location);
        thisLocation = location;
    }

    void StartTextAndButtons()
    {
        chatString = "How may we assist you?";
        option1String = "I'm looking for someone.";
        option2String = "I can't figure out where to go from here.";

        option1Action = delegate
        {
            AttemptStartLookup();
            ShowText();
        };

        option2Action = delegate
        {
            WhereToGo();
            ShowText();
        };
    }

    void WhereToGo()
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

    bool IsInCorrectLocation()
    {
        string nextDeliveryLocation = gameplayManager.NextDeliveryLocation;
        return (nextDeliveryLocation == "CLA" && thisLocation == "Central") ||
            (nextDeliveryLocation == "LLA NE" && thisLocation == "Northeast") ||
            (nextDeliveryLocation == "LLA SW" && thisLocation == "Southwest");
    }

    void CorrectCurrentLocation()
    {
        chatString = "You're in the right spot! I can help you.";
        option1String = "Okay!";
        option2String = "On second though, I'll come back later.";

        option1Action = delegate
        {
            StartTextAndButtons();
            ShowText();
        };

        option2Action = delegate
        {
            Depart();
            ShowText();
        };
    }

    void IncorrectCurrentLocation()
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

    void ShowText()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        // Write text using parent class
        currentCoroutine = StartCoroutine(WriteText("[Name]", chatString, option1String, option2String));

        // Add event listeners using parent class
        AddEventListeners(option1Action, option2Action);
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

    void ShowInputField(bool isActive)
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

    void LookupPerson()
    {
        ShowInputField(false);
        CheckLookupAgencyManagerListIsLoaded();

        string target = gameplayManager.CurrentTarget.ToLower();

        // Lookup
        if (inputField.text.ToLower() == target)
        {
            int index = lookupAgencyManager.LocationLookup(target);
            if (index == -1)
            {
                WrongPerson(); // Need a better solution
            }
            else
            {
                string location = lookupAgencyManager.LOCATION_TEXT[index];
            }
        }
        // Wrong person
        else
        {

        }
    }

    void CheckLookupAgencyManagerListIsLoaded()
    {
        if (!lookupAgencyManager.HasLoadedPopulationList)
        {
            lookupAgencyManager.LoadPopulationListFromTextAsset();
        }
    }

    void WrongPerson()
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

    void Depart()
    {
        chatString = "Have a good day!";
        option1String = "Bye.";
        option2String = "";

        option1Action = delegate { SceneManager.LoadScene("town"); };
        option2Action = null;
    }
}
