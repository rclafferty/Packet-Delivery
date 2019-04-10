using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    static Timer instance = null;

    const float EASY_MINUTES = 5.0f;
    const float DEFAULT_MINUTES = 4.0f;
    const float HARD_MINUTES = 3.0f;
    float difficultyMinutes;

    [SerializeField]
    float runningTime;
    bool running;

    float startTime;
    float currentTime;

    Coroutine currentTimer;

    [SerializeField]
    Text timerText;

    [SerializeField]
    GameObject object_timerText;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        runningTime = 0.0f;
        running = false;

        currentTimer = null;
        object_timerText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            currentTime = Time.time;
            float diff = currentTime - startTime;
            runningTime = difficultyMinutes - diff;
            // timerText.text = "Time left:  " + runningTime.ToString("#,##0.00") + " s";
            timerText.text = "Time left:  " + TimeToString(runningTime) + " s";

            if (currentTime <= 0.0f)
            {
                StopTimer();
            }
        }
    }

    public string TimeToString(float time)
    {
        int min = (int)(time / 60.0f);
        int sec = (int)(time - (min * 60));
        int millisec = (int)((time % 1) * 10);

        string text = min + ":" + sec.ToString("00") + "." + millisec;
        return text;
    }

    public void StopTimer()
    {
        running = false;
    }

    public void StartNewTimerIfNotAlreadyRunning()
    {
        if (running)
        {
            return;
        }

        object_timerText.SetActive(true);
        running = true;
        startTime = Time.time;
    }

    void Run()
    {

    }

    public float RunningTime
    {
        get
        {
            return runningTime;
        }
    }

    public void StopTimerIfRunning()
    {
        if (currentTimer != null)
        {
            StopCoroutine(currentTimer);
            object_timerText.SetActive(false);
        }
    }

    public void SetDifficulty(string difficulty)
    {
        if (currentTimer != null)
        {
            Debug.Log("Cannot edit running time while timer is running");
            return;
        }

        string lower = difficulty.ToLower().Trim();
        if (lower == "easy")
        {
            runningTime = EASY_MINUTES * 60.0f;
        }
        else if (lower == "medium" || lower == "default")
        {
            runningTime = DEFAULT_MINUTES * 60.0f;
        }
        else if (lower == "hard")
        {
            runningTime = HARD_MINUTES * 60.0f;
        }

        difficultyMinutes = runningTime;
    }
}