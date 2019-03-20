// using System;
using Assets.Scripts.Chat;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    static GameplayManager instance = null;
    
    [SerializeField]
    string currentTarget;
    Message currentTargetMessage;
    bool hasVisitedCLA;

    string currentLocation;

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
    }

    // Update is called once per frame
    void Update()
    {

    }

    public string CurrentTarget
    {
        get
        {
            return currentTarget;
        }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                currentTarget = "";
                return;
            }
            else
            {
                currentTarget = value;
            }
        }
    }

    public Message CurrentTargetMessage
    {
        get
        {
            return currentTargetMessage;
        }
        set
        {
            currentTargetMessage = value;
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
}
