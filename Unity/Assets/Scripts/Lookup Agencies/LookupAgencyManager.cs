using Assets.Scripts.Lookup_Agencies;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LookupAgencyManager : MonoBehaviour
{
    static LookupAgencyManager instance = null;

    [SerializeField] TextAsset[] populationListOptions;

    public readonly string[] LOCATION_TEXT = { "Northeast", "Southwest" };

    List<Person> listOfPeople;

    List<Person>[] peopleByLocation;

    // New variables
    List<Person> comprehensivePersonList;
    List<Person>[] peopleByQuadrant;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        gameObject.name = "LookupAgencyManager";
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadPopulationListFromTextAsset();
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

        List<Person> personList = null;

        for (int index = 0; index < peopleByLocation.Length; index++)
        {
            personList = peopleByLocation[index];

            for (int i = 0; i < personList.Count; i++)
            {
                temp = personList[i];
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

    // TODO: Change lookup system to 4 quadrants (NE, NW, SE, SW)
    // The NE LLA handles NE and NW quadrants
    // The SW LLA handles SE and SW quadrants
}