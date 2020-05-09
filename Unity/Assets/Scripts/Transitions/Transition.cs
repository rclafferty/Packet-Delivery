using Assets.Scripts.Behind_The_Scenes;
using Assets.Scripts.Lookup_Agencies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    // Black overlay image to use in fading in/out
    [SerializeField] Image fadeImage;

    // Flags to know if the scene needs to fade in and out -- both true by default
    [SerializeField] bool fadeIn = true;
    [SerializeField] bool fadeOut = true;

    // Manager references
    GameplayManager gameplayManager;
    CacheManager cacheManager;
    [SerializeField] HUDManager hudManager;
    LevelManager levelManager;

    // Time to complete the fade
    static readonly float FADE_DURATION = 0.25f;

    // This residence's IP address (if applicable)
    [SerializeField] string ipAddress;
    
    void Start()
    {
        // If the scene is flagged to fade in
        if (fadeIn)
        {
            // Fade in
            StartCoroutine(Fade(1, 0, FADE_DURATION));
        }
        // If the scene is not flagged to fade in
        else
        {
            // Set fade image as transparent
            Color c = fadeImage.color;
            c.a = 0.0f;
            fadeImage.color = c;

            // Disable the fade image to avoid any interaction issues
            fadeImage.enabled = false;
        }
        
        string[] houseParts = name.Split('-');

        // If this is a transition to a residence
        if (houseParts.Length > 2)
        {
            // Determine IP address from name of transition
            int residenceNumber = System.Convert.ToInt32(houseParts[1].Trim());
            char neighborhoodID = houseParts[2].Trim()[0];
            ipAddress = AddressManager.DetermineIPFromHouseInfo(residenceNumber, neighborhoodID);
        }
        
        // Find managers
        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        cacheManager = GameObject.Find("CacheManager").GetComponent<CacheManager>();
        hudManager = GameObject.Find("HUD").GetComponent<HUDManager>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }
    
    string FormatSceneName(in string text)
    {
        string stringToReturn = text;

        // Check if the name needs to be changed
        bool isUpperCase = (stringToReturn[0] >= 'A' && stringToReturn[0] <= 'Z');

        // If the name starts with an uppercase letter
        if (isUpperCase)
        {
            // Adjust the first character
            char firstCharacter = stringToReturn[0];
            int index = firstCharacter - 'A';
            firstCharacter = (char)('a' + index);
            
            stringToReturn = firstCharacter + stringToReturn.Substring(1);
        }

        // Return the (potentially) adjusted string
        return stringToReturn;
    }

    void LookupAgencyTransition(string sceneName, out string newSceneName)
    {
        newSceneName = "";

        // If the player is going to a local lookup agency
        if (sceneName.Contains("localLookup"))
        {
            // Get the scene details
            string[] newSceneDetails = sceneName.Split('-');
            newSceneName = "localLookupAgency";

            // Check if there's enough details in the name
            if (newSceneDetails.Length == 1)
            {
                // Output error
                Debug.Log("Not enough parts in the name");
            }
            // If there are enough details in the name
            else
            {
                // Get the neighborhood ID from the name
                string neighborhoodIDString = newSceneDetails[1].Trim(); // 2nd part of the name
                char neighborhoodIDChar = neighborhoodIDString[0]; // 1st character

                // Store the current neighborhood id
                gameplayManager.CurrentNeighborhoodID = neighborhoodIDChar;
            }
        }
        // Player is going to a central lookup agency
        else
        {
            // Set the neighborhood ID to root village
            gameplayManager.CurrentNeighborhoodID = 'X';
            newSceneName = sceneName;
        }
    }

    bool IsCorrectHouse(string[] details, Letter message, bool hasExitedMatrix)
    {
        // If the player has purchased the Exit the Matrix upgrade
        if (hasExitedMatrix)
        {
            // Reference the residence IP address
            string residentIP = AddressManager.DetermineIPFromHouseInfo(message.Recipient.HouseNumber, message.Recipient.NeighborhoodID);

            // If the ip addresses match, then it's the correct house. Else, it's not.
            return ipAddress == residentIP;
        }
        else
        {
            // Reference the residence house number
            int currentHouseNumber = System.Convert.ToInt32(details[1]);

            // If the house numbers match, then it's the correct house. Else, it's not
            return currentHouseNumber == message.Recipient.HouseNumber;
        }
    }

    bool HasCompletedAllPrerequisiteSteps(Letter message, bool hasExitedMatrix)
    {
        // If the player has purchased the "Exit the Matrix" upgrade
        if (hasExitedMatrix)
        {
            // If the player's next step is the current IP address, then they can enter. Else, there's more steps
            return gameplayManager.NextStep.nextStep == ipAddress;
        }
        else
        {
            // If the player's next step is the current house number, then they can enter. Else, there's more steps
            return gameplayManager.NextStep.nextStep == "Residence #" + message.Recipient.HouseNumber;
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
            // No one is home
            return false; 
        }
        // Else if house is correct
        else if (IsCorrectHouse(newSceneDetails, message, hasExitedTheMatrix))
        {
            // If recipient is cached
            if (cacheManager.IsPersonCached(message.Recipient))
            {
                // Go to the residence
                return true;
            }
            // Else if in right spot (gone through all steps)
            else if (HasCompletedAllPrerequisiteSteps(message, hasExitedTheMatrix))
            {
                // Go to the residence
                return true;
            }
            // Missing some steps
            else
            {
                // No one is home
                return false;
            }
        }
        // Error in location
        else
        {
            // No one is home
            return false;
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
        string thisSceneName = FormatSceneName(gameObject.name);
        string nextScene = thisSceneName;

        // By default, flag as correct house
        bool isSuccessful = false;

        // Check if the player is at a lookup agency or residence
        bool isLookupAgencyOrResidence = thisSceneName != "town" && thisSceneName != "office";

        // If they are at one of those locations
        if (isLookupAgencyOrResidence)
        {
            // If it's a lookup agency
            if (thisSceneName.Contains("Lookup"))
            {
                // Transition to the respective lookup agency
                LookupAgencyTransition(thisSceneName, out nextScene);

                // Successful transition
                isSuccessful = true;
            }
            // If it's a residence
            else if (thisSceneName.Contains("home"))
            {
                // Transition to the residence
                isSuccessful = HomeTransition(thisSceneName);
                nextScene = "home";
            }

            // Store the indoor location for reference
            gameplayManager.indoorLocation = nextScene;

            // Store the current outdoor location for spawning in the town
            gameplayManager.lastOutdoorPosition = this.transform.position;
        }
        else
        {
            // Town and office don't need special formatting
            isSuccessful = true;
        }

        if (thisSceneName != "town")
        {

            // Store the indoor location for reference
            gameplayManager.indoorLocation = nextScene;

            // Store the current outdoor location for spawning in the town
            gameplayManager.lastOutdoorPosition = this.transform.position;
        }

        // If it was a successful transition
        if (isSuccessful)
        {
            // Adjust the player's position to avoid triggers
            Vector3 playerPosition = player.transform.position;
            playerPosition.y -= 1.0f;

            // Store the adjusted position
            gameplayManager.CurrentSpawnLocation = playerPosition;

            // Fade out and transition
            StartCoroutine(TransitionToScene(0, 1, FADE_DURATION, nextScene));
        }
        // If it was NOT a successful transition -- residence
        else
        {
            // Display a notice saying "no one seems to be home"
            NoOneHome();
        }
    }

    void NoOneHome()
    {
        // Use the HUD manager to display a notice
        hudManager.NoOneHome();
    }

    private IEnumerator TransitionToScene(float start, float end, float timeToFade, string newScene)
    {
        // If the scene fades out
        if (fadeOut)
        {
            // Wait for fade transition
            yield return Fade(start, end, timeToFade);
        }

        // Use the level manager to load new scene
        levelManager.LoadLevel(newScene);
    }

    public IEnumerator Fade(float start, float end, float timeToFade)
    {
        // Enable the overlay black image, the component, and it's parent
        fadeImage.enabled = true; // Component
        fadeImage.gameObject.SetActive(true); // Object
        fadeImage.transform.parent.gameObject.SetActive(true); // Parent
        
        Color currentFadeColor = fadeImage.color;

        // Set starting alpha
        currentFadeColor.a = start;
        fadeImage.color = currentFadeColor;

        // Interpolate from start alpha to end alpha over timeToFade seconds
        for (float currentFadeTime = 0.0f; currentFadeColor.a != end; currentFadeTime += Time.deltaTime)
        {
            // Using interpolation, find the alpha value for that point in time
            float fadeAlpha = Mathf.Lerp(start, end, currentFadeTime / timeToFade);

            // Set the color's alpha value
            currentFadeColor.a = fadeAlpha;
            fadeImage.color = currentFadeColor;

            // Wait for the frame to update
            yield return new WaitForEndOfFrame();
        }

        // If it faded out
        if (end <= 0.01f)
        {
            // Disable the fade image
            fadeImage.enabled = false;
        }
    }

    public void FadeMethod(string scene)
    {
        // Use the transition coroutine to fade
        Debug.Log("Transitioning to " + scene);
        StartCoroutine(TransitionToScene(0, 1, FADE_DURATION, scene));
    }
}
