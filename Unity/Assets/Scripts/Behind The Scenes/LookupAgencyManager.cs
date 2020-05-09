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

    // All text assets to be loaded from file
    [SerializeField] TextAsset listOfPeopleTextAsset;

    // All people in all neighborhoods
    List<Person> listOfAllPeople;

    // Lookup tables to convert neighbhorhood IDs to names
    Dictionary<char, string> idLookupTable;

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

        listOfAllPeople = new List<Person>();

        // Load people, addresses, etc from the TextAssets
        LoadListOfPeople();
    }

    void LoadListOfPeople()
    {
        // Split text asset by line
        string[] linesOfPeople = listOfPeopleTextAsset.text.Split('\n');

        // For each line in the text asset
        foreach (string line in linesOfPeople)
        {
            // Get name, URL, neighborhood ID, neighborhood name, and house number
            string[] parts = line.Split(',');
            string name = parts[0].Trim();
            string url = parts[1].Trim();
            char neighborhoodID = parts[2].Trim()[0];
            string neighborhood = "";
            idLookupTable.TryGetValue(neighborhoodID, out neighborhood);
            int houseNumber = System.Convert.ToInt32(parts[3].Trim());

            Person thisPerson = new Person(name, url, neighborhood, neighborhoodID, houseNumber);

            // Store this person's profile in the list
            listOfAllPeople.Add(thisPerson);
        }
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
        idLookupTable.TryGetValue(id, out name);
        return name;
    }
}
