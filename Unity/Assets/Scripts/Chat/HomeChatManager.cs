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

    [SerializeField]
    EventSystem eventSystem;

    [SerializeField]
    Text chatText;

    [SerializeField]
    Button option1Button;
    [SerializeField]
    Text option1Text;

    [SerializeField]
    Button option2Button;
    [SerializeField]
    Text option2Text;

    List<Person> people;

    string chatText_message;
    string option1_message;
    string option2_message;

    UnityAction option1Action;
    UnityAction option2Action;

    readonly float CHAT_DELAY = 0.02f;

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
        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        chatText = GameObject.Find("ChatText").GetComponent<Text>();
        option1Text = GameObject.Find("Option1Text").GetComponent<Text>();
        option1Button = GameObject.Find("Option1Button").GetComponent<Button>();
        option2Text = GameObject.Find("Option2Text").GetComponent<Text>();
        option2Button = GameObject.Find("Option2Button").GetComponent<Button>();

        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
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
        if (gameplayManager.NextDeliveryLocation.ToLower() != "home")
        {
            chatText_message = "I wasn't expecting a package.";
            option1_message = "Sorry. I must have the wrong house.";
        }
        else
        {
            chatText_message = "Thank you very much!";
            option1_message = "Enjoy!";

            gameplayManager.CompleteTask();
        }
        option2_message = "";

        option1Action = delegate {
            DepartTextAndButtons();
            ShowText();
        };
        option2Action = delegate { ShowText(); };
    }
    
    public void ShowText()
    {
        isClickable = false;

        StartCoroutine(WriteText(chatText_message, option1_message, option2_message));

        AddEventListeners(option1Action, option2Action);

        isClickable = true;
    }

    public void DisplayText(string c, string o1, string o2, UnityAction a1, UnityAction a2, out bool isClickable)
    {
        isClickable = false;

        StartCoroutine(WriteText(c, o1, o2));
        AddEventListeners(a1, a2);
    }

    IEnumerator WriteText(string chat, string option1, string option2)
    {
        isClickable = false;

        ClearText();

        // Write chat text
        for (int i = 0; i < chat.Length; i++)
        {
            // Wait for CHAT_DELAY seconds before writing the next letter
            yield return new WaitForSeconds(CHAT_DELAY);
            chatText.text += chat[i];
        }

        // If there is no text for option 1
        if (string.IsNullOrEmpty(option1))
        {
            // Disable the button
            option1Button.gameObject.SetActive(false);
        }
        else
        {
            // Make sure the button is enabled
            option1Button.gameObject.SetActive(true);

            // Write option 1 text
            for (int i = 0; i < option1.Length; i++)
            {
                // Wait for CHAT_DELAY seconds before writing the next letter
                yield return new WaitForSeconds(CHAT_DELAY);
                option1Text.text += option1[i];
            }
        }

        // If there is no text for option 2
        if (string.IsNullOrEmpty(option2))
        {
            // Disable the button
            option2Button.gameObject.SetActive(false);
        }
        else
        {
            // Make sure the button is enabled
            option2Button.gameObject.SetActive(true);

            // Write option 2 text
            for (int i = 0; i < option2.Length; i++)
            {
                // Wait for CHAT_DELAY seconds before writing the next letter
                yield return new WaitForSeconds(CHAT_DELAY);
                option2Text.text += option2[i];
            }
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

    void ClearText()
    {
        chatText.text = "";
        option1Text.text = "";
        option2Text.text = "";

        eventSystem.SetSelectedGameObject(null);
    }

    public void DepartTextAndButtons()
    {
        chatText_message = "Oh. Ok then. Have a good day!";
        option1_message = "Bye.";
        option2_message = "";

        option1Action = delegate { GoToTown(); };
        option2Action = delegate { ShowText(); };
    }

    public void GoToTown()
    {
        levelManager.LoadLevel("town");
    }
}
