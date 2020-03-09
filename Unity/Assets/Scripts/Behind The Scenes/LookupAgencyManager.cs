using Assets.Scripts.Lookup_Agencies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookupAgencyManager : MonoBehaviour
{
    static LookupAgencyManager instance = null;

    [SerializeField] TextAsset listOfPeopleTextAsset;

    List<Person> listOfAllPeople;

    Dictionary<char, string> idLookupTable;
    Dictionary<string, char> reverseIDLookupTable;

    private void Awake()
    {
        // If there is already a Lookup Agency Manager -- Only need one
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        instance = this;

        idLookupTable = new Dictionary<char, string>();
        idLookupTable.Add('X', "Root Village");
        idLookupTable.Add('C', "COM Hills");
        idLookupTable.Add('O', "ORG Park");
        idLookupTable.Add('N', "NET Heights");

        reverseIDLookupTable = new Dictionary<string, char>();
        reverseIDLookupTable.Add("Root Village", 'X');
        reverseIDLookupTable.Add("Root", 'X');
        reverseIDLookupTable.Add("COM Hills", 'C');
        reverseIDLookupTable.Add("COM", 'C');
        reverseIDLookupTable.Add("ORG Park", 'O');
        reverseIDLookupTable.Add("ORG", 'O');
        reverseIDLookupTable.Add("NET Heights", 'N');
        reverseIDLookupTable.Add("NET", 'N');

        listOfAllPeople = new List<Person>();

        LoadListOfPeople();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddLookup(char key, string value, string altValue)
    {
        idLookupTable.Add(key, value);
        reverseIDLookupTable.Add(value, key);

        // If valid alt value
        if (!string.IsNullOrEmpty(altValue))
        {
            reverseIDLookupTable.Add(altValue, key);
        }
    }

    void LoadListOfPeople()
    {
        string[] linesOfPeople = listOfPeopleTextAsset.text.Split('\n');
        foreach (string line in linesOfPeople)
        {
            string[] parts = line.Split(',');
            string name = parts[0].Trim();
            string url = parts[1].Trim();
            char neighborhoodID = parts[2].Trim()[0];
            string neighborhood = "";
            idLookupTable.TryGetValue(neighborhoodID, out neighborhood);
            int houseNumber = System.Convert.ToInt32(parts[3].Trim());

            Person thisPerson = new Person(name, url, neighborhood, neighborhoodID, houseNumber);

            listOfAllPeople.Add(thisPerson);
        }
    }

    public Person FindPersonProfileByName(string personName)
    {
        Person targetPerson = null;

        foreach (Person thisPerson in listOfAllPeople)
        {
            if (thisPerson.Name.ToLower() == personName.ToLower())
            {
                targetPerson = thisPerson;
                break;
            }
        }

        return targetPerson;
    }

    public Person FindPersonProfileByURL(string url)
    {
        Person targetPerson = null;

        foreach (Person thisPerson in listOfAllPeople)
        {
            if (thisPerson.URL.ToLower() == url.ToLower())
            {
                targetPerson = thisPerson;
                break;
            }
        }

        return targetPerson;
    }

    public List<Person> GetListOfPeopleByNeighborhood(char id)
    {
        List<Person> neighborhoodPeople = new List<Person>();

        if (id == 'X')
        {
            neighborhoodPeople = listOfAllPeople;
        }
        else if (idLookupTable.ContainsKey(id))
        {
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
