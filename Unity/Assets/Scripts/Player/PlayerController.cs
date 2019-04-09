using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const float DEAD_VALUE = 0.4f;
    const float SPEED = 8f;
    Rigidbody2D thisRigidbody;

    // Start is called before the first frame update
    void Start()
    {

        thisRigidbody = GetComponent<Rigidbody2D>();
        // transform.position = GameObject.Find("GameplayManager").GetComponent<GameplayManager>().CurrentSpawnLocation;
    }

    // Update is called once per frame
    void Update()
    {
        float xMovement = Input.GetAxis("Horizontal");
        float yMovement = Input.GetAxis("Vertical");

        float speedModifier = 1;
        bool activateSpeedModifier = false;

#if UNITY_EDITOR
        // Debugging the game only
        activateSpeedModifier = Input.GetKey(KeyCode.LeftShift);
#endif

        if (activateSpeedModifier)
        {
#if UNITY_EDITOR
            speedModifier = 5;
#endif
        }

        // Move
        thisRigidbody.velocity = new Vector2(xMovement * SPEED * speedModifier, yMovement * SPEED * speedModifier);
    }
}
