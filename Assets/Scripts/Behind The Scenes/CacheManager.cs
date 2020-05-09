/* File: CacheManager.cs
 * Author: Casey Lafferty
 * Project: Packet Delivery
 */

using Assets.Scripts.Lookup_Agencies;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CacheManager : MonoBehaviour
{
    // Cache Manager singleton instance
    static CacheManager instance = null;

    // Gameplay manager reference
    [SerializeField] GameplayManager gameplayManager;

    // List of people and their relative details
    List<Person> listOfCachedDetails;

    // Starting capacity = 3
    public int maxCapacity = 3;

    private void Awake()
    {
        // If not the first instance of CacheManager
        if (instance != null)
        {
            // Only need one --> Delete
            Destroy(gameObject);
            return;
        }

        // Set the singleton as this instance
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        listOfCachedDetails = new List<Person>();
    }

    public void AddAddress(Person resident)
    {
        // If person is NOT already cached
        if (!IsPersonCached(resident))
        {
            // If the cache is at max capacity
            if (listOfCachedDetails.Count == maxCapacity)
            {
                // Remove the oldest cached details
                listOfCachedDetails.RemoveAt(0);
            }

            // Add the newest details to cache
            listOfCachedDetails.Add(resident);

            // Update the HUD
            gameplayManager.ForceUpdateHUD();
        }
    }

    public void AddOneSlot()
    {
        // Add one to the max capacity
        maxCapacity++;
    }

    void DisplayCachedAddressesTogether(string heading1, string heading2, Text uiText1, Text uiText2)
    {
        // If the list of details is invalid
        if (listOfCachedDetails == null)
            return;

        // Set address book header text
        uiText1.text = heading1 + "\n\n";
        uiText2.text = heading2 + "\n\n";

        string name = "";
        string neighborhood = "";
        string address = "-1";

        // Set default strings
        string[] addressDisplayStrings = new string[maxCapacity];
        for (int i = 0; i < maxCapacity; i++)
        {
            addressDisplayStrings[i] = "Slot " + (i + 1) + ":\nEmpty\n\n";
        }

        // Fill in list of cached details
        for (int i = 0; i < listOfCachedDetails.Count; i++)
        {
            neighborhood = listOfCachedDetails[i].Neighborhood;

            // If the player has the Exit the Matrix upgrade
            if (gameplayManager.HasUpgrade("Exit the Matrix"))
            {
                // Reference the recipient's URL
                name = listOfCachedDetails[i].URL;

                // Reference the recipient's IP address
                address = AddressManager.DetermineIPFromHouseInfo(listOfCachedDetails[i].HouseNumber, listOfCachedDetails[i].NeighborhoodID);
            }
            else
            {
                // Reference the recipient's name
                name = listOfCachedDetails[i].Name;

                // Reference the recipient's house number
                address = listOfCachedDetails[i].HouseNumber.ToString();
            }

            // Add this slot's details to the text
            addressDisplayStrings[i] = "Slot " + (i + 1) + ":\n" + name + "\n" + neighborhood + "\n" + address;
        }

        for (int i = 0; i < addressDisplayStrings.Length && i < 3; i++)
        {
            uiText1.text += addressDisplayStrings[i] + "\n\n";
        }

        for (int i = 3; i < addressDisplayStrings.Length && i < 5; i++)
        {
            uiText2.text += addressDisplayStrings[i] + "\n\n";
        }
    }

    /*
    void DisplayCachedAddressesByPart(string heading, Text uiText, int startIndex, int endIndex)
    {
        // If the list of details is invalid
        if (listOfCachedDetails == null)
            return;
        
        // Set address book header text
        uiText.text = heading;

        string name = "";
        string neighborhood = "";
        string address = "-1";

        int minIndex = Mathf.Min(listOfCachedDetails.Count, endIndex + 1);
        
        for (int i = startIndex; i < minIndex; i++)
        {
            uiText.text += "\n\n";

            neighborhood = listOfCachedDetails[i].Neighborhood;

            // If the player has the Exit the Matrix upgrade
            if (gameplayManager.HasUpgrade("Exit the Matrix"))
            {
                // Reference the recipient's URL
                name = listOfCachedDetails[i].URL;

                // Reference the recipient's IP address
                address = AddressManager.DetermineIPFromHouseInfo(listOfCachedDetails[i].HouseNumber, listOfCachedDetails[i].NeighborhoodID);
            }
            else
            {
                // Reference the recipient's name
                name = listOfCachedDetails[i].Name;

                // Reference the recipient's house number
                address = listOfCachedDetails[i].HouseNumber.ToString();
            }

            // Add this slot's details to the text
            uiText.text += (i + 1) + ") " + name + "\n" + neighborhood + "\n" + address;
        }

        int minEmptyIndex = Mathf.Min(maxCapacity, endIndex + 1);

        // For any empty slots
        for (int i = startIndex; i < minEmptyIndex; i++)
        {
            // Display "Empty"
            uiText.text += "\n\n" + (i + 1) + ") Empty";
        }
    }
    */

    public void DisplayCachedAddressesInTwoParts(Text uiText1, Text uiText2)
    {
        // DisplayCachedAddressesByPart("Cached Addresses:", uiText1, 0, 2);
        // DisplayCachedAddressesByPart("", uiText2, 3, 4);
        DisplayCachedAddressesTogether("Cached Addresses:", "", uiText1, uiText2);
    }

    /*
    public void DisplayCachedAddresses(Text uiText)
    {
        // If the list of details is invalid
        if (listOfCachedDetails == null)
            return;
        
        // Set address book header text
        uiText.text = "Cached Addresses:";

        string name = "";
        string neighborhood = "";
        string address = "-1";

        // Display all cached details
        for (int i = 0; i < listOfCachedDetails.Count; i++)
        {
            uiText.text += "\n\n";
            
            neighborhood = listOfCachedDetails[i].Neighborhood;

            // If the player has the Exit the Matrix upgrade
            if (gameplayManager.HasUpgrade("Exit the Matrix"))
            {
                // Reference the recipient's URL
                name = listOfCachedDetails[i].URL;

                // Reference the recipient's IP address
                address = AddressManager.DetermineIPFromHouseInfo(listOfCachedDetails[i].HouseNumber, listOfCachedDetails[i].NeighborhoodID);
            }
            else
            {
                // Reference the recipient's name
                name = listOfCachedDetails[i].Name;

                // Reference the recipient's house number
                address = listOfCachedDetails[i].HouseNumber.ToString();
            }

            // Add this slot's details to the text
            uiText.text += (i + 1) + ") " + name + "\n" + neighborhood + "\n" + address;
        }

        // For any empty slots
        for (int i = listOfCachedDetails.Count; i < maxCapacity; i++)
        {
            // Display "Empty"
            uiText.text += "\n\n" + (i + 1) + ") Empty";
        }
    }
    */

    public bool IsPersonCached(string name, out Person targetPerson)
    {
        targetPerson = null;

        // Check if the player has purchased the "exit the matrix" upgrade
        bool hasExitedTheMatrix = gameplayManager.HasUpgrade("Exit the Matrix");

        // Find the requested person
        foreach (Person thisPerson in listOfCachedDetails)
        {
            // If the player has purchased the upgrade
            if (hasExitedTheMatrix)
            {
                // If the URL matches
                if (thisPerson.URL == name)
                {
                    // Found the requested person in cache
                    targetPerson = thisPerson;
                    return true;
                }
            }
            // If the player has NOT purchased the upgrade
            else
            {
                // If the name matches
                if (thisPerson.Name == name)
                {
                    // Found the requested person in cache
                    targetPerson = thisPerson;
                    return true;
                }
            }
        }

        // Did not find the requested person in cache
        return false;
    }

    public bool IsPersonCached(Person target)
    {
        // Find the requested person
        foreach (Person thisPerson in listOfCachedDetails)
        {
            // If the references match
            if (thisPerson == target)
            {
                // Found the requested person in cache
                return true;
            }
        }

        // Did not find the requested person in cache
        return false;
    }
}