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

    const float RIGHT_X = -4.0f;
    const float LEFT_X = -13.0f;

    const float CHAT_DELAY = 0.02f;

    int dialogueIndex;

    Coroutine currentCoroutine;

    string[] dialogue =
    {
        "Hey there! I REALLY need this letter delivered today but I am too busy to do this.",
        "Can you deliver it for me? I'll pay you!",
        "Sure. Where is it going to?",
        "I need you to bring it to my Uncle Doug for me.",
        "I can do that.",
        "I already emailed you the rest of the details.",
        "Ok but...",
        "Thanks!",
        "But...",
        "...",
        "I don't know where his Uncle Doug lives..."
    };
    string[] names =
    {
        "Tanner",
        "Tanner",
        "Me",
        "Tanner",
        "Me",
        "Tanner",
        "Me",
        "Tanner",
        "Me",
        "Me",
        "Me"
    };

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

        FindObjectsForScene();

        dialogueIndex = 0;
        Dialogue();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FindObjectsForScene()
    {
        chatText = GameObject.Find("ChatText").GetComponent<Text>();
        option1Text = GameObject.Find("Option1Text").GetComponent<Text>();
        option1Button = GameObject.Find("Option1Button").GetComponent<Button>();
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

    public void StartDialogue()
    {
        Debug.Log("Starting dialogue...");
    }

    public void Dialogue()
    {
        Debug.Log("Dialogue Index: " + dialogueIndex);
        if (dialogueIndex >= dialogue.Length)
        {
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        message_ChatText = names[dialogueIndex] + ":\n" + dialogue[dialogueIndex];
        message_Option1Text = "Next";

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(WriteText(message_ChatText, message_Option1Text));

        option1Button.onClick.RemoveAllListeners();
        option1Button.onClick.AddListener(Dialogue);

        if (dialogue[dialogueIndex] == "But...")
        {
            if (dialogue[dialogueIndex + 1] == "...")
                Destroy(gameObject.GetComponent<SpriteRenderer>());
        }

        if (dialogue[dialogueIndex] == "...")
        {
            GameObject.Find("Player").GetComponent<SpriteRenderer>().sprite = sprite_PlayerDown;
        }

        dialogueIndex++;
    }

    public bool Complete
    {
        get
        {
            return dialogueIndex >= dialogue.Length;
        }
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
