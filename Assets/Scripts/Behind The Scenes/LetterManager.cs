/* File: LetterManager.cs
 * Author: Casey Lafferty
 * Project: Packet Delivery
 */

using Assets.Scripts.Behind_The_Scenes;
using Assets.Scripts.Lookup_Agencies;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// Manages the overall concept of letters to deliver, including loading them, storing them, and randomizing which ones the player sees at what time.
/// </summary>
public class LetterManager : MonoBehaviour
{
    // Singleton reference
    static LetterManager instance = null;

    // Necessary manager references
    [SerializeField] LookupAgencyManager lookupAgencyManager;

    // Letters to be delivered -- Text assets (to be parsed into objects)
    [SerializeField] TextAsset[] letterTextFiles;

    // Letters to be delivered -- List of letter objects
    List<Letter> lettersToDeliver;

    // Previous letter -- To prevent duplicates
    Letter previousLetter;

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
        lettersToDeliver = new List<Letter>();

        // Initialize the previous letter -- Null means first delivery
        previousLetter = null;

        // No current letters loaded
        RemainingLetterCount = 0;
    }
    
    void Start()
    {
        lookupAgencyManager.LoadListOfPeople();
        LoadLettersFromTextAsset();
    }

    public void AddLetter(Person s, Person r, Letter p, string b)
    {
        // Determine letter ID
        int id = lettersToDeliver.Count + 1;

        // Make a new letter object and add it to the list
        Letter newLetter = new Letter(id, s, r, p, b);
        lettersToDeliver.Add(newLetter);
    }

    public Letter GetNextLetter()
    {
        Debug.Log("Retrieving next message");

        // List is not initialized -- ERROR
        if (lettersToDeliver == null)
        {
            Debug.Log("List of letters is null");
            return null;
        }

        // No letters are loaded -- ERROR
        if (lettersToDeliver.Count == 0)
        {
            Debug.Log("No letters are loaded from file");
            return null;
        }

        // Check how many letters there are left to deliver
        int remaining = 0;
        foreach (Letter l in lettersToDeliver)
        {
            if (!l.IsDelivered)
            {
                remaining++;
            }
        }

        Debug.Log("Remaining letters: " + (remaining - 1));

        // If no more
        if (remaining == 0)
        {
            Debug.Log("No more letters to deliver -- Recycling all letters");

            // Recycle all letters
            for (int i = 0; i < lettersToDeliver.Count; i++)
            {
                lettersToDeliver[i].MarkDelivered(false);
            }
        }

        if (previousLetter != null)
        {
            Debug.Log("Prev Delivered ? " + previousLetter.IsDelivered + ", Prev ID Delivered ? " + lettersToDeliver[previousLetter.ID - 1].IsDelivered);
        }
        else
        {
            Debug.Log("Prev Letter is null");
        }

        // Get next random undelivered letter
        Letter nextLetter = null;
        do
        {
            // Determine random index
            int id = Random.Range(0, lettersToDeliver.Count);
            if (previousLetter != null)
            {
                if (id == previousLetter.ID)
                {
                    continue;
                }
            }
            // Get letter at that index
            nextLetter = lettersToDeliver[id];

            if (nextLetter.Prerequisite != null)
            {
                if (!nextLetter.Prerequisite.IsDelivered)
                {
                    nextLetter = FindUndeliveredPrereq(nextLetter);
                }
            }
        } while (nextLetter == null || nextLetter.IsDelivered); // If isDelivered, find next letter
        
        Debug.Log(nextLetter.ToString());

        // Return the random letter
        return nextLetter;
    }

    Letter FindUndeliveredPrereq(Letter thisLetter)
    {
        if (thisLetter.Prerequisite != null)
        {
            if (thisLetter.Prerequisite.IsDelivered)
            {
                return thisLetter;
            }
            else
            {
                return FindUndeliveredPrereq(thisLetter.Prerequisite);
            }
        }
        else
        {
            return thisLetter;
        }
    }

    public void MarkDelivered(int id)
    {
        // If list is not initialized -- ERROR
        if (lettersToDeliver == null)
            return;

        // If ID is not valid -- ERROR
        if (id < 0 || id >= lettersToDeliver.Count)
            return;

        // Mark letter at given ID as delivered
        lettersToDeliver[id].MarkDelivered(true);

        // List that letter as previous letter
        previousLetter = lettersToDeliver[id];
    }

    public void MarkAllUndelivered()
    {
        // If list is not initialized -- ERROR
        if (lettersToDeliver == null)
            return;

        // Mark all letters as undelivered
        for (int i = 0; i < lettersToDeliver.Count; i++)
        {
            lettersToDeliver[i].MarkDelivered(false);
        }
    }

    public void ParseAndAddLetter(string to, string from, int prereqID, string body)
    {
        // Parse recipient
        string[] toParts = to.Split(':');
        string recipient = toParts[1].Trim();

        // Parse sender
        string[] fromParts = from.Split(':');
        string sender = fromParts[1].Trim();

        // Lookup sender and recipient objects
        Person senderProfile = lookupAgencyManager.FindPersonProfileByName(sender);
        Person recipientProfile = lookupAgencyManager.FindPersonProfileByName(recipient);

        if (senderProfile == null || recipientProfile == null)
        {
            Debug.Log("Found " + sender + " ? " + (senderProfile != null));
            Debug.Log("Found " + recipient + " ? " + (recipientProfile != null));
        }

        // Get prerequisite letter
        Letter prereqLetter = null; // Null means no prerequisite
        if (prereqID != -1)
        {
            prereqLetter = lettersToDeliver[prereqID];
        }

        // Add letter to the list to be delivered
        AddLetter(senderProfile, recipientProfile, prereqLetter, body);
    }

    void LoadLettersFromTextAsset()
    {
        StringBuilder sb = new StringBuilder();

        // For all letter files
        foreach (TextAsset letter in letterTextFiles)
        {
            // Separate content by line
            string[] parts = letter.text.Split('\n');

            // Get message header parts
            string to = parts[0].Trim();
            string from = parts[1].Trim();

            // Prerequisite is part 3
            string prerequisiteLetter = parts[2].Trim();
            int prerequisiteLetterID = -1;
            string prereqIDString = prerequisiteLetter.Split(':')[1].Trim();
            if (prereqIDString.ToLower() != "none")
            {
                // Debug.Log(prerequisiteLetter + ", " + prereqIDString);
                prerequisiteLetterID = System.Convert.ToInt32(prereqIDString) - 1; // 0 based, not 1 based
            }

            // Body
            sb.Clear();

            // Read the letter body line-by-line and add it to the list of letters
            for (int i = 4; i < parts.Length; i++)
            {
                // Add each line to the stored body
                sb.Append(parts[i].Trim());
                sb.Append("\n");
            }
            
            string message = sb.ToString();

            // Send parts for parsing and then add to list of letters
            ParseAndAddLetter(to, from, prerequisiteLetterID, message);
        }

        // Update number of remaining letters
        RemainingLetterCount = lettersToDeliver.Count;
    }

    public void ResetMessages()
    {
        // Mark all letters as NOT delivered
        for (int i = 0; i < lettersToDeliver.Count; i++)
        {
            lettersToDeliver[i].MarkDelivered(false);
        }
    }

    public int RemainingLetterCount { get; private set; }
}
