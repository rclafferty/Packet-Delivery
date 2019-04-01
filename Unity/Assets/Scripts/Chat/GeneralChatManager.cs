using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GeneralChatManager : MonoBehaviour
{
    [SerializeField]
    GameplayManager gameplayManager;

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
    
    string chatText_message;
    string option1_message;
    string option2_message;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectsForScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FindObjectsForScene()
    {
        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();

        chatText = GameObject.Find("ChatText").GetComponent<Text>();
        option1Text = GameObject.Find("ChatText").GetComponent<Text>();
        option2Text = GameObject.Find("ChatText").GetComponent<Text>();

        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        inputField = GameObject.Find("InputField").GetComponent<InputField>();
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

            a1 = delegate { ShowInputField(true); };
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
}
