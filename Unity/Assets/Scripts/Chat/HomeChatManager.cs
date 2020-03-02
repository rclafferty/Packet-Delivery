using Assets.Scripts.Lookup_Agencies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeChatManager : MonoBehaviour
{
    GameplayManager gameplayManager;
    LevelManager levelManager;

    [SerializeField] EventSystem eventSystem;

    [SerializeField] Text chatText;

    [SerializeField] Button option1Button;
    [SerializeField] Text option1Text;

    [SerializeField] Button option2Button;
    [SerializeField] Text option2Text;

    List<Person> people;

    string chatText_message;
    string option1_message;
    string option2_message;

    UnityAction option1Action;
    UnityAction option2Action;

    readonly float CHAT_DELAY = 0.005f;

    Coroutine currentCoroutine;

    bool isClickable;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectsForScene();
        StartTextAndButtons();

        isClickable = false;

        ShowText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FindObjectsForScene()
    {
        // Find necessary persistent managers in scene
        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    void StartTextAndButtons()
    {
        chatText_message = "Hello there! What brings you to my home?";
        option1_message = "I have a package for you.";
        option2_message = "I'm sorry. I think I am at the wrong house.";

        option1Action = delegate {
            DeliverPackage();
            ShowText();
        };
        option2Action = delegate {
            DepartTextAndButtons();
            ShowText();
        };
    }

    void DeliverPackage()
    {
        // Trim all values
        gameplayManager.NextDeliveryLocation = gameplayManager.NextDeliveryLocation.Trim();
        gameplayManager.currentAddress = gameplayManager.currentAddress.Trim();
        gameplayManager.deliveryAddress = gameplayManager.deliveryAddress.Trim();
        
        // If the next stop is not "home"
        if (gameplayManager.NextDeliveryLocation.ToLower() != "home")
        {
            // "I wasn't expecting a package" type of message
            WrongLocation();
        }
        // If the player is not at the right address
        else if (gameplayManager.currentAddress != gameplayManager.deliveryAddress)
        {
            // "I wasn't expecting a package" type of message
            WrongLocation();
        }
        // If at an empty address -- Error catching
        else if (gameplayManager.currentAddress == gameplayManager.deliveryAddress && gameplayManager.currentAddress == "")
        {
            // "I wasn't expecting a package" type of message
            WrongLocation();
        }
        // In the correct spot
        else
        {
            chatText_message = "Thank you very much!";
            option1_message = "Enjoy!";

            option1Action = delegate {
                if (gameplayManager.HasStartingLetter)
                    MorePackagesText();
                else
                    DepartTextAndButtons();
                ShowText();
                gameplayManager.CompleteTask();
            };
        }
    }

    void WrongLocation()
    {
        // Display some error message in chat
        chatText_message = "I wasn't expecting a package.";
        option1_message = "Sorry. I must have the wrong house.";

        option1Action = delegate {
            DepartTextAndButtons();
            ShowText();
        };
    }

    void MorePackagesText()
    {
        // Premise for returning and delivering another package
        chatText_message = "I want to hire you to send another package. I'll email you the details.";
        option1_message = "Thank you!";
        option2_message = "";

        option1Action = delegate {
            DepartTextAndButtons();
            ShowText();
        };
        option2Action = delegate { ShowText(); };

        // No longer has first letter
        gameplayManager.HasStartingLetter = false;
    }
    
    public void ShowText()
    {
        // Disable buttons
        isClickable = false;

        // If there is already a coroutine running
        if (currentCoroutine != null)
        {
            // Stop it
            StopCoroutine(currentCoroutine);
        }

        // Start this coroutine instead
        currentCoroutine = StartCoroutine(WriteText(chatText_message, option1_message, option2_message));

        // Make the buttons do what they need to
        AddEventListeners(option1Action, option2Action);

        // Enable buttons
        isClickable = true;
    }

    public void DisplayText(string c, string o1, string o2, UnityAction a1, UnityAction a2, out bool isClickable)
    {
        // Freeze UI buttons
        isClickable = false;

        // If there is a coroutine currently running
        if (currentCoroutine != null)
        {
            // Stop it
            StopCoroutine(currentCoroutine);
        }

        // Start this coroutine instead
        currentCoroutine = StartCoroutine(WriteText(c, o1, o2));

        // Make the buttons do what they need to
        AddEventListeners(a1, a2);
    }

    IEnumerator WriteText(string chat, string option1, string option2)
    {
        // Disable the UI buttons
        isClickable = false;

        // Erase all UI text
        ClearText();

        // Hardcoded locations
        string[] locations = { "103A", "403B", "301D", "403D" };
        string[] names = { "Uncle Doug", "Rebecca Lee", "Mayor Clark", "Mrs. Daily" };

        string thisName = "";
        
        // Find the current person's name
        for (int i = 0; i < locations.Length; i++)
        {
            if (gameplayManager.currentAddress == locations[i])
            {
                thisName = names[i];
                break;
            }
        }

        chat = thisName + ":\n" + chat;

        // Disable button 1 if null or ""
        if (string.IsNullOrEmpty(option1))
        {
            option1Button.interactable = false;
        }

        // Disable button 2 if null or ""
        if (string.IsNullOrEmpty(option2))
        {
            option2Button.interactable = false;
        }

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

        // Enable UI buttons
        isClickable = true;
    }

    void AddEventListeners(UnityAction a1, UnityAction a2)
    {
        // Add option 1 (button 1) listener
        option1Button.onClick.RemoveAllListeners();
        option1Button.onClick.AddListener(a1);

        // Add option 2 (button 2) listener
        option2Button.onClick.RemoveAllListeners();
        option2Button.onClick.AddListener(a2);
    }

    void ClearText()
    {
        chatText.text = "";
        option1Text.text = "";
        option2Text.text = "";

        // Deselect any previously selected buttons
        eventSystem.SetSelectedGameObject(null);
    }

    public void DepartTextAndButtons()
    {
        chatText_message = "Have a good day!";
        option1_message = "Bye.";
        option2_message = "";

        // Make both buttons redirect to town
        option1Action = delegate { GoToTown(); };
        option2Action = delegate { GoToTown(); };
    }

    public void GoToTown()
    {
        // Use Level Manager to change scenes
        levelManager.LoadLevel("town");
    }
}
