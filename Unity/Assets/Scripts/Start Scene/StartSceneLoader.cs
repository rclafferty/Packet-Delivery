using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneLoader : MonoBehaviour
{
    GameObject startNPC;

    // Start is called before the first frame update
    void Start()
    {
        // Load Prefabs
        startNPC = Resources.Load<GameObject>("Prefabs/StartingNPC");

        // Instantiate prefabs

        // Set a OnSceneWasLoaded event
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSceneWasLoaded()
    {
        StartingNPCManager npcManager = startNPC.GetComponent<StartingNPCManager>();
        npcManager.MoveNPC(true);
        npcManager.StartDialogue();
        npcManager.MoveNPC(false);
    }
}
