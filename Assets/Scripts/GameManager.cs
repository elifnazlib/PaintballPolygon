using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public int innerScore1, innerScore2, innerScore3, innerScore4, innerScore5;
    [SerializeField] private int score = 0;

    public void UpdateScore(string raycastedGameObject) {
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
            default:
                break;
        }
        Debug.Log(score);
        
    }
}
