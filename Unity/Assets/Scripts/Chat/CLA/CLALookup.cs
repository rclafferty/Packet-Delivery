using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CLALookup : MonoBehaviour
{
    CLAChatManager chatManager;
    GameplayManager gameplayManager;
    EventSystem eventSystem;

    [SerializeField]
    string target;

    string chatText;
    string option1String;
    string option2String;

    // Mutex of sorts
    bool isClickable;

    // Location Labels
    string[] LOCATION_LABELS = { "Northeast", "Southwest" };

    // Start is called before the first frame update
    void Start()
    {
        isClickable = false;

        chatText = "Welcome to the Central Lookup Agency. How may we assist you?";

        chatManager = GameObject.Find("ChatManager").GetComponent<CLAChatManager>();
        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        Debug.Log("GameplayManager == null ? " + (gameplayManager == null));
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        target = gameplayManager.CurrentTarget;

        Debug.Log("Target: " + target);
        
        if (string.IsNullOrEmpty(target))
        {
            Setup_JustHereToChat();
        }
        else
        {
            Setup_FindTarget(target);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Setup_JustHereToChat()
    {
        option1String = "I just came by to say hi. How are you?";

        option2String = "Nevermind.";

        DisplayText(ChooseOption1, GoToTown);
    }

    void Setup_FindTarget(string thisTarget)
    {
        PopulateListOfPeople();

        option1String = "I'm looking for a " + thisTarget + ". Could you help me find them?";
        option2String = "Nevermind.";

        DisplayText(ChooseOption1, GoToTown);
    }

    public void ChooseOption1()
    {
        // If not able to choose an option
        if (!isClickable)
        {
            return;
        }

        UnityAction newAction1 = null;
        UnityAction newAction2 = null;

        // If no target
        if (chatText.ToLower().Contains("i'm busy"))
        {
            option1String = "Okay. I'll leave you be.";
            option2String = "";

            newAction1 = ChooseOption1;
            newAction2 = ChooseOption2;
        }
        // Searching for target
        else if (option1String.ToLower().Contains("i'm looking for"))
        {
            // Have the player choose the target from the list
            chatText = "Check this list and select the person you are looking for.";
            option1String = "Okay.";
            option2String = "";

            newAction1 = ShowList;
            newAction2 = ShowList;
        }
        // Just checked the list
        else if (chatText.ToLower().Contains("check this list") || chatText.ToLower().Contains("please choose your recipient"))
        {
            // ShowList();
            HideList();

            GameObject selectedObject = eventSystem.currentSelectedGameObject;
            if (selectedObject.GetComponentInChildren<Text>().text == target)
            {
                // Find which LLA to go to
                GameObject[] list = GameObject.FindGameObjectsWithTag("PersonLabel");

                GameObject thisObject;
                Text thisText;

                string lla = "";
                string direction = "";

                /*for (int i = 0; i < list.Length; i++)
                {
                    thisObject = list[i];
                    thisText = thisObject.GetComponentInChildren<Text>();
                    if (thisText.text == target)
                    {
                        int[] locations = gameplayManager.PeopleLocations;
                        int locationIndex = locations[i];
                        direction = LOCATION_LABELS[locationIndex];
                        lla = "Local Lookup Agency (" + direction + ")";
                    }
                }*/

                string[] namesOfPeople = gameplayManager.ListOfPeople;
                int[] locations = gameplayManager.PeopleLocations;

                for (int i = 0; i < namesOfPeople.Length; i++)
                {
                    string name = namesOfPeople[i];
                    if (name == target)
                    {
                        int locationIndex = locations[i];
                        direction = LOCATION_LABELS[locationIndex];
                        lla = "Local Lookup Agency (" + direction + ")";
                        break;
                    }
                }

                // Tell the player
                chatText = "Check with the " + lla + ", which is to the " + direction + " of us. They will be able to assist you further.";
                option1String = "Thank you for your help.";
                option2String = "";

                newAction1 = GoToTown;
                newAction2 = GoToTown;

                gameplayManager.VisitedCLA = true;
            }
            else
            {
                // Try again
                chatText = "That is not the person you said you are looking for. Please choose your recipient.";
                option1String = "My mistake.";
                option2String = "I'll try again later";

                newAction1 = ShowList;
                newAction2 = ShowList;
            }
        }

        DisplayText(newAction1, newAction2);
    }

    public void ChooseOption2()
    {
        // If not able to choose an option
        if (!isClickable)
        {
            return;
        }
    }

    void DisplayText(UnityAction option1, UnityAction option2)
    {
        chatManager.DisplayText(chatText, option1String, option2String, option1, option2, out isClickable);
    }

    public void GoToTown()
    {
        if (!isClickable)
        {
            return;
        }

        GameObject.Find("LevelManager").GetComponent<LevelManager>().LoadLevel("town");
    }

    public void ShowList()
    {
        chatManager.ToggleListOfPeople(true);
    }

    public void HideList()
    {
        chatManager.ToggleListOfPeople(false);
    }

    void PopulateListOfPeople()
    {
        string[] list = gameplayManager.ListOfPeople;
        int[] locations = gameplayManager.PeopleLocations;

        GameObject g;
        Button g_button;
        Text child;
        int locationIndex;

        GameObject[] nameButtons = GameObject.FindGameObjectsWithTag("PersonLabel");

        for (int i = 0; i < nameButtons.Length; i++)
        {
            g = nameButtons[i];
            g_button = g.GetComponent<Button>();
            child = g.GetComponentInChildren<Text>();
            child.text = list[i];

            // Set event listener for each button
            g_button.onClick.RemoveAllListeners();
            g_button.onClick.AddListener(ChooseOption1);
        }

        GameObject[] locationButtons = GameObject.FindGameObjectsWithTag("LocationLabel");

        for (int i = 0; i < locationButtons.Length; i++)
        {
            g = locationButtons[i];
            child = g.GetComponentInChildren<Text>();
            locationIndex = locations[i];
            child.text = LOCATION_LABELS[locationIndex];
        }
    }
}
