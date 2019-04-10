using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    [SerializeField]
    Sprite defaultSprite;

    [SerializeField]
    Sprite[] randomImages;

    // Start is called before the first frame update
    void Start()
    {
        if (randomImages == null)
        {
            GetComponent<SpriteRenderer>().sprite = defaultSprite;
        }
        else if (randomImages.Length == 0)
        {
            GetComponent<SpriteRenderer>().sprite = defaultSprite;
        }
        else
        {
            int index = Random.Range(0, randomImages.Length);
            GetComponent<SpriteRenderer>().sprite = randomImages[index];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
