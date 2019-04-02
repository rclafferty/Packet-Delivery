using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Assets.Scripts.Lookup_Agencies;

public class GeneralChatManager : MonoBehaviour
{
    GameplayManager gameplayManager;
    LevelManager levelManager;
    LookupAgencyManager lookupManager;

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

    [SerializeField]
    InputField inputField;

    List<Person> people;

    string chatText_message;
    string option1_message;
    string option2_message;

    UnityAction option1Action;
    UnityAction option2Action;

    readonly float CHAT_DELAY = 0.02f;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectsForScene();
        SetupBySceneName();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FindObjectsForScene()
    {
        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        lookupManager = GameObject.Find("LookupAgencyManager").GetComponent<LookupAgencyManager>();

        chatText = GameObject.Find("ChatText").GetComponent<Text>();
        option1Text = GameObject.Find("ChatText").GetComponent<Text>();
        option2Text = GameObject.Find("ChatText").GetComponent<Text>();

        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        inputField = GameObject.Find("InputField").GetComponent<InputField>();
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
        chatText_message = "Welcome to the " + location + " Lookup Agency." + chatText_message;
    }

    void StartTextAndButtons()
    {
        chatText_message = "How may we assist you?";
        option1_message = "I'm looking for someone.";
        option2_message = "I can't figure out where to go from here.";

        option1Action = delegate { ShowInputField(true); };
        option2Action = DepartTextAndButtons;
    }

    void DepartTextAndButtons()
    {
        chatText_message = "Come back soon!";
        option1_message = "Bye.";
        option2_message = "";

        option1Action = GoToTown;
        option2Action = null;
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
        inputField.gameObject.SetActive(isActive);
        option1Button.gameObject.SetActive(!isActive);

        if (isActive)
        {
            chatText_message = "Who are you looking for?";
            option1_message = "";
            option2_message = "This person.";

            option1Action = null;
            option2Action = LookupPerson;
        }
    }

    public void LookupPerson()
    {
        ShowInputField(false);

        if (inputField.text == gameplayManager.CurrentTarget)
        {
            // Lookup
            chatText_message = "Good. You remembered!";
            option1_message = "Thanks.";
            option2_message = "Bye.";

            // a1 = delegate { ShowInputField(true); };
            option1Action = GoToTown;
            // a2 = StartTextAndButtons;
            option2Action = null;
        }
        else
        {
            chatText_message = "Hmmm... That's not the person written on your package.";
            option1_message = "Whoops.";
            option2_message = "I'll try again later";

            option1Action = delegate { ShowInputField(true); };

            // a2 = StartTextAndButtons;
            option2Action = null;
        }
    }

    public void GoToTown()
    {
        levelManager.LoadLevel("town");
    }

    public void DisplayText(string c, string o1, string o2, UnityAction a1, UnityAction a2, out bool isClickable)
    {
        isClickable = false;

        WriteText(c, o1, o2);
        AddEventListeners(a1, a2);

        isClickable = true;
    }

    IEnumerator WriteText(string chat, string option1, string option2)
    {
        ClearText();

        // Write chat text
        for (int i = 0; i < chat.Length; i++)
        {
            // Wait for CHAT_DELAY seconds before writing the next letter
            yield return new WaitForSeconds(CHAT_DELAY);
            chatText.text += chat[i];
        }

        // If there is no text for option 1
        if (string.IsNullOrEmpty(option2))
        {
            // Disable the button
            option1Button.gameObject.SetActive(false);
        }
        else
        {
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