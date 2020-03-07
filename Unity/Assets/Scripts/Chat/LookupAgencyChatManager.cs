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

    [SerializeField] GameplayManager gameplayManager;
    [SerializeField] LookupAgencyManager lookupAgencyManager;

    public List<Person> listOfPeople;

    [SerializeField] char neighborhoodID;

    [SerializeField] EventSystem eventSystem;
    [SerializeField] Text chatPromptText;
    [SerializeField] Text option1Text;
    [SerializeField] Text option2Text;
    [SerializeField] Button option1Button;
    [SerializeField] Button option2Button;
    [SerializeField] InputField inputField;

    [SerializeField] Transition fadeTransitionObject;

    string chatTextMessage;
    string option1Message;
    string option2Message;

    UnityAction option1Action;
    UnityAction option2Action;

    Coroutine currentCoroutine;

    private void Awake()
    {
        IsInputFieldActive = false;

        if (SceneManager.GetActiveScene().name == "localLookupAgency")
        {
            neighborhoodID = GameObject.Find("GameplayManager").GetComponent<GameplayManager>().CurrentNeighborhoodID;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        FindObjectsForScene();

        listOfPeople = lookupAgencyManager.GetListOfPeopleByNeighborhood(neighborhoodID);

        StartDialogue();
        chatTextMessage = "Welcome to the " + lookupAgencyManager.GetNeighborhoodNameFromID(neighborhoodID) + " Lookup Agency. " + chatTextMessage;

        DisplayText();

        ToggleInputField(false);
    }

    private void Update()
    {
        if (IsInputFieldActive)
        {
            // If pressed enter key
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
        listOfPeople = lookupAgencyManager.GetListOfPeopleByNeighborhood(neighborhoodID);
    }

    public bool LookupPerson(string name, out Person thisPerson)
    {
        thisPerson = null;

        foreach (Person p in listOfPeople)
        {
            if (p.Name.ToLower() == name.ToLower())
            {
                thisPerson = p;
                return true;
            }
        }

        return false;
    }

    void StartDialogue()
    {
        ToggleInputField(false);

        chatTextMessage = "How may we assist you?";
        option1Message = "I'm looking for someone.";
        option2Message = "I can't figure out where to go from here.";

        option1Action = delegate
        {
            // Show input for lookup
            ToggleInputField(true);
            DisplayText();
        };
        option2Action = delegate
        {
            WhereToGo();
            DisplayText();
        };
    }

    public void WhereToGo()
    {
        if (neighborhoodID == gameplayManager.NextStep.neighborhoodID)
        {
            chatTextMessage = "You're in the right spot! I can help you.";
            option1Message = "Okay!";
            option2Message = "On second thought, I'll come back later.";
            option1Action = delegate {
                StartDialogue();
                DisplayText();
            };
            option2Action = delegate {
                DepartDialogue();
                DisplayText();
            };
        }
        else
        {
            IncorrectLocationDialogue();
        }
    }

    void ToggleInputField(bool isActive)
    {
        // Disable first
        IsInputFieldActive = false;
        inputField.gameObject.SetActive(false);

        if (neighborhoodID == gameplayManager.NextStep.neighborhoodID)
        {
            // Correct place
            if (gameplayManager.CurrentMessage == null)
            {
                chatTextMessage = "You don't have a package with you to deliver.";
                option1Message = "Whoops. I'll come back when I have one.";
                option2Message = "";

                option1Action = delegate {
                    DepartDialogue();
                    DisplayText();
                };
                option2Action = delegate {
                    DepartDialogue();
                    DisplayText();
                };

                return;
            }
            else
            {
                IsInputFieldActive = isActive;
                inputField.gameObject.SetActive(isActive);
                option1Button.gameObject.SetActive(!isActive);
                option2Button.gameObject.SetActive(true);

                chatTextMessage = "Who are you looking for?";
                option1Message = "";
                option2Message = "This person.";

                option1Action = delegate { DisplayText(); };
                option2Action = delegate
                {
                    LookupPerson();
                    DisplayText();
                };
            }
        }
        else
        {
            IncorrectLocationDialogue();
        }
    }

    void LookupPerson()
    {
        ToggleInputField(false);

        string targetName = "";
        Person thisPersonProfile = null;
        if (gameplayManager.HasExitedTheMatrix)
        {
            targetName = gameplayManager.CurrentMessage.Recipient.URL;
        }
        else
        {
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
                chatTextMessage = "Hmm... I don't know that person. Did you spell the name correctly?";
                option1Message = "Let me try again.";
                option2Message = "I'll check and come back.";

                option1Action = delegate
                {
                    ToggleInputField(true);
                    DisplayText();
                };
                option2Action = delegate
                {
                    DepartDialogue();
                    DisplayText();
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

                    chatTextMessage = "It seems that person lives in " + nextLocation + ". Check with their Lookup Agency office for more specific details.";
                    option1Message = "Thanks!";
                    option2Message = "";

                    GameplayManager.DeliveryInstructions nextInstructions;
                    nextInstructions.nextStep = nextLocation + " Lookup Agency";
                    nextInstructions.neighborhoodID = thisPersonProfile.NeighborhoodID;

                    if (gameplayManager.HasExitedTheMatrix)
                    {
                        nextInstructions.recipient = thisPersonProfile.URL;
                    }
                    else
                    {
                        nextInstructions.recipient = thisPersonProfile.Name;
                    }

                    gameplayManager.SetNextSteps(nextInstructions);

                    option1Action = delegate
                    {
                        DepartDialogue();
                        DisplayText();
                    };
                    option2Action = delegate
                    {
                        DepartDialogue();
                        DisplayText();
                    };
                }
                // If currently at LLA
                else
                {
                    // Find next location
                    string nextLocation = lookupAgencyManager.GetNeighborhoodNameFromID(thisPersonProfile.NeighborhoodID);

                    chatTextMessage = "That person lives at Residence #" + thisPersonProfile.HouseNumber + ".";
                    option1Message = "Thanks!";
                    option2Message = "";

                    GameplayManager.DeliveryInstructions nextInstructions;
                    nextInstructions.nextStep = "Residence #" + thisPersonProfile.HouseNumber;
                    nextInstructions.neighborhoodID = thisPersonProfile.NeighborhoodID;

                    if (gameplayManager.HasExitedTheMatrix)
                    {
                        nextInstructions.recipient = thisPersonProfile.URL;
                    }
                    else
                    {
                        nextInstructions.recipient = thisPersonProfile.Name;
                    }

                    gameplayManager.SetNextSteps(nextInstructions);

                    option1Action = delegate
                    {
                        DepartDialogue();
                        DisplayText();
                    };
                    option2Action = delegate
                    {
                        DepartDialogue();
                        DisplayText();
                    };
                }
            }
        }
        // Does NOT match the name on the package
        else
        {
            Debug.Log("ERROR -- Looking for " + gameplayManager.CurrentMessage.Recipient.Name);
            chatTextMessage = "Hmm... That's not the person written on your package. Who is it again that you're looking for?";
            option1Message = "Let me try again.";
            option2Message = "I'll check and come back.";

            option1Action = delegate
            {
                ToggleInputField(true);
                DisplayText();
            };
            option2Action = delegate
            {
                DepartDialogue();
                DisplayText();
            };
        }
    }
    
    private void IncorrectLocationDialogue()
    {
        // Incorrect place
        string centralLookupLocation = lookupAgencyManager.GetNeighborhoodNameFromID('X');
        chatTextMessage = "Hmm... I'm not sure you're in the right spot. Check the Lookup Agency in " + centralLookupLocation + " for your next steps.";
        option1Message = "I'll do that.";
        option2Message = "";

        option1Action = delegate
        {
            DepartDialogue();
            DisplayText();
        };
        option2Action = delegate
        {
            DepartDialogue();
            DisplayText();
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
            yield return new WaitForSeconds(CHAT_DELAY);
            chatPromptText.text += chat[i];
        }

        // Write option 1 button text
        for (int i = 0; i < option1.Length; i++)
        {
            yield return new WaitForSeconds(CHAT_DELAY);
            option1Text.text += option1[i];
        }

        // Write option 2 button text
        for (int i = 0; i < option2.Length; i++)
        {
            yield return new WaitForSeconds(CHAT_DELAY);
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
        chatTextMessage = "Come back soon!";
        option1Message = "Bye.";
        option2Message = "";

        option1Action = delegate { GoToTown(); };
        option2Action = delegate { GoToTown(); };
    }

    public char DetermineNextLocation(Person p)
    {
        if (neighborhoodID == 'X')
        {
            return p.NeighborhoodID;
        }
        else
        {
            return 'H'; // Deliver to home
        }
    }

    public void GoToTown()
    {
        fadeTransitionObject.FadeMethod("town");
    }
    
    public bool IsInputFieldActive { get; private set; }
}
