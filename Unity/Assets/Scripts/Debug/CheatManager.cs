using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    static CheatManager instance = null;

    LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
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
    }

    public void SetLevelManager(LevelManager l)
    {
        levelManager = l;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            levelManager.LoadLevel("office");
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            levelManager.LoadLevel("centralLookupAgency");
        }
    }
}
