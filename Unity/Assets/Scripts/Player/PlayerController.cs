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

        // Animate
        /* if (yMovement > DEAD_VALUE)
        {
            Animate(Direction.Up);
        }
        else if (yMovement < 0 - DEAD_VALUE)
        {
            Animate(Direction.Down);
        }
        else if (xMovement < 0 - DEAD_VALUE)
        {
            Animate(Direction.Left);
        }
        else if (xMovement > DEAD_VALUE)
        {
            Animate(Direction.Right);
        }
        else
        {
            Animate(Direction.Idle);
        } */
    }

    void Animate(Vector2 direction)
    {
        playerAnimator.SetFloat(horizontalFloatName, direction.x);
        playerAnimator.SetFloat(verticalFloatName, direction.y);
    }

    void Animate(Direction direction)
    {
        if (direction == playerDirection)
            return;

        playerDirection = direction;
        if (playerDirection == Direction.Idle)
        {
            // Set all release triggers
            playerAnimator.SetTrigger("UpReleaseTrigger");
            playerAnimator.SetTrigger("DownReleaseTrigger");
            playerAnimator.SetTrigger("LeftReleaseTrigger");
            playerAnimator.SetTrigger("RightReleaseTrigger");
        }
        else
        {
            // Reset all other triggers
            playerAnimator.ResetTrigger("UpTrigger");
            playerAnimator.ResetTrigger("DownTrigger");
            playerAnimator.ResetTrigger("LeftTrigger");
            playerAnimator.ResetTrigger("RightTrigger");

            if (playerDirection == Direction.Up)
            {
                // Set up trigger
                playerAnimator.SetTrigger("UpTrigger");
            }
            else if (playerDirection == Direction.Down)
            {
                // Set down trigger
                playerAnimator.SetTrigger("DownTrigger");
            }
            else if (playerDirection == Direction.Left)
            {
                // Set left trigger
                playerAnimator.SetTrigger("LeftTrigger");
            }
            else if (playerDirection == Direction.Right)
            {
                // Set right trigger
                playerAnimator.SetTrigger("RightTrigger");
            }

            // Reset release triggers
            playerAnimator.ResetTrigger("UpReleaseTrigger");
            playerAnimator.ResetTrigger("DownReleaseTrigger");
            playerAnimator.ResetTrigger("LeftReleaseTrigger");
            playerAnimator.ResetTrigger("RightReleaseTrigger");
        }
    }

#if UNITY_EDITOR
    public void DebugActivateSpeedModifier(float mod)
    {
        speedModifier = mod;
    }
#endif
}
