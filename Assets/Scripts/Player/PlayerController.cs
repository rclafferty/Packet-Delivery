/* File: PlayerController.cs
 * Author: Casey Lafferty
 * Project: Packet Delivery
 */

 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Button press threshold
    const float DEAD_VALUE = 0.4f;

    // Walking speed
    public static readonly float SPEED = 5f;

    GameplayManager gameplayManager;

    // Reference to the rigidbody (for physics and movement)
    Rigidbody2D thisRigidbody;

    // Flag indicating if the player is "running"
    bool activateSpeedModifier;

    // Speed modifier to allow running faster
    float speedModifier;

    // Reference to the animator (for walking animation)
    [SerializeField] Animator playerAnimator;

    // Animator parameter values for assisting in walking animations
    string horizontalFloatName = "HorizontalValue";
    string verticalFloatName = "VerticalValue";

    // Object and image that show the whole map
    [SerializeField] GameObject bigMap;
    
    // Mobile-specific variables
    Vector2 touchOrigin = -Vector2.one;

    public static readonly string MAP_KEY = "TAB";

    // Start is called before the first frame update
    void Start()
    {
        // Find necessary managers
        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();

        // Get necessary component values
        thisRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        // By default, walk normal
        activateSpeedModifier = false;
        speedModifier = 1;
        
        // Allow walking (can be set to false to freeze walking)
        IsWalkingEnabled = true;

        // Disable town map (if in town scene)
        if (bigMap != null)
            bigMap.SetActive(false);
    }
    
    void Update()
    {
        float xMovement = 0;
        float yMovement = 0;

        // If walking is allowed
        if (IsWalkingEnabled)
        {
            // If the application is an executable for PC/Mac/Linux or WebGL or in-editor
#if UNITY_STANDALONE || UNITY_WEBGL || UNITY_EDITOR
            // Get the movement from the Horizontal/Vertical axis
            xMovement = Input.GetAxisRaw("Horizontal");
            yMovement = Input.GetAxisRaw("Vertical");

            // If the application is on mobile/tablet
#else
            // If the player is touching the screen
            if (Input.touchCount > 0)
            {
                // Get the first touch
                Touch myTouch = Input.touches[0];

                // If the phase is "Begin" -- player put finger down
                if (myTouch.phase == TouchPhase.Began)
                {
                    // Get starting position
                    touchOrigin = myTouch.position;
                }
                // If the phase is "Ended" -- player took finger off
                else if (myTouch.phase == TouchPhase.Ended)
                {
                    // Reset position
                    touchOrigin = -Vector2.one;
                }
                // If the player has moved their finger or is stationary
                else if (myTouch.phase == TouchPhase.Moved || myTouch.phase == TouchPhase.Stationary)
                {
                    // Inside the bounds of the screen
                    Vector2 touchEnd = myTouch.position;
                    float x = touchEnd.x - touchOrigin.x;
                    float y = touchEnd.y - touchOrigin.y;
                
                    // Determine if movement was in the x direction
                    if (Mathf.Abs(x) > Mathf.Abs(y))
                    {
                        // Movement was right
                        if (x > 0)
                        {
                            xMovement = 1;
                        }
                        // Movement was left
                        else
                        {
                            xMovement = -1;
                        }
                    }
                    // Movement was in the y direction
                    else
                    {
                        // Movement was up
                        if (y > 0)
                        {
                            yMovement = 1;
                        }
                        // Movement was down
                        else
                        {
                            yMovement = -1;
                        }
                    }
                }
            }
#endif

            // If the running button(s) is/are pressed
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                // If the player has the company running shoes upgrade
                if (gameplayManager.HasUpgrade("Company Running Shoes"))
                {
                    // Running speed
                    speedModifier = 3;
                }
            }
            // Shift is not pressed
            else
            {
                // Normal walking speed
                speedModifier = 1;
            }

            // Calculate velocity in each direction
            float xVelocity = xMovement * SPEED * speedModifier;
            float yVelocity = yMovement * SPEED * speedModifier;
            
            // Move in the calculated direction
            thisRigidbody.velocity = new Vector2(xVelocity, yVelocity);

            // Animate based on calculated velocity
            Animate(thisRigidbody.velocity);

            // If the map button is pressed
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                // If town map reference is valid
                if (bigMap != null)
                {
                    // Toggle the town map
                    bigMap.SetActive(!bigMap.activeInHierarchy);
                }
            }
        }
        else
        {
            // No animation -- stand still
            Animate(Vector2.zero);
        }
    }

    public void Animate(Vector2 direction)
    {
        // Set animation parameters on the Animator
        playerAnimator.SetFloat(horizontalFloatName, direction.x);
        playerAnimator.SetFloat(verticalFloatName, direction.y);
    }
    
    // Flag to determine if walking is allowed
    public bool IsWalkingEnabled { get; set; }
}
