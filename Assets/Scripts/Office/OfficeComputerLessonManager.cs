using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeComputerLessonManager : MonoBehaviour
{
    [SerializeField] GameObject[] uiObjectsToToggle;
    [SerializeField] GameObject addressBookLesson;
    [SerializeField] GameObject addressBookLessonButton;
    [SerializeField] GameObject exitTheMatrixLesson;
    [SerializeField] GameObject exitTheMatrixLessonButton;

    static bool hasShownExitTheMatrixLesson = false;
    static bool hasShownAddressBookLesson = false;

    // Start is called before the first frame update
    void Start()
    {
        ReturnToComputer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleUIObjects(bool isShown)
    {
        foreach (GameObject g in uiObjectsToToggle)
        {
            g.SetActive(isShown);
        }

        bool isAddressBookButtonShown = isShown && hasShownAddressBookLesson;
        addressBookLessonButton.SetActive(isAddressBookButtonShown);

        bool isExitTheMatrixButtonShown = isShown && hasShownExitTheMatrixLesson;
        exitTheMatrixLessonButton.SetActive(isExitTheMatrixButtonShown);
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

    public void PurchaseExitTheMatrix()
    {
        if (!hasShownExitTheMatrixLesson)
        {
            ShowExitTheMatrixLesson();
            hasShownExitTheMatrixLesson = true;
        }
    }

    public void PurchaseAddressBook()
    {
        if (!hasShownAddressBookLesson)
        {
            ShowAddressBookLesson();
            hasShownAddressBookLesson = true;
        }
    }
}
