using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer2 : MonoBehaviour
{
    static Timer2 instance = null;
    
    public struct DifficultySetting
    {
        public string name;
        public float timeLimit;
    };

    List<DifficultySetting> difficulties;
    int difficultyIndex;

    [SerializeField] Text timerText;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // Default: Easy difficulty
        difficultyIndex = 0;

        // Initialize all difficulty settings
        difficulties = new List<DifficultySetting>();
        DifficultySetting thisSetting;

        // Easy setting
        thisSetting.name = "easy";
        thisSetting.timeLimit = 7.0f;
        difficulties.Add(thisSetting);

        // Medium setting
        thisSetting.name = "medium";
        thisSetting.timeLimit = 5.0f;
        difficulties.Add(thisSetting);

        // Hard setting
        thisSetting.name = "hard";
        thisSetting.timeLimit = 3.0f;
        difficulties.Add(thisSetting);

        // Set initial property values
        RemainingTime = 0.0f;
        IsRunning = false;
        timerText.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsRunning)
        {
            IncrementTimer();
            ShowTimerText();
        }
    }

    void IncrementTimer()
    {
        float currentTime = Time.time;
        float timeLimit = difficulties[difficultyIndex].timeLimit;
        RemainingTime = timeLimit - (currentTime - StartTime);
    }

    void ShowTimerText()
    {
        if (timerText == null)
        {
            Debug.Log("Timer text is null");
            return;
        }

        string timeString = TimeToString(RemainingTime);
        string prefixText = "";
        if (CheckIfOverdue())
        {
            prefixText = "OVERDUE!";
        }
        else
        {
            prefixText = "Time left:";
        }

        timerText.text = prefixText + " " + timeString + " s";
    }

    bool CheckIfOverdue()
    {
        if (RemainingTime <= 0.0f)
        {
            if (!IsOverdue)
            {
                IsOverdue = true;
                timerText.color = Color.red;
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    public string TimeToString(in float time)
    {
        float thisTime = time;
        if (thisTime < 0)
        {
            thisTime *= -1;
        }

        int min = (int)(time / 60.0f);
        int sec = (int)(time - (min * 60.0f));
        int millisec = (int)((time % 1) * 10);

        string thisTimeText = min + ":" + sec.ToString("00") + "." + millisec;
        return thisTimeText;
    }

    public bool SetDifficulty(in string name)
    {
        if (!IsDifficultiesValid())
            return false;

        for (int i = 0; i < difficulties.Count; i++)
        {
            if (name == difficulties[i].name)
            {
                difficultyIndex = i;
                return true;
            }
        }

        // None found
        return false;
    }

    bool IsDifficultiesValid()
    {
        if (difficulties == null)
        {
            return false;
        }

        if (difficulties.Count == 0)
        {
            return false;
        }

        return true;
    }

    public void StartTimerIfNotRunning()
    {
        if (IsRunning)
            return;

        IsRunning = true;
        timerText.gameObject.SetActive(true);
    }

    public void StopTimerIfRunning()
    {
        if (!IsRunning)
            return;

        IsRunning = false;
        timerText.text = "";
        timerText.gameObject.SetActive(false);
    }

    // Auto property
    public float StartTime { get; set; }
    public float RemainingTime { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsOverdue { get; private set; }
}
