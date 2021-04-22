using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [SerializeField] private int health = 200;
    public int GetHealth() { return health; }


    public void PlayerTakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;
            GlobalVariables.endgame = true;
        }
    }

    public bool IsAlive()
    {
        if (health > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "exit" && GlobalVariables.orbTaken)
        {
            GlobalVariables.endgame = true;
            GlobalVariables.victory = true;
        }
    }
}
