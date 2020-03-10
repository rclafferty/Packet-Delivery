using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddressManager : MonoBehaviour
{
    GameplayManager gameplayManager;
    [SerializeField] House[] houseObjects;

    // Start is called before the first frame update
    void Start()
    {
        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        if (gameplayManager.HasUpgrade("Exit the Matrix"))
        {
            ExitTheMatrix();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExitTheMatrix()
    {
        for (int i = 0; i < houseObjects.Length; i++)
        {
            if (string.IsNullOrEmpty(houseObjects[i].ipAddress))
            {
                string neighborhood = houseObjects[i].name.Split(' ')[1];

                int index = 0;
                if (neighborhood == "COM")
                {
                    index = 67;
                }
                else if (neighborhood == "NET")
                {
                    index = 78;
                }
                else if (neighborhood == "ORG")
                {
                    index = 79;
                }

                int residenceNumber = System.Convert.ToInt32(houseObjects[i].residenceNumber);
                string ip = AddressManager.DetermineIPFromHouseInfo(residenceNumber, neighborhood[0]);

                houseObjects[i].ipAddress = ip;
            }

            houseObjects[i].residenceText.text = houseObjects[i].ipAddress;
        }
    }

    public static string DetermineIPFromHouseInfo(int houseNumber, char neighborhoodID)
    {
        int index = (char)neighborhoodID;

        int group = houseNumber / 100;
        string ip = "192." + index.ToString() + "." + group.ToString() + "." + houseNumber;

        return ip;
    }
}
