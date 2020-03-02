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
        // If there is already a Letter Manager -- Only need one
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // Set this as the Letter Manager instance
        instance = this;

        // Persist across scenes
        DontDestroyOnLoad(gameObject);

        // Initialize the list of letters
        listOfLetters = new ArrayList();

        // No current letters loaded
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
        string recipientLineURL = "none";
        string senderLine = "none";
        string senderLineURL = "none";
        string urgencyLine = "Normal";
        string message = "none";

        string[] parts = null;

        StringBuilder sb = new StringBuilder();

        // For all letter files
        foreach (TextAsset letter in letterTextFiles)
        {
            // Separate content by line
            parts = letter.text.Split('\n');

            // Get message header parts
            recipientLine = parts[0].Trim();
            recipientLineURL = parts[1].Trim();
            senderLine = parts[2].Trim();
            senderLineURL = parts[3].Trim();
            urgencyLine = parts[4].Trim();
            
            // Parts[5] is a blank line

            // Body
            sb.Clear();

            // Read the letter body line-by-line and add it to the 
            for (int i = 6; i < parts.Length; i++)
            {
                // Add each line to the stored body
                sb.Append(parts[i].Trim());
                sb.Append("\n");
            }
            
            message = sb.ToString();

            // Create new letter object
            Letter newLetter = Letter.ParseMessage(listOfLetters.Count, recipientLine, recipientLineURL, senderLine, senderLineURL, message, urgencyLine);

            // Add new letter to the letters to deliver
            listOfLetters.Add(newLetter);
        }

        // Update number of remaining letters
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
            // If the letter hasn't been previously selected
            if (!letter.HasBeenDelivered && !letter.IsOnHold)
            {
                thisLetter = letter;

                // Put the letter on hold
                letter.IsOnHold = true;
                break;
            }
        }

        // TODO: Randomize the letters
        // TODO: Set seed for testing

        // Update number of remaining letters
        RemainingLetterCount--;

        // Give the current letter to be delivered
        return thisLetter;
    }

    public Letter GetStartingMessage()
    {
        // Find Uncle Doug's letter
        foreach (Letter letter in listOfLetters)
        {
            if (letter.Recipient.ToLower() == "Uncle Doug".ToLower())
            {
                return letter;
            }
        }

        // TODO: Get random first letter

        return null;
    }

    public void ResetMessages()
    {
        // Mark all letters as NOT delivered
        foreach (Letter letter in listOfLetters)
        {
            letter.HasBeenDelivered = false;
        }
    }

    /// <summary>
    /// Get n number of messages at a time
    /// To be used in Logistics after upgrading the bag size
    /// </summary>
    /// <param name="n">Number of messages to retrieve</param>
    /// <returns></returns>
    public Letter[] GetNextMessages(in int n)
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

        // Retrieve n number of messages to return
        for (int i = 0; i < numberOfMessages; i++)
        {
            toReturn[i] = GetNextMessage();
        }

        return toReturn;
    }

    public void ClearOnHold()
    {
        // Mark all letters as ready (not on hold)
        foreach (Letter letter in listOfLetters)
        {
            letter.IsOnHold = false;
        }
    }

    public void MarkMessageAsDelivered(int messageID)
    {
        // Find letter with the message ID
        for (int i = 0; i < listOfLetters.Count; i++)
        {
            Letter thisLetter = (Letter)listOfLetters[i];

            // If found the correct message
            if (thisLetter.MessageID == messageID)
            {
                // Mark as delivered
                ((Letter)listOfLetters[i]).HasBeenDelivered = true;
            }
        }
    }

    public int RemainingLetterCount { get; private set; }
}
