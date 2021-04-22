using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Players projectile for dealing damage on enemies and function to pick up ammo
public class Projectile : MonoBehaviour
{
    [SerializeField] private int dealDamage = 25;
    private bool canPickUpAmmo;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        canPickUpAmmo = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            collision.gameObject.GetComponent<Enemy>().EnemyTakeDamage(dealDamage);
            dealDamage = 0;
        }

        else if (collision.gameObject.GetComponent<EnemyHighAltitude>() != null)
        {
            collision.gameObject.GetComponent<EnemyHighAltitude>().EnemyTakeDamage(dealDamage);
            dealDamage = 0;
        }

        else if (canPickUpAmmo && collision.gameObject.GetComponent<PlayerStats>() != null)
        {
             collision.gameObject.GetComponentInChildren<RangedNinjaWeapon>(true).PickUpAmmo(1);
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }

    }

}
