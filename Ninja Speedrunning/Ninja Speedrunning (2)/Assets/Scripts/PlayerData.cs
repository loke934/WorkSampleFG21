using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//all data inside to binary, use as save file. Se if this is needed later
public class PlayerData
{
    public string playername;
    public float time;

    public PlayerData (string name, float timeScore)
    {
        playername = name;
        time = timeScore;
    }

}
