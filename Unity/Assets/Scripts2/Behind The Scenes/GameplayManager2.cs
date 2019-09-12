using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager2 : MonoBehaviour
{
    static GameplayManager2 instance = null;

    Timer deliveryTimer;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        ResourceManager = GameObject.Find("ResourceManager").GetComponent<ResourceManager2>();
        ResourceManager.GameplayManager = this;
        LetterManager = ResourceManager.LetterManager;
    }

    // Auto property -- Managers
    public ResourceManager2 ResourceManager { get; private set; }
    private LetterManager2 LetterManager { get; set; }

    // Auto property -- Values
    public bool HasVisitedCLA { get; set; }
    public Vector3 SpawnLocation { get; set; } // in town
}
