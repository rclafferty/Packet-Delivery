using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const float DEAD_VALUE = 0.4f;
    const float SPEED = 8f;
    Rigidbody2D thisRigidbody;
    SpriteRenderer thisSpriteRenderer;

    Sprite[] characterSprites_Up;
    Sprite[] characterSprites_Left;
    Sprite[] characterSprites_Down;
    Sprite[] characterSprites_Right;

    const int IDLE_INDEX = 1;

    int upMovementIndex;
    int leftMovementIndex;
    int downMovementIndex;
    int rightMovementIndex;

    bool isMovingUp;
    bool isMovingLeft;
    bool isMovingDown;
    bool isMovingRight;

    float transitionTimer;
    const float TRANSITION_TIMER_MAX = 2f;

    // Start is called before the first frame update
    void Start()
    {
        thisRigidbody = GetComponent<Rigidbody2D>();
        thisSpriteRenderer = GetComponent<SpriteRenderer>();

        characterSprites_Up = new Sprite[3];
        characterSprites_Left = new Sprite[3];
        characterSprites_Down = new Sprite[3];
        characterSprites_Right = new Sprite[3];

        // Load in sprites
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Player");
        Debug.Log("Loaded Sprites ? " + (sprites != null));

        int spritesIndex = 0;
        
        // Up animation sprites
        for (int index = 0; index < 3; index++, spritesIndex++)
        {
            characterSprites_Up[index] = sprites[spritesIndex];
        }

        // Left animation sprites
        for (int index = 0; index < 3; index++, spritesIndex++)
        {
            characterSprites_Left[index] = sprites[spritesIndex];
        }

        // Down animation sprites
        for (int index = 0; index < 3; index++, spritesIndex++)
        {
            characterSprites_Down[index] = sprites[spritesIndex];
        }

        // Right animation sprites
        for (int index = 0; index < 3; index++, spritesIndex++)
        {
            characterSprites_Right[index] = sprites[spritesIndex];
        }

        ResetDirections();
    }

    // Update is called once per frame
    void Update()
    {
        float xMovement = Input.GetAxis("Horizontal");
        float yMovement = Input.GetAxis("Vertical");

        // Move
        thisRigidbody.velocity = new Vector2(xMovement * SPEED, yMovement * SPEED);

        if (xMovement > DEAD_VALUE)
        {
            if (!isMovingRight)
            {
                ResetDirections();
                isMovingRight = true;
            }

            if (transitionTimer > 0)
            {
                TransitionCountdown();
                return;
            }

            thisSpriteRenderer.sprite = characterSprites_Right[rightMovementIndex++];

            if (rightMovementIndex == characterSprites_Right.Length)
            {
                rightMovementIndex = 0;
            }
        }
        else if (xMovement < 0 - DEAD_VALUE)
        {
            if (!isMovingLeft)
            {
                ResetDirections();
                isMovingLeft = true;
            }

            if (transitionTimer > 0)
            {
                TransitionCountdown();
                return;
            }

            thisSpriteRenderer.sprite = characterSprites_Left[leftMovementIndex++];

            if (leftMovementIndex == characterSprites_Left.Length)
            {
                leftMovementIndex = 0;
            }
        }
        else if (yMovement > DEAD_VALUE)
        {
            if (!isMovingUp)
            {
                ResetDirections();
                isMovingUp = true;
            }

            if (transitionTimer > 0)
            {
                TransitionCountdown();
                return;
            }

            thisSpriteRenderer.sprite = characterSprites_Up[upMovementIndex++];

            if (upMovementIndex == characterSprites_Up.Length)
            {
                upMovementIndex = 0;
            }
        }
        else if (yMovement < 0 - DEAD_VALUE)
        {
            if (!isMovingDown)
            {
                ResetDirections();
                isMovingDown = true;
            }

            if (transitionTimer > 0)
            {
                TransitionCountdown();
                return;
            }

            thisSpriteRenderer.sprite = characterSprites_Down[downMovementIndex++];

            if (downMovementIndex == characterSprites_Down.Length)
            {
                downMovementIndex = 0;
            }
        }
        else
        {
            // Find the current direction and switch to idle sprite
            if (isMovingUp)
            {
                thisSpriteRenderer.sprite = characterSprites_Up[IDLE_INDEX];
            }
            else if (isMovingLeft)
            {
                thisSpriteRenderer.sprite = characterSprites_Left[IDLE_INDEX];
            }
            else if (isMovingRight)
            {
                thisSpriteRenderer.sprite = characterSprites_Right[IDLE_INDEX];
            }
            else // if (isMovingDown) or not moving at all
            {
                thisSpriteRenderer.sprite = characterSprites_Down[IDLE_INDEX];
            }
        }
    }

    void ResetDirections()
    {
        isMovingUp = false;
        isMovingLeft = false;
        isMovingDown = false;
        isMovingRight = false;

        upMovementIndex = 0;
        downMovementIndex = 0;
        leftMovementIndex = 0;
        rightMovementIndex = 0;

        transitionTimer = -1;
    }

    void TransitionCountdown()
    {
        transitionTimer -= Time.deltaTime;
    }
}
