using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    Upgrade[] upgrades;
    int dollars;

    // Start is called before the first frame update
    void Start()
    {
        dollars = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Purchase an upgrade based on the index of the upgrade
    /// </summary>
    /// <param name="index"></param>
    public void Purchase(int index)
    {
        // If index is out of bounds
        if (index >= upgrades.Length)
        {
            Debug.Log("Unable to purchase upgrade at index " + index);
            return;
        }

        Purchase(upgrades[index]);
    }

    /// <summary>
    /// Purchase an upgrade based given the name of the upgrade
    /// </summary>
    /// <param name="name"></param>
    public void Purchase(string name)
    {
        foreach (Upgrade u in upgrades)
        {
            if (u.Name == name)
            {
                Purchase(u);
                return;
            }
        }

        Debug.Log("Unable to find upgrade " + name);
    }

    void Purchase(Upgrade u)
    {
        int cost = u.Cost;
        if (dollars <= cost)
        {
            u.Purchase();
        }
    }
}
