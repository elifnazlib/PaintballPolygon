using TMPro;
using UnityEngine;

// This script is used to control the game logic.
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int innerScore1, innerScore2, innerScore3, innerScore4, innerScore5; // Scores for inner circles

    [SerializeField] private int score = 0; // Total score of the player
    [SerializeField] private TextMeshProUGUI scoreText; // TextMeshProUGUI instance to show the score on the screen

    public void UpdateScore(string raycastedGameObject)
    {
        // This method updates the score according to the raycasted object
        switch (raycastedGameObject)
        {
            case "Inner 1":
                score += innerScore1;
                break;
            case "Inner 2":
                score += innerScore2;
                break;
            case "Inner 3":
                score += innerScore3;
                break;
            case "Inner 4":
                score += innerScore4;
                break;
            case "Inner 5":
                score += innerScore5;
                break;
        }

        scoreText.text = score.ToString(); // Updating the score text on the screen
    }
}