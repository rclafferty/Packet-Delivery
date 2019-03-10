using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenSetup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button start = GameObject.Find("StartButton").GetComponent<Button>();
        start.onClick.AddListener(StartButtonAction);

        Button quit = GameObject.Find("QuitButton").GetComponent<Button>();
        quit.onClick.AddListener(QuitButtonAction);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartButtonAction()
    {
        GameObject.Find("LevelManager").GetComponent<LevelManager>().LoadLevel("town");
    }

    void QuitButtonAction()
    {
        GameObject.Find("LevelManager").GetComponent<LevelManager>().QuitGame();
    }
}
