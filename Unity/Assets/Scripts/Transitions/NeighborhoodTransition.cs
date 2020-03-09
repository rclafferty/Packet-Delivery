using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NeighborhoodTransition : MonoBehaviour
{
    [SerializeField] Image fadeImage;

    PlayerController player;
    Rigidbody2D playerRigidbody;
    [SerializeField] GameObject transitionMarker;

    [SerializeField] bool transitionX = true;
    [SerializeField] bool transitionY = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Fade(float start, float end, float timeToFade)
    {
        fadeImage.enabled = true;
        fadeImage.gameObject.SetActive(true);
        Color currentFadeColor = fadeImage.color;

        // Set starting alpha
        currentFadeColor.a = start;
        fadeImage.color = currentFadeColor;
        
        for (float currentFadeTime = 0.0f; currentFadeColor.a != end; currentFadeTime += Time.deltaTime)
        {
            float fadeAlpha = Mathf.Lerp(start, end, currentFadeTime / timeToFade);
            currentFadeColor.a = fadeAlpha;
            fadeImage.color = currentFadeColor;

            yield return new WaitForEndOfFrame();
        }

        if (end == 0.0f)
        {
            fadeImage.enabled = false;
        }
    }

    public IEnumerator Transition()
    {
        yield return Fade(0, 1, 0.5f);

        Vector3 newPosition = playerRigidbody.position;
        Vector3 targetPosition = transitionMarker.transform.position;
        if (transitionX)
        {
            newPosition.x = targetPosition.x;
        }
        if (transitionY)
        {
            newPosition.y = targetPosition.y;
        }

        playerRigidbody.position = newPosition;

        yield return Fade(1, 0, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.GetComponent<PlayerController>();
        playerRigidbody = collision.GetComponent<Rigidbody2D>();

        StartCoroutine(Transition());
    }
}
