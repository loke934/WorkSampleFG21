using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHighAltitude : MonoBehaviour
{
    [SerializeField] private int health = 100;
   
    enum Patrolling
    {
       firstLocation,
       otherLocation
    }
    Patrolling state = Patrolling.firstLocation;
    private Vector3 firstLocation;
    private Vector3 otherLocation;
    private float timer;

    [SerializeField] private float lengthOfPatrolling = 20f; //Adjuste to the lenth of the platform!!!
    [SerializeField] private float timeToReachLocation = 6f;

    //ModifyLanternLight
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject lantern;
    [SerializeField] private Light lanternlight;
    private float FOV = 30f;
    private float viewDistance = 30f;

    //Die
    [SerializeField] private AudioSource dieSound;

    void Start()
    {
        ModifyLanternLight(FOV, viewDistance, Color.yellow);
        firstLocation = transform.localPosition;
        otherLocation = firstLocation + transform.right * lengthOfPatrolling;
    }

    void Update()
    {
        if (health > 0 && !GlobalVariables.gamepaused)
        {
            DoPatrolling();

            if (GlobalVariables.allAttack)
            {
                ModifyLanternLight(FOV, viewDistance, Color.red);
            }
        }
    }

    void DoPatrolling()
    {
        timer += Time.deltaTime;

        if (state == Patrolling.firstLocation)
        {
            transform.localPosition = Vector3.Lerp(firstLocation, otherLocation, timer / timeToReachLocation);
            if (timer >= timeToReachLocation)
            {
                state = Patrolling.otherLocation;
                timer = 0;
            }
        }

        else if (state == Patrolling.otherLocation)
        {
            transform.localPosition = Vector3.Lerp(otherLocation, firstLocation, timer / timeToReachLocation);
            if (timer >= timeToReachLocation)
            {
                state = Patrolling.firstLocation;
                timer = 0;
            }
        }
    }

    public void EnemyTakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    private void Die()
    {
        dieSound.Play();
        lanternlight.enabled = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;
    }

    private void ModifyLanternLight(float fov, float range, Color color)
    {
        FOV = fov;
        viewDistance = range;
        lanternlight.color = color;
        lanternlight.spotAngle = fov;
        lanternlight.range = range;
    }
}
