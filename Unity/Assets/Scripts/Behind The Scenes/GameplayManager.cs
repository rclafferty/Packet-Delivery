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
    
    [SerializeField]
    string currentTarget;
    bool hasVisitedCLA;

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

        LoadPopulationList();

        currentTarget = "Chung Esch";
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

    void LoadPopulationList()
    {
        string[] list;
        LocalLookupAgencyLocation[] locations;

        string name = "";

        using (StreamReader sr = new StreamReader("Assets/Resources/PopulationList/populationList.txt"))
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
}
