using System;
using UnityEngine;
using TMPro;

// This script is used to control the timer. (https://discussions.unity.com/t/simple-timer/56201)
public class Timer : MonoBehaviour
{
    [SerializeField] private float targetTime = 60.0f; // Target time for the timer
    [SerializeField] private TextMeshProUGUI timerText; // TextMeshProUGUI instance to show the timer on the screen
    private bool isTimerEnded = false; // Is used to check if the timer is ended
    [SerializeField] private GameObject gameOverPanel; // Game over panel to activate
    [SerializeField] private GameObject accuracyTargetBoard; // Accuracy target board to activate 
    [SerializeField] private GameObject weaponObject; // Weapon object to deactivate
    [SerializeField] private GameObject crosshairObject; // Crosshair object to deactivate
    [SerializeField] private GameManager gameManager; // GameManager instance to set the game over flag
    [SerializeField] private Cinemachine.CinemachineVirtualCamera cmVirtualCamera; // Cinemachine virtual camera to turn off follow
    
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
        gameManager.isGameOver = true; // Set the game over flag in the GameManager
        isTimerEnded = true; // Set the timer as ended
        gameOverPanel.SetActive(true); // Activate the game over panel
        accuracyTargetBoard.SetActive(true); // Activate the accuracy target board
        weaponObject.SetActive(false); // Deactivate the weapon object
        crosshairObject.SetActive(false); // Deactivate the crosshair object
        // Unlock the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        cmVirtualCamera.Follow = null; // Turn off the follow of the Cinemachine virtual camera
    }
}
