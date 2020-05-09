/* File: UpgradeManager.cs
 * Author: Casey Lafferty
 * Project: Packet Delivery
 */

 using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    // Upgrade Manager singleton reference
    static UpgradeManager instance = null;

    // Necessary manager references
    [SerializeField] GameplayManager gameplayManager;

    // List of all possible upgrades
    List<Upgrade> listOfUpgrades;

    private void Awake()
    {
        // If there is already an Upgrade Manager reference
        if (instance != null)
        {
            // Only need one --> Delete
            Destroy(gameObject);
            return;
        }

        // Set this as the singleton reference
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize list of upgrades
        listOfUpgrades = new List<Upgrade>();
    }

    public void AddUpgrade(string title, int cost, bool isRepeatable)
    {
        // Create a new upgrade based on the given parameters
        Upgrade newUpgrade = new Upgrade(title, cost, isRepeatable);

        // Add the upgrade to the list
        listOfUpgrades.Add(newUpgrade);
    }

    Upgrade FindUpgrade(string title)
    {
        // Search through the list for the requested upgrade
        foreach (Upgrade thisUpgrade in listOfUpgrades)
        {
            // If the titles match
            if (thisUpgrade.Title.ToLower() == title.ToLower())
            {
                // Return this upgrade profile
                return thisUpgrade;
            }
        }

        return null;
    }

    public bool HasPurchasedUpgrade(string title)
    {
        // Search through the list for the requested upgrade
        Upgrade thisUpgrade = FindUpgrade(title);

        // If this upgrade was found
        if (thisUpgrade != null)
        {
            // If it is repeatable, it can be purchased again
            if (thisUpgrade.IsRepeatable)
                return false;
            // It is not repeatable -- check if it's been purchased once or not
            else
                return thisUpgrade.IsUnlocked;
        }
        else
        {
            // Did not find it -- Did not purchase
            return false;
        }
    }

    public int GetQuantity(string title)
    {
        // Search through the list for the requested upgrade
        Upgrade thisUpgrade = FindUpgrade(title);

        // If found, return the number of times this upgrade has been purchased
        // Else, return -1 --> error
        return thisUpgrade != null ? thisUpgrade.Quantity : -1;
    }
    
    public bool IsRepeatable(string title)
    {
        // Search through the list for the requested upgrade
        Upgrade thisUpgrade = FindUpgrade(title);

        // If found, check if it is repeatable
        // Else, not repeatable by default -- false (error)
        return thisUpgrade != null ? thisUpgrade.IsRepeatable : false;
    }

    public int GetUpgradeCost(string title)
    {
        // Search through the list for the requested upgrade
        Upgrade thisUpgrade = FindUpgrade(title);

        // If found, return the cost of this upgrade
        // Else, return -1 --> error
        return thisUpgrade != null ? thisUpgrade.Cost : -1;
    }

    public bool AttemptPurchase(string title)
    {
        bool isSuccessful = false;

        // Search through the list for the requested upgrade
        for (int i = 0; i < listOfUpgrades.Count; i++)
        {
            // If the titles match
            if (listOfUpgrades[i].Title.ToLower() == title.ToLower())
            {
                // If the upgrade was already purchased and NOT repeatable
                if (listOfUpgrades[i].IsUnlocked && !listOfUpgrades[i].IsRepeatable)
                {
                    // Previous success
                    isSuccessful = true;
                }
                // If the player has enough money to purchase it
                else if (gameplayManager.Money >= listOfUpgrades[i].Cost)
                {
                    // Subtract the amount of money required
                    gameplayManager.Money -= listOfUpgrades[i].Cost;

                    // Mark as purchased
                    listOfUpgrades[i].Purchase();

                    // Success!
                    isSuccessful = true;
                }
                // If the player does not have enough money
                else
                {
                    // Not successful
                    isSuccessful = false;
                }

                break;
            }
        }

        // Update the HUD
        gameplayManager.ForceUpdateHUD();

        // Notify if the purchase was successful
        return isSuccessful;
    }

    public void ResetUpgrades()
    {
        // Reset each upgrade via their Reset() method
        foreach (Upgrade u in listOfUpgrades)
        {
            u.Reset();
        }
    }
}
