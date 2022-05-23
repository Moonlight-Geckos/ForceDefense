using UnityEngine;
using UnityEngine.Pool;


public static class TimersPool
{
    static int maxPoolSize = 100;

    static private LinkedPool<Timer> _pool;

    static public IObjectPool<Timer> Pool
    {
        get
        {
            if (_pool == null)
            {
                _pool = new LinkedPool<Timer>(
                    CreatePooledItem,
                    OnTakeFromPool,
                    OnReturnedToPool,
                    null,
                    false,
                    maxPoolSize
                );
                GameManager.ClearPoolsEvent.AddListener(ClearPool);
            }
            return _pool;
        }
    }

    static Timer CreatePooledItem()
    {
        Timer timer = new Timer();
        return timer;
    }

    static void OnReturnedToPool(Timer timer)
    {
        timer.Available = false;
        timer.Stop();
    }

    static void OnTakeFromPool(Timer timer)
    {
        timer.Available = true;
        timer.Stop();
    }

    static public void UpdateTimers(float t)
    {
        Timer.UpdateTimers(t);
    }

    static void ClearPool()
    {
        _pool.Clear();
    }

}