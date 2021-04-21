using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeleeNinjaWeapon : MonoBehaviour
{
    [SerializeField] private ParticleSystem bloodEffect;

    [SerializeField] private int dealDamage = 25;

    [SerializeField] private float distance = 1f;
    [SerializeField] private float time = 0.12f;
    private bool attack = false;
    private float timer;
    private Vector3 startPosition;
    private Vector3 endPosition;
    enum katanaPosition
    {
        moveForward,
        moveBackward,
        done,
    }
   katanaPosition state = katanaPosition.done;

    void Start()
    {
        startPosition = transform.localPosition;
        endPosition = startPosition + Vector3.forward * distance;
    }

    void Update()
    {
        if (state != katanaPosition.done)
        {
            Stab();
        }
    }

    public void UseMeleeWeapon()
    {
        attack = true;
        state = katanaPosition.moveForward;           
    }
    private void Stab()
    {
        timer += Time.deltaTime;
        if (state == katanaPosition.moveForward)
        {
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, timer / time);
            if (timer >= time)
            {
                state = katanaPosition.moveBackward;
                timer = 0;
            }
        }
        else if (state == katanaPosition.moveBackward)
        {
            transform.localPosition = Vector3.Lerp(endPosition, startPosition, timer / time);
            if (timer >= time)
            {
                state = katanaPosition.done;
                timer = 0;
                attack = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (attack == true && other.gameObject.GetComponent<Enemy>() != null)
        {
            bloodEffect.Play();
            other.gameObject.GetComponent<Enemy>().EnemyTakeDamage(dealDamage);
            attack = false;
        }
    }

}
