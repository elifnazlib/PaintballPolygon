using System;
using UnityEngine;
using TMPro;

// This script is used to control the timer. (https://discussions.unity.com/t/simple-timer/56201)
public class Timer : MonoBehaviour
{
    [SerializeField] private float targetTime = 60.0f; // Target time for the timer
    [SerializeField] private TextMeshProUGUI timerText; // TextMeshProUGUI instance to show the timer on the screen
    private bool isTimerEnded = false; // Is used to check if the timer is ended
    
    void Update()
    {
        if (!isTimerEnded) // If the timer is not ended
        {
            targetTime -= Time.deltaTime; // Decrease the target time by the time passed since the last frame
            timerText.text = TimeSpan.FromSeconds(targetTime).ToString(@"mm\:ss"); // Update the timer text on the screen
        }

        if (targetTime <= 0.0f) // If the target time is less than or equal to 0
        {
            TimerEnded(); // Call the TimerEnded method
        }
    }
    
    void TimerEnded()
    {
        isTimerEnded = true; // Set the timer as ended
        Debug.Log("Timer has ended!"); // Debugging
        // TODO: Game Over Panel
    }
}
