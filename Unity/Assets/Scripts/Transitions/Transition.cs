using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    [SerializeField] Image fadeImage;
    [SerializeField] bool fadeIn = true;
    [SerializeField] bool fadeOut = true;

    static readonly float FADE_DURATION = 0.25f;

    [SerializeField] HUDManager hudManager;
    [SerializeField] string ipAddress;

    // Start is called before the first frame update
    void Start()
    {
        if (fadeIn)
        {
            StartCoroutine(Fade(1, 0, FADE_DURATION));
        }
        else
        {
            Color c = fadeImage.color;
            c.a = 0.0f;
            fadeImage.color = c;

            fadeImage.enabled = false;
        }

        hudManager = GameObject.Find("HUD").GetComponent<HUDManager>();

        string[] houseParts = name.Split('-');
        if (houseParts.Length > 1)
        {
            int residenceNumber = System.Convert.ToInt32(houseParts[1].Trim());
            char neighborhoodID = houseParts[2].Trim()[0];
            ipAddress = AddressManager.DetermineIPFromHouseInfo(residenceNumber, neighborhoodID);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Only concerned with the player
        if (collision.name != "Player")
        {
            return;
        }
        
        string newScene = gameObject.name;
        bool isLowerCase = (newScene[0] >= 'a' && newScene[0] <= 'z');

        if (!isLowerCase)
        {
            bool isUpperCase = (newScene[0] >= 'A' && newScene[0] <= 'Z');

            if (isUpperCase)
            {
                char firstCharacter = newScene[0];
                int index = firstCharacter - 'A';
                firstCharacter = (char)('a' + index);

                newScene = firstCharacter + newScene.Substring(1);
            }
        }

        bool noOneHome = false;

        GameObject player = GameObject.Find("Player");

        GameplayManager gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        if (newScene.Contains("localLookup"))
        {
            string[] newSceneDetails = newScene.Split('-');
            newScene = "localLookupAgency";

            if (newSceneDetails.Length == 1)
            {
                Debug.Log("Not enough parts in the name");
            }
            else
            {
                gameplayManager.CurrentNeighborhoodID = newSceneDetails[1].Trim()[0]; // 1st character of trimmed 2nd part of the gameobject name
            }
        }
        else if (newScene.Contains("centralLookup"))
        {
            gameplayManager.CurrentNeighborhoodID = 'X';
        }
        else if (newScene.Contains("home"))
        {
            string[] newSceneDetails = newScene.Split('-');
            newScene = "home";

            if (gameplayManager.HasUpgrade("Exit the Matrix"))
            {
                if (gameplayManager.NextStep.nextStep == ipAddress)
                {
                    noOneHome = false;
                }
                else
                {
                    NoOneHome();
                    noOneHome = true;
                }
            }
            else if (newSceneDetails.Length == 1)
            {
                Debug.Log("Not enough parts in the name");
                NoOneHome();
                noOneHome = true;
            }
            else
            {
                gameplayManager.currentAddress = newSceneDetails[1].Trim();

                if (gameplayManager.CurrentMessage == null)
                {
                    NoOneHome();
                    noOneHome = true;
                }
                else if (System.Convert.ToInt32(gameplayManager.currentAddress) != gameplayManager.CurrentMessage.Recipient.HouseNumber)
                {
                    NoOneHome();
                    noOneHome = true;
                }
                else if (gameplayManager.CurrentNeighborhoodID != gameplayManager.CurrentMessage.Recipient.NeighborhoodID)
                {
                    NoOneHome();
                    noOneHome = true;
                }
                else
                {
                    Debug.Log("Entering " + gameplayManager.currentAddress + " -- " + gameplayManager.CurrentMessage.Recipient.HouseNumber + " " + gameplayManager.CurrentMessage.Recipient.Neighborhood + " -- Next: " + gameplayManager.NextDeliveryLocation);

                    noOneHome = false;
                }
            }
        }

        if (newScene != "town")
        {
            gameplayManager.indoorLocation = newScene;
            gameplayManager.lastOutdoorPosition = this.transform.position;  
        }

        if (!noOneHome)
        {
            Vector3 playerPosition = player.transform.position;
            playerPosition.y -= 1.0f;
            gameplayManager.CurrentSpawnLocation = playerPosition;

            StartCoroutine(TransitionToScene(0, 1, FADE_DURATION, newScene));
        }
    }

    void NoOneHome()
    {
        Debug.Log("Hmm... No one seems to be home right now.");
        hudManager.NoOneHome();
    }

    private IEnumerator TransitionToScene(float start, float end, float timeToFade, string newScene)
    {
        if (fadeOut)
        {
            yield return Fade(start, end, timeToFade);
        }

        LevelManager levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        levelManager.LoadLevel(newScene);
    }

    public IEnumerator Fade(float start, float end, float timeToFade)
    {
        fadeImage.enabled = true;
        fadeImage.gameObject.SetActive(true);
        fadeImage.transform.parent.gameObject.SetActive(true);

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

        if (end <= 0.01f)
        {
            fadeImage.enabled = false;
        }
    }

    public void FadeMethod(string scene)
    {
        StartCoroutine(TransitionToScene(0, 1, FADE_DURATION, scene));
    }
}
