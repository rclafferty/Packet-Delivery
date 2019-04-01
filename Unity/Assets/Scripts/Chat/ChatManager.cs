using Assets.Scripts.Behind_The_Scenes;
using Assets.Scripts.Chat;
using Assets.Scripts.Lookup_Agencies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    [SerializeField]
    GameObject chatCanvas;

    [SerializeField]
    GameObject chatTextObject;
    [SerializeField]
    Text chatText;

    [SerializeField]
    GameObject option1Object;
    [SerializeField]
    Button option1Button;
    [SerializeField]
    Text option1Text;

    [SerializeField]
    GameObject option2Object;
    [SerializeField]
    Button option2Button;
    [SerializeField]
    Text option2Text;

    [SerializeField]
    EventSystem eventSystem;

    [SerializeField]
    GameObject[] peopleButtons;
    [SerializeField]
    GameObject[] locationButtons;

    [SerializeField]
    InputField inputField;

    List<Person> people;

    [SerializeField]
    LookupAgencyManager lookupManager;

    [SerializeField]
    GameplayManager gameplayManager;

    [SerializeField]
    LevelManager levelManager;

    static readonly float CHAT_DELAY = 0.02f;

    string chatText_message;
    string option1_message;
    string option2_message;

    bool isClickable;

    ConversationTree conversationTree;

    [SerializeField]
    UnityAction action1;
    [SerializeField]
    UnityAction action2;

    [SerializeField]
    CLALookupManager claManager;

    // Start is called before the first frame update
    void Start()
    {
        isClickable = false;

        FindManagersInScene();
        
        UnityAction action1 = delegate { ToggleInputField(true); };
        UnityAction action2 = GoToTown;

        PopulateList();

        ToggleListOfPeople(false);
        ToggleInputField(false);

        DisplayText(chatText_message, option1_message, option2_message, action1, action2, out isClickable);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FindManagersInScene()
    {
        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    void SetupBySceneName()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        
        /*if (string.IsNullOrEmpty(gameplayManager.CurrentTarget))
        {
            option1_message = "I just came by to say hi. How are you?";
            option2_message = "Nevermind.";

            DisplayText(ChooseOption1, GoToTown);
        }
        else*/ if (sceneName.ToLower().Contains("centrallookupagency"))
        {
            // Get entire list of people
            people = lookupManager.CLAListOfPeople;
            StartTextAndButtons();
            chatText_message = "Welcome to the Central Lookup Agency. " + chatText_message;
        }
        else if (sceneName.ToLower().Contains("locallookupagencyne"))
        {
            // Get list of people for LLA NE
            people = lookupManager.GetNamesByLocation("Northeast");
            StartTextAndButtons();
            chatText_message = "Welcome to the Northeast Lookup Agency. " + chatText_message;
        }
        else if (sceneName.ToLower().Contains("locallookupagencysw"))
        {
            // Get list of people for LLA SW
            people = lookupManager.GetNamesByLocation("Southwest");
            StartTextAndButtons();
            chatText_message = "Welcome to the Southwest Lookup Agency. " + chatText_message;
        }
    }

    void StartTextAndButtons()
    {
        chatText_message = "How may we assist you?";
        //option1_message = "I'm looking for a " + gameplayManager.CurrentTarget + ". Do you know where I can find them?";
        option1_message = "I'm looking for someone.";
        action1 = ChooseOption1;
        option2_message = "I can't figure out where to go from here.";
        action2 = ChooseOption2;
    }

    void ClearText()
    {
        chatText.text = "";
        option1Text.text = "";
        option2Text.text = "";

        eventSystem.SetSelectedGameObject(null);
    }

    void DisplayText(string c, string o1, string o2)
    {
        StartCoroutine(WriteText(c, o1, o2));
    }

    public void DisplayText(string c, string o1, string o2, UnityAction action1, UnityAction action2, out bool isClickable)
    {
        isClickable = false;

        StartCoroutine(WriteText(c, o1, o2));

        if (action1 == null)
        {
            Debug.Log("Action 1 is null");
        }
        
        if (action2 == null)
        {
            Debug.Log("Action 2 is null");
        }

        AddEventListeners(action1, action2);

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

        // Write option 1 text
        for (int i = 0; i < option1.Length; i++)
        {
            // Wait for CHAT_DELAY seconds before writing the next letter
            yield return new WaitForSeconds(CHAT_DELAY);
            option1Text.text += option1[i];
        }

        // If there is only one option
        if (string.IsNullOrEmpty(option2))
        {
            // Disable the button
            option2Button.gameObject.SetActive(false);
        }
        // if there are two options
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

    public void ToggleListOfPeople(bool isActive)
    {
        // This method passes in a boolean value indicating if the GUI should be active or inactive
        // If the boolean value isActive is true, then SetActive() will show the GUI
        // If the boolean value isActive is false, then SetActive() will hide the GUI
        // listOfPeopleGUI.SetActive(isActive);

        // If the list of people is set to active, the chat canvas will be inactive
        // If the list of people is set to inactive, the chat canvas will be active
        chatCanvas.SetActive(!isActive);
    }

    public void ToggleInputField(bool isActive)
    {
        ShowInputField(isActive);
    }

    public void GoToTown()
    {
        levelManager.LoadLevel("town");
    }

    public void ChooseOption1()
    {
        if (!isClickable)
            return;

        UnityAction newAction1 = null;
        UnityAction newAction2 = null;

        string prevMessage = chatText_message.ToLower();

        if (prevMessage.Contains("i'm busy"))
        {
            option1_message = "Okay. I'll leave you be.";
            option2_message = "";

            newAction1 = ChooseOption1;
            newAction2 = ChooseOption2;
        }
        else if (prevMessage.Contains("how may we assist you"))
        {
            chatText_message = "Check this list and see which region of town they are located in.";
            option1_message = "Okay.";
            newAction1 = ChooseOption1;
            option2_message = "Actualy, I'll try back another time.";
            newAction2 = ChooseOption2;
        }
        else if (prevMessage.Contains("check this list"))
        {
            GiveDirections();
        }

        DisplayText(chatText_message, option1_message, option2_message, newAction1, newAction2, out isClickable);
    }

    public void ChooseOption2()
    {
        if (!isClickable)
            return;

        GoToTown();
    }

    // possible: public string[] GiveDirections(), where this returns {next location, direction}
    public void GiveDirections()
    {
        chatText_message = "Sorry. I don't have directions to give you today.";
        option1_message = "Okay. Thanks anyways.";
        option2_message = "";
    }

    public void PopulateList()
    {
        string name = SceneManager.GetActiveScene().name.ToLower();
        if (name.Contains("central"))
        {
            CLAPopulateList();
        }
        else if (name.Contains("ne"))
        {
            LLAPopulateList(0);
        }
        else
        {
            LLAPopulateList(1);
        }
    }

    public void CLAPopulateList()
    {
        // Get entire list of people

        // List each person and their respective location (NE vs SW)
    }

    public void LLAPopulateList(int index)
    {
        // Get list of people in the indicated area (0 = NE, 1 = SW, etc...)

        // List each person within that indicated area
    }

    public void ShowInputField(bool tf)
    {
        inputField.gameObject.SetActive(tf);
        option1Button.gameObject.SetActive(!tf);
        // option2Button.gameObject.SetActive(!tf);

        if (tf)
        {
            chatText_message = "Who are you looking for?";
            option1_message = "";
            option2_message = "This person.";

            UnityAction a1 = null;
            UnityAction a2 = LookupPerson;

            DisplayText(chatText_message, option1_message, option2_message, a1, a2, out isClickable);
        }
    }

    public void LookupPerson()
    {
        ShowInputField(false);

        UnityAction a1 = null;
        UnityAction a2 = null;

        if (inputField.text == gameplayManager.CurrentTarget)
        {
            // Lookup
            chatText_message = "Good. You remembered!";
            option1_message = "Thanks.";
            option2_message = "Bye.";

            a1 = delegate{ShowInputField(true);};
            a2 = StartTextAndButtons;
        }
        else
        {
            chatText_message = "Hmmm... That's not the person written on your package.";
            option1_message = "Whoops.";
            option2_message = "I'll try again later";

            a1 = delegate{ShowInputField(true);};
            
            a2 = StartTextAndButtons;
        }

        // DisplayText(chatText_message, option1_message, option2_message, a1, a2, out isClickable);
    }
}
