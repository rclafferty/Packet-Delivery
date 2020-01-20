using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeComputerManager : MonoBehaviour
{
    [SerializeField] Canvas officeComputerCanvas;

    bool isComputerShown;

    // Start is called before the first frame update
    void Start()
    {
        ShowHideComputerCanvas(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ShowHideComputerCanvas(bool isShown)
    {
        isComputerShown = isShown;
        officeComputerCanvas.gameObject.SetActive(isShown);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isComputerShown)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShowHideComputerCanvas(true);
            }
        }
    }

    public void NewDeliveryRequest()
    {
        
    }

    public void ViewDeliveryDetails()
    {

    }

    public void NextDestination()
    {

    }

    public void Logistics()
    {

    }
}
