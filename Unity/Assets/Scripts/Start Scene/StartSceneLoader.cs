using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneLoader : MonoBehaviour
{
    [SerializeField]
    GameObject startNPC;

    public Sprite sprite_PlayerRight;
    public Sprite sprite_PlayerDown;
    public Sprite sprite_NPCLeft;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSceneWasLoaded(Scene scene, LoadSceneMode lsm)
    {
        if (scene.name == "start_town")
        {
            GameObject player = GameObject.Find("Player");
            player.GetComponent<SpriteRenderer>().sprite = sprite_PlayerRight;

            startNPC = GameObject.Find("Starting NPC");
            StartingNPCManager npcManager = startNPC.GetComponent<StartingNPCManager>();
            startNPC.GetComponent<SpriteRenderer>().sprite = sprite_NPCLeft;
            npcManager.sprite_PlayerDown = sprite_PlayerDown;

            // Temporarily disabled: NPC enters, talks, exits
            // npcManager.MoveNPC(true);
            // npcManager.StartDialogue();
            // npcManager.MoveNPC(false);

            SceneManager.sceneLoaded -= OnSceneWasLoaded;
        }
    }
}
