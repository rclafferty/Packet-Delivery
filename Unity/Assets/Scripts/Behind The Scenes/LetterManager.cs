using Assets.Scripts.Behind_The_Scenes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// Manages the overall concept of letters to deliver, including loading them, storing them, and randomizing which ones the player sees at what time.
/// </summary>
public class LetterManager : MonoBehaviour
{
    static LetterManager instance = null;

    [SerializeField]
    Message[] letters;
    bool[] isDelivered;
    bool[] onHold;

    [SerializeField]
    int remaining;

    string[] URGENCY_STATUS = { "Normal", "Expedited", "Urgent" };

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLetterFromFile()
    {
        LoadLetters();
    }

    private void LoadLetters()
    {
        // Define the first and last index for letters
        const int MIN_INDEX = 1;
        const int MAX_INDEX = 4;

        // Calculate the difference in the indexes
        int diff = MAX_INDEX - MIN_INDEX + 1;

        letters = new Message[diff];
        isDelivered = new bool[diff];
        onHold = new bool[diff];
        
        string filepath = "";
        for (int index = MIN_INDEX - 1; index < MAX_INDEX; index++)
        {
#if UNITY_EDITOR
            filepath = "Assets/Resources/Letters/letter" + (index + 1).ToString("00") + ".txt";
#else
            filepath = "Assets/Resources/Letters/letter" + (index + 1).ToString("00") + ".txt";
#endif

            string target = "none";
            string sender = "none";
            string urgent = "Normal";
            string message = "none";

            StringBuilder sb = new StringBuilder();
            string line = "";

            using (StreamReader sr = new StreamReader(filepath))
            {
                target = sr.ReadLine();
                sender = sr.ReadLine();
                urgent = sr.ReadLine();

                // Blank line
                sr.ReadLine();

                do
                {
                    line = sr.ReadLine();
                    if (line == "")
                    {
                        line = "\n\n";
                    }
                    sb.Append(line);
                } while (!sr.EndOfStream);
            } // End StreamReader

            message = sb.ToString();

            letters[index] = Message.ParseMessage(index, target, sender, message, urgent, false);
            isDelivered[index] = false;
        }

        remaining = letters.Length;
    }

    public Message GetNextMessage()
    {
        // not initialized
        if (isDelivered == null)
        {
            return null;
        }

        if (remaining <= 0)
        {
            return null;
        }

        int index = -1;

        do
        {
            index = (int)Random.Range(0, letters.Length);

            if (isDelivered[index])
            {
                index = -1;
            }
            if (onHold[index])
            {
                index = -1;
            }
        } while (index == -1);

        Message thisMessage = letters[index];
        onHold[index] = true;

        remaining--;

        return thisMessage;
    }

    public Message[] GetNextMessages(int n)
    {
        int numberOfMessages = n;

        // Cannot have a negative number of messages
        if (numberOfMessages <= 0)
        {
            return null;
        }
        // Return only the amount remaining
        else if (numberOfMessages > remaining)
        {
            numberOfMessages = remaining;
        }
        // else numberOfMessages <= remaining and numberOfMessages > 0, do nothing special

        Message[] toReturn = new Message[numberOfMessages];

        for (int i = 0; i < numberOfMessages; i++)
        {
            toReturn[i] = GetNextMessage();
        }

        return toReturn;
    }

    public void ClearOnHold()
    {
        for (int i = 0; i < onHold.Length; i++)
        {
            if (onHold[i] == true)
            {
                onHold[i] = false;
            }
        }
    }

    public int RemainingLetters
    {
        get
        {
            return remaining;
        }
    }
}
