using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Resets the global variables at start
public class GameManagement : MonoBehaviour
{
    void Start()
    {
        GlobalVariables.gamepaused = false;
        GlobalVariables.orbTaken = false;
        GlobalVariables.endgame = false;
        GlobalVariables.victory = false;
        GlobalVariables.ranged = false;
        GlobalVariables.sneakMode = false;
        GlobalVariables.sprintMode = false;
        GlobalVariables.allAttack = false;
    }
}
