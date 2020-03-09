using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NotepadManager : MonoBehaviour
{
    static NotepadManager instance = null;

    [SerializeField] GameplayManager gameplayManager;

    [SerializeField] RawImage backgroundImage;
    [SerializeField] Text notepadText;

    List<string> todoList;

    private void Awake()
    {
        // If there is already a Notepad Manager -- Only need one
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // Set this as the Notepad Manager instance
        instance = this;

        // Persist across scenes
        DontDestroyOnLoad(gameObject);

        // Initialize the To-Do list
        todoList = new List<string>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set default empty text
        ClearNotepad();

        // Hide the Task Tracker
        backgroundImage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // If the user presses Tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Show or hide the task tracker
            ToggleTaskTracker();
        }
    }

    public void ToggleTaskTracker()
    {
        // If the player has purchased the upgrade
        if (gameplayManager.HasUpgrade("Task Tracker"))
        {
            // Get the active scene name
            string sceneName = SceneManager.GetActiveScene().name;

            // If it's not a restricted scene
            if (sceneName != "loading" && sceneName != "title" && sceneName != "start_town")
            {
                // Toggle the task tracker
                backgroundImage.gameObject.SetActive(!backgroundImage.gameObject.activeInHierarchy);

                // Update the text
                DisplayText();
            }
        }
    }

    public void ToggleTaskTracker(bool isShown)
    {
        // If the player has purchased the upgrade
        if (gameplayManager.HasUpgrade("Task Tracker"))
        {
            // Get the active scene name
            string sceneName = SceneManager.GetActiveScene().name;

            // Ift it's not a restricted scene
            if (sceneName != "loading" && sceneName != "title" && sceneName != "start_town")
            {
                // Toggle the task tracker
                backgroundImage.gameObject.SetActive(isShown);

                // Update the text
                DisplayText();
            }
        }
    }

    public void ClearNotepad()
    {
        // Clear the To-Do list
        todoList.Clear();

        // Set the default empty text
        DisplayText();
    }

    public void AddItemToNotepad(string item)
    {
        // Add item to the To-Do list
        todoList.Add(item);

        // Update the GUI to reflect new item
        DisplayText();
    }

    public void DisplayText()
    {
        string displayText = "Recipient: ";

        // If there is NOT an active delivery
        if (string.IsNullOrEmpty(gameplayManager.CurrentTarget))
        {
            displayText += "None";
        }
        // If there IS an active delivery
        else
        {
            // Display the recipient's name
            displayText += gameplayManager.CurrentTarget;
        }
        
        displayText += "\nNext Step: ";

        // If the To-Do list is empty
        if (todoList.Count == 0)
        {
            displayText += "None";
        }
        // If the todo list is NOT empty
        else
        {
            // Display the last item on the To-Do list
            displayText += todoList[todoList.Count - 1];
        }

        // Display the formatted text
        notepadText.text = displayText;
    }
}
