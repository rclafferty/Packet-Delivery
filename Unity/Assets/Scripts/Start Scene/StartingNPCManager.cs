/* File: StartingNPCManager.cs
 * Author: Casey Lafferty
 * Project: Packet Delivery
 */

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartingNPCManager : MonoBehaviour
{
    // Event System reference for clearing clicked buttons
    [SerializeField] EventSystem eventSystem;

    // Chat text and button references
    [SerializeField] Text chatText;
    [SerializeField] Button option1Button;
    [SerializeField] Text option1Text;

    // Next scene to navigate to after dialogue
    [SerializeField] string nextSceneName;

    // Dialogue text to be loaded from file
    [SerializeField] TextAsset introDialogueText;
    
    // Delay between each character when writing text to UI
    const float CHAT_DELAY = 0.005f;

    // Which step in the dialogue sequence is currently shown
    int dialogueIndex;

    // Only one coroutine should run at a time
    Coroutine currentCoroutine;

    // List of dialogue lines
    ArrayList dialogueLines;

    // Information about each dialogue line
    struct DialogueLine
    {
        // Who is speaking
        public string speaker;

        // The sentence(s) being spoken
        public string line; 

        // The response from the player to the dialogue
        public string response;
    }

    // Global strings to format before placing them in the text objects
    string message_ChatText;
    string message_Option1Text;

    // Player sprite to display after dialogue
    public Sprite sprite_PlayerDown;
    
    void Start()
    {
        // Set a default scene to transition to after dialogue
        if (string.IsNullOrEmpty(nextSceneName))
        {
            // By default, go to office
            nextSceneName = "office";
        }

        // Load/parse dialogue text
        ParseDialogueText();

        // Set all appropriate button events
        option1Button.onClick.RemoveAllListeners();
        option1Button.onClick.AddListener(ModifiedDialogue);

        // Start the dialogue
        dialogueIndex = 0;
        ModifiedDialogue();
    }

    private void ParseDialogueText()
    {
        // Get dialogue information from text file split by line
        string[] parts = introDialogueText.text.Split('\n');
        string[] speakerParts = parts[0].Split(':');
        string speaker = speakerParts[1].Trim();
        
        dialogueLines = new ArrayList();
        
        DialogueLine dialogueLine;
        
        // Same speaker throughout
        dialogueLine.speaker = speaker;
        
        // For all lines in the dialogue
        for (int i = 2 /* line 1 is blank */; i < parts.Length; i += 2)
        {
            // Get the line and response
            dialogueLine.line = parts[i].Trim();
            dialogueLine.response = parts[i + 1].Trim();
            dialogueLines.Add(dialogueLine);
        }
    }
    
    public void ModifiedDialogue()
    {
        // If the dialogue has ended
        if (dialogueIndex >= dialogueLines.Count)
        {
            // Load next scene
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        // Deselect all buttons
        eventSystem.SetSelectedGameObject(null);

        // Get this current dialogue line and response
        DialogueLine thisLine = (DialogueLine)dialogueLines[dialogueIndex];
        string nextSpeakerLine = thisLine.speaker + ":\n" + thisLine.line;
        string nextResponseLine = thisLine.response;
        
        // If there is a currently running coroutine
        if (currentCoroutine != null)
        {
            // Stop the previous action
            StopCoroutine(currentCoroutine);
        }

        // Display dialogue to UI
        currentCoroutine = StartCoroutine(WriteText(nextSpeakerLine, nextResponseLine));

        // Increment index to next dialogue line
        dialogueIndex++;
    }

    IEnumerator WriteText(string chat, string option1)
    {
        chatText.text = "";
        option1Text.text = "";
        
        // Disable button 1 if null or ""
        option1Button.interactable = !string.IsNullOrEmpty(option1);

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
    }
}
