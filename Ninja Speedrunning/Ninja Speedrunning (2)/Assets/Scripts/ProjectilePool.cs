using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    // Hur många projektiler vi tror att vi kommer ha i leveln samtidigt
    [SerializeField]
    private int PoolSize = 20;

    // Ditt prefab för spelarens projektiler
    [SerializeField]
    private GameObject playerProjectilePrefab;

    // Ditt prefab för enemys projektiler
    [SerializeField]
    private GameObject enemyProjectilePrefab;

    // Listor med alla projektiler som vi skapar
    private static List<GameObject> playerProjectilePool;
    private static List<GameObject> enemyProjectilePool;

    private void Awake()
    {
        // Här skapar vi alla projektiler när spelet startar

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
        // Skapar en kopia av prefab, sätter den till inaktiv och lägger till i poolen
        var playerProjectile = Instantiate(prefab);
        playerProjectile.gameObject.SetActive(false);
        playerProjectilePool.Add(playerProjectile);
        return playerProjectile;
    }

    private static GameObject CreateEnemyProjectile(GameObject prefab)
    {
        // Skapar en kopia av prefab, sätter den till inaktiv och lägger till i poolen
        var enemyProjectile = Instantiate(prefab);
        enemyProjectile.gameObject.SetActive(false);
        enemyProjectilePool.Add(enemyProjectile);
        return enemyProjectile;
    }

    //Kallas från där jag nu har Instantiate? Var blir de aktiva?
    public static GameObject GetEnemyProjectile(Vector3 position, Quaternion rotation)
    {
        GameObject projectileToReturn = null;
        // Loopar igenom alla projektiler i poolen för att hitta en projektil som inte är aktiv
        foreach (var enemyProjectile in enemyProjectilePool)
        {
            // Om den inte är aktiv så väljer vi den
            if (!enemyProjectile.activeInHierarchy)
            {
                projectileToReturn = enemyProjectile;
            }
        }
        // Om vi inte har hittat en inaktiv projektil betyder det att alla är upptagna/aktiva i leveln, då måste vi skapa en ny
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
