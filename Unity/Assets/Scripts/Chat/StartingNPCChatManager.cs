using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartingNPCChatManager : MonoBehaviour
{
    public readonly float CHAT_DELAY = 0.005f;

    [SerializeField] EventSystem eventSystem;
    [SerializeField] Text chatText;
    [SerializeField] Text option1Text;
    [SerializeField] Text option2Text;
    [SerializeField] Button option1Button;
    [SerializeField] Button option2Button;

    [SerializeField] string nextSceneName;

    [SerializeField] TextAsset introDialogueText;

    string chatString;
    string option1String;
    string option2String;

    UnityAction option1Action;
    UnityAction option2Action;

    Coroutine currentCoroutine;

    struct DialogueLine
    {
        public string speaker;
        public string line;
        public string response;
    }

    List<DialogueLine> dialogueLines;

    int dialogueIndex;

    // Start is called before the first frame update
    void Start()
    {
        dialogueIndex = 0;

        if (string.IsNullOrEmpty(nextSceneName))
        {
            nextSceneName = "office";
        }

        // Set scene objects using the parent class
        // SetSceneObjects(eventSystem, chatText, option1Text, option1Button, option2Text, option2Button);

        ParseDialogueText();

        // Add event listeners using parent class
        AddEventListeners(Dialogue, null /* truly null */);
    }

    void ParseDialogueText()
    {
        string[] dialogueParts = introDialogueText.text.Split('\n');
        string[] speakerParts = dialogueParts[0].Split(':');
        string speaker = speakerParts[1].Trim();

        dialogueLines = new List<DialogueLine>();

        DialogueLine dialogueLine;
        dialogueLine.speaker = speaker;

        // line 1 is blank
        for (int i = 2; i < dialogueParts.Length; i += 2)
        {
            dialogueLine.line = dialogueParts[i].Trim();
            dialogueLine.response = dialogueParts[i + 1].Trim();
            dialogueLines.Add(dialogueLine);
        }
    }

    void Dialogue()
    {
        if (dialogueIndex >= dialogueLines.Count)
        {
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        DialogueLine thisLine = dialogueLines[dialogueIndex];
        if (thisLine.line == "Thanks! I've got to run now!")
        {
            Destroy(gameObject.GetComponent<SpriteRenderer>());
        }

        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        // Write text using parent class
        currentCoroutine = StartCoroutine(WriteTextToUI(thisLine.speaker, thisLine.line, thisLine.response));

        dialogueIndex++;
    }

    IEnumerator WriteTextToUI(string chat, string option1, string option2)
    {
        // Clear displayed text
        chatText.text = "";
        option1Text.text = "";
        option2Text.text = "";

        // Deselect all buttons
        eventSystem.SetSelectedGameObject(null);

        option1Button.interactable = !string.IsNullOrEmpty(option1);
        option2Button.interactable = !string.IsNullOrEmpty(option2);

        // Write to chat prompt
        for (int i = 0; i < chat.Length; i++)
        {
            yield return new WaitForSeconds(CHAT_DELAY);
            chatText.text += chat[i];
        }

        // Write option 1 button text
        for (int i = 0; i < option1.Length; i++)
        {
            yield return new WaitForSeconds(CHAT_DELAY);
            option1Text.text += option1[i];
        }

        // Write option 2 button text
        for (int i = 0; i < option2.Length; i++)
        {
            yield return new WaitForSeconds(CHAT_DELAY);
            option2Text.text += option2[i];
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
