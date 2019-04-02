﻿using System.Collections;
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

            // a1 = delegate { ShowInputField(true); };
            a1 = GoToTown;
            // a2 = StartTextAndButtons;
            a2 = null;
        }
        else
        {
            chatText_message = "Hmmm... That's not the person written on your package.";
            option1_message = "Whoops.";
            option2_message = "I'll try again later";

            a1 = delegate { ShowInputField(true); };

            // a2 = StartTextAndButtons;
            a2 = null;
        }
    }

    public void GoToTown()
    {
        levelManager.LoadLevel("town");
    }

    public void DisplayText(string c, string o1, string o2, UnityAction a1, UnityAction a2, out bool isClickable)
    {
        isClickable = false;
    }

    IEnumerator WriteText(string chat, string option1, string option2)
    {
        return null;
    }
}