using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private AudioSource bulletImpact;
    [SerializeField] private ParticleSystem bulletExplotion;
    [SerializeField] private int dealsDamage = 25;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerStats>() != null)
        {
            bulletImpact.Play();
            collision.gameObject.GetComponent<PlayerStats>().PlayerTakeDamage(dealsDamage);
            dealsDamage = 0;
        }
        StartCoroutine(CollisionRutine());
    }

    private IEnumerator CollisionRutine()
    {
        bulletExplotion.Play();
        while (bulletExplotion.isPlaying)
        {
            yield return null;
        }
        //Destroy(gameObject);
        gameObject.SetActive(false);

    }
}
