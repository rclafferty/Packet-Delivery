using Assets.Scripts.Behind_The_Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorkGUIManager : MonoBehaviour
{
    [SerializeField]
    LetterManager letterManager;
    [SerializeField]
    GameplayManager gameplayManager;

    [SerializeField]
    Message letter1;
    [SerializeField]
    Message letter2;

    [SerializeField]
    Text letter1Text;
    [SerializeField]
    Text letter2Text;

    [SerializeField]
    GameObject errorWindow;
    [SerializeField]
    Text errorWindowText;

    [SerializeField]
    Text statusText;

    const string ERROR_MESSAGE = "Please complete your current delivery before taking on a new one";

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        errorWindow.SetActive(false);
        UpdateStatusGUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSceneLoaded(Scene thisScene, LoadSceneMode loadMode)
    {
        /*letterManager = GameObject.Find("LetterManager").GetComponent<LetterManager>();
        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();

        errorWindow = GameObject.Find("Error Window");
        errorWindowText = GameObject.Find("Error Text").GetComponent<Text>();

        letter1Text = GameObject.Find("Letter Text 1").GetComponent<Text>();
        letter2Text = GameObject.Find("Letter Text 2").GetComponent<Text>();*/

        Message target1Message = letterManager.GetNextMessage();
        letter1Text.text = "To: " + target1Message.Recipient + "\nFrom: " + target1Message.Sender;

        Message target2Message = letterManager.GetNextMessage();
        letter2Text.text = "To: " + target2Message.Recipient + "\nFrom: " + target2Message.Sender;

        UpdateStatusGUI();
    }

    public void ChooseLetter1()
    {
        Debug.Log("Chose Letter 1");
        if (gameplayManager.HasCurrentTarget)
        {
            ShowError(ERROR_MESSAGE);
            return;
        }

        gameplayManager.CurrentTargetMessage = letter1;
        gameplayManager.CurrentTarget = letter1.Recipient;
        UpdateStatusGUI();
    }

    public void ChooseLetter2()
    {
        Debug.Log("Chose Letter 2");
        if (gameplayManager.HasCurrentTarget)
        {
            ShowError(ERROR_MESSAGE);
            return;
        }

        gameplayManager.CurrentTargetMessage = letter2;
        gameplayManager.CurrentTarget = letter2.Recipient;
        UpdateStatusGUI();
    }

    IEnumerator ShowError(string message)
    {
        errorWindow.SetActive(true);
        errorWindowText.text = message;

        yield return new WaitForSeconds(5);

        errorWindow.SetActive(false);
    }

    void UpdateStatusGUI()
    {
        statusText.text = "Packet Delivery, Inc.\n\nCurrent Recipient\n" + gameplayManager.CurrentTarget + "\n\nUpgrades\nNone";
    }
}
