using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeComputerLessonManager : MonoBehaviour
{
    HUDManager hudManager;

    [SerializeField] GameObject[] uiObjectsToToggle;
    [SerializeField] GameObject addressBookLesson;
    [SerializeField] GameObject addressBookLessonButton;
    [SerializeField] GameObject exitTheMatrixLesson;
    [SerializeField] GameObject exitTheMatrixLessonButton;

    static bool hasShownExitTheMatrixLesson = false;
    static bool hasShownAddressBookLesson = false;
    static bool firstAddressBookLesson = true;

    enum Lesson { None, ExitTheMatrix, AddressBook };
    Lesson currentLesson;

    // Start is called before the first frame update
    void Start()
    {
        ReturnToComputer();

        hudManager = GameObject.Find("HUD").GetComponent<HUDManager>();
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

        if (currentLesson == Lesson.AddressBook && firstAddressBookLesson)
        {
            firstAddressBookLesson = false;

            // Show address book
            hudManager.ToggleAddressBook(isShown: true);
        }

        currentLesson = Lesson.None;
    }

    public void ShowExitTheMatrixLesson()
    {
        ToggleUIObjects(isShown: false);
        exitTheMatrixLesson.SetActive(true);

        currentLesson = Lesson.ExitTheMatrix;
    }

    public void ShowAddressBookLesson()
    {
        ToggleUIObjects(isShown: false);
        addressBookLesson.SetActive(true);

        currentLesson = Lesson.AddressBook;
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
