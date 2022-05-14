using UnityEngine;
using UnityEngine.Pool;


[CreateAssetMenu(menuName = "ProjectilePool")]
public class ProjectilePool : ScriptableObject
{
    #region Serialized

    [SerializeField]
    private GameObject projectilePrefab;

    [Range(10, 10000)]
    [SerializeField]
    int maxPoolSize = 100;

    #endregion

    private GameObject itemsParent;

    class ReturnToPool : MonoBehaviour, IDisposable
    {
        public ProjectilePool pool;
        public void Dispose()
        {
            pool.Pool.Release(gameObject);
        }
    }

    private ObjectPool<GameObject> _pool;
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
                    true,
                    maxPoolSize);
                GameManager.ClearPoolsEvent.AddListener(ClearPool);
            }

            return _pool;
        }
    }

    GameObject CreatePooledItem()
    {
        GameObject obj = Instantiate(projectilePrefab);
        obj.AddComponent<ReturnToPool>().pool = this;
        if (!itemsParent)
        {
            itemsParent = GameObject.Find(projectilePrefab.name + " Parent");
            itemsParent = new GameObject();
            itemsParent.name = projectilePrefab.name + " Parent";
        }
        obj.transform.parent = itemsParent.transform;
        return obj;
    }

    void OnReturnedToPool(GameObject obj)
    {
        if (obj == null)
            return;
        TrailRenderer tr = obj.GetComponent<TrailRenderer>();
        if (tr)
            tr.emitting = false;
        obj.gameObject.SetActive(false);
    }

    void OnTakeFromPool(GameObject obj)
    {
        obj.gameObject.SetActive(true);
    }

    // If the pool capacity is reached then any items returned will be destroyed.
    // We can control what the destroy behavior does, here we destroy the GameObject.
    void OnDestroyPoolObject(GameObject obj)
    {
        Destroy(obj.gameObject);
    }

    private void ClearPool()
    {
        _pool.Clear();
    }
}