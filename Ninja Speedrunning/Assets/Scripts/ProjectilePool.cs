using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private int PoolSize = 20;

    [SerializeField] private GameObject playerProjectilePrefab;
    [SerializeField] private GameObject enemyProjectilePrefab;

    private static List<GameObject> playerProjectilePool;
    private static List<GameObject> enemyProjectilePool;
    private void Awake()
    {
        playerProjectilePool = new List<GameObject>(PoolSize);
        enemyProjectilePool = new List<GameObject>(PoolSize);

        for (var i = 0; i < PoolSize; i++)
        {
            CreateEnemyProjectile(enemyProjectilePrefab);
            CreatePlayerProjectile(playerProjectilePrefab);
        }
    }

    private static GameObject CreatePlayerProjectile(GameObject prefab)
    {
        var playerProjectile = Instantiate(prefab);
        playerProjectile.gameObject.SetActive(false);
        playerProjectilePool.Add(playerProjectile);
        return playerProjectile;
    }

    private static GameObject CreateEnemyProjectile(GameObject prefab)
    {
        var enemyProjectile = Instantiate(prefab);
        enemyProjectile.gameObject.SetActive(false);
        enemyProjectilePool.Add(enemyProjectile);
        return enemyProjectile;
    }
    
    public static GameObject GetEnemyProjectile(Vector3 position, Quaternion rotation)
    {
        GameObject projectileToReturn = null;
        foreach (var enemyProjectile in enemyProjectilePool)
        {
            if (!enemyProjectile.activeInHierarchy)
            {
                projectileToReturn = enemyProjectile;
            }
        }
        if (projectileToReturn == null)
        {
            projectileToReturn = CreateEnemyProjectile(enemyProjectilePool[0]);
        }
        projectileToReturn.transform.position = position;
        projectileToReturn.transform.rotation = rotation;
        projectileToReturn.gameObject.SetActive(true);
        return projectileToReturn;
    }

    public static GameObject GetPlayerProjectile(Vector3 position, Quaternion rotation)
    {
        GameObject projectileToReturn = null;
        foreach (var playerProjectile in playerProjectilePool)
        {
            if (!playerProjectile.activeInHierarchy)
            {
                projectileToReturn = playerProjectile;
            }
        }

        if (projectileToReturn == null)
        {
            projectileToReturn = CreatePlayerProjectile(playerProjectilePool[0]);
        }
        projectileToReturn.transform.position = position;
        projectileToReturn.transform.rotation = rotation;
        projectileToReturn.gameObject.SetActive(true);
        return projectileToReturn;
    }
}
