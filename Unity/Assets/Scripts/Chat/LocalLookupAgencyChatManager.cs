using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalLookupAgencyChatManager : BaseLookupAgencyChatManager
{
    // Start is called before the first frame update
    void Start()
    {
        string location = SceneManager.GetActiveScene().name.ToLower();
        if (location == "locallookupagencyne")
        {
            thisLocation = "Northeast";
        }
        else
        {
            thisLocation = "Southwest";
        }

        listOfPeople = lookupAgencyManager.GetNamesByLocation(thisLocation);
    }

    protected override bool IsInCorrectLocation()
    {
        string nextDeliveryLocation = gameplayManager.NextDeliveryLocation;
        return (nextDeliveryLocation == "LLA NE" && thisLocation == "Northeast") ||
            (nextDeliveryLocation == "LLA SW" && thisLocation == "Southwest");
    }
}
