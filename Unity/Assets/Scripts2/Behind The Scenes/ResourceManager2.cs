using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameplayManager = GameObject.Find("LetterManager").GetComponent<GameplayManager2>();
        LetterManager = GameObject.Find("LetterManager").GetComponent<LetterManager2>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Auto Property
    public GameplayManager2 GameplayManager { get; set; }
    public LetterManager2 LetterManager { get; set; }
}
