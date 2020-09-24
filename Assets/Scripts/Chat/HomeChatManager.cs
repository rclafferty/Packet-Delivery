/* File: HomeChatManager.cs
 * Author: Casey Lafferty
 * Project: Packet Delivery
 */

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HomeChatManager : MonoBehaviour
{
    // Manager references
    GameplayManager gameplayManager;
    LevelManager levelManager;
    CacheManager cacheManager;

    // Event system reference to release pressed buttons -- also plays into controller support
    [SerializeField] EventSystem eventSystem;

    // Primary text chat (recipient's dialogue)
    [SerializeField] Text chatText;

    // Option button and text references
    [SerializeField] Button option1Button;
    [SerializeField] Text option1Text;
    [SerializeField] Button option2Button;
    [SerializeField] Text option2Text;

    // Other UI Components
    [SerializeField] Transform moneyParent;
    [SerializeField] GameObject moneySpriteObject;

    // NPC Sprites
    [SerializeField] SpriteRenderer npcSpriteRenderer;
    [SerializeField] Sprite[] npcMaleSprites;
    [SerializeField] Sprite[] npcFemaleSprites;

    [SerializeField] bool[] isMaleRecipientIndex;
    
    // Global strings to format before placing them in the text objects
    string chatTextMessage;
    string option1Message;
    string option2Message;

    // Events to invoke when option buttons are pressed
    UnityAction option1Action;
    UnityAction option2Action;

    // Fade in/out object reference
    [SerializeField] Transition fadeTransitionObject;
    
    // Delay between each character when showing dialogue
    readonly float CHAT_DELAY = 0.005f;

    // Only one coroutine should run at a time
    Coroutine currentCoroutine;
    
    void Start()
    {
        // Find objects that persist across scenes
        FindObjectsForScene();

        // Set up the starting dialogue
        StartDialogue();

        // Display text to UI
        DisplayText();
    }

    void FindObjectsForScene()
    {
        // Find necessary persistent managers in scene
        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        cacheManager = GameObject.Find("CacheManager").GetComponent<CacheManager>();
    }

    void StartDialogue()
    {
        int messageID = gameplayManager.CurrentMessage.ID - 1; // Letters are 1 indexed, array is 0 indexed
        if (isMaleRecipientIndex[messageID])
        {
            // Set sprite as random male sprite
            int randomMaleSpriteIndex = Random.Range(0, npcMaleSprites.Length);
            Sprite randomMaleSprite = npcMaleSprites[randomMaleSpriteIndex];
            npcSpriteRenderer.sprite = randomMaleSprite;
        }
        else
        {
            // Set sprite as random female sprite
            int randomFemaleSpriteIndex = Random.Range(0, npcFemaleSprites.Length);
            Sprite randomFemaleSprite = npcFemaleSprites[randomFemaleSpriteIndex];
            npcSpriteRenderer.sprite = randomFemaleSprite;
        }

        // Set chat text
        chatTextMessage = "Are you the one with my package?";
        option1Message = "I am!";
        option2Message = "I'm sorry. I think I am at the wrong house.";

        // Set event actions to invoke on button press

        // "I am the one with your package"
        option1Action = delegate {
            // Attempt to deliver package
            DeliverPackage();

            // Update the UI
            DisplayText();
        };
        // "I am not the one with your package"
        option2Action = delegate {
            // Leave the residence
            DepartDialogue();

            // Update the UI
            DisplayText();
        };
    }

    void DeliverPackage()
    {
        // If the recipient is cached in the address book -- If the player was able to skip straight to the residence
        bool isCached = cacheManager.IsPersonCached(gameplayManager.CurrentMessage.Recipient);

        // If the player has purchased the "Exit the Matrix" upgrade
        if (gameplayManager.HasUpgrade("Exit the Matrix"))
        {
            // Reference the recipient's IP
            string ipAddress = AddressManager.DetermineIPFromHouseInfo(gameplayManager.CurrentMessage.Recipient.HouseNumber, gameplayManager.CurrentMessage.Recipient.NeighborhoodID);

            // Check if this is the next step in the delivery process
            bool isNextStep = gameplayManager.NextStep.nextStep == ipAddress;

            // If the recipient is cached in the address book or went through all prerequisite steps
            if (isCached || isNextStep)
            {
                // Deliver the package
                Success();
            }
            else
            {
                // Incorrect location -- Leads to depart dialogue
                WrongLocation();
            }
        }
        // If the resident was not cached in the address book
        else
        {
            // Check if this is the next step in the delivery process
            bool isNextStep = gameplayManager.NextStep.nextStep == "Residence #" + gameplayManager.CurrentMessage.Recipient.HouseNumber;

            // If the recipient is cached in the address book or went through all prerequisite steps
            if (isCached || isNextStep)
            {
                // Deliver the package
                Success();
            }
            else
            {
                // Incorrect location -- Leads to depart dialogue
                WrongLocation();
            }
        }
    }

    private void Success()
    {
        // Recipient: "Thank you for my package"
        chatTextMessage = "Thank you very much! Here is your <b>$10</b>.";
        option1Message = "";
        option2Message = "Enjoy!";

        // "You're welcome" / "Enjoy!"
        option1Action = delegate
        {
            // Say goodbye
            DepartDialogue();

            // Update UI
            DisplayText();

            // Mark as delivered
            gameplayManager.CompleteTask();
            
            // Money animation
            moneySpriteObject.GetComponent<Animator>().SetTrigger("Pay Money");
        };

        // For redundancy -- They're both the same action, but only one should be enabled
        option2Action = option1Action;
    }

    [System.Obsolete("Instead, change to UI notification BEFORE entering house.")]
    void WrongLocation()
    {
        // Display some error message in chat
        chatTextMessage = "I wasn't expecting a package.";
        option1Message = "";
        option2Message = "Sorry. I must have the wrong house.";

        // "Sorry, wrong house"
        option1Action = delegate {
            // Say goodbye
            DepartDialogue();

            // Update UI
            DisplayText();
        };
        // For redundancy -- They're both the same action, but only one should be enabled
        option2Action = delegate {
            // Say goodbye
            DepartDialogue();

            // Update UI
            DisplayText();
        };
    }

    void DisplayText()
    {
        // If the player clicked through the dialogue before it was finished
        if (currentCoroutine != null)
        {
            // Stop the previous coroutine so we can start a new one
            StopCoroutine(currentCoroutine);
        }

        // Start new coroutine to write text to UI
        currentCoroutine = StartCoroutine(WriteTextToUI(chatTextMessage, option1Message, option2Message));

        // Add event listeners to invoke dialogue option events
        AddEventListeners(option1Action, option2Action);
    }

    IEnumerator WriteTextToUI(string chat, string option1, string option2)
    {
        // Clear displayed text
        chatText.text = "";
        option1Text.text = "";
        option2Text.text = "";

        // Deselect all buttons
        eventSystem.SetSelectedGameObject(null);

        // If either option is null or "", disable it
        option1Button.interactable = !string.IsNullOrEmpty(option1);
        option2Button.interactable = !string.IsNullOrEmpty(option2);

        // Display the person's name
        string name = gameplayManager.CurrentMessage.Recipient.Name;
        if (gameplayManager.HasUpgrade("Exit the Matrix"))
        {
            name = gameplayManager.CurrentMessage.Recipient.URL;
        }

        chatText.text = "<b>" + name + "</b>:\n";

        // Write to chat prompt
        for (int i = 0; i < chat.Length; i++)
        {
            // Delay between characters
            yield return new WaitForSeconds(CHAT_DELAY);

            // Write next character
            chatText.text += chat[i];
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
        chatTextMessage = "Have a good day!";
        option1Message = "";
        option2Message = "Bye.";

        // Make both buttons redirect to town
        option1Action = delegate { GoToTown(); };
        option2Action = delegate { GoToTown(); };
    }

    public void GoToTown()
    {
        // Fade out, transition to town
        fadeTransitionObject.FadeMethod("town");
    }
}
