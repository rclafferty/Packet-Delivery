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

    [SerializeField] TextAsset[] letterTextFiles;
    ArrayList listOfLetters;

    string[] URGENCY_STATUS = { "Normal", "Expedited", "Urgent" };

    public static bool isFirstLetter = true;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        listOfLetters = new ArrayList();

        RemainingLetterCount = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadLettersFromTextAsset();
    }

    void LoadLettersFromTextAsset()
    {
        string recipientLine = "none";
        string senderLine = "none";
        string urgencyLine = "Normal";
        string message = "none";

        string[] parts = null;

        StringBuilder sb = new StringBuilder();

        foreach (TextAsset letter in letterTextFiles)
        {
            parts = letter.text.Split('\n');

            // Get message header parts
            recipientLine = parts[0].Trim();
            senderLine = parts[1].Trim();
            urgencyLine = parts[2].Trim();

            // Parts[3] is a blank line

            // Body
            sb.Clear();
            for (int i = 4; i < parts.Length; i++)
            {
                sb.Append(parts[i].Trim());
                sb.Append("\n");
            }

            message = sb.ToString();

            Letter newLetter = Letter.ParseMessage(listOfLetters.Count, recipientLine, senderLine, message, urgencyLine);
            listOfLetters.Add(newLetter);
        }

        RemainingLetterCount = listOfLetters.Count;
    }

    public Letter GetNextMessage()
    {
        // not initialized
        if (listOfLetters == null)
        {
            return null;
        }

        // No more messages
        if (RemainingLetterCount <= 0)
        {
            return null;
        }

        // Deliver letters in order
        Letter thisLetter = null;
        foreach (Letter letter in listOfLetters)
        {
            if (!letter.HasBeenDelivered && !letter.IsOnHold)
            {
                thisLetter = letter;
                letter.IsOnHold = true;
                break;
            }
        }

        RemainingLetterCount--;

        return thisLetter;
    }

    public Letter GetStartingMessage()
    {
        foreach (Letter letter in listOfLetters)
        {
            if (letter.Recipient.ToLower() == "Uncle Doug".ToLower())
            {
                return letter;
            }
        }

        return null;
    }

    /// <summary>
    /// Get n number of messages at a time
    /// To be used in Logistics after upgrading the bag size
    /// </summary>
    /// <param name="n">Number of messages to retrieve</param>
    /// <returns></returns>
    public Letter[] GetNextMessages(int n)
    {
        int numberOfMessages = n;

        // Cannot have a negative number of messages
        if (numberOfMessages <= 0)
        {
            return null;
        }
        // Return only the amount remaining
        else if (numberOfMessages > RemainingLetterCount)
        {
            numberOfMessages = RemainingLetterCount;
        }
        // else numberOfMessages <= remaining and numberOfMessages > 0, do nothing special

        Letter[] toReturn = new Letter[numberOfMessages];

        for (int i = 0; i < numberOfMessages; i++)
        {
            toReturn[i] = GetNextMessage();
        }

        return toReturn;
    }

    public void ClearOnHold()
    {
        foreach (Letter letter in listOfLetters)
        {
            letter.IsOnHold = false;
        }
    }

    public void MarkMessageAsDelivered(int messageID)
    {
        for (int i = 0; i < listOfLetters.Count; i++)
        {
            Letter thisLetter = (Letter)listOfLetters[i];
            if (thisLetter.MessageID == messageID)
            {
                ((Letter)listOfLetters[i]).HasBeenDelivered = true;
            }
        }
    }

    public int RemainingLetterCount { get; private set; }
}
