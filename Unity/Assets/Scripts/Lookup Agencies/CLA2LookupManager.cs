using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLA2LookupManager : MonoBehaviour
{
    [SerializeField]
    LookupAgencyManager lookupManager;

    List<Person> listOfPeople;

    // Start is called before the first frame update
    void Start()
    {
        listOfPeople = lookupManager.CLAListOfPeople;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Person> People
    {
        get
        {
            return listOfPeople;
        }
    }
}