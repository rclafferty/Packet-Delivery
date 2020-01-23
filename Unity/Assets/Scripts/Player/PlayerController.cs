using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const float DEAD_VALUE = 0.4f;
    const float SPEED = 5f;
    Rigidbody2D thisRigidbody;

    bool activateSpeedModifier;
    float speedModifier;

    [SerializeField] Animator playerAnimator;

    string horizontalFloatName = "HorizontalValue";
    string verticalFloatName = "VerticalValue";

    enum Direction {
        Idle,
        Up,
        Down,
        Left,
        Right
    };

    Direction playerDirection;

    // Start is called before the first frame update
    void Start()
    {
        thisRigidbody = GetComponent<Rigidbody2D>();
        // transform.position = GameObject.Find("GameplayManager").GetComponent<GameplayManager>().CurrentSpawnLocation;

        activateSpeedModifier = false;
        speedModifier = 1;

        playerAnimator = gameObject.GetComponent<Animator>();

        playerDirection = Direction.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        float xMovement = Input.GetAxis("Horizontal");
        float yMovement = Input.GetAxis("Vertical");

#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftShift))
        {
            DebugActivateSpeedModifier(3f);
        }
        else
        {
            DebugActivateSpeedModifier(1f);
        }
#endif

        // Move
        thisRigidbody.velocity = new Vector2(xMovement * SPEED * speedModifier, yMovement * SPEED * speedModifier);

        if (thisRigidbody.velocity.y > 0.1f)
        {
            Debug.Log("Moving up");
        }

        Animate(thisRigidbody.velocity);
    }

    void Animate(Vector2 direction)
    {
        playerAnimator.SetFloat(horizontalFloatName, direction.x);
        playerAnimator.SetFloat(verticalFloatName, direction.y);
    }

#if UNITY_EDITOR
    public void DebugActivateSpeedModifier(float mod)
    {
        speedModifier = mod;
    }
#endif
}
