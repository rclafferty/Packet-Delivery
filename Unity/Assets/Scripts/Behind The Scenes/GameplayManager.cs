// using System;
using Assets.Scripts.Behind_The_Scenes;
// using Assets.Scripts.Chat;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    static GameplayManager instance = null;
    
    [SerializeField]
    string currentTarget;

    [SerializeField]
    Message currentTargetMessage;
    bool hasVisitedCLA;

    string currentLocation;

    [SerializeField]
    LetterManager letterManager;

    int remainingTasks;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        hasVisitedCLA = false;

        if (name.Contains("cla2"))
            this.CurrentTargetMessage = new Message("Test", "Dummy", "Hi. How are ya?", 0, false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetLetterManager(LetterManager lm)
    {
        if (lm == null)
        {
            return;
        }

        letterManager = lm;
        remainingTasks = letterManager.RemainingLetters;

        // Get initial target
        CurrentTargetMessage = lm.GetNextMessage();
    }

    public string CurrentTarget
    {
        get
        {
            return currentTarget;
        }
        // set
        // {
        //     if (string.IsNullOrEmpty(value))
        //     {
        //         currentTarget = "";
        //         return;
        //     }
        //     else
        //     {
        //         currentTarget = value;
        //     }
        // }
    }

    public Message CurrentTargetMessage
    {
        get
        {
            return currentTargetMessage;
        }
        set
        {
            if (value == null)
            {
                currentTargetMessage = null;
                currentTarget = "";
            }
            else
            {
                currentTargetMessage = value;
                currentTarget = currentTargetMessage.Recipient;

                Debug.Log("Current target: " + currentTarget);
            }

            string name = SceneManager.GetActiveScene().name.ToLower();
            if (name.Contains("cla2"))
                gameObject.GetComponent<CLA2GameplayManager>().SetTargetText();
        }
    }

    public bool HasCurrentTarget
    {
        get
        {
            return (currentTarget != "");
        }
    }

    public bool VisitedCLA
    {
        get
        {
            return hasVisitedCLA;
        }
        set
        {
            hasVisitedCLA = value;
        }
    }

    public string CurrentLocation
    {
        get
        {
            return currentLocation;
        }
        set
        {
            currentLocation = value;
        }
    }

    public void CompleteTask()
    {
        currentTargetMessage = null;
        currentTarget = "";
    }

    public int RemainingTasks
    {
        get
        {
            return remainingTasks;
        }
        set
        {
            remainingTasks = value;
        }
    }

    public bool HasRemainingTasks
    {
        get
        {
            remainingTasks = letterManager.RemainingLetters;
            Debug.Log(remainingTasks + " remaining letters");
            return (remainingTasks > 0);
        }
    }
}