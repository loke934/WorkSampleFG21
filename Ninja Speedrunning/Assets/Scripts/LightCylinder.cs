using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The large lantern light on high altitude enemy. When player collides all ground-enemy start chase/attack.
public class LightCylinder : MonoBehaviour
{
    enum LanternPosition
    {
        firstLocation,
        otherLocation
    }
    LanternPosition state = LanternPosition.firstLocation;
    private Vector3 firstLocation;
    private Vector3 otherLocation;
    private float timer;

    [SerializeField] private float lengthOfSweep = 10f; 
    [SerializeField] private float timeToReachLocation = 6f;

    private void Start()
    {
        firstLocation = transform.localPosition;
        otherLocation = firstLocation + Vector3.right * lengthOfSweep;
    }

    private void Update()
    {
        DoLanternSweep();
    }

    void DoLanternSweep()
    {
        timer += Time.deltaTime;

        if (state == LanternPosition.firstLocation)
        {
            transform.localPosition = Vector3.Lerp(firstLocation, otherLocation, timer / timeToReachLocation);
            if (timer >= timeToReachLocation)
            {
                state = LanternPosition.otherLocation;
                timer = 0;
            }
        }

        else if (state == LanternPosition.otherLocation)
        {
            transform.localPosition = Vector3.Lerp(otherLocation, firstLocation, timer / timeToReachLocation);
            if (timer >= timeToReachLocation)
            {
                state = LanternPosition.firstLocation;
                timer = 0;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerStats>() != null)
        {
            GlobalVariables.player_reference = other.gameObject;
            GlobalVariables.allAttack = true;
        }
    }
}
