using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    static Timer instance = null;

    const float EASY_MINUTES = 4.5f;
    const float DEFAULT_MINUTES = 3.0f;
    const float HARD_MINUTES = 1.5f;

    const float DEBUG_MINUTES = 0.2f;
    float difficultyMinutes;

    [SerializeField]
    float runningTime;
    [SerializeField]
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
            // timerText.text = "Time left:  " + TimeToString(runningTime) + " s";

            string time = TimeToString(runningTime);

            if (runningTime <= 0.0f)
            {
                // StopTimerIfRunning();
                timerText.text = "OVERDUE!  " + time + " s";
                timerText.color = Color.red;
            }
            else
            {
                timerText.text = "Time left:  " + time + " s";
                // timerText.color = Color.white;
            }
        }
    }

    public string TimeToString(float time)
    {
        if (time < 0)
        {
            time = -1 * time;
        }

        int min = (int)(time / 60.0f);
        int sec = (int)(time - (min * 60));
        int millisec = (int)((time % 1) * 10);

        string text = min + ":" + sec.ToString("00") + "." + millisec;
        return text;
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
        running = false;

        if (currentTimer != null)
        {
            StopCoroutine(currentTimer);
        }

        timerText.text = "";
        object_timerText.SetActive(false);
    }

    void Timeout()
    {
        running = false;
        if (currentTimer != null)
        {
            StopCoroutine(currentTimer);
        }

        timerText.text = "OUT OF TIME -- MESSAGE LOST";

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