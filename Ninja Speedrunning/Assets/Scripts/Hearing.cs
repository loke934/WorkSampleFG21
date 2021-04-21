using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//If player trigger the hearing the enemy will start to chase/attack.
public class Hearing : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerStats>() != null)
        {
            GlobalVariables.player_reference = other.gameObject;
            enemy.playerFound = true;
        }
    }
}
