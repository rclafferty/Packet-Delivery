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

    string nextDeliveryLocation;

    string direction_building = "";
    string direction_color = "";
    string direction_direction = "";

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
            this.CurrentTargetMessage = new Message(-1, "Test", "Dummy", "Hi. How are ya?", 0, false);

        CompleteTask();
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
    }

    public string NextDeliveryLocation
    {
        get
        {
            return nextDeliveryLocation;
        }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                nextDeliveryLocation = "Office";
            }
            else
            {
                nextDeliveryLocation = value;
            }
        }
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

        ResetDirectionsToOffice();
    }

    void ResetDirectionsToOffice()
    {
        nextDeliveryLocation = "Office";

        direction_color = "red";
        direction_color = "office";
        direction_direction = "northwest";
    }

    public void GetNextMessage()
    {
        this.CurrentTargetMessage = letterManager.GetNextMessage();
        nextDeliveryLocation = "CLA";
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

    public string[] Directions
    {
        get
        {
            string[] directions = { direction_color, direction_building, direction_direction };
            return directions;
        }
        set
        {
            string[] directions = value;
            if (directions == null)
            {
                ResetDirectionsToOffice();
            }
            else if (directions.Length < 3)
            {
                ResetDirectionsToOffice();
            }
            else
            {
                direction_color = directions[0];
                direction_building = directions[1];
                direction_direction = directions[2];
            }
        }
    }
}