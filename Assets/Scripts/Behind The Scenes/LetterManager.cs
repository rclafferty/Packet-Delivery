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
        LoadLettersFromTextAsset();
    }

    public void AddLetter(Person s, Person r, string b)
    {
        // Determine letter ID
        int id = lettersToDeliver.Count + 1;

        // Make a new letter object and add it to the list
        Letter newLetter = new Letter(id, s, r, b);
        lettersToDeliver.Add(newLetter);
    }

    public Letter GetNextLetter()
    {
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
        } while (nextLetter == null || nextLetter.IsDelivered); // If isDelivered, find next letter


        Debug.Log(nextLetter.ToString());

        // Return the random letter
        return nextLetter;
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

    public void ParseAndAddLetter(string to, string toURL, string from, string fromURL, string body)
    {
        // Parse recipient
        string[] toParts = to.Split(':');
        string recipient = toParts[1].Trim();

        // Parse recipient URL
        string[] toURLParts = toURL.Split(':');
        string recipientURL = toURLParts[1].Trim();

        // Parse sender
        string[] fromParts = from.Split(':');
        string sender = fromParts[1].Trim();

        // Parse sender URL
        string[] fromURLParts = fromURL.Split(':');
        string senderURL = fromURLParts[1].Trim();

        // Lookup sender and recipient objects
        Person senderProfile = lookupAgencyManager.FindPersonProfileByName(sender);
        Person recipientProfile = lookupAgencyManager.FindPersonProfileByName(recipient);

        if (senderProfile == null || recipientProfile == null)
        {
            Debug.Log("Found " + sender + " ? " + (senderProfile != null));
            Debug.Log("Found " + recipient + " ? " + (recipientProfile != null));
        }

        // Add letter to the list to be delivered
        AddLetter(senderProfile, recipientProfile, body);
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
            string toURL = parts[1].Trim();
            string from = parts[2].Trim();
            string fromURL = parts[3].Trim();

            // Urgency line is part 4 -- not used
            // Parts[5] is a blank line

            // Body
            sb.Clear();

            // Read the letter body line-by-line and add it to the list of letters
            for (int i = 6; i < parts.Length; i++)
            {
                // Add each line to the stored body
                sb.Append(parts[i].Trim());
                sb.Append("\n");
            }
            
            string message = sb.ToString();

            // Send parts for parsing and then add to list of letters
            ParseAndAddLetter(to, toURL, from, fromURL, message);
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
