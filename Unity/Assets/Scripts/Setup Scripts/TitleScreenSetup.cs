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
        GameObject.Find("LevelManager").GetComponent<LevelManager>().LoadLevel("town");
    }

    public void QuitButtonAction()
    {
        GameObject.Find("LevelManager").GetComponent<LevelManager>().QuitGame();
    }
}
