using Assets.Scripts.Lookup_Agencies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CLA2LookupManager : MonoBehaviour
{
    [SerializeField]
    LookupAgencyManager lookupManager;

    List<Person> listOfPeople;

    Text peopleText;

    // Start is called before the first frame update
    void Start()
    {
        lookupManager.LoadPopulationListFromTextAsset();
        // lookupManager.LoadPopulationList();
        listOfPeople = lookupManager.CLAListOfPeople;

        Debug.Log("List of people is null ? " + (listOfPeople == null));

        peopleText = GameObject.Find("People Text").GetComponent<Text>();
        for (int i = 0; i < listOfPeople.Count; i++)
        {
            peopleText.text += listOfPeople[i].Name + "\n";
        }
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