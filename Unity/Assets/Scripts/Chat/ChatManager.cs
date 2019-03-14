using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    static ChatManager instance = null;

    GameplayManager gameplayManager;

    EventSystem eventSystem;

    string currentTarget;

    Text chatText;

    Button option1Button;
    Text option1Text;

    Button option2Button;
    Text option2Text;

    const float CHAT_DELAY = 0.0f;// 0.05f;

    bool isClickable;

    const string STARTING_CHAT_TEXT = "Welcome to the Central Lookup Agency. How may I assist you?";

    const string TEST_CHAT = STARTING_CHAT_TEXT;
    // const string TEST_OPTION1 = "I'm looking for a Casey Lafferty. Could you help me find them?";
    const string TEST_TARGET = "Casey Lafferty";
    const string TEST_OPTION2 = "Nevermind.";
    
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        
        chatText = GameObject.Find("ChatText").GetComponent<Text>();

        option1Button = GameObject.Find("Option1Button").GetComponent<Button>();
        option1Text = GameObject.Find("Option1Text").GetComponent<Text>();

        option2Button = GameObject.Find("Option2Button").GetComponent<Button>();
        option2Text = GameObject.Find("Option2Text").GetComponent<Text>();

        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        // TODO: Get start text
        string startChatText = TEST_CHAT;
        string startOption1 = "";
        string startOption2 = "";

        currentTarget = gameplayManager.CurrentTarget;

        currentTarget = TEST_TARGET;

        if (currentTarget == "")
        {
            startOption1 = "I just stopped in to say hi.";
        }
        else
        {
            startOption1 = "I'm looking for " + currentTarget + ". Could you help me find them?";
        }
        startOption2 = TEST_OPTION2;

        isClickable = false;

        StartCoroutine(DisplayStartingText(startChatText, startOption1, startOption2));
    }

    IEnumerator DisplayStartingText(string c, string o1, string o2)
    {
        yield return StartCoroutine(WriteText(c, o1, o2));

        // Add option 1 listener
        option1Button.onClick.RemoveAllListeners();
        option1Button.onClick.AddListener(ChooseOption1);

        // Add option 2 listener
        option2Button.onClick.RemoveAllListeners();
        option2Button.onClick.AddListener(GoToTown);

        // Allow the user to click the options
        isClickable = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ClearText()
    {
        chatText.text = " ";
        option1Text.text = " ";
        option2Text.text = " ";

        eventSystem.SetSelectedGameObject(null);
    }

    IEnumerator WriteText(string chat, string option1, string option2)
    {
        ClearText();

        for (int i = 0; i < chat.Length; i++)
        {
            yield return new WaitForSeconds(CHAT_DELAY);
            chatText.text += chat[i];
        }

        for (int i = 0; i < option1.Length; i++)
        {
            yield return new WaitForSeconds(CHAT_DELAY);
            option1Text.text += option1[i];
        }

        // If there is only one option
        if (string.IsNullOrEmpty(option2))
        {
            // Disable the button
            //option2Button.enabled = false;
            option2Button.gameObject.SetActive(false);
        }
        // if there are two options
        else
        {
            // Make sure the button is enabled
            option2Button.gameObject.SetActive(true);

            for (int i = 0; i < option2.Length; i++)
            {
                yield return new WaitForSeconds(CHAT_DELAY);
                option2Text.text += option2[i];
            }
        }
    }

    void ChooseOption1()
    {
        if (!isClickable)
        {
            return;
        }

        const string NEW_CHAT_TEXT = "Try visiting the Northeast Local Lookup Agency. They should be able to assist you further.";
        const string NEW_OPTION_1 = "Thank you for your help!";
        option1Button.onClick.RemoveAllListeners();
        option1Button.onClick.AddListener(GoToTown);
        const string NEW_OPTION_2 = "";

        StartCoroutine(WriteText(NEW_CHAT_TEXT, NEW_OPTION_1, NEW_OPTION_2));
    }

    void ChooseOption2()
    {
        if (!isClickable)
        {
            return;
        }

    }

    void GoToTown()
    {
        if (!isClickable)
        {
            return;
        }

        GameObject.Find("LevelManager").GetComponent<LevelManager>().LoadLevel("town");
    }
}
