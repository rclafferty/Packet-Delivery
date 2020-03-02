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
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Fade(1, 0, 0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Fade(float start, float end, float timeToFade)
    {
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

            Debug.Log("Fade Transitioning");
        }
    }

    public IEnumerator Transition()
    {
        yield return Fade(0, 1, 0.5f);

        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        playerRigidbody.position = transitionMarker.transform.position;

        yield return Fade(1, 0, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.GetComponent<PlayerController>();
        playerRigidbody = collision.GetComponent<Rigidbody2D>();

        StartCoroutine(Transition());
    }
}
