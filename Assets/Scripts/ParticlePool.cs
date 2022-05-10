using UnityEngine;
using UnityEngine.Pool;


[CreateAssetMenu(menuName = "ParticlePool")]
public class ParticlePool : ScriptableObject
{
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

    static private ObjectPool<ParticleSystem> _pool;
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
                    10, 
                    maxPoolSize);
            }
            return _pool;
        }
    }

    public void createItem(Vector3 position)
    {
        var ps = Pool.Get();
        ps.transform.position = position;
        ps.Play();
    }


    ParticleSystem CreatePooledItem()
    {
        GameObject obj = Instantiate(particlesHead);
        GameObject itemsParent = GameObject.Find(particlesHead.name + " Parent");
        if (!itemsParent)
        {
            itemsParent = new GameObject();
            itemsParent.name = particlesHead.name + " Parent";
        }
        obj.transform.parent = itemsParent.transform;
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

    // If the pool capacity is reached then any items returned will be destroyed.
    // We can control what the destroy behavior does, here we destroy the GameObject.
    void OnDestroyPoolObject(ParticleSystem system)
    {
        Destroy(system.gameObject);
    }
}
