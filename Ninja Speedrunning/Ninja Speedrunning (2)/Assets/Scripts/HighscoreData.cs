using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HighscoreData 
{
    public List<PlayerData> scores; 
    
    //Constructor
    public HighscoreData()
    {
        scores = new List<PlayerData>();
    }
}
