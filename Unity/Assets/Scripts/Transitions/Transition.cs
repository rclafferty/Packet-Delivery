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
        if (collision.name == "Player")
        {
            LevelManager lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();

            string newScene = gameObject.name;
            bool isLowerCase = (newScene[0] >= 'a' && newScene[0] <= 'z');

            if (!isLowerCase)
            {
                bool isUpperCase = (newScene[0] >= 'A' && newScene[0] <= 'Z');

                if (isUpperCase)
                {
                    char c = newScene[0];
                    int index = c - 'A';
                    c = (char)('a' + index);

                    newScene = c + newScene.Substring(1);
                }
            }

            GameObject player = GameObject.Find("Player");

            GameplayManager gm = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
            if (newScene != "town")
            {
                gm.indoorLocation = newScene;
            }

            Vector3 pos = player.transform.position;
            pos.y -= 1.0f;
            gm.CurrentSpawnLocation = pos;

            lm.LoadLevel(newScene);
        }
    }
}
