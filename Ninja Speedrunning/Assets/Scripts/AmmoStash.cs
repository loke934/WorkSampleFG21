using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoStash : MonoBehaviour
{
    //Amount of ammo the player gets from the ammo stash, designer to decide amount.
    [SerializeField]private int ammo_amount = 5;
   
  private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerStats>() != null)
        {
            other.gameObject.GetComponentInChildren<RangedNinjaWeapon>(true).PickUpAmmo(ammo_amount);
        }
        gameObject.SetActive(false);
    }
}
