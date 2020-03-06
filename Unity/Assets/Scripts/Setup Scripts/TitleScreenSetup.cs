using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenSetup : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button quitButton;

    // Start is called before the first frame update
    void Start()
    {
        //startButton.onClick.AddListener(StartButtonAction);
        quitButton.onClick.AddListener(QuitButtonAction);
    }

    public void StartButtonAction()
    {
        GameObject.Find("LevelManager").GetComponent<LevelManager>().LoadLevel("start_town");
    }

    public void QuitButtonAction()
    {
        GameObject.Find("LevelManager").GetComponent<LevelManager>().QuitGame();
    }
}
