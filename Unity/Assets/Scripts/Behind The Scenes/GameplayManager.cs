// using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public enum LocalLookupAgencyLocation
    {
        NE = 0,
        SW = 1
    }
    static GameplayManager instance = null;
    string[] listOfNames;
    LocalLookupAgencyLocation[] peopleLocations;

    ArrayList[] namesPerLocation;
    
    [SerializeField]
    string currentTarget;
    bool hasVisitedCLA;

    static readonly float CHAT_DELAY = 0.01f;

    string currentLocation;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("Destroying GameplayManager duplicate");
            Destroy(gameObject);
            return;
        }

        namesPerLocation = new ArrayList[2];

        for (int i = 0; i < namesPerLocation.Length; i++)
        {
            namesPerLocation[i] = new ArrayList();
        }
        
        LoadPopulationList();

        currentTarget = listOfNames[0];
        hasVisitedCLA = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public string CurrentTarget
    {
        get
        {
            return currentTarget;
        }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                currentTarget = "";
                return;
            }
            else
            {
                currentTarget = value;
            }
        }
    }

    public static float ChatDelay
    {
        get
        {
            return CHAT_DELAY;
        }
    }

    void LoadPopulationList()
    {
        string[] list;
        LocalLookupAgencyLocation[] locations;

        string name = "";

        int index = Mathf.FloorToInt(Random.Range(0, 4));
        /*string filepath = "Assets/Resources/PopulationList/populationList" + index + ".txt";

        using (StreamReader sr = new StreamReader(filepath))
        {
            int numberOfNames = System.Convert.ToInt32(sr.ReadLine());
            list = new string[numberOfNames];
            locations = new LocalLookupAgencyLocation[numberOfNames];

            for (int i = 0; i < numberOfNames; i++)
            {
                name = sr.ReadLine();
                list[i] = name;

                int x = 0;

                do
                {
                    // Establish a random location for each person
                    x = (int)Random.Range(0, 2);
                } while (x > 1);

                locations[i] = (LocalLookupAgencyLocation)x;
                namesPerLocation[x].Add(name);
            }
        }*/

        string filepath = "Assets/Resources/PopulationList/populationList" + index + "_with_index.txt";

        using (StreamReader sr = new StreamReader(filepath))
        {
            int numberOfNames = System.Convert.ToInt32(sr.ReadLine());
            list = new string[numberOfNames];
            locations = new LocalLookupAgencyLocation[numberOfNames];

            for (int i = 0; i < numberOfNames; i++)
            {
                name = sr.ReadLine();
                string[] parts = name.Split('\t');
                list[i] = parts[0];
                
                int x = System.Convert.ToInt32(parts[1]);
                
                locations[i] = (LocalLookupAgencyLocation)x;
                namesPerLocation[x].Add(name);
            }
        }

        listOfNames = list;
        peopleLocations = locations;
    }

    public bool VisitedCLA
    {
        get
        {
            return hasVisitedCLA;
        }
        set
        {
            hasVisitedCLA = value;
        }
    }

    public string[] ListOfPeople
    {
        get
        {
            return listOfNames;
        }
    }

    public int[] PeopleLocations
    {
        get
        {
            int[] loc = new int[peopleLocations.Length];
            for (int i = 0; i < peopleLocations.Length; i++)
            {
                loc[i] = (int)peopleLocations[i];
            }
            return loc;
        }
    }

    public ArrayList GetNamesFromLocation(string direction)
    {
        if (direction.ToLower() == "northeast")
        {
            return namesPerLocation[(int)LocalLookupAgencyLocation.NE];
        }
        else if (direction.ToLower() == "southwest")
        {
            return namesPerLocation[(int)LocalLookupAgencyLocation.SW];
        }

        return null;
    }

    public string CurrentLocation
    {
        get
        {
            return currentLocation;
        }
        set
        {
            currentLocation = value;
        }
    }
}
