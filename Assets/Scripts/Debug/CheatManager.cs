using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatManager : MonoBehaviour
{
    static CheatManager instance = null;

    LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
#else
        // Don't need cheats on full game
        Destroy(gameObject);
#endif
    }

    public void SetLevelManager(LevelManager l)
    {
        levelManager = l;
    }

    // Update is called once per frame
    void Update()
    {

#if UNITY_EDITOR
        /*if (Input.GetKeyDown(KeyCode.F1))
        {
            levelManager.LoadLevel("office");
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            levelManager.LoadLevel("centralLookupAgency");
        }
        else*/ if (Input.GetKeyDown(KeyCode.F3))
        {
            GameObject.Find("GameplayManager").GetComponent<GameplayManager>().CompleteTask();
        }
        /* else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            GameObject.Find("GameplayManager").GetComponent<GameplayManager>().DebugChangePlayerPosition(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameObject.Find("GameplayManager").GetComponent<GameplayManager>().DebugChangePlayerPosition(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject.Find("GameplayManager").GetComponent<GameplayManager>().DebugChangePlayerPosition(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameObject.Find("GameplayManager").GetComponent<GameplayManager>().DebugChangePlayerPosition(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GameObject.Find("GameplayManager").GetComponent<GameplayManager>().DebugChangePlayerPosition(4);
        } */
#endif
    }
}
