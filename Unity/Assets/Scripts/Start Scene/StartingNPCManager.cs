using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartingNPCManager : MonoBehaviour
{
    [SerializeField] EventSystem eventSystem;

    [SerializeField] Text chatText;
    [SerializeField] Button option1Button;
    [SerializeField] Text option1Text;

    [SerializeField] string nextSceneName;

    [SerializeField] TextAsset introDialogueText;

    const float RIGHT_X = -4.0f;
    const float LEFT_X = -13.0f;

    const float CHAT_DELAY = 0.02f;

    int dialogueIndex;

    Coroutine currentCoroutine;

    ArrayList dialogueLines;
    struct DialogueLine
    {
        public string speaker;
        public string line;
        public string response;
    }

    string message_ChatText;
    string message_Option1Text;

    public Sprite sprite_PlayerDown;

    // Start is called before the first frame update
    void Start()
    {
        if (string.IsNullOrEmpty(nextSceneName))
        {
            nextSceneName = "office";
        }

        ParseDialogueText();

        option1Button.onClick.RemoveAllListeners();
        option1Button.onClick.AddListener(ModifiedDialogue);

        dialogueIndex = 0;
        ModifiedDialogue();
    }

    private void ParseDialogueText()
    {
        string[] parts = introDialogueText.text.Split('\n');
        string[] speakerParts = parts[0].Split(':');
        string speaker = speakerParts[1].Trim();

        dialogueLines = new ArrayList();

        DialogueLine dialogueLine;
        dialogueLine.speaker = speaker;
        for (int i = 2 /* line 1 is blank */; i < parts.Length; i += 2)
        {
            dialogueLine.line = parts[i].Trim();
            dialogueLine.response = parts[i + 1].Trim();
            dialogueLines.Add(dialogueLine);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveNPC(bool direction)
    {
        Vector3 start = gameObject.transform.position;
        Vector3 end = gameObject.transform.position;

        if (direction)
        {
            // Move left -- start
            start.x = RIGHT_X;
            end.x = LEFT_X;
        }
        else
        {
            // Move right -- end
            start.x = RIGHT_X;
            end.x = LEFT_X;
        }

        // Lerp to moveToX
        // Vector3.Lerp(start, end, 3.0f);
    }

    public void ModifiedDialogue()
    {
        if (dialogueIndex >= dialogueLines.Count)
        {
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        DialogueLine thisLine = (DialogueLine)dialogueLines[dialogueIndex];
        string nextSpeakerLine = thisLine.speaker + ":\n" + thisLine.line;
        string nextResponseLine = thisLine.response;
        
        if (thisLine.line == "Thanks! I've got to run now!")
        {
            Destroy(gameObject.GetComponent<SpriteRenderer>());
        }

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(WriteText(nextSpeakerLine, nextResponseLine));

        dialogueIndex++;
    }

    IEnumerator WriteText(string chat, string option1)
    {
        chatText.text = "";
        option1Text.text = "";
        
        bool option1ButtonInteractable = !string.IsNullOrEmpty(option1);
        
        // Disable button 1 if null or ""
        if (!option1ButtonInteractable)
        {
            option1Button.interactable = false;
        }

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

        // Enable button 1 if not empty
        option1Button.interactable = option1ButtonInteractable;
    }
}
