using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// This script is used to control the timer. (https://discussions.unity.com/t/simple-timer/56201)
public class Timer : MonoBehaviour
{
    [SerializeField] private float targetTime = 60.0f; // Target time for the timer
    [SerializeField] private TextMeshProUGUI timerText;
    private bool isTimerEnded = false;
    
    void Update()
    {
        if (!isTimerEnded)
        {
            targetTime -= Time.deltaTime; // Decrease the target time by the time passed since the last frame
            Debug.Log((int)targetTime); // Debugging
            int timerTextInteger = (int) targetTime;
            timerText.text = timerTextInteger.ToString();
        }

        if (targetTime <= 0.0f) // If the target time is less than or equal to 0
        {
            TimerEnded(); // Call the TimerEnded method
        }
    }
    
    void TimerEnded()
    {
        isTimerEnded = true;
        Debug.Log("Timer has ended!"); // Debugging
    }
}
