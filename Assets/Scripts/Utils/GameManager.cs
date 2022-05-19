using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    #region Serialized
    [SerializeField]
    private int[] maxMultipliers;


    #endregion

    #region Private

    private static Transform playerPos;
    private static GameManager _instance;
    private static UnityEvent<bool> finishGameEvent = new UnityEvent<bool>();
    private static UnityEvent clearPoolsEvent = new UnityEvent();
    private static UnityEvent spawnBossEvent = new UnityEvent();
    private static UnityEvent finalizeGame = new UnityEvent();
    private static UnityEvent spawnCastleEvent = new UnityEvent();
    private static int currentMultiplierInd = 0;
    private static int collectedGems;
    private static bool started;
    private static bool bossSpawned;

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
    static public bool BossSpawned
    {
        get { return bossSpawned; }
        set { bossSpawned = value; }
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
    static public UnityEvent FinalizeGame
    {
        get { return finalizeGame; }
    }
    public int CurrentMultiplier
    {
        get
        {
            return maxMultipliers[currentMultiplierInd];
        }
    }
    public int CurrentMultiplierInd
    {
        get
        {
            return currentMultiplierInd;
        }
    }
    public int MultipliersCount
    {
        get
        {
            return maxMultipliers.Length;
        }
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

    void Update()
    {
        TimersPool.UpdateTimers();
    }

    public void LoseGame()
    {
        started = false;
        finishGameEvent.Invoke(false);
    }
    public void AddGem()
    {
        collectedGems += CurrentMultiplier;
    }
    public void IncrementMultiplier()
    {
        if(currentMultiplierInd < maxMultipliers.Length - 1)
            currentMultiplierInd++;
        else
        {
            FinalizeGame.Invoke();
        }
    }
}
