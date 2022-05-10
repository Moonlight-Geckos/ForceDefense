using UnityEngine;
using UnityEngine.Pool;


[CreateAssetMenu(menuName = "ProjectilePool")]
public class ProjectilePool : ScriptableObject
{
    [SerializeField]
    private GameObject projectilePrefab;

    [Range(10, 10000)]
    [SerializeField]
    int maxPoolSize = 10;

    class ReturnToPool : MonoBehaviour, IDisposable
    {
        public ProjectilePool pool;
        public void Dispose()
        {
            pool.Pool.Release(gameObject);
        }
    }

    static private ObjectPool<GameObject> _pool;
    public IObjectPool<GameObject> Pool
    {
        get
        {
            if (_pool == null)
            {
                _pool = new ObjectPool<GameObject>(
                    CreatePooledItem,
                    OnTakeFromPool,
                    OnReturnedToPool,
                    OnDestroyPoolObject,
                    false,
                    10,
                    maxPoolSize);
            }
            return _pool;
        }
    }

    GameObject CreatePooledItem()
    {
        GameObject obj = Instantiate(projectilePrefab);
        obj.AddComponent<ReturnToPool>().pool = this;
        GameObject itemsParent = GameObject.Find(projectilePrefab.name + " Parent");
        if (!itemsParent)
        {
            itemsParent = new GameObject();
            itemsParent.name = projectilePrefab.name + " Parent";
        }
        obj.transform.parent = itemsParent.transform;
        return obj;
    }

    void OnReturnedToPool(GameObject system)
    {
        system.gameObject.SetActive(false);
    }

    void OnTakeFromPool(GameObject system)
    {
        system.gameObject.SetActive(true);
    }

    // If the pool capacity is reached then any items returned will be destroyed.
    // We can control what the destroy behavior does, here we destroy the GameObject.
    void OnDestroyPoolObject(GameObject system)
    {
        Destroy(system.gameObject);
    }
}
