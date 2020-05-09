/* File: StartSceneLoader.cs
 * Author: Casey Lafferty
 * Project: Packet Delivery
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneLoader : MonoBehaviour
{
    // The NPC the player is talking to
    [SerializeField] GameObject startNPC;

    // NPC and player sprites
    public Sprite sprite_PlayerRight;
    public Sprite sprite_PlayerDown;
    public Sprite sprite_NPCLeft;
    
    public void OnSceneWasLoaded(Scene scene, LoadSceneMode lsm)
    {
        // If the current scene is the starting dialogue
        if (scene.name == "start_town")
        {
            // Find the player and set the sprite
            GameObject player = GameObject.Find("Player");
            player.GetComponent<SpriteRenderer>().sprite = sprite_PlayerRight;

            // Find the NPC and set the sprite
            startNPC = GameObject.Find("Starting NPC");
            startNPC.GetComponent<SpriteRenderer>().sprite = sprite_NPCLeft;

            // Set the player sprite for end of dialogue
            StartingNPCManager npcManager = startNPC.GetComponent<StartingNPCManager>();
            npcManager.sprite_PlayerDown = sprite_PlayerDown;

            // Remove this method from the scene change events
            SceneManager.sceneLoaded -= OnSceneWasLoaded;
        }
    }
}
