using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartingNPCManager : MonoBehaviour
{
    GameplayManager gameplayManager;
    LevelManager levelManager;
    LookupAgencyManager lookupManager;

    [SerializeField]
    EventSystem eventSystem;

    [SerializeField]
    Text chatText;

    [SerializeField]
    Button option1Button;
    [SerializeField]
    Text option1Text;

    [SerializeField]
    Button option2Button;
    [SerializeField]
    Text option2Text;

    [SerializeField]
    InputField inputField;

    const float RIGHT_X = -4.0f;
    const float LEFT_X = -13.0f;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectsForScene();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FindObjectsForScene()
    {
        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        lookupManager = GameObject.Find("LookupAgencyManager").GetComponent<LookupAgencyManager>();

        chatText = GameObject.Find("ChatText").GetComponent<Text>();
        option1Text = GameObject.Find("Option1Text").GetComponent<Text>();
        option1Button = GameObject.Find("Option1Button").GetComponent<Button>();
        option2Text = GameObject.Find("Option2Text").GetComponent<Text>();
        option2Button = GameObject.Find("Option2Button").GetComponent<Button>();

        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        inputField = GameObject.Find("InputField").GetComponent<InputField>();
    }

    public void MoveNPC(bool direction)
    {
        Vector3 start = gameObject.transform.position;
        Vector3 end = gameObject.transform.position;

        if (direction)
        {
            // Move left -- start
            start.x = RIGHT_X;
            end.x = LEFT_X;
        }
        else
        {
            // Move right -- end
            start.x = RIGHT_X;
            end.x = LEFT_X;
        }

        // Lerp to moveToX

        Vector3.Lerp(start, end, 3.0f);
    }

    public void StartDialogue()
    {
        Debug.Log("Starting dialogue...");
        // Hey there! I REALLY need this letter delivered today but I am too busy to do this. Can you deliver it for me? I'll pay you!

        // Sure. Where is it going to?

        // I need you to bring it to my Uncle Doug for me. Thanks! [Exit]

        // But...
        // ...
        // [to self -- face camera] I don't know where his Uncle Doug lives...
    }
}
