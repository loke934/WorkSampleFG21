﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreManager : MonoBehaviour
{
    [SerializeField] private Text positionList;
    [SerializeField] private Text nameList;
    [SerializeField] private Text timeList;
    [SerializeField] private Text leaderBoardMini;
    private int minIndex = 0;
    private int maxIndex = 10;
    [SerializeField] private int displayHighscores = 3;

    //Make object of class HighscoreData
    private HighscoreData highscoreDataObject = new HighscoreData();

    private void Awake()
    {
        if (SaveSystem.LoadHighscores() != null)
        {
            highscoreDataObject = SaveSystem.LoadHighscores();
            SortScores();
        }
    }

    private void SortScores()
    {
        List<PlayerData> tempList = new List<PlayerData>();
        while (highscoreDataObject.scores.Count > 0)
        {
            PlayerData highestscore = null;
            foreach (var score in highscoreDataObject.scores)
            {
                if (highestscore == null || score.time < highestscore.time)
                {
                    highestscore = score;
                }
            }
            tempList.Add(highestscore);
            highscoreDataObject.scores.Remove(highestscore);
        }
        highscoreDataObject.scores = tempList;
    }

    public void DisplayHighscoresMini()
    {
        string leaderboardstring = "";
        int length = displayHighscores;
        if (highscoreDataObject.scores.Count < displayHighscores)
        {
            length = highscoreDataObject.scores.Count;
        }
        for (int i = 0; i < length; i++)
        {
            leaderboardstring += $" {i + 1}) {highscoreDataObject.scores[i].playername} : {highscoreDataObject.scores[i].time} \n " ;
        }
        leaderBoardMini.text = leaderboardstring;
    }

    public void HighscoreAddSortSave(PlayerData p)
    {
        highscoreDataObject.scores.Add(p);
        SortScores();
        SaveSystem.SaveHighscores(highscoreDataObject);
    }

    public void DisplayHighscoresMainMenu()
    {
        string positionstring = "";
        string namestring = "";
        string timestring = "";
        if (maxIndex > highscoreDataObject.scores.Count)
        {
            maxIndex = highscoreDataObject.scores.Count;
        }

        for (int i = minIndex; i < maxIndex; i++)
        {
            positionstring += $"{i + 1} \n";
            namestring += $"{highscoreDataObject.scores[i].playername} \n";
            timestring += $"{highscoreDataObject.scores[i].time} \n";
        }

        positionList.text = positionstring;
        nameList.text = namestring;
        timeList.text = timestring;
    }
    public void ScrollList(int i)
    {
        if (minIndex + i >= 0 || maxIndex + i >= highscoreDataObject.scores.Count)
        {
            minIndex += i;
            maxIndex += i;
            DisplayHighscoresMainMenu();
        }
    }
}
