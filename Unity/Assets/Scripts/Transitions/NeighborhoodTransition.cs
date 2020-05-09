/* File: NeighborhoodTransition.cs
 * Author: Casey Lafferty
 * Project: Packet Delivery
 */

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NeighborhoodTransition : MonoBehaviour
{
    // Black fade image to use in transitioning
    [SerializeField] Image fadeImage;

    // Necessary player and component references
    PlayerController player;
    Rigidbody2D playerRigidbody;

    // In-game marker to transition towards
    [SerializeField] GameObject transitionMarker;

    // Flags to indicate which X and Y values will be affected
    [SerializeField] bool transitionX = true;
    [SerializeField] bool transitionY = true;
    
    public IEnumerator Fade(float start, float end, float timeToFade)
    {
        // Ensure transition image is enabled
        fadeImage.enabled = true;
        fadeImage.gameObject.SetActive(true);
        Color currentFadeColor = fadeImage.color;

        // Set starting alpha
        currentFadeColor.a = start;
        fadeImage.color = currentFadeColor;
        
        // Interpolate transition in/out over timeToFade seconds
        for (float currentFadeTime = 0.0f; currentFadeColor.a != end; currentFadeTime += Time.deltaTime)
        {
            // Calculate what the alpha should be at this point in the interpolation
            float fadeAlpha = Mathf.Lerp(start, end, currentFadeTime / timeToFade);

            // Set alpha in color
            currentFadeColor.a = fadeAlpha;
            fadeImage.color = currentFadeColor;

            // Wait for the frame to update
            yield return new WaitForEndOfFrame();
        }

        // If the image is transparent at the end
        if (end == 0.0f)
        {
            // Disable to avoid UI issues
            fadeImage.enabled = false;
        }
    }

    public IEnumerator Transition()
    {
        // Fade out to black
        yield return Fade(0, 1, 0.5f);

        // Calculate the new position
        Vector3 newPosition = playerRigidbody.position;
        Vector3 targetPosition = transitionMarker.transform.position;

        // If X will be affected
        if (transitionX)
        {
            // Set new X value
            newPosition.x = targetPosition.x;
        }

        // If Y will be affected
        if (transitionY)
        {
            // Set new Y value
            newPosition.y = targetPosition.y;
        }

        // Set the new position
        playerRigidbody.position = newPosition;

        // Fade in
        yield return Fade(1, 0, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the player and component references
        player = collision.GetComponent<PlayerController>();
        playerRigidbody = collision.GetComponent<Rigidbody2D>();

        // Start the transition
        StartCoroutine(Transition());
    }
}
