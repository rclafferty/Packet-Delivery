using Assets.Scripts.Lookup_Agencies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLALookupManager : MonoBehaviour
{
    [SerializeField]
    LookupAgencyManager manager;

    List<Person> listOfPeople;

    // Start is called before the first frame update
    void Start()
    {
        listOfPeople = manager.CLAListOfPeople;
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
