using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenSetup : MonoBehaviour
{
    [SerializeField]
    Button startButton;

    [SerializeField]
    Button quitButton;

    // Start is called before the first frame update
    void Start()
    {
        // Button startButton = GameObject.Find("StartButton").GetComponent<Button>();
        startButton.onClick.AddListener(StartButtonAction);

        // Button quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        quitButton.onClick.AddListener(QuitButtonAction);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButtonAction()
    {
        // GameObject.Find("LevelManager").GetComponent<LevelManager>().LoadLevel("town");
        GameObject.Find("LevelManager").GetComponent<LevelManager>().LoadLevel("start_town");
    }

    /*public void StartButtonAction()
    {
        const float WAIT_SECONDS = 3.0f;
        Image backgroundImage = GameObject.Find("Background Image").GetComponent<Image>();
        Color backgroundColor = backgroundImage.color;

        startButton.enabled = false;

        // Load level
        StartCoroutine(FadeToNewLevel("town"));
    }

    IEnumerator FadeToNewLevel(string level)
    {
        Image backgroundImage = GameObject.Find("Background Image").GetComponent<Image>();
        Color backgroundColor = backgroundImage.color;

        const float WAIT_SECONDS = 3.0f;
        float timePassed = 0.0f;
        const float DELAY = 0.01f;

        while (timePassed < WAIT_SECONDS)
        {
            yield return new WaitForSeconds(DELAY);
            timePassed += DELAY;
            backgroundColor.a = (1 - (timePassed / WAIT_SECONDS));
            Update();
        }

        GameObject.Find("LevelManager").GetComponent<LevelManager>().LoadLevel(level);
    }*/

    public void QuitButtonAction()
    {
        GameObject.Find("LevelManager").GetComponent<LevelManager>().QuitGame();
    }
}
