using Assets.Scripts.Behind_The_Scenes;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class WorkUIManager : MonoBehaviour
{
    GameplayManager gameplayManager;
    LevelManager levelManager;
    Timer timer;

    GameObject prefab_CheckMark;
    GameObject prefab_ErrorWindow;
    GameObject prefab_LetterImage;

    GameObject object_CheckMark;
    GameObject object_LetterImage;
    GameObject object_Monitor;

    Text screenText;

    Message currentMessage;

    const string SPACING = "\n\n";

    const float TYPING_DELAY = 0.02f;

    bool hasNextMessage;
    bool currentlyHasMessage;

    Coroutine currentCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        hasNextMessage = false;
        currentlyHasMessage = true;

        currentMessage = null;

        FindSceneObjects();

        DisplayImages();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FindSceneObjects()
    {
        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        timer = GameObject.Find("Timer").GetComponent<Timer>();

        screenText = GameObject.Find("ScreenText").GetComponent<Text>();
        prefab_LetterImage = Resources.Load<GameObject>("Prefabs/LetterImage");
        prefab_CheckMark = Resources.Load<GameObject>("Prefabs/CheckMark");

        object_Monitor = GameObject.Find("Monitor");

        // Instantiate the check mark
        object_CheckMark = Instantiate(prefab_CheckMark, object_Monitor.transform);
        object_CheckMark.name = "CheckMark";

        // Instantiate the letter image
        object_LetterImage = Instantiate(prefab_LetterImage, object_Monitor.transform);
        object_LetterImage.name = "LetterImage";

        Button letterButton = GameObject.Find("LetterButton").GetComponent<Button>();
        letterButton.onClick.RemoveAllListeners();
        letterButton.onClick.AddListener(delegate { AcceptLetter(); });

        Button exitButton = GameObject.Find("ExitButton").GetComponent<Button>();
        exitButton.onClick.RemoveAllListeners();
        exitButton.onClick.AddListener(delegate {
            if (!(gameplayManager.CurrentTarget == "None" || gameplayManager.CurrentTarget == ""))
                timer.StartNewTimerIfNotAlreadyRunning();
            levelManager.LoadLevel("town");
        });
    }

    void DisplayImages()
    {
        // Find if there is another message to deliver
        hasNextMessage = gameplayManager.HasRemainingTasks;
        currentlyHasMessage = gameplayManager.HasCurrentTarget;
        
        // if the player currently has a task -- "You're all caught up"
        if (currentlyHasMessage)
        {
            Debug.Log("Already has a message to deliver...");

            ShowNewMessage(false);

        }
        // if there is another message -- new message
        else if (hasNextMessage)
        {
            Debug.Log("New message to deliver...");

            ShowNewMessage(true);
        }
        // No messages and no target
        else
        {
            Debug.Log("No new messages...");

            ShowNewMessage(false);
        }
    }

    public void AcceptLetter()
    {
        gameplayManager.GetNextMessage();
        DisplayImages();
    }

    public void ShowNewMessage(bool tf)
    {
        if (object_CheckMark != null)
        {
            object_CheckMark.SetActive(!tf);
        }

        if (object_LetterImage != null)
        {
            object_LetterImage.SetActive(tf);
        }

        DisplayText();
    }

    void DisplayText()
    {
        currentMessage = gameplayManager.CurrentTargetMessage;

        string text = "Upgrades: " + "none" + SPACING + "\nCurrent Message" + SPACING + DisplayMessage(currentMessage);

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(WriteText(text));
    }

    string DisplayMessage(Message m)
    {
        const string NONE = "None";

        string target = "";
        string sender = "";
        string text = "";

        if (m == null)
        {
            target = NONE;
            sender = NONE;
            text = NONE;
        }
        else
        {
            // Check if recipient is "" or null
            if (string.IsNullOrEmpty(m.Recipient))
            {
                target = NONE;
            }
            else
            {
                target = m.Recipient;
            }

            // Check if sender is "" or null
            if (string.IsNullOrEmpty(m.Sender))
            {
                sender = NONE;
            }
            else
            {
                sender = m.Sender;
            }

            // Check if message is "" or null
            if (string.IsNullOrEmpty(m.MessageBody))
            {
                text = NONE;
            }
            else
            {
                text = m.MessageBody;
            }
        }

        StringBuilder sb = new StringBuilder();
        sb.Append("To: ");
        sb.Append(target);
        sb.Append("\n");
        sb.Append("From: ");
        sb.Append(sender);
        sb.Append("\n\n");
        sb.Append("Message Body:");
        sb.Append("\n");
        sb.Append(text);

        return sb.ToString();
    }

    IEnumerator WriteText(string text)
    {
        screenText.text = "";

        // Write chat text
        for (int i = 0; i < text.Length; i++)
        {
            // Wait for CHAT_DELAY seconds before writing the next letter
            yield return new WaitForSeconds(TYPING_DELAY);
            screenText.text += text[i];
        }
    }
}
