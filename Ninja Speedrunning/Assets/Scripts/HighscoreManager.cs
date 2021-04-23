using System.Collections;
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

    private HighscoreData savedHighscores;
    
    private void Awake()
    {
        savedHighscores = SaveSystem.LoadHighscores() ?? new HighscoreData();
        if (savedHighscores != null)
        {
            SortScores();
        }
    }

    private void SortScores()
    {
        List<PlayerData> tempList = new List<PlayerData>();
        while (savedHighscores.scores.Count > 0)
        {
            PlayerData playerScore = null;
            foreach (var score in savedHighscores.scores)
            {
                if (playerScore == null || score.time < playerScore.time)
                {
                    playerScore = score;
                }
            }

            tempList.Add(playerScore);
            savedHighscores.scores.Remove(playerScore); //Removes the value (PlayerData name + time) stored in highscores
        }
        savedHighscores.scores = tempList;
    }

    public void DisplayHighscoresMini()
    {
        string leaderboardstring = "";
        int length = displayHighscores;
        if (savedHighscores.scores.Count < displayHighscores)
        {
            length = savedHighscores.scores.Count;
        }
        for (int i = 0; i < length; i++)
        {
            leaderboardstring += $" {i + 1}) {savedHighscores.scores[i].playername} : {savedHighscores.scores[i].time} \n " ;
        }
        leaderBoardMini.text = leaderboardstring;
    }

    public void HighscoreAddSortSave(PlayerData p)
    {
        savedHighscores.scores.Add(p);
        SortScores();
        SaveSystem.SaveHighscores(savedHighscores);
    }

    public void DisplayHighscoresMainMenu()
    {
        string positionstring = "";
        string namestring = "";
        string timestring = "";
        if (maxIndex > savedHighscores.scores.Count)
        {
            maxIndex = savedHighscores.scores.Count;
        }

        for (int i = minIndex; i < maxIndex; i++)
        {
            positionstring += $"{i + 1} \n";
            namestring += $"{savedHighscores.scores[i].playername} \n";
            timestring += $"{savedHighscores.scores[i].time} \n";
        }

        positionList.text = positionstring;
        nameList.text = namestring;
        timeList.text = timestring;
    }
    public void ScrollList(int i)
    {
        if (minIndex + i >= 0 || maxIndex + i >= savedHighscores.scores.Count)
        {
            minIndex += i;
            maxIndex += i;
            DisplayHighscoresMainMenu();
        }
    }
}
