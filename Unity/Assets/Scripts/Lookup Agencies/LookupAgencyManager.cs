using Assets.Scripts.Lookup_Agencies;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LookupAgencyManager : MonoBehaviour
{
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
        LoadPopulationList();
        gameObject.name = "LookupAgencyManager";
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

    public void LoadPopulationList()
    {
        const int MIN_INDEX = 0;
        const int MAX_INDEX = 4;

        int index = Mathf.FloorToInt(Random.Range(MIN_INDEX, MAX_INDEX));

        string filepath = "PopulationList/populationList" + index + "_with_index.txt";
#if UNITY_EDITOR
        filepath = "Assets/Resources/" + filepath;
#endif

        listOfPeople = new List<Person>();
        peopleByLocation = new List<Person>[LOCATION_TEXT.Length];
        for (int i = 0; i < LOCATION_TEXT.Length; i++)
        {
            peopleByLocation[i] = new List<Person>();
        }

        Person temp;

        string line;
        string[] parts;

        string location = "";
        int locationIndex = 0;

        using (StreamReader sr = new StreamReader(filepath))
        {
            int numberOfPeople = System.Convert.ToInt32(sr.ReadLine());

            for (int i = 0; i < numberOfPeople; i++)
            {
                line = sr.ReadLine();
                parts = line.Split('\t');

                name = parts[0];
                locationIndex = System.Convert.ToInt32(parts[1]);
                location = LOCATION_TEXT[locationIndex];

                temp = new Person(name, location, locationIndex);
                listOfPeople.Add(temp);
                peopleByLocation[locationIndex].Add(temp);
            }
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
                Debug.Log("Comparing " + name + " to " + temp.Name);

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