using System;
using TMPro;
using UnityEngine;
using System.Collections;
using StarterAssets;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Random = System.Random;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

// This script is used to control the game logic.
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int innerScore1, innerScore2, innerScore3, innerScore4, innerScore5; // Scores for inner circles

    [SerializeField] private int score = 0;             // Total score of the player
    [SerializeField] private TextMeshProUGUI scoreText; // TextMeshProUGUI instance to show the score on the screen
    
    public float floatSpeed = 20f;
    public float fadeDuration = 1.5f;
    private Vector3 _moveDirection = Vector3.up;
    
    private string _activeSceneName;

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseMenuPanel;          // Pause menu panel to enable/disable
    
    [Header ("Sensitivity Settings")]
    [SerializeField] private FirstPersonController firstPersonControllerScript; // FirstPersonController instance to set the sensitivity
    [SerializeField] private PlayerInput playerInput;                           // PlayerInput instance to enable/disable input actions
    [SerializeField] private Weapon weaponScript;                               // Weapon instance to enable/disable shooting
    [SerializeField] private Slider sensitivitySlider;                          // Slider instance to get the sensitivity value
    [SerializeField] private GameObject settingsPanel;                          // Settings panel to enable/disable
    [SerializeField] private TextMeshProUGUI sensitivityValueText;              // TextMeshProUGUI instance to show the sensitivity value
    
    [Header ("Crosshair")]
    [SerializeField] private Image crosshairImage;                              // Crosshair image to change
    
    private void Awake()
    {
        _activeSceneName = SceneManager.GetActiveScene().name;
    }

    private void Start()
    {
        // Setting the mouse sensitivity and crosshair that the player set before
        if (_activeSceneName == "Playground3")
        {
            sensitivitySlider.value = Settings.Instance.mouseSensitivity;
            sensitivityValueText.text = sensitivitySlider.value.ToString("F2");
            firstPersonControllerScript.RotationSpeed = sensitivitySlider.value;
            
            crosshairImage.sprite = Settings.Instance.crosshairImage.sprite;
        }
    }

    private void Update()
    {
        // Pause menu functionality
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (_activeSceneName == "Playground3")
            {
                if (!pauseMenuPanel.activeSelf && !settingsPanel.activeSelf)
                {
                    PauseOptions();
                }
                else
                {
                    ResumeOptions();
                }
            }
        }
    }

    public void UpdateScore(string raycastedGameObject, GameObject targetBoardCanvas, int comboMultiplier)
    {
        int tempScore = 0; // Temporary score variable to hold the score before updating the UI
        Color tempColor = Color.white; // Temporary color variable for the floating text
        // This method updates the score according to the raycasted object
        switch (raycastedGameObject)
        {
            case "Inner 1":
                tempScore = innerScore1;
                tempColor = Color.yellow; // Setting color for Inner 1
                score += innerScore1  * comboMultiplier;
                break;
            case "Inner 2":
                tempScore = innerScore2;
                tempColor = Color.red; // Setting color for Inner 2
                score += innerScore2  * comboMultiplier;
                break;
            case "Inner 3":
                tempScore = innerScore3;
                tempColor = new Color(0, 201, 255, 255);
                score += innerScore3  * comboMultiplier;
                break;
            case "Inner 4":
                tempScore = innerScore4;
                tempColor = Color.black; // Setting color for Inner 4
                score += innerScore4  * comboMultiplier;
                break;
            case "Inner 5":
                tempScore = innerScore5;
                tempColor = Color.white; // Setting color for Inner 5
                score += innerScore5 * comboMultiplier;
                break;
        }
        
        Random random = new Random(); // Creating a random instance to generate random colors
        TextMeshProUGUI floatingText = null;
        TextMeshProUGUI comboText = null;
        
        if (random.Next(0, 2) == 0)
        {
            floatingText = targetBoardCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }
        else
        {
            floatingText = targetBoardCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        }
        
        floatingText.color = tempColor; // Changing the color of the floating text
        floatingText.text = $"+{tempScore}"; // Setting the text of the floating text

        if (comboMultiplier > 1)
        {
            comboText = targetBoardCanvas.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            comboText.transform.SetParent(floatingText.transform, false);
            comboText.transform.position = new Vector3(floatingText.transform.position.x, floatingText.transform.position.y + 0.35f, floatingText.transform.position.z);
            comboText.text = $"x{comboMultiplier}";
            comboText.transform.DOPunchPosition(-1 * Vector3.forward, 0.1f, 4, 1f, false);
            comboText.transform.DOScale(comboText.transform.localScale * 2, fadeDuration);
            // comboText.transform.DOShakePosition(fadeDuration, 0.1f, 5, 0f, false, true, ShakeRandomnessMode.Harmonic);
        }
        
        StartCoroutine(FadeAndMove(floatingText, floatingText.transform.position));
        
        scoreText.text = score.ToString(); // Updating the score text on the screen
    }
    
    private IEnumerator FadeAndMove(TextMeshProUGUI text, Vector3 startPos)
    {
        float elapsed = 0f;
        CanvasGroup canvasGroup = text.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = text.gameObject.AddComponent<CanvasGroup>();
        }

        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;

            text.transform.position = startPos + _moveDirection * floatSpeed * t; // TODO: Targetboard may destroy itself before lerp is done
            canvasGroup.alpha = 1f - t;

            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        text.gameObject.SetActive(false);
    }
    
    public void StartGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene("Playground3");
    }
    
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
    }
    
    public void BackToPauseMenu()
    {
        settingsPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }
    
    public void ResumeOptions()
    {
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        
        weaponScript.enabled = true;
        playerInput.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f; // Resume the game
    }
    
    public void PauseOptions()
    {
        pauseMenuPanel.SetActive(true);
        
        weaponScript.enabled = false;
        playerInput.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f; // Pause the game
    }
    
    public void RestartGame()
    {
        Settings.Instance.mouseSensitivity = sensitivitySlider.value;
        Settings.Instance.crosshairImage.sprite = crosshairImage.sprite;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Playground3");
    }
    
    public void ExitToMainMenu()
    {
        Settings.Instance.mouseSensitivity = sensitivitySlider.value;
        Settings.Instance.crosshairImage.sprite = crosshairImage.sprite;
        
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
    
    public void ResetMouseSensitivity()
    {
        sensitivitySlider.value = 1.0f;
        firstPersonControllerScript.RotationSpeed = 1.0f;
    }

    public void OnSensitivityChanged()
    {
        sensitivityValueText.text = sensitivitySlider.value.ToString("F2");
        firstPersonControllerScript.RotationSpeed = sensitivitySlider.value;
    }
    
    public void ChangeCrosshairImage()
    {
        var button = EventSystem.current.currentSelectedGameObject; // clicked button
        var icon = button.transform.Find("Icon").GetComponent<Image>();
        crosshairImage.sprite = icon.sprite;
    }
    
}