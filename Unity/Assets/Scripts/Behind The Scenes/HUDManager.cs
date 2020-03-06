using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    static HUDManager instance = null;

    [SerializeField] GameplayManager gameplayManager;

    [SerializeField] GameObject moneyBackdrop;
    [SerializeField] Text moneyText;

    [SerializeField] GameObject taskTrackerObject;
    [SerializeField] Text taskTrackerText;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // Set this as the HUD Manager instance
        instance = this;

        // Persist across scenes
        DontDestroyOnLoad(gameObject);

        // Set default empty text
        ClearCurrentTask();
    }

    // Start is called before the first frame update
    void Start()
    {
        ToggleDisplay(false);
    }

    // Update is called once per frame
    void Update()
    {
        // If the user presses Tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool isCurrentlyActive = taskTrackerObject.activeInHierarchy;

            // Show or hide the task tracker
            ToggleTaskTracker(!isCurrentlyActive);
        }
    }

    public void ToggleDisplay(bool isShown)
    {
        // Toggle scene objects
        moneyBackdrop.SetActive(isShown);
        ToggleTaskTracker(isShown);

        // Update the text
        DisplayText();
    }

    void DisplayMoney(int money)
    {
        moneyText.text = "$" + money;
    }

    private void ToggleTaskTracker(bool isShown)
    {
        if (gameplayManager.HasTaskTracker)
        {
            // Get the active scene name
            string sceneName = SceneManager.GetActiveScene().name;

            // If it's not a restricted scene
            if (sceneName != "loading" && sceneName != "title" && sceneName != "start_town")
            {
                // Toggle the task tracker
                taskTrackerObject.SetActive(isShown);

                // Update the text
                DisplayText();
            }
        }
        else
        {
            taskTrackerObject.SetActive(false); // Hide it if not active
        }
    }

    public void DisplayText()
    {
        DisplayMoney(gameplayManager.Money);

        string textToDisplay = "Recipient: ";

        // If there is NOT an active delivery
        if (string.IsNullOrEmpty(gameplayManager.NextStep.recipient))
        {
            textToDisplay += "None";
        }
        // If there IS an active delivery
        else
        {
            // Display the recipient's name
            textToDisplay += gameplayManager.NextStep.recipient;
        }

        textToDisplay += "\nNext: ";

        // If the To-Do list is empty
        if (string.IsNullOrEmpty(gameplayManager.NextStep.nextStep))
        {
            textToDisplay += "None";
        }
        // If the todo list is NOT empty
        else
        {
            // Display the last item on the To-Do list
            textToDisplay += gameplayManager.NextStep.nextStep;
        }

        // string textToDisplay = "Recipient: " + gameplayManager.NextStep.recipient + "\nNext: " + gameplayManager.NextStep.nextStep;
        Debug.Log(textToDisplay);

        // Display the formatted text
        // taskTrackerText.text = displayText;
        taskTrackerText.text = textToDisplay;
    }

    public void ClearCurrentTask()
    {
        CurrentTask = "None";
        DisplayText();
    }

    public string CurrentTask { get; private set; }
}
