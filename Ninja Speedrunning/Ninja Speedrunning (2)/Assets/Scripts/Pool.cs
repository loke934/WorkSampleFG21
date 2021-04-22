using System;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    EnemyProjectile,
    PlayerProjectile,
    // nåt annat?
}

public interface IPoolObject
{
    bool IsAvailable { get; }
    void Return();
}

[Serializable]
public struct PoolSetting
{
    public PoolType PoolType;
    public int PoolSize;
    public PoolBehaviour PoolPrefab;
}

public abstract class PoolBehaviour : MonoBehaviour, IPoolObject
{
    private Pool m_pool;

    public bool IsAvailable => this.gameObject.activeInHierarchy;

    protected abstract void OnEnable();

    public void Return()
    {
        this.gameObject.SetActive(false);
    }
}


/*public class EnemyProjectile : PoolBehaviour
{
    protected override void OnEnable()
    {
        // Här behöver du resetta allt som behöver nollställas
        // tex rigidbody.angularVelocity och rigidbody.velocity bör sättas till Vecto3.zero;
        // kan vara fler saker som behöver nollställas här
    }
}
*/
public class Pool : MonoBehaviour
{
    private static Dictionary<PoolType, List<PoolBehaviour>> s_pools = new Dictionary<PoolType, List<PoolBehaviour>>();
    private static Pool instance;

    [SerializeField]
    private List<PoolSetting> m_poolSettings;

    private void Awake()
    {
        instance = this;
        CreatePools();
    }

    private void CreatePools()
    {
        // Skapa alla objekt här här
  
        // Loopa igenom alla poolSettings och skapa rätt antal objekt, lägg sedan till dom till s_pools

    }

    public static T GetPoolObject<T>(PoolType type, Vector3 position, Quaternion rotation) where T : Component
    {
        return GetPoolObject(type, position, rotation).GetComponent<T>();
    }

    public static PoolBehaviour GetPoolObject(PoolType type, Vector3 position, Quaternion rotation)
    {
        return instance.InternalGetPoolObject(type, position, rotation);
    }

    private PoolBehaviour InternalGetPoolObject(PoolType type, Vector3 position, Quaternion rotation)
    {
        return null;
        // Loopa igenom objekten i s_pools[type]
        // Det första som är IsAvailable returnerar du och sätter poolObject.gameObject.SetActive(true) samt sätter position/rotation på det
        // Finns inget objekt som är IsAvailable måste du skapa ett nytt objekt (Instantiate) och lägga till i poolen
    }
}


// Där du tidigare skapat en projectile typ 
//      Instantiate(projectilePrefab, position, rotation);
// Ersätter du nu med t.ex
//      Pool.GetPoolObject(PoolType.EnemyProjectile, position, rotation);
//      Pool.GetPoolObject<EnemyProjectile>(PoolType.EnemyProjectile, position, rotation);
// Och där du har gjort t.ex
//      Destroy(projectile);
//      Destroy(this);
// Gör du istället
//      projectile.Return();
//      this.Return();
// Alla klasserna här inne bör ligga i separata filer tex Pool.cs/PoolType.cs/PoolBehaviour.cs osv