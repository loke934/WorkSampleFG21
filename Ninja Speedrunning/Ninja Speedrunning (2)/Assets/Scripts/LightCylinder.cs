using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The large lantern light on high altitude enemy. When player collides all ground-enemy start chase/attack.
public class LightCylinder : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerStats>() != null)
        {
            GlobalVariables.player_reference = other.gameObject;
            GlobalVariables.allAttack = true;
        }
    }
}
