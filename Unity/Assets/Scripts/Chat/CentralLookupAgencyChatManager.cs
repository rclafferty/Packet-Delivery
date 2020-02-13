using Assets.Scripts.Lookup_Agencies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CentralLookupAgencyChatManager : BaseLookupAgencyChatManager
{
    // Start is called before the first frame update
    void Start()
    {
        thisLocation = "Central";
        listOfPeople = lookupAgencyManager.GetNamesByLocation(thisLocation);
    }

    protected override bool IsInCorrectLocation()
    {
        return gameplayManager.NextDeliveryLocation == "CLA";
    }
}
