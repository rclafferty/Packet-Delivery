using Assets.Scripts.Lookup_Agencies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CacheManager : MonoBehaviour
{
    static CacheManager instance = null;

    List<Person> listOfAddresses;

    [SerializeField] GameplayManager gameplayManager;

    int capacity = 3;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        listOfAddresses = new List<Person>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddAddress(Person resident)
    {
        if (!IsPersonCached(resident))
        {
            if (listOfAddresses.Count == capacity)
            {
                listOfAddresses.RemoveAt(0);
            }

            listOfAddresses.Add(resident);

            gameplayManager.ForceUpdateHUD();
        }
    }

    public void AddOneSlot()
    {
        capacity++;
    }

    public void DisplayCachedAddresses(Text uiText)
    {
        if (listOfAddresses == null)
            return;

        uiText.text = "Cached Addresses:";

        string name = "";
        string neighborhood = "";
        string address = "-1";

        for (int i = 0; i < listOfAddresses.Count; i++)
        {
            uiText.text += "\n\n";

            neighborhood = listOfAddresses[i].Neighborhood;

            if (gameplayManager.HasUpgrade("Exit the Matrix"))
            {
                name = listOfAddresses[i].URL;
                address = AddressManager.DetermineIPFromHouseInfo(listOfAddresses[i].HouseNumber, listOfAddresses[i].NeighborhoodID);
            }
            else
            {
                name = listOfAddresses[i].Name;
                address = listOfAddresses[i].HouseNumber.ToString();
            }

            uiText.text += (i + 1) + ") " + name + "\n" + neighborhood + "\n" + address;
        }
    }

    public bool IsPersonCached(string name, out Person targetPerson)
    {
        targetPerson = null;
        bool hasExitedTheMatrix = gameplayManager.HasUpgrade("Exit the Matrix");

        foreach (Person thisPerson in listOfAddresses)
        {
            if (hasExitedTheMatrix)
            {
                if (thisPerson.URL == name)
                {
                    targetPerson = thisPerson;
                    return true;
                }
            }
            else
            {
                if (thisPerson.Name == name)
                {
                    targetPerson = thisPerson;
                    return true;
                }
            }
        }

        return false;
    }

    public bool IsPersonCached(Person target)
    {
        foreach (Person thisPerson in listOfAddresses)
        {
            if (thisPerson == target)
            {
                return true;
            }
        }

        return false;
    }
}