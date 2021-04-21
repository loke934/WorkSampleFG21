using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls the enemys spawning when orb is taken, listener to Unityevent Orb is taken in Orb script.
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int amoutOfSpawnedEnemies = 3;
    [SerializeField] private float randomRangeMin = -25f;
    [SerializeField] private float randomRangeMax = 25f;
    public void OrbPickedUpEventEnemySpawner()
    {
        for (int i = 0; i < amoutOfSpawnedEnemies; i++)
        {
            var randomSpawnPoint = new Vector3(Random.Range(randomRangeMin, randomRangeMax), 0, Random.Range(randomRangeMin, randomRangeMax));
            GameObject spawnEnemy = Instantiate(enemyPrefab, randomSpawnPoint, transform.rotation);
        }
    }
}
