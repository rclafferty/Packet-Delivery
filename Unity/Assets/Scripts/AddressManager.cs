using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddressManager : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] spriteRenderers;
    [SerializeField] Sprite[] transparentText;
    [SerializeField] Sprite[] transparentIPs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableExitTheMatrix()
    {
        if (spriteRenderers == null)
            return;

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].sprite = transparentIPs[i];
        }
    }
}
