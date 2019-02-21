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

    // Start is called before the first frame update
    void Start()
    {
        thisRigidbody = GetComponent<Rigidbody2D>();
        thisSpriteRenderer = GetComponent<SpriteRenderer>();

        // Load in sprites
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Player");

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
            // thisSpriteRenderer.sprite
        }
    }
}
