using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [Space]
    [SerializeField] private float timeLimit = 30f; // Seconds
    private float currentTime;

    bool isTimerPaused = true;

    public delegate void OnTimerEnd();
    public OnTimerEnd onTimerEnd;

    private void Update()
    {
        if (isTimerPaused) return;

        currentTime -= Time.deltaTime;
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        int seconds = Mathf.FloorToInt(currentTime);

        timerText.text = string.Format("{0:00}", seconds);

        if (seconds <= 0)
        {
            onTimerEnd.Invoke();
            SetPauseTimer(true);
        }
    }

    public void SetPauseTimer(bool state)
    {
        isTimerPaused = state;
    }

    public void ResetTimer()
    {
        currentTime = timeLimit;
        UpdateTimer();
    }


}
