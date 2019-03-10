using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Scramble : MonoBehaviour
{
    private static Dictionary<char, int> dictionaryCharToNum;
    private static Dictionary<int, char> dictionaryNumToChar;

    // Start is called before the first frame update
    void Start()
    {
        InitializeDictionaries();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EncryptGUI()
    {
        InputField scrambleTextUI = GameObject.Find("ScrambleInputField").GetComponent<InputField>();
        string scrambleText = scrambleTextUI.text.ToUpper().Trim();
        string scrambled = Scramble.EncryptVigenere(scrambleText, scrambleText);
        //Debug.Log("Encrypted: " + scrambled);
        scrambleTextUI.text = scrambled;
    }

    private void InitializeDictionaries()
    {
        Scramble.dictionaryCharToNum = new Dictionary<char, int>();
        Scramble.dictionaryNumToChar = new Dictionary<int, char>();
        char[] specialChars = { '.', '!', '?', ' ', '&' };

        int i = 0;
        for (i = 0; i < 26; i++)
        {
            dictionaryCharToNum.Add((char)('A' + i), i);
            dictionaryNumToChar.Add(i, (char)('A' + i));
        }

        for (int j = 0; j < 10; j++, i++)
        {
            char x = j.ToString()[0];
            dictionaryCharToNum.Add(x, i);
            dictionaryNumToChar.Add(i, x);
        }
        
        for (int j = 0; j < specialChars.Length; j++, i++)
        {
            dictionaryCharToNum.Add(specialChars[j], i);
            dictionaryNumToChar.Add(i, specialChars[j]);
        }

        //Debug.Log("Size of dictionaries: " + dictionaryCharToNum.Count);
    }

    private static string EncryptVigenere(string plaintext, string key)
    {
        return BuildText(plaintext, key, true);
    }

    private static string DecryptVigenere(string ciphertext, string key)
    {
        return BuildText(ciphertext, key, false);
    }

    private static string BuildText(string text, string key, bool op)
    {
        if (string.IsNullOrEmpty(key))
        {
            //Debug.Log("INPUT IS NULL");
            return "";
        }

        string message = text;
        string keyText = MakeKey(key, message.Length);
        StringBuilder textBuilder = new StringBuilder();

        int sizeOfDictionary = dictionaryCharToNum.Count;
        int sizeOfText = message.Length;

        for (int i = 0; i < sizeOfText; i++)
        {
            int textInt = -1;
            int keyInt = -1;
            dictionaryCharToNum.TryGetValue(text[i], out textInt);
            dictionaryCharToNum.TryGetValue(keyText[i], out keyInt);

            int temp = -1;
            if (op)
            {
                temp = (textInt + keyInt + 8) % sizeOfDictionary;
            }
            else
            {
                temp = (textInt - keyInt - 8) % sizeOfDictionary;
                if (temp < 0)
                {
                    temp += sizeOfDictionary;
                }
            }

            char encryptedChar = '?';
            dictionaryNumToChar.TryGetValue(temp, out encryptedChar);
            //Debug.Log(encryptedChar);

            textBuilder.Append(encryptedChar);
        }

        //Debug.Log("Built Text: " + textBuilder.ToString());
        return textBuilder.ToString();
    }

    private static string MakeKey(string key, int length)
    {
        StringBuilder keytextBuilder = new StringBuilder();
        int keyindex = 0;
        for (int i = 0; i < length; i++)
        {
            keytextBuilder.Append(key[keyindex++]);
            if (keyindex == key.Length)
                keyindex = 0;
        }

        return keytextBuilder.ToString();
    }
}
