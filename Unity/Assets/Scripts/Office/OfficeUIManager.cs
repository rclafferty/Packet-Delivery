using Assets.Scripts.Behind_The_Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OfficeUIManager : MonoBehaviour
{
    [SerializeField]
    GameplayManager gameplayManager;

    [SerializeField]
    LetterManager letterManager;

    [SerializeField]
    Text statusText;

    [SerializeField]
    Button letter1Button;

    [SerializeField]
    Text letter1Text;

    [SerializeField]
    Button checkMessagesButton;

    [SerializeField]
    Text checkMessagesText;

    [SerializeField]
    Button completeTaskButton;

    [SerializeField]
    Text completeTaskText;

    [SerializeField]
    GameObject errorWindow;

    [SerializeField]
    Text errorText;

    Message currentMessage;

    // Start is called before the first frame update
    void Start()
    {
        // gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        // statusText = GameObject.Find("Status Text").GetComponent<Text>();
        // SceneManager.sceneLoaded += OnLevelFinishedLoading;

        errorWindow = GameObject.Find("Error Window");

        // errorWindow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*void OnLevelFinishedLoading(Scene scene, LoadSceneMode loadMode)
    {
        string title = "Packet Delivery, Inc.";
        string recipientText = "Current Recipient\n" + gameplayManager.CurrentTarget;
        string upgradeText = "Upgrades\nNone";
    }*/

    public void AcceptMessage()
    {
        if (gameplayManager.HasCurrentTarget)
        {
            StartCoroutine(ShowErrorMessage("Please complete your current delivery before taking on a new one!"));
            return;
        }

        gameplayManager.CurrentTargetMessage = currentMessage;
        UpdateStatusGUI();
    }

    public void NextMessage()
    {
        if (gameplayManager.HasCurrentTarget)
        {
            StartCoroutine(ShowErrorMessage("Please complete your current delivery before taking on a new one!"));
            return;
        }

        if (gameplayManager.HasRemainingTasks)
        {
            currentMessage = letterManager.GetNextMessage();
            UpdateMessageText();
        }
        else
        {
            letter1Text.text = "No more messages!";
        }
    }

    public void CompleteTask()
    {
        gameplayManager.CurrentTargetMessage = null;
        UpdateStatusGUI();
    }

    const string TITLE = "Packet Delivery, Inc.";
    const string RECIPIENT_TITLE = "Current Recipient";
    const string UPGRADES_TITLE = "Available Upgrades";

    public void UpdateStatusGUI()
    {
        string target = gameplayManager.CurrentTarget;
        if (target == "")
        {
            target = "None";
        }

        string recipientText = RECIPIENT_TITLE + "\n" + target;
        string upgradesText = UPGRADES_TITLE + "\n" + "None";

        statusText.text = TITLE + "\n\n" + recipientText + "\n\n" + upgradesText;
    }

    public void UpdateMessageText()
    {
        letter1Text.text = "To: " + currentMessage.Recipient + "\nFrom: " + currentMessage.Sender + "\nClass: " + currentMessage.Urgency;
    }

    IEnumerator ShowErrorMessage(string message)
    {
        errorText.text = message;
        errorWindow.SetActive(true);

        yield return new WaitForSeconds(3);

        errorWindow.SetActive(false);
    }
}
