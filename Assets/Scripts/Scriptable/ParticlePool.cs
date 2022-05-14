using UnityEngine;
using UnityEngine.Pool;


[CreateAssetMenu(menuName = "ParticlePool")]
public class ParticlePool : ScriptableObject
{
    [SerializeField]
    private bool makeParent = true;
    class ReturnToPool : MonoBehaviour, IDisposable
    {
        public ParticleSystem system;
        public IObjectPool<ParticleSystem> pool;

        void Start()
        {
            system = GetComponent<ParticleSystem>();
            var main = system.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

        void OnParticleSystemStopped()
        {
            Dispose();
        }

        public void Dispose()
        {
            pool.Release(system);
        }
    }
    [SerializeField]
    private GameObject particlesHead;

    [Range(10, 10000)]
    [SerializeField]
    int maxPoolSize = 10;

    private ObjectPool<ParticleSystem> _pool;
    private IObjectPool<ParticleSystem> Pool
    {
        get
        {
            if (_pool == null)
            {
                _pool = new ObjectPool<ParticleSystem>(
                    CreatePooledItem,
                    OnTakeFromPool, 
                    OnReturnedToPool,
                    OnDestroyPoolObject,
                    false,
                    maxPoolSize);
                GameManager.ClearPoolsEvent.AddListener(ClearPool);
            }
            return _pool;
        }
    }

    public Transform createItem(Transform transform, Vector3? forward = null)
    {
        
        var ps = Pool.Get();
        ps.transform.position = transform.position;
        if(forward != null)
            ps.transform.forward = (Vector3)forward;
        ps.Play();
        return ps.transform;
    }


    ParticleSystem CreatePooledItem()
    {
        GameObject obj = Instantiate(particlesHead);
        if (makeParent)
        {
            GameObject itemsParent = GameObject.Find(particlesHead.name + " Parent");
            if (!itemsParent)
            {
                itemsParent = new GameObject();
                itemsParent.name = particlesHead.name + " Parent";
            }

            obj.transform.parent = itemsParent.transform;
        }
        ParticleSystem particleSystem = obj.GetComponent<ParticleSystem>();
        particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        ReturnToPool returnToPool = obj.AddComponent<ReturnToPool>();
        returnToPool.pool = Pool;
        return particleSystem;
    }

    void OnReturnedToPool(ParticleSystem system)
    {
        system.gameObject.SetActive(false);
    }

    void OnTakeFromPool(ParticleSystem system)
    {
        system.gameObject.SetActive(true);
    }
    void OnDestroyPoolObject(ParticleSystem system)
    {
        Destroy(system);
    }
    void ClearPool()
    {
        _pool.Dispose();
        _pool.Clear();
    }
}
