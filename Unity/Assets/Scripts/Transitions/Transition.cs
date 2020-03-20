using Assets.Scripts.Behind_The_Scenes;
using Assets.Scripts.Lookup_Agencies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    [SerializeField] Image fadeImage;
    [SerializeField] bool fadeIn = true;
    [SerializeField] bool fadeOut = true;

    GameplayManager gameplayManager;
    CacheManager cacheManager;

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
        if (houseParts.Length > 2)
        {
            int residenceNumber = System.Convert.ToInt32(houseParts[1].Trim());
            char neighborhoodID = houseParts[2].Trim()[0];
            ipAddress = AddressManager.DetermineIPFromHouseInfo(residenceNumber, neighborhoodID);
        }
        
        // Find managers
        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        cacheManager = GameObject.Find("CacheManager").GetComponent<CacheManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    string ConvertToLowercase(in string text)
    {
        string stringToReturn = text;

        bool isUpperCase = (stringToReturn[0] >= 'A' && stringToReturn[0] <= 'Z');

        if (isUpperCase)
        {
            char firstCharacter = stringToReturn[0];
            int index = firstCharacter - 'A';
            firstCharacter = (char)('a' + index);

            stringToReturn = firstCharacter + stringToReturn.Substring(1);
        }

        return stringToReturn;
    }

    void LookupAgencyTransition(string sceneName, out string newSceneName)
    {
        newSceneName = "";

        if (sceneName.Contains("localLookup"))
        {
            string[] newSceneDetails = sceneName.Split('-');
            newSceneName = "localLookupAgency";

            if (newSceneDetails.Length == 1)
            {
                Debug.Log("Not enough parts in the name");
            }
            else
            {
                gameplayManager.CurrentNeighborhoodID = newSceneDetails[1].Trim()[0]; // 1st character of trimmed 2nd part of the gameobject name
            }
        }
        else
        {
            gameplayManager.CurrentNeighborhoodID = 'X';
            newSceneName = sceneName;
        }
    }

    bool IsCorrectHouse(string[] details, Letter message, bool hasExitedMatrix)
    {
        if (hasExitedMatrix)
        {
            if (ipAddress == message.Recipient.URL)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (System.Convert.ToInt32(details[1]) == message.Recipient.HouseNumber)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    bool HasCompletedAllSteps(Letter message, bool hasExitedMatrix)
    {
        if (hasExitedMatrix)
        {
            if (gameplayManager.NextStep.nextStep == ipAddress)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (gameplayManager.NextStep.nextStep == "Residence #" + message.Recipient.HouseNumber)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    bool HomeTransition(string thisSceneName)
    {
        string[] newSceneDetails = thisSceneName.Split('-');
        if (newSceneDetails.Length < 3)
            return false;

        int houseNumber = System.Convert.ToInt32(newSceneDetails[1]);
        char neighborhoodID = newSceneDetails[2].Trim()[0];

        bool hasExitedTheMatrix = gameplayManager.HasUpgrade("Exit the Matrix");

        Letter message = gameplayManager.CurrentMessage;

        // If letter is null
        if (message == null)
        {
            return false; // No one is home
        }
        // Else if house is correct
        else if (IsCorrectHouse(newSceneDetails, message, hasExitedTheMatrix))
        {
            // If recipient is cached
            if (cacheManager.IsPersonCached(message.Recipient))
            {
                return true;
            }
            // Else if in right spot (gone through all steps)
            else if (HasCompletedAllSteps(message, hasExitedTheMatrix))
            {
                return true;
            }
            // Missing some steps
            else
            {
                return false; // No one is home
            }
        }
        // Error in location
        else
        {
            return false; // No one is home
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Only concerned with the player
        if (collision.name != "Player")
        {
            return;
        }
        
        // Find player
        GameObject player = GameObject.Find("Player");

        // Get scene name
        string thisSceneName = ConvertToLowercase(gameObject.name);
        string nextScene = thisSceneName;

        // By default, flag as correct house
        bool isSuccessful = false;

        if (thisSceneName != "town" && thisSceneName != "office")
        {
            if (thisSceneName.Contains("Lookup"))
            {
                LookupAgencyTransition(thisSceneName, out nextScene);
                isSuccessful = true;
            }
            else if (thisSceneName.Contains("home"))
            {
                isSuccessful = HomeTransition(thisSceneName);
                nextScene = "home";
            }

            gameplayManager.indoorLocation = nextScene;
            gameplayManager.lastOutdoorPosition = this.transform.position;
        }
        else
        {
            isSuccessful = true;
        }

        if (thisSceneName != "town")
        {
            gameplayManager.indoorLocation = nextScene;
            gameplayManager.lastOutdoorPosition = this.transform.position;
        }

        if (isSuccessful)
        {
            Vector3 playerPosition = player.transform.position;
            playerPosition.y -= 1.0f;
            gameplayManager.CurrentSpawnLocation = playerPosition;

            StartCoroutine(TransitionToScene(0, 1, FADE_DURATION, nextScene));
        }
        else
        {
            NoOneHome();
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
