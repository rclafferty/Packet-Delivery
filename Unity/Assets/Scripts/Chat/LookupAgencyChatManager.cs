/* File: LookupAgencyChatManager.cs
 * Author: Casey Lafferty
 * Project: Packet Delivery
 */

using Assets.Scripts.Lookup_Agencies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LookupAgencyChatManager : MonoBehaviour
{
    public readonly float CHAT_DELAY = 0.005f;

    // Necessary manager references
    [SerializeField] GameplayManager gameplayManager;
    [SerializeField] LookupAgencyManager lookupAgencyManager;

    // List of people in this lookup agency's domain
    public List<Person> listOfPeople;

    // Current neighborhood ID
    [SerializeField] char neighborhoodID;

    // Event system reference to release pressed buttons -- also plays into controller support
    [SerializeField] EventSystem eventSystem;

    // Primary text chat (agent's dialogue)
    [SerializeField] Text chatPromptText;

    // Option button and text references
    [SerializeField] Text option1Text;
    [SerializeField] Text option2Text;
    [SerializeField] Button option1Button;
    [SerializeField] Button option2Button;

    // Input field for "Who are you looking for?"
    [SerializeField] InputField inputField;

    // Global strings to format before placing them in the text objects
    string chatTextMessage;
    string option1Message;
    string option2Message;

    // Events to invoke when option buttons are pressed
    UnityAction option1Action;
    UnityAction option2Action;

    // Fade in/out object reference
    [SerializeField] Transition fadeTransitionObject;

    // Only one coroutine should run at a time
    Coroutine currentCoroutine;

    private void Awake()
    {
        // Indicate the input field is disabled until needed
        IsInputFieldActive = false;

        // If this is a local lookup agency
        if (SceneManager.GetActiveScene().name == "localLookupAgency")
        {
            // Figure out which local lookup agency it is by neighborhood ID
            neighborhoodID = GameObject.Find("GameplayManager").GetComponent<GameplayManager>().CurrentNeighborhoodID;
        }
    }
    
    void Start()
    {
        // Find managers needed for scene
        FindObjectsForScene();

        // Get list of people within this lookup agency's domain
        GatherListOfPeople();

        // Set up the start dialogue
        StartDialogue();

        // Set greeting for first interaction
        chatTextMessage = "Welcome to the " + lookupAgencyManager.GetNeighborhoodNameFromID(neighborhoodID) + " Lookup Agency. " + chatTextMessage;

        // Update the UI
        DisplayText();

        // Disable the input field until needed
        ToggleInputField(false);
    }

    private void Update()
    {
        // If showing the input field
        if (IsInputFieldActive)
        {
            // Allow enter key to submit input text
            if (Input.GetKeyDown(KeyCode.Return))
            {
                // Submit text in input field
                option2Button.onClick.Invoke();
            }
        }
    }

    void FindObjectsForScene()
    {
        // Persistent objects -- must look up at runtime
        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        lookupAgencyManager = GameObject.Find("LookupAgencyManager").GetComponent<LookupAgencyManager>();
    }

    public void GatherListOfPeople()
    {
        // Get list of people within this lookup agency's domain
        listOfPeople = lookupAgencyManager.GetListOfPeopleByNeighborhood(neighborhoodID);
    }

    public bool LookupPerson(string name, out Person thisPerson)
    {
        thisPerson = null;

        // Find the requested person
        foreach (Person p in listOfPeople)
        {
            // If the names match
            if (p.Name.ToLower() == name.ToLower())
            {
                // Return the person reference
                thisPerson = p;
                
                // Indicate success
                return true;
            }
        }

        // Could not find the person
        return false;
    }

    void StartDialogue()
    {
        // Disable the input field
        ToggleInputField(false);

        // Set greeting and options
        chatTextMessage = "How may we assist you?";
        option1Message = "I'm looking for someone.";
        option2Message = "I can't figure out where to go from here.";

        // "I'm looking for someone"
        option1Action = delegate
        {
            // Show input for lookup
            ToggleInputField(true);

            // Update the UI
            DisplayText();
        };
        // "I can't figure out where to go from here."
        option2Action = delegate
        {
            // Hint at player's next step
            WhereToGo();

            // Update the UI
            DisplayText();
        };
    }

    public void WhereToGo()
    {
        // If there is no active delivery
        if (gameplayManager.CurrentMessage == null)
        {
            // Suggest they return to office to start one
            NoLetterDialogue();
        }
        // If they're in the correct location
        else if (neighborhoodID == gameplayManager.NextStep.neighborhoodID)
        {
            // "I can help you"
            CorrectLocationDialogue();
        }
        // If they're in the wrong location
        else
        {
            // Hint at their next step
            IncorrectLocationDialogue();
        }
    }

    private void CorrectLocationDialogue()
    {
        // "I can help you"
        chatTextMessage = "You're in the right spot! I can help you.";
        option1Message = "Okay!";
        option2Message = "On second thought, I'll come back later.";

        // "Okay!"
        option1Action = delegate
        {
            // Restart dialogue
            StartDialogue();

            // Update the UI
            DisplayText();
        };
        // "I'll come back later"
        option2Action = delegate
        {
            // Say goodbye and update the UI
            DepartAndUpdateUI();
        };
    }

    private void NoLetterDialogue()
    {
        // Hint at where to get a letter
        chatTextMessage = "You don't seem to have a letter with you. Start by getting one from your office."; 
        option1Message = "Whoops. I'll come back when I have one.";
        option2Message = "I have something else.";

        // "I'll come back"
        option1Action = delegate
        {
            // Say goodbye and update the UI
            DepartAndUpdateUI();
        };
        // "I have another request"
        option2Action = delegate
        {
            // Say goodbye and update the UI
            DepartAndUpdateUI();
        };
    }

    void ToggleInputField(bool isActive)
    {
        // Disable the input field first
        IsInputFieldActive = false;
        inputField.gameObject.SetActive(false);

        // If they're at the right lookup agency
        if (neighborhoodID == gameplayManager.NextStep.neighborhoodID)
        {
            // If there is NOT an active delivery
            if (gameplayManager.CurrentMessage == null)
            {
                // Hint at getting a letter first
                NoLetterDialogue();
                return;
            }
            // If there IS an active delivery
            else
            {
                // Enable the input field
                IsInputFieldActive = isActive;
                inputField.gameObject.SetActive(isActive);

                // Enable/disable the appropriate buttons
                option1Button.gameObject.SetActive(!isActive);
                option2Button.gameObject.SetActive(true);

                // Set up prompts
                chatTextMessage = "Who are you looking for?";
                option1Message = "";
                option2Message = "This person.";

                // Redundant in case option1Button is not properly disabled
                option1Action = delegate
                {
                    // Re-update the UI
                    DisplayText();
                };
                // "This person"
                option2Action = delegate
                {
                    // Lookup the requested person
                    LookupPerson();

                    // Update the UI
                    DisplayText();
                };
            }
        }
        // If they're NOT at the right lookup agency
        else
        {
            // Hint at the correct lookup agency
            IncorrectLocationDialogue();
        }
    }

    void LookupPerson()
    {
        // Disable the input field
        ToggleInputField(false);
        
        string targetName = "";
        Person thisPersonProfile = null;

        // If the player has purchased the "Exit the Matrix" upgrade
        if (gameplayManager.HasUpgrade("Exit the Matrix"))
        {
            // Reference the recipient's URL
            targetName = gameplayManager.CurrentMessage.Recipient.URL;
        }
        else
        {
            // Reference the recipient's name
            targetName = gameplayManager.CurrentMessage.Recipient.Name;
        }

        // If the input name matches the target message
        if (inputField.text.ToLower().Trim() == targetName.ToLower().Trim())
        {
            // Find the person's profile
            thisPersonProfile = gameplayManager.CurrentMessage.Recipient;

            // If this person doesn't exist -- ERROR
            if (thisPersonProfile == null)
            {
                // Hint at possible cause for not finding the person
                chatTextMessage = "Hmm... I don't know that person. Did you spell the name correctly?";
                option1Message = "Let me try again.";
                option2Message = "I'll check and come back.";

                // "Let me try again"
                option1Action = delegate
                {
                    // Re-display the input field
                    ToggleInputField(true);
                    
                    // Update the UI
                    DisplayText();
                };
                // "I'll check and come back"
                option2Action = delegate
                {
                    // Say goodbye and update the UI
                    DepartAndUpdateUI();
                };
            }
            // If the person DOES exist -- expected
            else
            {
                // If currently at CLA
                if (neighborhoodID == 'X')
                {
                    // Find next location
                    string nextLocation = lookupAgencyManager.GetNeighborhoodNameFromID(thisPersonProfile.NeighborhoodID);

                    // Tell the player where to go
                    chatTextMessage = "It seems that person lives in " + nextLocation + ". Check with their Lookup Agency office for more specific details.";
                    option1Message = "Thanks!";
                    option2Message = "";

                    // "Thanks"
                    option1Action = delegate
                    {
                        // Say goodbye and update the UI
                        DepartAndUpdateUI();
                    };
                    // Redundant in case the option2Button did not get disabled properly
                    option2Action = delegate
                    {
                        // Say goodbye and update the UI
                        DepartAndUpdateUI();
                    };

                    // Store next step instructions
                    GameplayManager.DeliveryInstructions nextInstructions;
                    nextInstructions.nextStep = nextLocation + " Lookup Agency";
                    nextInstructions.neighborhoodID = thisPersonProfile.NeighborhoodID;

                    // If the player has purchased the "Exit the Matrix" upgrade
                    if (gameplayManager.HasUpgrade("Exit the Matrix"))
                    {
                        // Reference the recipient's URL
                        nextInstructions.recipient = thisPersonProfile.URL;
                    }
                    else
                    {
                        // Reference the recipient's name
                        nextInstructions.recipient = thisPersonProfile.Name;
                    }

                    // Store next step instructions
                    gameplayManager.SetNextSteps(nextInstructions);
                }
                // If currently at local lookup agency
                else
                {
                    // Find next location
                    string nextLocation = lookupAgencyManager.GetNeighborhoodNameFromID(thisPersonProfile.NeighborhoodID);

                    // Tell the player the residence number
                    chatTextMessage = "That person lives at Residence #" + thisPersonProfile.HouseNumber + ".";
                    option1Message = "Thanks!";
                    option2Message = "";

                    // "Thanks"
                    option1Action = delegate
                    {
                        // Say goodbye and update the UI
                        DepartAndUpdateUI();
                    };
                    // Redundant in case the option2Button did not get disabled properly
                    option2Action = delegate
                    {
                        // Say goodbye and update the UI
                        DepartAndUpdateUI();
                    };

                    // Store the next step instructions
                    GameplayManager.DeliveryInstructions nextInstructions;
                    nextInstructions.neighborhoodID = thisPersonProfile.NeighborhoodID;

                    // If the player has purchased the "Exit the Matrix" upgrade
                    if (gameplayManager.HasUpgrade("Exit the Matrix"))
                    {
                        // Reference the recipient's URL
                        nextInstructions.recipient = thisPersonProfile.URL;

                        // Determine the recipient's IP
                        nextInstructions.nextStep = AddressManager.DetermineIPFromHouseInfo(thisPersonProfile.HouseNumber, thisPersonProfile.NeighborhoodID);

                        // Adjust the message to the player to reflect the IP instead of the house number
                        chatTextMessage = "That person lives at " + nextInstructions.nextStep + ".";
                    }
                    else
                    {
                        // Reference the recipient's name
                        nextInstructions.recipient = thisPersonProfile.Name;

                        // Use the recipient's house number
                        nextInstructions.nextStep = "Residence #" + thisPersonProfile.HouseNumber;
                    }

                    // Store the next step instructions
                    gameplayManager.SetNextSteps(nextInstructions);
                }
            }
        }
        // Does NOT match the name on the package
        else
        {
            // Tell the player it doesn't match
            chatTextMessage = "Hmm... That's not the person written on your package. Who is it again that you're looking for?";
            option1Message = "Let me try again.";
            option2Message = "I'll check and come back.";

            // "Let me try again"
            option1Action = delegate
            {
                // Re-show the input field
                ToggleInputField(true);
                
                // Update the UI
                DisplayText();
            };
            // "I'll come back"
            option2Action = delegate
            {
                // Say goodbye and update the UI
                DepartAndUpdateUI();
            };
        }
    }

    private void DepartAndUpdateUI()
    {
        // Say goodbye
        DepartDialogue();

        // Update the UI
        DisplayText();
    }

    private void IncorrectLocationDialogue()
    {
        // Incorrect place
        string centralLookupLocation = lookupAgencyManager.GetNeighborhoodNameFromID('X');
        chatTextMessage = "Hmm... I'm not sure you're in the right spot. Check the Lookup Agency in " + centralLookupLocation + " for your next steps.";
        option1Message = "I'll do that.";
        option2Message = "";

        // "I'll go to the right place"
        option1Action = delegate
        {
            // Say goodbye and update the UI
            DepartAndUpdateUI();
        };
        // Redundant in case the option2Button did not get disabled properly
        option2Action = delegate
        {
            // Say goodbye and update the UI
            DepartAndUpdateUI();
        };
    }

    void DisplayText()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(WriteTextToUI(chatTextMessage, option1Message, option2Message));
        AddEventListeners(option1Action, option2Action);
    }

    IEnumerator WriteTextToUI(string chat, string option1, string option2)
    {
        // Clear displayed text
        chatPromptText.text = "";
        option1Text.text = "";
        option2Text.text = "";

        // Deselect all buttons
        eventSystem.SetSelectedGameObject(null);
        
        option1Button.interactable = !string.IsNullOrEmpty(option1);
        option2Button.interactable = !string.IsNullOrEmpty(option2);

        // Write to chat prompt
        for (int i = 0; i < chat.Length; i++)
        {
            // Delay between characters
            yield return new WaitForSeconds(CHAT_DELAY);

            // Write next character
            chatPromptText.text += chat[i];
        }

        // Write option 1 button text
        for (int i = 0; i < option1.Length; i++)
        {
            // Delay between characters
            yield return new WaitForSeconds(CHAT_DELAY);

            // Write next character
            option1Text.text += option1[i];
        }

        // Write option 2 button text
        for (int i = 0; i < option2.Length; i++)
        {
            // Delay between characters
            yield return new WaitForSeconds(CHAT_DELAY);

            // Write next character
            option2Text.text += option2[i];
        }
    }

    void AddEventListeners(UnityAction a1, UnityAction a2)
    {
        // Add option 1 listener
        option1Button.onClick.RemoveAllListeners();
        option1Button.onClick.AddListener(a1);

        // Add option 2 listener
        option2Button.onClick.RemoveAllListeners();
        option2Button.onClick.AddListener(a2);
    }

    public void DepartDialogue()
    {
        // Say goodbye
        chatTextMessage = "Come back soon!";
        option1Message = "Bye.";
        option2Message = "";

        // Make both buttons redirect to twon
        option1Action = delegate { GoToTown(); };
        option2Action = delegate { GoToTown(); };
    }

    public char DetermineNextLocation(Person p)
    {
        // If the current neighborhood is Root Village ('X'), go to the appropriate
        // local lookup agency (p.NeighborhoodID). If the player is at a local lookup agency
        // (not 'X'), then the next step is go to the residence ('H')
        return neighborhoodID == 'X' ? p.NeighborhoodID : 'H';
    }

    public void GoToTown()
    {
        // Fade out and transition to town
        fadeTransitionObject.FadeMethod("town");
    }
    
    public bool IsInputFieldActive { get; private set; }
}
