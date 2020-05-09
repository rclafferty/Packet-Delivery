using UnityEngine;

public class TitleScreenSetup : MonoBehaviour
{
    // Necessary manager references
    LevelManager levelManager;

    // Necessary UI components
    [SerializeField] GameObject optionsCanvas;
    [SerializeField] OptionsMenu optionsMenu;
    [SerializeField] GameObject[] startScreenButtons;

    // Start is called before the first frame update
    void Start()
    {
        // Find managers in scene
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        optionsCanvas.SetActive(false);
    }

    public void StartButtonAction()
    {
        // Move on to the starting dialogue scene
        levelManager.LoadLevel("start_town");
    }

    public void QuitButtonAction()
    {
        // Quit the game via the level manager
        levelManager.QuitGame();
    }

    public void ToggleOptions(bool isOptionsShown)
    {
        optionsCanvas.SetActive(isOptionsShown);

        foreach (GameObject g in startScreenButtons)
        {
            g.SetActive(!isOptionsShown);
        }

        // optionsCanvas.GetComponent<OptionsMenu>().ToggleMenuItems(isOptionsShown);
    }
}
