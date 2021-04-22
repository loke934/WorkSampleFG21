using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Orb : MonoBehaviour
{
    [SerializeField] private UnityEvent OnOrbPickedUp;
    [SerializeField] private GameObject orb;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerStats>() != null)
        {
            OnOrbPickedUp.Invoke();
            GlobalVariables.orbTaken = true;
            Destroy(gameObject);
            orb.gameObject.SetActive(true);
        }
    }
}
