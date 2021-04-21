using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using System;

public class GameStartMenu : MonoBehaviour
{
    [SerializeField] private Canvas keycontrolCanvas;
    [SerializeField] private Canvas leaderboardCanvas;
    [SerializeField] private Canvas instructionCanvas;
    private HighscoreManager highscoremanager;

    public void OpenControls()
    {
        keycontrolCanvas.gameObject.SetActive(!keycontrolCanvas.gameObject.activeSelf);
    }

    public void OpenLeaderBorard()
    {
        highscoremanager = GetComponent<HighscoreManager>();
        highscoremanager.DisplayHighscoresMainMenu();
        leaderboardCanvas.gameObject.SetActive(!leaderboardCanvas.gameObject.activeSelf);
    }

    public void OpenInstructions()
    {
        instructionCanvas.gameObject.SetActive(!instructionCanvas.gameObject.activeSelf);
    }
}
