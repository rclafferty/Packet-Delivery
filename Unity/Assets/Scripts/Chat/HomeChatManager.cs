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

    [SerializeField] Transition fadeTransitionObject;

    List<Person> people;

    string chatTextMessage;
    string option1Message;
    string option2Message;

    UnityAction option1Action;
    UnityAction option2Action;

    readonly float CHAT_DELAY = 0.005f;

    Coroutine currentCoroutine;

    bool isClickable;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectsForScene();
        StartDialogue();

        isClickable = false;

        DisplayText();
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

    void StartDialogue()
    {
        chatTextMessage = "Are you the one with my package?";
        option1Message = "I am!";
        option2Message = "I'm sorry. I think I am at the wrong house.";

        option1Action = delegate {
            DeliverPackage();
            DisplayText();
        };
        option2Action = delegate {
            DepartDialogue();
            DisplayText();
        };
    }

    void DeliverPackage()
    {
        Debug.Log("Current Address: " + gameplayManager.currentAddress);

        if (gameplayManager.HasUpgrade("Exit the Matrix"))
        {
            if (gameplayManager.NextStep.nextStep == AddressManager.DetermineIPFromHouseInfo(gameplayManager.CurrentMessage.Recipient.HouseNumber, gameplayManager.CurrentMessage.Recipient.NeighborhoodID))
            {
                chatTextMessage = "Thank you very much!";
                option1Message = "Enjoy!";

                option1Action = delegate
                {
                    DepartDialogue();
                    DisplayText();
                    gameplayManager.CompleteTask();
                };
            }
            else
            {
                WrongLocation();
            }
        }
        else
        { 
            if (!gameplayManager.NextStep.nextStep.Contains("Residence #"))
            {
                WrongLocation();
            }
            else if (gameplayManager.currentAddress != gameplayManager.CurrentMessage.Recipient.HouseNumber.ToString())
            {
                WrongLocation();
            }
            else
            {
                chatTextMessage = "Thank you very much!";
                option1Message = "Enjoy!";

                option1Action = delegate
                {
                    DepartDialogue();
                    DisplayText();
                    gameplayManager.CompleteTask();
                };
            }
        }
    }

    [System.Obsolete("Instead, change to UI notification BEFORE entering house.")]
    void WrongLocation()
    {
        // Display some error message in chat
        chatTextMessage = "I wasn't expecting a package.";
        option1Message = "Sorry. I must have the wrong house.";
        option2Message = "";

        option1Action = delegate {
            DepartDialogue();
            DisplayText();
        };
        option2Action = delegate
        {
            DepartDialogue();
            DisplayText();
        };
    }

    void DisplayText()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(WriteTextToUI(chatTextMessage, option1Message, option2Message));
        AddEventListeners(option1Action, option2Action);
    }

    IEnumerator WriteTextToUI(string chat, string option1, string option2)
    {
        // Clear displayed text
        chatText.text = "";
        option1Text.text = "";
        option2Text.text = "";

        // Deselect all buttons
        eventSystem.SetSelectedGameObject(null);

        option1Button.interactable = !string.IsNullOrEmpty(option1);
        option2Button.interactable = !string.IsNullOrEmpty(option2);

        // Write to chat prompt
        for (int i = 0; i < chat.Length; i++)
        {
            yield return new WaitForSeconds(CHAT_DELAY);
            chatText.text += chat[i];
        }

        // Write option 1 button text
        for (int i = 0; i < option1.Length; i++)
        {
            yield return new WaitForSeconds(CHAT_DELAY);
            option1Text.text += option1[i];
        }

        // Write option 2 button text
        for (int i = 0; i < option2.Length; i++)
        {
            yield return new WaitForSeconds(CHAT_DELAY);
            option2Text.text += option2[i];
        }
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

    public void DepartDialogue()
    {
        chatTextMessage = "Have a good day!";
        option1Message = "Bye.";
        option2Message = "";

        // Make both buttons redirect to town
        option1Action = delegate { GoToTown(); };
        option2Action = delegate { GoToTown(); };
    }

    public void GoToTown()
    {
        fadeTransitionObject.FadeMethod("town");
    }
}
