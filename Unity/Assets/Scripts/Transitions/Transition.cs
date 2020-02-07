using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Only concerned with the player
        if (collision.name != "Player")
        {
            return;
        }

        LevelManager levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        
        string newScene = gameObject.name;
        bool isLowerCase = (newScene[0] >= 'a' && newScene[0] <= 'z');

        if (!isLowerCase)
        {
            bool isUpperCase = (newScene[0] >= 'A' && newScene[0] <= 'Z');

            if (isUpperCase)
            {
                char firstCharacter = newScene[0];
                int index = firstCharacter - 'A';
                firstCharacter = (char)('a' + index);

                newScene = firstCharacter + newScene.Substring(1);
            }
        }

        GameObject player = GameObject.Find("Player");

        GameplayManager gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        if (newScene.Contains("home"))
        {
            string[] newSceneDetails = newScene.Split('-');
            newScene = "home";

            if (newSceneDetails.Length == 1)
            {
                Debug.Log("Not enough parts in the name");
            }
            else
            {
                gameplayManager.currentAddress = newSceneDetails[1].Trim();
                Debug.Log("Entering " + gameplayManager.currentAddress + " -- " + gameplayManager.GetLetterAddress() + " -- Next: " + gameplayManager.NextDeliveryLocation);
            }
        }

        if (newScene != "town")
        {
            gameplayManager.indoorLocation = newScene;
            gameplayManager.lastOutdoorPosition = this.transform.position;  
        }

        Vector3 playerPosition = player.transform.position;
        playerPosition.y -= 1.0f;
        gameplayManager.CurrentSpawnLocation = playerPosition;

        levelManager.LoadLevel(newScene);
    }
}
