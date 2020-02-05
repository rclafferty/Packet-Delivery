using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    protected readonly float CHAT_DELAY = 0.005f;

    EventSystem eventSystem;
    Text chatPromptText;
    Text option1Text;
    Text option2Text;
    Button option1Button;
    Button option2Button;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetSceneObjects(EventSystem eventSys, Text chatPrompt, Text option1T, Button option1B, Text option2T, Button option2B)
    {
        eventSystem = eventSys;
        chatPromptText = chatPrompt;
        option1Text = option1T;
        option1Button = option1B;
        option2Text = option2T;
        option2Button = option2B;
    }

    public IEnumerator WriteText(string speaker, string chatPrompt, string option1, string option2)
    {
        ClearText();

        if (option1Button != null)
        {
            option1Button.interactable = !(string.IsNullOrEmpty(option1));
        }
        
        if (option2Button != null)
        {
            option2Button.interactable = !(string.IsNullOrEmpty(option2));
        }

        if (!string.IsNullOrEmpty(speaker))
        {
            for (int i = 0; i < speaker.Length; i++)
            {
                yield return new WaitForSeconds(CHAT_DELAY);
                chatPromptText.text += speaker[i];
            }

            yield return new WaitForSeconds(CHAT_DELAY);
            chatPromptText.text += ":\n";
        }

        for (int i = 0; i < chatPrompt.Length; i++)
        {
            yield return new WaitForSeconds(CHAT_DELAY);
            chatPromptText.text += chatPrompt[i];
        }

        for (int i = 0; i < option1.Length; i++)
        {
            yield return new WaitForSeconds(CHAT_DELAY);
            option1Text.text += option1[i];
        }

        for (int i = 0; i < option1.Length; i++)
        {
            yield return new WaitForSeconds(CHAT_DELAY);
            option2Text.text += option2[i];
        }
    }

    void ClearText()
    {
        if (eventSystem != null)
        {
            eventSystem.SetSelectedGameObject(null);
        }

        if (chatPromptText != null)
        {
            chatPromptText.text = "";
        }

        if (option1Text != null)
        {
            option1Text.text = "";
        }

        if (option2Text != null)
        {
            option2Text.text = "";
        }
    }

    public void AddEventListeners(UnityAction action1, UnityAction action2)
    {
        if (option1Button != null)
        {
            option1Button.onClick.RemoveAllListeners();

            if (action1 != null)
            {
                option1Button.onClick.AddListener(action1);
            }
        }

        if (option2Button != null)
        {
            option2Button.onClick.RemoveAllListeners();

            if (action2 != null)
            {
                option2Button.onClick.AddListener(action2);
            }
        }
    }
}
