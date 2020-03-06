using Assets.Scripts.Behind_The_Scenes;
using Assets.Scripts.Lookup_Agencies;
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

    [SerializeField] LookupAgencyManager lookupAgencyManager;

    [SerializeField] TextAsset[] letterTextFiles;
    List<Letter> lettersToDeliver;

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
        lettersToDeliver = new List<Letter>();

        // No current letters loaded
        RemainingLetterCount = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadLettersFromTextAsset();
    }

    public void AddLetter(Person s, Person r, string b)
    {
        int id = lettersToDeliver.Count + 1;
        Letter newLetter = new Letter(id, s, r, b);
        lettersToDeliver.Add(newLetter);
    }

    public Letter GetNextLetter()
    {
        if (lettersToDeliver == null)
            return null;

        if (lettersToDeliver.Count == 0)
            return null;
        
        int id = Random.Range(0, lettersToDeliver.Count);
        return lettersToDeliver[id];
    }

    public void MarkDelivered(int id)
    {
        if (lettersToDeliver == null)
            return;

        if (id < 0 || id > lettersToDeliver.Count)
            return;

        lettersToDeliver[id].MarkDelivered(true);
    }

    public void MarkAllUndelivered()
    {
        if (lettersToDeliver == null)
            return;

        for (int i = 0; i < lettersToDeliver.Count; i++)
        {
            lettersToDeliver[i].MarkDelivered(false);
        }
    }

    public void ParseAndAddLetter(string to, string toURL, string from, string fromURL, string body)
    {
        string[] toParts = to.Split(':');
        string recipient = toParts[1].Trim();

        string[] toURLParts = toURL.Split(':');
        string recipientURL = toURLParts[1].Trim();

        string[] fromParts = from.Split(':');
        string sender = fromParts[1].Trim();

        string[] fromURLParts = fromURL.Split(':');
        string senderURL = fromURLParts[1].Trim();

        // Lookup sender and recipient
        Person senderProfile = lookupAgencyManager.FindPersonProfile(sender);
        Person recipientProfile = lookupAgencyManager.FindPersonProfile(recipient);

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
            // urgencyLine = parts[4].Trim();

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
            
            string message = sb.ToString();

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
