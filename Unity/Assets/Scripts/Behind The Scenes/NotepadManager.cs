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
    string startingText = "To-Do Delivery List\n\nRecipient: None\n\n";
    string templateText = "To-Do Delivery List\n\nRecipient: None\n\n- Receive Package From Office\n- Central Lookup Agency\n- Local Lookup Agency\n- Home";

    List<string> todoList;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        todoList = new List<string>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ClearNotepad();

        backgroundImage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleTaskTracker();
        }
    }

    public void ToggleTaskTracker()
    {
        if (gameplayManager.HasTaskTracker)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName != "loading" && sceneName != "title" && sceneName != "start_town")
            {
                backgroundImage.gameObject.SetActive(!backgroundImage.gameObject.activeInHierarchy);
                DisplayText();
            }
        }
    }

    public void ToggleTaskTracker(bool isShown)
    {
        if (gameplayManager.HasTaskTracker)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName != "loading" && sceneName != "title" && sceneName != "start_town")
            {
                backgroundImage.gameObject.SetActive(isShown);
                DisplayText();
            }
        }
    }

    public void ClearNotepad()
    {
        notepadText.text = startingText;
        todoList.Clear();
        DisplayText();
    }

    public void AddItemToNotepad(string item)
    {
        todoList.Add(item);
        DisplayText();
    }

    public void DisplayText()
    {
        string displayText = "Recipient: ";

        if (string.IsNullOrEmpty(gameplayManager.CurrentTarget))
        {
            displayText += "None";
        }
        else
        {
            displayText += gameplayManager.CurrentTarget;
        }

        displayText += "\nNext Step: ";

        if (todoList.Count == 0)
        {
            displayText += "None";
        }
        else
        {
            displayText += todoList[todoList.Count - 1];
        }

        notepadText.text = displayText;
    }
}
