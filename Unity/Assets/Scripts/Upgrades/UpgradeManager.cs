using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    List<Upgrade> listOfUpgrades;

    // Start is called before the first frame update
    void Start()
    {
        listOfUpgrades = new List<Upgrade>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddUpgrade(string title, int cost)
    {
        Upgrade newUpgrade = new Upgrade(title, cost);
        listOfUpgrades.Add(newUpgrade);
    }

    public bool AttemptPurchaseUpgrade(string title)
    {
        bool isSuccessful = false;

        for (int i = 0; i < listOfUpgrades.Count; i++)
        {
            if (listOfUpgrades[i].Title.ToLower() == title.ToLower())
            {
                if (!listOfUpgrades[i].IsUnlocked)
                {
                    listOfUpgrades[i].Purchase();
                    isSuccessful = true;
                    break;
                }
            }
        }

        return isSuccessful;
    }

    public int NumberOfUpgradesPurchased
    {
        get
        {
            int purchased = 0;
            foreach (Upgrade upgrade in listOfUpgrades)
            {
                if (upgrade.IsUnlocked)
                {
                    purchased++;
                }
            }

            return purchased;
        }
    }
}
