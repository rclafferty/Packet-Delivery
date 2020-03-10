using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    static UpgradeManager instance = null;

    [SerializeField] GameplayManager gameplayManager;

    List<Upgrade> listOfUpgrades;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        listOfUpgrades = new List<Upgrade>();
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

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

    public bool HasPurchasedUpgrade(string title)
    {
        foreach (Upgrade thisUpgrade in listOfUpgrades)
        {
            if (thisUpgrade.Title.ToLower() == title.ToLower())
            {
                return thisUpgrade.IsUnlocked;
            }
        }

        return false;
    }

    public int GetUpgradeCost(string title)
    {
        foreach (Upgrade thisUpgrade in listOfUpgrades)
        {
            if (thisUpgrade.Title.ToLower() == title.ToLower())
            {
                return thisUpgrade.Cost;
            }
        }

        return -1;
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

    public bool AttemptPurchase(string title)
    {
        bool isSuccessful = false;
        for (int i = 0; i < listOfUpgrades.Count; i++)
        {
            if (listOfUpgrades[i].Title.ToLower() == title.ToLower())
            {
                if (listOfUpgrades[i].IsUnlocked)
                {
                    isSuccessful = true;
                }
                else if (gameplayManager.Money >= listOfUpgrades[i].Cost)
                {
                    gameplayManager.Money -= listOfUpgrades[i].Cost;
                    listOfUpgrades[i].Purchase();
                    isSuccessful = true;
                }
                else
                {
                    isSuccessful = false;
                }

                break;
            }
        }

        gameplayManager.ForceUpdateHUD();
        return isSuccessful;
    }

#if UNITY_EDITOR
    public bool ForcePurchaseUpgrade(string title)
    {
        bool isSuccessful = false;
        for (int i = 0; i < listOfUpgrades.Count; i++)
        {
            if (listOfUpgrades[i].Title.ToLower() == title.ToLower())
            {
                if (!listOfUpgrades[i].IsUnlocked)
                {
                    listOfUpgrades[i].Purchase();
                }

                isSuccessful = true;
                break;
            }
        }

        gameplayManager.ForceUpdateHUD();
        return isSuccessful;
    }
#endif
}
