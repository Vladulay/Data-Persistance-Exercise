using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HallOfFameUIHandler : MonoBehaviour
{
    // Data
    private string[] playerNames;
    private int[] points;

    // UI
    private TextMeshProUGUI firstPlace;
    private TextMeshProUGUI secondPlace;
    private TextMeshProUGUI thirdPlace;

    void Start()
    {
        RetrieveHighscores();

        SetUI();
    }

    public void RetrieveHighscores()
    {
        playerNames = new string[3];
        points = new int[3];

        playerNames = MainManager.Instance.bestPlayerName;
        points = MainManager.Instance.highscore;
    }

    public void SetUI()
    {
        firstPlace = GameObject.Find("1st Place").GetComponent<TextMeshProUGUI>();
        secondPlace = GameObject.Find("2nd Place").GetComponent<TextMeshProUGUI>();
        thirdPlace = GameObject.Find("3rd Place").GetComponent<TextMeshProUGUI>();

        firstPlace.text = "1st: " + playerNames[0] + " with " + points[0] + " points";
        secondPlace.text = "2nd: " + playerNames[1] + " with " + points[1] + " points";
        thirdPlace.text = "3rd: " + playerNames[02] + " with " + points[2] + " points";
    }
}
