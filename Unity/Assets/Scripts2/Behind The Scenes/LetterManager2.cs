using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LetterManager2 : MonoBehaviour
{
    static LetterManager2 instance = null;

    List<Letter2> allLetters;

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
        allLetters = new List<Letter2>();

        const int MIN_INDEX = 1;
        const int MAX_INDEX = 4;

        TextAsset t = null;
        string[] tempParts;

        string r;
        string recipient;
        string s;
        string sender;
        StringBuilder messageBuilder = new StringBuilder();

        for (int i = MIN_INDEX; i < MAX_INDEX; i++)
        {
            t = Resources.Load<TextAsset>("Letters/letter" + i.ToString("00") + ".txt");
            tempParts = t.text.Split('\n');
            r = tempParts[0];
            s = tempParts[1];
            // 2 is class

            for (int m = 3; m < tempParts.Length; m++)
            {
                messageBuilder.Append(tempParts.Length);
                messageBuilder.Append("\n\n");
            }

            tempParts = r.Split(':');
            recipient = tempParts[1].Trim();

            tempParts = s.Split(':');
            sender = tempParts[1].Trim();

            allLetters.Add(new Letter2(allLetters.Count, sender, recipient, messageBuilder.ToString()));
        }
    }

    public Letter2 GetNextMessage()
    {
        foreach (Letter2 l in allLetters)
        {
            if (!l.Delivered)
            {
                CurrentLetter = l;
                return l;
            }
        }

        return null;
    }

    public void LetterDelivered(Letter2 thisLetter)
    {
        Letter2 currLetter = null;
        for (int i = 0; i < allLetters.Count; i++)
        {
            currLetter = allLetters[i];

            if (thisLetter.ID == currLetter.ID)
            {
                thisLetter.Delivered = true;
                currLetter.Delivered = true;

                allLetters.RemoveAt(i);
            }
        }
    }

    public Letter2 GetStartingLetter()
    {
        foreach (Letter2 l in allLetters)
        {
            if (l.Recipient.ToLower() == "Uncle Doug".ToLower())
            {
                return l;
            }
        }

        return null;
    }

    public int RemainingLetters
    {
        get
        {
            return allLetters.Count;
        }
    }

    // Auto property
    public Letter2 CurrentLetter { get; private set; }
}

public class Letter2
{
    public Letter2(int id, string to, string from, string body)
    {
        this.ID = id;
        this.Sender = from;
        this.Recipient = to;
        this.Message = body;

        this.Delivered = false;
    }

    // Auto property
    public int ID { get; private set; }
    public string Recipient { get; private set; }
    public string Sender { get; private set; }
    public string Message { get; private set; }
    public bool Delivered { get; set; }
}