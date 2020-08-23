using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeComputerLessonManager : MonoBehaviour
{
    [SerializeField] GameObject[] uiObjectsToToggle;
    [SerializeField] GameObject exitTheMatrixLesson;
    [SerializeField] GameObject addressBookLesson;

    // Start is called before the first frame update
    void Start()
    {
        ReturnToComputer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ToggleUIObjects(bool isShown)
    {
        foreach (GameObject g in uiObjectsToToggle)
        {
            g.SetActive(isShown);
        }
    }

    public void ReturnToComputer()
    {
        ToggleUIObjects(isShown: true);
        exitTheMatrixLesson.SetActive(false);
        addressBookLesson.SetActive(false);
    }

    public void ShowExitTheMatrixLesson()
    {
        ToggleUIObjects(isShown: false);
        exitTheMatrixLesson.SetActive(true);
    }

    public void ShowAddressBookLesson()
    {
        ToggleUIObjects(isShown: false);
        addressBookLesson.SetActive(true);
    }
}
