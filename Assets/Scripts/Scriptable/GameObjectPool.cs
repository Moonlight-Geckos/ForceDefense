using UnityEngine;
using UnityEngine.Pool;


[CreateAssetMenu(menuName = "PickupPool")]
public class GameObjectPool : ScriptableObject
{
    #region Serialized

    [SerializeField]
    private GameObject pickupPrefab;

    [Range(10, 10000)]
    [SerializeField]
    int maxPoolSize = 100;

    #endregion

    private GameObject itemsParent;

    class ReturnToPool : MonoBehaviour, IDisposable
    {
        public GameObjectPool pool;
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
        GameObject obj = Instantiate(pickupPrefab);
        obj.AddComponent<ReturnToPool>().pool = this;
        if (!itemsParent)
        {
            itemsParent = GameObject.Find(pickupPrefab.name + " Parent");
            itemsParent = new GameObject();
            itemsParent.name = pickupPrefab.name + " Parent";
        }
        obj.transform.parent = itemsParent.transform;
        return obj;
    }

    void OnReturnedToPool(GameObject obj)
    {
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