using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour {
    public Text scoreDisplay;
    public Text efficiencyDisplay;

    public int score = 0;
    public int totalPossibleScore = 0;

    public void IncreaseScore(int points) {
        score += points;
        totalPossibleScore += points;

        int currentScore = int.Parse(scoreDisplay.text);
        scoreDisplay.text = (currentScore + points).ToString();

        UpdateEfficiency();
    }
	
    public void IncreaseMissedScore(int points)
    {
        totalPossibleScore += points;
        UpdateEfficiency();
    }

    private void UpdateEfficiency()
    {
        if (totalPossibleScore <= 0) return;

        var percentage = (float)score / (float)totalPossibleScore * 100f;
        var missedPoints = totalPossibleScore - score;

        efficiencyDisplay.text = string.Format("{0:F1}%\n(missed {1})", percentage, missedPoints);
    }
}
