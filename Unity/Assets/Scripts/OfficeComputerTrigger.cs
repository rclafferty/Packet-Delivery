using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeComputerTrigger : MonoBehaviour
{
    [SerializeField] Canvas computerCanvas;
    bool isComputerUIShown;

    // Start is called before the first frame update
    void Start()
    {
        ShowHideComputerUI(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowHideComputerUI(bool isShown)
    {
        isComputerUIShown = isShown;
        computerCanvas.gameObject.SetActive(isShown);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isComputerUIShown)
        {
            if (collision.name == "Player" && Input.GetKeyDown(KeyCode.Space))
            {
                ShowHideComputerUI(true);
            }
        }
    }
}
