/* File: AddressManager.cs
 * Author: Casey Lafferty
 * Project: Packet Delivery
 */

using UnityEngine;
using UnityEngine.UI;

public class AddressManager : MonoBehaviour
{
    // Necessary manager references
    GameplayManager gameplayManager;

    // All house objects in the scene
    [SerializeField] House[] houseObjects;

    // All lookup agency labels in the scene
    [SerializeField] Text rootLookupAgencyLabel;
    [SerializeField] Text comLookupAgencyLabel;
    [SerializeField] Text netLookupAgencyLabel;
    [SerializeField] Text orgLookupAgencyLabel;

    // Start is called before the first frame update
    void Start()
    {
        // Find the Gameplay Manager
        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();

        // If the player has already purchased the "Exit the Matrix" upgrade
        if (gameplayManager.HasUpgrade("Exit the Matrix"))
        {
            // Change all house to IP addresses
            ExitTheMatrix();
        }
    }

    public void ExitTheMatrix()
    {
        // Change all house numbers to IP addresses
        for (int i = 0; i < houseObjects.Length; i++)
        {
            // If the ip address is missing
            if (string.IsNullOrEmpty(houseObjects[i].ipAddress))
            {
                // Calculate it based on the name and neighborhood
                string neighborhood = houseObjects[i].name.Split(' ')[1];

                int index = 0;

                // If in the COM neighborhood
                if (neighborhood == "COM")
                {
                    // ASCII value for 'C' (for simplicity)
                    index = 67;
                }
                // If in the NET neighborhood
                else if (neighborhood == "NET")
                {
                    // ASCII value for 'N' (for simplicity)
                    index = 78;
                }
                // If in the ORG neighborhood
                else if (neighborhood == "ORG")
                {
                    // ASCII value for 'O' (for simplicity)
                    index = 79;
                }

                // Calculate the IP from the house number and above neighborhood information
                int residenceNumber = System.Convert.ToInt32(houseObjects[i].residenceNumber);
                string ip = AddressManager.DetermineIPFromHouseInfo(residenceNumber, neighborhood[0]);

                // Set the IP
                houseObjects[i].ipAddress = ip;
            }

            // Display the house's IP
            houseObjects[i].residenceText.text = houseObjects[i].ipAddress;
        }

        // Change the Lookup Agency names
        rootLookupAgencyLabel.text = "Root DNS Server";
        comLookupAgencyLabel.text = "COM Top-Level Domain DNS Server";
        netLookupAgencyLabel.text = "NET Top-Level Domain DNS Server";
        orgLookupAgencyLabel.text = "ORG Top-Level Domain DNS Server";
    }

    public static string DetermineIPFromHouseInfo(int houseNumber, char neighborhoodID)
    {
        int index = (char)neighborhoodID;

        // If in House 102, in group 1. If in House 203, in group 2, etc.
        int group = houseNumber / 100;

        // Piece together arbitrary IP
        string ip = "192." + index.ToString() + "." + group.ToString() + "." + houseNumber;

        return ip;
    }
}
