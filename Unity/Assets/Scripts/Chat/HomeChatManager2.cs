using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeChatManager2 : ChatManager
{
    GameplayManager gameplayManager;

    [SerializeField] EventSystem eventSystem;
    [SerializeField] Text chatText;
    [SerializeField] Text option1Text;
    [SerializeField] Text option2Text;
    [SerializeField] Button option1Button;
    [SerializeField] Button option2Button;

    string chatString;
    string option1String;
    string option2String;

    UnityAction option1Action;
    UnityAction option2Action;

    Coroutine currentCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        // Set scene objects using the parent class
        SetSceneObjects(eventSystem, chatText, option1Text, option1Button, option2Text, option2Button);

        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
    }

    void StartTextAndButtons()
    {
        chatString = "Hello there! What brings you to my home?";
        option1String = "I have a package for you.";
        option2String = "I'm sorry. I think I am at the wrong house.";

        option1Action = delegate
        {
            DeliverPackage();
            ShowText();
        };

        option2Action = delegate
        {
            Depart();
            ShowText();
        };
    }

    void ShowText()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        // Write text using parent class
        currentCoroutine = StartCoroutine(WriteText("[Name]", chatString, option1String, option2String));

        // Add event listeners using parent class
        AddEventListeners(option1Action, option2Action);
    }

    void AttemptToDeliverPackage()
    {
        if (gameplayManager == null)
            return;

        if (gameplayManager.NextDeliveryLocation.ToLower() != "home")
        {
            WrongLocation();
        }
        else if (gameplayManager.currentAddress != gameplayManager.deliveryAddress)
        {
            WrongLocation();
        }
        else if (gameplayManager.currentAddress == gameplayManager.deliveryAddress && gameplayManager.currentAddress == "")
        {
            WrongLocation();
        }
        else
        {
            DeliverPackage();
        }
    }

    void DeliverPackage()
    {
        chatString = "Thank you very much!";
        option1String = "Enjoy!";
        option2String = "";

        option1Action = delegate
        {
            if (gameplayManager.HasStartingLetter)
            {
                MorePackagesRequest();
            }
            else
            {
                Depart();
            }

            ShowText();
            gameplayManager.CompleteTask();
        };
    }

    void WrongLocation()
    {
        chatString = "I wasn't expecting a package.";
        option1String = "Sorry. I must have the wrong house.";
        option2String = "";

        option1Action = delegate
        {
            Depart();
            ShowText();
        };

        option2Action = null;
    }

    void MorePackagesRequest()
    {
        chatString = "I want to hire you to send another package. I'll email you the details.";
        option1String = "Thank you!";
        option2String = "";

        option1Action = delegate
        {
            Depart();
            ShowText();
        };

        option2Action = null;

        gameplayManager.HasStartingLetter = false;
    }

    void Depart()
    {
        chatString = "Have a good day!";
        option1String = "Bye.";
        option2String = "";

        option1Action = delegate { SceneManager.LoadScene("town"); };
        option2Action = null;
    }
}
