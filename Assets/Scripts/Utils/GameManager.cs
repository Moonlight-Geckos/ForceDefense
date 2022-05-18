using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    #region Serialized
    [SerializeField]
    [Range(0f, 40f)]
    private float maxMultiplier = 11f;

    [SerializeField]
    [Range(0f, 0.4f)]
    private float multiplierSpeedPerSecond = 0.7f;

    [SerializeField]
    [Range(0f, 40f)]
    private float bossDuration = 20f;

    #endregion

    #region Private

    private static Transform playerPos;
    private static Transform bossEnemy;
    private static GameManager _instance;
    private static UnityEvent<bool> finishGameEvent = new UnityEvent<bool>();
    private static UnityEvent clearPoolsEvent = new UnityEvent();
    private static UnityEvent spawnBossEvent = new UnityEvent();
    private static UnityEvent spawnCastleEvent = new UnityEvent();
    private static float currentMultiplier = 1;
    private static int collectedGems;
    private static bool started;

    #endregion

    static public Vector3 PlayerPos
    {
        get { return playerPos.position; }
    }
    static public bool Started
    {
        get { return started; }
        set { started = value; }
    }
    static public GameManager Instance
    {
        get { return _instance; }
    }
    static public UnityEvent<bool> FinishGameEvent
    {
        get { return finishGameEvent; }
    }
    static public UnityEvent ClearPoolsEvent
    {
        get { return clearPoolsEvent; }
    }
    static public UnityEvent SpawnBossEvent
    {
        get { return spawnBossEvent; }
    }
    static public UnityEvent SpawnCastleEvent
    {
        get { return spawnCastleEvent; }
    }
    static public int CurrentMultiplier
    {
        get
        {
            return (int)currentMultiplier;
        }
    }

    public float BossDuration
    {
        get { return bossDuration; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
        clearPoolsEvent.Invoke();
        playerPos = GameObject.FindGameObjectWithTag("Character").transform;
    }

    void FixedUpdate()
    {
        TimersPool.UpdateTimers();
        if(!started)
            currentMultiplier += Time.fixedDeltaTime * maxMultiplier / bossDuration;
    }

    public static void LoseGame()
    {
        started = false;
        finishGameEvent.Invoke(false);
    }
    public static void AddGem()
    {
        collectedGems += CurrentMultiplier;
    }
}
