/* File: HUDManager.cs
 * Author: Casey Lafferty
 * Project: Packet Delivery
 */

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    // HUDManager singleton reference
    static HUDManager instance = null;

    // Manager references
    [SerializeField] GameplayManager gameplayManager;
    [SerializeField] CacheManager cacheManager;

    // Money text and related objects
    [SerializeField] GameObject moneyBackdrop;
    [SerializeField] Text moneyText;

    // Task tracker text and related objects
    [SerializeField] GameObject taskTrackerObject;
    [SerializeField] Text taskTrackerText;

    // Address book text and related objects
    [SerializeField] GameObject addressBookObject;
    [SerializeField] Text addressBookText;
    [SerializeField] Text addressBookTextPt2;

    // "No one seems to be home" notice object
    [SerializeField] GameObject noOneHomeObject;

    public static readonly string TASK_TRACKER_KEY = "1";
    public static readonly string ADDRESS_BOOK_KEY = "2";

    private void Awake()
    {
        // If not the first instance of HUDManager
        if (instance != null)
        {
            // Only need one --> delete
            Destroy(gameObject);
            return;
        }

        // Set this as the HUD Manager instance
        instance = this;

        // Persist across scenes
        DontDestroyOnLoad(gameObject);

        // Set default empty text
        ClearCurrentTask();

        // Hide no one home text
        noOneHomeObject.SetActive(false);
    }
    
    void Start()
    {
        // Disable all HUD elements to start -- will enable them later
        ToggleDisplay(false);
    }
    
    void Update()
    {
        // If the user presses Tab
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            bool isCurrentlyActive = taskTrackerObject.activeInHierarchy;

            // Show or hide the task tracker
            ToggleTaskTracker(!isCurrentlyActive);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            bool isCurrentlyActive = addressBookObject.activeInHierarchy;

            // Show or hide the address book
            ToggleAddressBook(!isCurrentlyActive);
        }
    }

    public void ToggleDisplay(bool isShown)
    {
        // Toggle scene objects
        moneyBackdrop.SetActive(isShown);
        ToggleTaskTracker(isShown);
        ToggleAddressBook(isShown);

        // Update the text
        DisplayText();
    }

    void DisplayMoney(int money)
    {
        // Format the money display
        moneyText.text = "$" + money;
    }

    public void ToggleTaskTracker(bool isShown)
    {
        // If the player has purchased the task tracker
        if (gameplayManager.HasUpgrade("Task Tracker"))
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
        // If the player has NOT purchased the task tracker
        else
        {
            // Hide it
            taskTrackerObject.SetActive(false);
        }
    }

    public void ToggleAddressBook(bool isShown)
    {
        // If the player has purchased the address book
        if (gameplayManager.HasUpgrade("Address Book"))
        {
            // Get the active scene name
            string sceneName = SceneManager.GetActiveScene().name;

            // If it's not a restricted scene
            if (sceneName != "loading" && sceneName != "title" && sceneName != "start_town")
            {
                // Toggle the address book
                addressBookObject.SetActive(isShown);

                // Update the text
                DisplayText();
            }
        }
        // If the player has NOT purchased the address book
        else
        {
            // Hide it
            addressBookObject.SetActive(false);
        }
    }

    public void DisplayText()
    {
        // Display the player's money
        DisplayMoney(gameplayManager.Money);
        
        // Display the task tracker
        DisplayTaskTrackerInformation();

        // Display cached addresses from the local DNS cache
        cacheManager.DisplayCachedAddressesInTwoParts(addressBookText, addressBookTextPt2);
    }

    private void DisplayTaskTrackerInformation()
    {
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

        // Display the formatted text
        // taskTrackerText.text = displayText;
        taskTrackerText.text = textToDisplay;
    }

    public void ClearCurrentTask()
    {
        // Reset to default values
        CurrentTask = "None";

        // Update the HUD
        DisplayText();
    }

    public void NoOneHome()
    {
        // Display notification that no one is home
        StartCoroutine(DisplayNoOneHome());
    }

    IEnumerator DisplayNoOneHome()
    {
        // Display notice
        noOneHomeObject.SetActive(true);
        
        // Wait 3 seconds
        yield return new WaitForSeconds(3);

        // Hide notice
        noOneHomeObject.SetActive(false);
    }

    public string CurrentTask { get; private set; }
}
