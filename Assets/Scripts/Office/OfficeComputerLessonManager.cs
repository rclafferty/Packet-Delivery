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
    [SerializeField] GameObject exitTheMatrixLessonPage2;
    [SerializeField] GameObject exitTheMatrixLessonPage3;
    [SerializeField] GameObject exitTheMatrixLessonButton;
    [SerializeField] GameObject exitTheMatrixNextLessonButton;
    [SerializeField] GameObject exitTheMatrixPreviousLessonButton;

    static bool hasShownExitTheMatrixLesson = false;
    static bool hasShownAddressBookLesson = false;
    static bool firstAddressBookLesson = true;

    enum Lesson { None, ExitTheMatrixP1, ExitTheMatrixP2, ExitTheMatrixP3, AddressBook };
    Lesson currentLesson;

    // Start is called before the first frame update
    void Start()
    {
        ReturnToComputer();

        hudManager = GameObject.Find("HUD").GetComponent<HUDManager>();

        currentLesson = Lesson.None;
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
        exitTheMatrixLessonPage2.SetActive(false);
        exitTheMatrixLessonPage3.SetActive(false);
        addressBookLesson.SetActive(false);

        exitTheMatrixNextLessonButton.SetActive(false);
        exitTheMatrixPreviousLessonButton.SetActive(false);

        if (currentLesson == Lesson.AddressBook && firstAddressBookLesson)
        {
            firstAddressBookLesson = false;

            // Show address book
            hudManager.ToggleAddressBook(isShown: true);
        }

        currentLesson = Lesson.None;
    }

    void ShowETM()
    {
        exitTheMatrixLesson.SetActive(true);
    }

    void ShowETMPage(int page)
    {
        switch (page)
        {
            case 1:
                Debug.Log("Showing Page 1");
                currentLesson = Lesson.ExitTheMatrixP1;

                // Show first lesson
                exitTheMatrixLesson.SetActive(true);

                // Hide other lessons
                exitTheMatrixLessonPage2.SetActive(false);
                exitTheMatrixLessonPage3.SetActive(false);

                // Show next button
                exitTheMatrixNextLessonButton.SetActive(true);

                // Hide previous button
                exitTheMatrixPreviousLessonButton.SetActive(false);
                break;
            case 2:
                Debug.Log("Showing Page 2");
                currentLesson = Lesson.ExitTheMatrixP2;

                // Show second lesson
                exitTheMatrixLessonPage2.SetActive(true);

                // Hide other lessons
                exitTheMatrixLesson.SetActive(false);
                exitTheMatrixLessonPage3.SetActive(false);

                // Show next button and previous button
                exitTheMatrixNextLessonButton.SetActive(true);
                exitTheMatrixPreviousLessonButton.SetActive(true);
                
                break;
            case 3:
                Debug.Log("Showing Page 3");
                currentLesson = Lesson.ExitTheMatrixP3;

                // Show third lesson
                exitTheMatrixLessonPage3.SetActive(true);

                // Hide other lessons
                exitTheMatrixLesson.SetActive(false);
                exitTheMatrixLessonPage2.SetActive(false);

                // Hide next button
                exitTheMatrixNextLessonButton.SetActive(false);

                // Show previous button
                exitTheMatrixPreviousLessonButton.SetActive(true);
                break;
            default:
                Debug.Log("Incorrect page -- " + page);
                break;
        }
    }

    public void ShowPreviousExitTheMatrixLesson()
    {
        ToggleUIObjects(isShown: false);

        switch (currentLesson)
        {
            case Lesson.ExitTheMatrixP2:
                ShowETMPage(page: 1);
                break;

            case Lesson.ExitTheMatrixP3:
                ShowETMPage(page: 2);
                break;

            default:
                Debug.Log("Incorrect lesson -- Lesson." + currentLesson);
                break;
        }
    }

    public void ShowNextExitTheMatrixLesson()
    {
        ToggleUIObjects(isShown: false);

        Debug.Log(currentLesson);
        switch (currentLesson)
        {
            case Lesson.None:
                ShowETMPage(page: 1);
                break;

            case Lesson.ExitTheMatrixP1:
                ShowETMPage(page: 2);
                break;

            case Lesson.ExitTheMatrixP2:
                ShowETMPage(page: 3);
                break;

            default:
                Debug.Log("Incorrect lesson -- Lesson." + currentLesson);
                break;
        }
    }

    public void ShowAddressBookLesson()
    {
        ToggleUIObjects(isShown: false);
        addressBookLesson.SetActive(true);

        currentLesson = Lesson.AddressBook;
    }

    public void PurchaseExitTheMatrix()
    {
        Debug.Log(hasShownExitTheMatrixLesson);
        if (!hasShownExitTheMatrixLesson)
        {
            ShowNextExitTheMatrixLesson();
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
