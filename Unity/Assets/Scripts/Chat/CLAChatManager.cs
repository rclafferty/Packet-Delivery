using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CLAChatManager : MonoBehaviour
{
    GameObject listOfPeopleGUI;

    [SerializeField]
    EventSystem eventSystem;

    string currentTarget;

    [SerializeField]
    GameObject chatCanvas;

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

    float CHAT_DELAY;

    // Start is called before the first frame update
    void Start()
    {
        CHAT_DELAY = 0.05f;
        listOfPeopleGUI = GameObject.Find("List of People");
        listOfPeopleGUI.SetActive(false);

        // Dead code while I debug
        /*eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        chatText = GameObject.Find("ChatText").GetComponent<Text>();

        option1Button = GameObject.Find("Option1Button").GetComponent<Button>();
        option1Text = GameObject.Find("Option1Text").GetComponent<Text>();

        option2Button = GameObject.Find("Option2Button").GetComponent<Button>();
        option2Text = GameObject.Find("Option2Text").GetComponent<Text>();*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayText(string c, string o1, string o2, UnityAction action1, UnityAction action2, out bool isClickable)
    {
        isClickable = false;

        Debug.Log("Printing the following:\nChat: " + c + "\nOption 1: " + o1 + "\nOption2: " + o2);

        StartCoroutine(WriteText(c, o1, o2));
        AddEventListeners(action1, action2);

        isClickable = true;
    }

    void ClearText()
    {
        // Set all text strings to empty
        chatText.text = " ";
        option1Text.text = " ";
        option2Text.text = " ";

        // Deselect all previously selected buttons
        eventSystem.SetSelectedGameObject(null);
    }

    IEnumerator WriteText(string chat, string option1, string option2)
    {
        ClearText();

        Debug.Log("Printing the following:\nChat: " + chat + "\nOption 1: " + option1 + "\nOption2: " + option2);

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

    void AddEventListeners(UnityAction action1, UnityAction action2)
    {
        // Add option 1 listener
        option1Button.onClick.RemoveAllListeners();
        option1Button.onClick.AddListener(action1);

        // Add option 2 listener
        option2Button.onClick.RemoveAllListeners();
        option2Button.onClick.AddListener(action2);
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
}