using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    #region Serialized
    [SerializeField]
    private int[] maxMultipliers;

    [SerializeField]
    private GameObject[] skins;


    #endregion

    #region Private

    private static Transform playerPos;
    private static Transform bossTrigerPos;
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
    private static float toMultiplierProgress;
    private static float toFinishProgress = 0;
    private float toMultiplierDistance = 1;
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

    public float ToMultiplierProgress
    {
        get
        {
            return toMultiplierProgress;
        }
    }
    public float ToFinishProgress
    {
        get
        {
            return toFinishProgress;
        }
    }
    public int CollectedGems
    {
        get
        {
            return collectedGems;
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
        if (!PlayerPrefs.HasKey("Skin"))
        {
            PlayerPrefs.SetInt("Skin", 0);
        }
        clearPoolsEvent.Invoke();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        int ind = PlayerPrefs.GetInt("Skin");
        GameObject skin;
        if (ind < skins.Length)
            skin = Instantiate(skins[2]);
        else
            skin = Instantiate(skins[0]);
        skin.transform.parent = playerPos.GetComponentInChildren<Character>().transform;
        skin.transform.localPosition = Vector3.zero;
        bossTrigerPos = GameObject.FindGameObjectWithTag("BossTrigger").transform;
        toMultiplierDistance = Vector3.Distance(playerPos.position, bossTrigerPos.position);
        toFinishProgress = 0;
        toMultiplierProgress = 0;
        currentMultiplierInd = 0;
        collectedGems = 0;
        started = false;
        bossSpawned = false;
        SpawnCastleEvent.AddListener(() =>
        {
            toFinishProgress = 1;
        });
        finishGameEvent.AddListener((bool win) =>
        {
            collectedGems *= CurrentMultiplier;
            int prevGems = PlayerPrefs.GetInt("Gems");
            PlayerPrefs.SetInt("Gems", prevGems + collectedGems);
        });
    }

    void Update()
    {
        TimersPool.UpdateTimers();
        HeadUI.Instance.UpdateProgress();
        if (bossTrigerPos != null && playerPos.position.z <= bossTrigerPos.position.z)
            toMultiplierProgress = 1 - (Vector3.Distance(playerPos.position, bossTrigerPos.position) / toMultiplierDistance);
        else
            toMultiplierProgress = 1;
    }
    public void AddGem()
    {
        HeadUI.Instance.UpdateGems();
        collectedGems++;
    }
    public void IncrementMultiplier()
    {
        if(currentMultiplierInd < maxMultipliers.Length - 1)
            currentMultiplierInd++;
        else
        {
            FinalizeGame.Invoke();
        }
        toFinishProgress = Mathf.Pow((float)(currentMultiplierInd) / maxMultipliers.Length, 2);
    }
}
