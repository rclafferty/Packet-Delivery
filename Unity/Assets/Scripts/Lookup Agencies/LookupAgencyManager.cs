using Assets.Scripts.Lookup_Agencies;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LookupAgencyManager : MonoBehaviour
{
    [SerializeField] TextAsset[] populationListOptions;

    public readonly string[] LOCATION_TEXT = { "Northeast", "Southwest" };

    static LookupAgencyManager instance = null;

    List<Person> listOfPeople;

    List<Person>[] peopleByLocation;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.name = "LookupAgencyManager";
        LoadPopulationListFromTextAsset();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool HasLoadedPopulationList
    {
        get
        {
            bool locationsIsLoaded = (peopleByLocation != null);
            bool peopleAreLoaded = (listOfPeople != null);

            return (locationsIsLoaded && peopleAreLoaded);
        }
    }

    public void LoadPopulationListFromTextAsset()
    {
        if (populationListOptions == null)
            // Invalid
            return;

        if (populationListOptions.Length == 0)
            // Invalid
            return;

        listOfPeople = new List<Person>();
        peopleByLocation = new List<Person>[LOCATION_TEXT.Length];
        for (int i = 0; i < LOCATION_TEXT.Length; i++)
        {
            peopleByLocation[i] = new List<Person>();
        }
        
        // Temporariliy disabled: Get random list
        // int index = Mathf.FloorToInt(Random.Range(0, populationListOptions.Length));

        // Get first list
        int index = 0;

        string[] populationListLines = populationListOptions[index].text.Split('\n');
        int numberOfPeople = System.Convert.ToInt32(populationListLines[0]);
        for (int i = 1; i < numberOfPeople; i++)
        {
            string[] lineParts = populationListLines[i].Split('\t');
            string name = lineParts[0];
            int locationIndex = System.Convert.ToInt32(lineParts[1]);
            string location = LOCATION_TEXT[locationIndex];

            Person thisPerson = new Person(name, location, locationIndex);
            listOfPeople.Add(thisPerson);

            Debug.Log("people list ? " + (peopleByLocation != null) + ", i = " + i);
            peopleByLocation[locationIndex].Add(thisPerson);
        }
    }

    public List<Person> GetNamesByLocation(string location)
    {
        string lower = location.ToLower();

        if (lower == "central")
        {
            return listOfPeople;
        }

        string directionLower;

        // Search through each location text
        for (int i = 0; i < LOCATION_TEXT.Length; i++)
        {
            directionLower = LOCATION_TEXT[i].ToLower();

            // If the desired location matches location at that index
            // e.g. ["Northeast", "Southwest"] and looking for "Northeast"
            if (lower == directionLower)
            {
                // Return the list of people at that location
                Debug.Log("people list ? " + (peopleByLocation != null) + ", i = " + i);
                return peopleByLocation[i];
            }
        }

        // Not a valid location
        return null;
    }

    public List<Person> CLAListOfPeople
    {
        get
        {
            return listOfPeople;
        }
    }

    public int LocationLookup(string name)
    {
        Person temp = null;
        string lowerName = name.ToLower();
        string lowerTempName = "";
        string[] parts = lowerName.Split(' ');

        for (int index = 0; index < peopleByLocation.Length; index++)
        {
            List<Person> list = peopleByLocation[index];

            for (int i = 0; i < list.Count; i++)
            {
                temp = list[i];
                lowerTempName = temp.Name.ToLower();

                for (int j = 0; j < parts.Length; j++)
                {
                    if (lowerTempName.Contains(parts[j]))
                    {
                        return index;
                    }
                }
            }
        }

        return -1;
    }
}