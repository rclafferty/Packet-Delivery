using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CLA2GameplayManager : MonoBehaviour
{
    [SerializeField]
    GameplayManager gameplayManager;

    [SerializeField]
    Text targetText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTargetText()
    {
        string name = SceneManager.GetActiveScene().name.ToLower();
        if (name.Contains("cla2"))
            targetText.text = "Current Target: " + gameplayManager.CurrentTarget;
    }
}
