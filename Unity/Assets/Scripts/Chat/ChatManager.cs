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

    List<Person> people;

    [SerializeField]
    LookupAgencyManager lookupManager;

    [SerializeField]
    GameplayManager gameplayManager;

    [SerializeField]
    LevelManager levelManager;

    [SerializeField]
    GameObject listOfPeopleGUI;

    static readonly float CHAT_DELAY = 0.05f;

    string chatText_message;
    string option1_message;
    string option2_message;

    bool isClickable;

    ConversationTree conversationTree;

    // Start is called before the first frame update
    void Start()
    {
        isClickable = false;

        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        // lookupManager = GameObject.Find("LookupAgencyManager").GetComponent<LookupAgencyManager>();

        string sceneName = SceneManager.GetActiveScene().name;

        UnityAction action1;
        UnityAction action2;

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

            chatText_message = "Welcome to the Central Lookup Agency. How may we assist you?";
            option1_message = "I'm looking for a " + gameplayManager.CurrentTarget + ". Do you know where I can find them?";
            action1 = ChooseOption1;
            option2_message = "I can't figure out where to go from here.";
            action2 = ChooseOption2;
        }
        else if (sceneName.ToLower().Contains("locallookupagencyne"))
        {
            // Get list of people for LLA NE
            people = lookupManager.GetNamesByLocation("Northeast");

            chatText_message = "Welcome to the Northeast Local Lookup Agency. How may we assist you?";
            option1_message = "I'm looking for a " + gameplayManager.CurrentTarget + ". Do you know where I can find them?";
            action1 = ChooseOption1;
            option2_message = "I can't figure out where to go from here.";
            action2 = ChooseOption2;
        }
        else if (sceneName.ToLower().Contains("locallookupagencysw"))
        {
            // Get list of people for LLA SW
            people = lookupManager.GetNamesByLocation("Southwest");

            chatText_message = "Welcome to the Southwest Local Lookup Agency. How may we assist you?";
            option1_message = "I'm looking for a " + gameplayManager.CurrentTarget + ". Do you know where I can find them?";
            action1 = ChooseOption1;
            option2_message = "I can't figure out where to go from here.";
            action2 = ChooseOption2;
        }

        PopulateList();

        ToggleListOfPeople(false);

        DisplayText(chatText_message, option1_message, option2_message, null, null, out isClickable);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ClearText()
    {
        chatText.text = "";
        option1Text.text = "";
        option2Text.text = "";

        eventSystem.SetSelectedGameObject(null);
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
        listOfPeopleGUI.SetActive(isActive);

        // If the list of people is set to active, the chat canvas will be inactive
        // If the list of people is set to inactive, the chat canvas will be active
        chatCanvas.SetActive(!isActive);
    }

    public void GoToTown()
    {
        levelManager.LoadLevel("town");
    }

    public void ChooseOption(int option)
    {
        if (!isClickable)
            return;

        if (option == 1)
        {
            ChooseOption1();
        }
        else if (option == 2)
        {
            ChooseOption2();
        }
    }

    public void ChooseOption1()
    {
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
        GoToTown();
    }

    // possible: public string[] GiveDirections(), where this returns {next location, direction}
    public void GiveDirections()
    {

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
        // Get whole list of people

        // List each person and their respective location (NE vs SW)
    }

    public void LLAPopulateList(int index)
    {
        // Get list of people in the indicated area (0 = NE, 1 = SW, etc...)

        // List each person within that indicated area
    }
}
