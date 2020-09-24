/* File: LookupAgencyManager.cs
 * Author: Casey Lafferty
 * Project: Packet Delivery
 */

using Assets.Scripts.Lookup_Agencies;
using System.Collections.Generic;
using UnityEngine;

public class LookupAgencyManager : MonoBehaviour
{
    // Lookup Agency Manager singleton reference
    static LookupAgencyManager instance = null;

    [SerializeField] UpgradeManager upgradeManager;

    // All text assets to be loaded from file
    [SerializeField] TextAsset listOfPeopleTextAsset;

    // All people in all neighborhoods
    List<Person> listOfAllPeople;

    // Lookup tables to convert neighbhorhood IDs to names
    Dictionary<char, string> idLookupTable;
    Dictionary<char, string> idLookupTableExitMatrix;

    private void Awake()
    {
        // If there is already a Lookup Agency Manager
        if (instance != null)
        {
            // Only need one --> Delete
            Destroy(gameObject);
            return;
        }

        // Set singleton instance
        DontDestroyOnLoad(gameObject);
        instance = this;

        // Load values into the ID lookup table
        idLookupTable = new Dictionary<char, string>();
        idLookupTable.Add('X', "Root Village");
        idLookupTable.Add('C', "COM Hills");
        idLookupTable.Add('O', "ORG Park");
        idLookupTable.Add('N', "NET Heights");

        // Load values into the ID lookup table
        idLookupTableExitMatrix = new Dictionary<char, string>();
        idLookupTableExitMatrix.Add('X', "Root DNS");
        idLookupTableExitMatrix.Add('C', "COM Top-Level Domain");
        idLookupTableExitMatrix.Add('O', "ORG Top-Level Domain");
        idLookupTableExitMatrix.Add('N', "NET Top-Level Domain");

        listOfAllPeople = new List<Person>();

        // Load people, addresses, etc from the TextAssets
        // LoadListOfPeople();
    }

    public void LoadListOfPeople()
    {
        // Split text asset by line
        string[] linesOfPeople = listOfPeopleTextAsset.text.Split('\n');

        // For each line in the text asset
        foreach (string line in linesOfPeople)
        {
            // Get name, URL, neighborhood ID, neighborhood name, and house number
            string[] parts = line.Split(',');
            string name = parts[0].Trim();
            char neighborhoodID = parts[1].Trim()[0];
            string url = ConstructURLFromName(name, neighborhoodID);
            // Debug.Log(url);
            string neighborhood = "";
            idLookupTable.TryGetValue(neighborhoodID, out neighborhood);
            int houseNumber = System.Convert.ToInt32(parts[2].Trim());

            Person thisPerson = new Person(name, url, neighborhood, neighborhoodID, houseNumber);

            // Store this person's profile in the list
            listOfAllPeople.Add(thisPerson);
        }
    }

    string ConstructURLFromName(string name, char neighborhoodID)
    {
        string domain = "";
        switch (neighborhoodID)
        {
            case 'C':
                domain = "com";
                break;
            case 'O':
                domain = "org";
                break;
            case 'N':
                domain = "net";
                break;
            default:
                Debug.Log("No domain found for name " + name + " at code " + neighborhoodID);
                break;
        }

        string lowercaseName = name.ToLower().Replace(' ', '-');
        string url = "www." + lowercaseName + "." + domain;
        return url;
    }

    public Person FindPersonProfileByName(string personName)
    {
        Person targetPerson = null;

        // Find the person by name in the list -- unsorted
        foreach (Person thisPerson in listOfAllPeople)
        {
            // Compare each name by lowercase
            if (thisPerson.Name.ToLower() == personName.ToLower())
            {
                // Matches -- Return this profile
                targetPerson = thisPerson;
                break;
            }
        }

        return targetPerson;
    }

    public Person FindPersonProfileByURL(string url)
    {
        Person targetPerson = null;

        // Find the person by URL in the list -- unsorted
        foreach (Person thisPerson in listOfAllPeople)
        {
            // Compare each URL by lowercase
            if (thisPerson.URL.ToLower() == url.ToLower())
            {
                // Matches -- Return this profile
                targetPerson = thisPerson;
                break;
            }
        }

        return targetPerson;
    }

    public List<Person> GetListOfPeopleByNeighborhood(char id)
    {
        List<Person> neighborhoodPeople = new List<Person>();

        // If in Root Village
        if (id == 'X')
        {
            // Return the whole list of people
            neighborhoodPeople = listOfAllPeople;
        }
        // If at a local lookup agency
        else if (idLookupTable.ContainsKey(id))
        {
            // Find all people within that neighborhood
            foreach (Person thisPerson in listOfAllPeople)
            {
                if (thisPerson.NeighborhoodID == id)
                {
                    neighborhoodPeople.Add(thisPerson);
                }
            }
        }
        
        return neighborhoodPeople;
    }

    public string GetNeighborhoodNameFromID(char id)
    {
        string name = "";
        if (upgradeManager.HasPurchasedUpgrade("Exit the Matrix"))
        {
            idLookupTableExitMatrix.TryGetValue(id, out name);
        }
        else
        {
            idLookupTable.TryGetValue(id, out name);
        }
        return name;
    }

    public string GetNeighborhoodNameFromID(char id, bool forcePreExit = true, bool forcePostExit = false)
    {
        string name = "";
        if (forcePreExit)
        {
            idLookupTable.TryGetValue(id, out name);
        }
        else if (forcePostExit)
        {
            idLookupTableExitMatrix.TryGetValue(id, out name);
        }
        else
        {
            return GetNeighborhoodNameFromID(id);
        }
        return name;
    }
}
