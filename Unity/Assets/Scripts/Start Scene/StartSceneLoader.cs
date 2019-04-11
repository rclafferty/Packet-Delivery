using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneLoader : MonoBehaviour
{
    [SerializeField]
    GameObject startNPC;

    // Start is called before the first frame update
    void Start()
    {
        // Load Prefabs
        // startNPC = Resources.Load<GameObject>("Prefabs/StartingNPC");
        // startNPC = GameObject.Find("Starting NPC");

        // Instantiate prefabs

        // Set a OnSceneWasLoaded event
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSceneWasLoaded(Scene scene, LoadSceneMode lsm)
    {
        if (scene.name == "start_town")
        {
            startNPC = GameObject.Find("Starting NPC");
            StartingNPCManager npcManager = startNPC.GetComponent<StartingNPCManager>();
            npcManager.MoveNPC(true);
            npcManager.StartDialogue();
            npcManager.MoveNPC(false);

            SceneManager.sceneLoaded -= OnSceneWasLoaded;
        }
    }
}
