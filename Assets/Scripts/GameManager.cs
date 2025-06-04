using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using System.Collections;
using Random = System.Random;

// This script is used to control the game logic.
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int innerScore1, innerScore2, innerScore3, innerScore4, innerScore5; // Scores for inner circles

    [SerializeField] private int score = 0; // Total score of the player
    [SerializeField] private TextMeshProUGUI scoreText; // TextMeshProUGUI instance to show the score on the screen
    
    public float floatSpeed = 20f;
    public float fadeDuration = 1.5f;
    private Vector3 moveDirection = Vector3.up;

    public void UpdateScore(string raycastedGameObject, GameObject targetBoardCanvas)
    {
        int tempScore = 0; // Temporary score variable to hold the score before updating the UI
        Color tempColor = Color.white; // Temporary color variable for the floating text
        // This method updates the score according to the raycasted object
        switch (raycastedGameObject)
        {
            case "Inner 1":
                tempScore = innerScore1;
                tempColor = Color.yellow; // Setting color for Inner 1
                score += innerScore1;
                break;
            case "Inner 2":
                tempScore = innerScore2;
                tempColor = Color.red; // Setting color for Inner 2
                score += innerScore2;
                break;
            case "Inner 3":
                tempScore = innerScore3;
                tempColor = new Color(0, 201, 255, 255);
                score += innerScore3;
                break;
            case "Inner 4":
                tempScore = innerScore4;
                tempColor = Color.black; // Setting color for Inner 4
                score += innerScore4;
                break;
            case "Inner 5":
                tempScore = innerScore5;
                tempColor = Color.white; // Setting color for Inner 5
                score += innerScore5;
                break;
        }
        
        Random random = new Random(); // Creating a random instance to generate random colors
        TextMeshProUGUI floatingText = null;
        
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

            text.transform.position = startPos + moveDirection * floatSpeed * t; // TODO: Targetboard may destroy itself before lerp is done
            canvasGroup.alpha = 1f - t;

            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        text.gameObject.SetActive(false);
    }
    
}