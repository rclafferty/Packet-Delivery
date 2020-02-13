using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Assets.Scripts.Lookup_Agencies;
using Assets.Scripts.Behind_The_Scenes;

public class GeneralChatManager : MonoBehaviour
{
    GameplayManager gameplayManager;
    LevelManager levelManager;
    [SerializeField] LookupAgencyManager lookupManager;

    [SerializeField] EventSystem eventSystem;

    [SerializeField] Text chatText;

    [SerializeField] Button option1Button;
    [SerializeField] Text option1Text;

    [SerializeField] Button option2Button;
    [SerializeField] Text option2Text;

    [SerializeField] InputField inputField;

    List<Person> people;

    string chatText_message;
    string option1_message;
    string option2_message;

    UnityAction option1Action;
    UnityAction option2Action;

    readonly float CHAT_DELAY = 0.005f;

    bool isClickable;

    bool inputFieldActive;

    Coroutine currentCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectsForScene();
        SetupBySceneName();

        isClickable = false;
        inputFieldActive = false;

        ShowText();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputFieldActive)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                option2Button.onClick.Invoke();
            }
        }
    }

    void FindObjectsForScene()
    {
        // Dynamic objects -- must look up at runtime
        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        lookupManager = GameObject.Find("LookupAgencyManager").GetComponent<LookupAgencyManager>();
    }

    void SetupBySceneName()
    {
        string sceneName = SceneManager.GetActiveScene().name.ToLower();
        string location = "";

        if (sceneName.Contains("central"))
        {
            location = "Central";
        }
        else if (sceneName.Contains("locallookupagencyne"))
        {
            location = "Northeast";
        }
        else if (sceneName.Contains("locallookupagencysw"))
        {
            location = "Southwest";
        }

        people = lookupManager.GetNamesByLocation(location);
        StartTextAndButtons();
        chatText_message = "Welcome to the " + location + " Lookup Agency. " + chatText_message;

        ShowInputField(false);
    }

    void StartTextAndButtons()
    {
        chatText_message = "How may we assist you?";
        option1_message = "I'm looking for someone.";
        option2_message = "I can't figure out where to go from here.";

        option1Action = delegate {
            ShowInputField(true);
            ShowText();
        };
        option2Action = delegate {
            WhereToGo();
            ShowText();
        };
    }

    void WhereToGo()
    {
        string location = gameplayManager.NextDeliveryLocation;
        string sceneName = SceneManager.GetActiveScene().name.ToLower();
        Debug.Log("Location = " + location + "\tScene Name = " + sceneName);

        bool isCurrentSpot = (
            (location == "CLA" && sceneName == "centrallookupagency") ||
            (location == "LLA NE" && sceneName == "locallookupagencyne") ||
            (location == "LLA SW" && sceneName == "locallookupagencysw")
        );

        if (isCurrentSpot)
        {
            chatText_message = "You're in the right spot! I can help you.";
            option1_message = "Okay!";
            option2_message = "On second thought, I'll come back later.";
            option1Action = delegate {
                StartTextAndButtons();
                ShowText();
            };
            option2Action = delegate {
                DepartTextAndButtons();
                ShowText();
            };
        }
        else
        {
            if (location == "CLA")
            {
                chatText_message = "Please visit the Central Lookup Agency for further directions";
            }
            if (location == "LLA NE")
            {
                chatText_message = FormatChatMessage("Local Lookup Agency", "northeast");

                gameplayManager.AddTodoItem("NE Local Lookup Agency");
            }
            else if (location == "LLA SW")
            {
                chatText_message = FormatChatMessage("Local Lookup Agency", "southwest");

                gameplayManager.AddTodoItem("SW Local Lookup Agency");
            }
            else if (location == "Office")
            {
                GameplayManager.DeliveryDirections nextStep;
                nextStep = gameplayManager.NextStep;

                if (nextStep.mapDirection == "northeast")
                {
                    nextStep.mapDirection = "north";
                }
                if (nextStep.mapDirection == "southwest")
                {
                    nextStep.mapDirection = "south";
                }

                chatText_message = FormatChatMessage(nextStep.color + " " + nextStep.building, nextStep.mapDirection);
            }
            else // if (location == "Home")
            {
                GameplayManager.DeliveryDirections nextStep;
                nextStep = gameplayManager.NextStep;

                if (nextStep.mapDirection == "northeast")
                {
                    nextStep.mapDirection = "north";
                }
                if (nextStep.mapDirection == "southwest")
                {
                    nextStep.mapDirection = "south";
                }

                // chatText_message = FormatChatMessage(nextStep.color + " " + nextStep.building, nextStep.mapDirection);
                string displayedAddress = gameplayManager.CurrentTargetMessage.Address;
                displayedAddress = displayedAddress[0].ToString() + displayedAddress[1].ToString() + displayedAddress[2].ToString() + " " + displayedAddress[3].ToString();
                chatText_message = "Please visit the " + nextStep.color + " " + nextStep.building + " at " + displayedAddress + " St";

                gameplayManager.AddTodoItem("Deliver to " + displayedAddress);
            }

            option1_message = "Thank you for your help.";
            option2_message = "";
            option1Action = delegate {
                DepartTextAndButtons();
                ShowText();
            };
            option2Action = delegate {
                DepartTextAndButtons();
                ShowText();
            };
        }
    }

    string FormatChatMessage(string building, string direction)
    {
        const string chatTemplate = "Please visit the # on the ## side of town for your next step.";
        string message = chatTemplate;
        message = message.Replace("##", direction);
        message = message.Replace("#", building);

        return message;
    }

    public void ShowText()
    {
        DisplayText(chatText_message, option1_message, option2_message, option1Action, option2Action, out isClickable);
    }

    public void DepartTextAndButtons()
    {
        chatText_message = "Come back soon!";
        option1_message = "Bye.";
        option2_message = "";

        option1Action = delegate { GoToTown(); };
        option2Action = delegate { ShowText(); };
    }

    void ClearText()
    {
        chatText.text = "";
        option1Text.text = "";
        option2Text.text = "";

        eventSystem.SetSelectedGameObject(null);
    }

    void ShowInputField(bool isActive)
    {
        // Disable first
        inputFieldActive = false;

        string location = gameplayManager.NextDeliveryLocation;
        string sceneName = SceneManager.GetActiveScene().name.ToLower();
        // Debug.Log("Location = " + location + "\tScene Name = " + sceneName);

        bool isCurrentSpot = (
            (sceneName == "centrallookupagency") ||
            (location == "LLA NE" && sceneName == "locallookupagencyne") ||
            (location == "LLA SW" && sceneName == "locallookupagencysw")
        );

        inputField.gameObject.SetActive(false);
        option1Button.gameObject.SetActive(true);
        option2Button.gameObject.SetActive(true);

        if (isActive)
        {
            if (gameplayManager.CurrentTargetMessage == null)
            {
                chatText_message = "You don't have a package with you to deliver.";
                option1_message = "Whoops. I'll come back when I have one.";
                option2_message = "";

                option1Action = delegate {
                    DepartTextAndButtons();
                    ShowText();
                };
                // option2Action = LookupPerson;
                option2Action = delegate {
                    DepartTextAndButtons();
                    ShowText();
                };

                return;
            }
            else if (!isCurrentSpot)
            {
                chatText_message = "Hmm... I'm not sure you're in the right spot. Try visiting the Central Lookup Agency to get started with your delivery.";
                option1_message = "I'll do that.";
                option2_message = "";

                option1Action = delegate
                {
                    DepartTextAndButtons();
                    ShowText();
                };
                // option2Action = LookupPerson;
                option2Action = delegate
                {
                    DepartTextAndButtons();
                    ShowText();
                };

                return;
            }
            else
            {
                inputField.gameObject.SetActive(isActive);
                option1Button.gameObject.SetActive(!isActive);
                option2Button.gameObject.SetActive(true);

                chatText_message = "Who are you looking for?";
                option1_message = "";
                option2_message = "This person.";

                option1Action = delegate { ShowText(); };
                // option2Action = LookupPerson;
                option2Action = delegate
                {
                    LookupPerson();
                    ShowText();
                };
            }
        }

        // Enable if applicable
        inputFieldActive = isActive;
    }

    public void LookupPerson()
    {
        ShowInputField(false);

        string target = gameplayManager.CurrentTarget.ToLower();
        if (!lookupManager.HasLoadedPopulationList)
        {
            lookupManager.LoadPopulationListFromTextAsset();
            // lookupManager.LoadPopulationList();
        }

        if (inputField.text.ToLower() == target)
        {
            // Lookup
            int index = lookupManager.LocationLookup(target); // NOT case-sensitive
            Debug.Log(target + " is at index " + index);

            // This line throws errors!!!!!!!!!!!!!!!!!!!
            string location = lookupManager.LOCATION_TEXT[index];

            if (SceneManager.GetActiveScene().name.ToLower().Contains("central"))
            {
                // Set next location
                if (location.ToLower() == "northeast")
                {
                    gameplayManager.NextDeliveryLocation = "LLA NE";
                }
                else if (location.ToLower() == "southwest")
                {
                    gameplayManager.NextDeliveryLocation = "LLA SW";
                }
            }
            else
            {
                GameplayManager.DeliveryDirections nextStep;
                nextStep.building = "house";
                nextStep.color = "yellow";
                nextStep.mapDirection = lookupManager.LOCATION_TEXT[lookupManager.LocationLookup(target)];

                gameplayManager.NextDeliveryLocation = "Home";

                gameplayManager.NextStep = nextStep;
            }

            WhereToGo();
        }
        else
        {
            chatText_message = "Hmmm... That's not the person written on your package.";
            option1_message = "Whoops.";
            option2_message = "I'll try again later";

            option1Action = delegate {
                ShowInputField(true);
                ShowText();
            };

            // a2 = StartTextAndButtons;
            option2Action = delegate {
                DepartTextAndButtons();
                ShowText();
            };
        }
    }

    public void GoToTown()
    {
        levelManager.LoadLevel("town");
    }

    public void DisplayText(string c, string o1, string o2, UnityAction a1, UnityAction a2, out bool isClickable)
    {
        isClickable = false;

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(WriteText(c, o1, o2));
        AddEventListeners(a1, a2);
    }

    IEnumerator WriteText(string chat, string option1, string option2)
    {
        isClickable = false;

        ClearText();

        bool option1ButtonInteractable = !string.IsNullOrEmpty(option1);
        bool option2ButtonInteractable = !string.IsNullOrEmpty(option2);

        // Disable button 1 if null or ""
        if (!option1ButtonInteractable)
        {
            option1Button.interactable = false;
        }

        // Disable button 2 if null or ""
        if (!option2ButtonInteractable)
        {
            option2Button.interactable = false;
        }

        // Enable button 1 if not empty
        option1Button.interactable = option1ButtonInteractable;

        // Enable button 2 if not empty
        option2Button.interactable = option2ButtonInteractable;

        // Write chat text
        for (int i = 0; i < chat.Length; i++)
        {
            // Wait for CHAT_DELAY seconds before writing the next letter
            yield return new WaitForSeconds(CHAT_DELAY);
            chatText.text += chat[i];
        }

        // Write option 1 text
        for (int i = 0; i < option1.Length; i++)
        {
            // Wait for CHAT_DELAY seconds before writing the next letter
            yield return new WaitForSeconds(CHAT_DELAY);
            option1Text.text += option1[i];
        }

        // Write option 2 text
        for (int i = 0; i < option2.Length; i++)
        {
            // Wait for CHAT_DELAY seconds before writing the next letter
            yield return new WaitForSeconds(CHAT_DELAY);
            option2Text.text += option2[i];
        }

        isClickable = true;
    }

    void AddEventListeners(UnityAction a1, UnityAction a2)
    {
        // Add option 1 listener
        option1Button.onClick.RemoveAllListeners();
        option1Button.onClick.AddListener(a1);

        // Add option 2 listener
        option2Button.onClick.RemoveAllListeners();
        option2Button.onClick.AddListener(a2);
    }
}