using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Timer : MonoBehaviour
{
    public float TimeRemaining = 60;
    private bool TimerIsRunning = false;

    private void Start()
    {
        TimerIsRunning = true;
    }

    private void Update()
    {
        if (!TimerIsRunning) return;
        if (TimeRemaining > 0)
        {
            TimeRemaining -= Time.deltaTime;
        }
        else
        {
            Terminate();
        }
    }

    private void Terminate()
    {
        TimeRemaining = 0;
        TimerIsRunning = false;
    }

    public bool HasExpired()
    {
        return TimerIsRunning;
    }

    public string GetTimeDisplay()
    {
        var timeToDisplay = TimeRemaining;
        if (TimerIsRunning)
            timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        return $"{minutes:00}:{seconds:00}";
    }
}


