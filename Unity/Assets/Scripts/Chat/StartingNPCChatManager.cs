using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartingNPCChatManager : ChatManager
{
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
        SetSceneObjects(eventSystem, chatText, option1Text, option1Button, option2Text, option2Button);

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
        currentCoroutine = StartCoroutine(WriteText(thisLine.speaker, thisLine.line, thisLine.response, ""));

        dialogueIndex++;
    }
}
