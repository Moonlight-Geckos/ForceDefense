using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField]
    private MultiplierCustomization[] customizations;

    [SerializeField]
    private GameObjectPool gemPool;

    [SerializeField]
    private GameObjectPool multipliersPool;

    [SerializeField]
    private GameObject carriageBoss;

    [SerializeField]
    private GameObject castleObject;

    [SerializeField]
    private GameObject multiplierPlane;

    [SerializeField]
    [Range(0f, 40f)]
    private float bossDuration = 20f;

    [SerializeField]
    [Range(0f, 10f)]
    private int maxGemsPerBlock = 7;
    static private Vector3 limits;
    private Transform[] groundParts = new Transform[3];
    private int currentGroundPart = 0;
    private Timer bossTimer;
    private bool bossSpawned = true;
    private Queue<GameObject> muliplierPlanesQueue = new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < 3; i++)
        {
            groundParts[i] = transform.GetChild(i);
        }
        limits = transform.GetChild(0).GetComponent<MeshRenderer>().bounds.size;
        GameManager.SpawnBossEvent.AddListener(SpawnBoss);
        GameManager.SpawnCastleEvent.AddListener(SpawnCastle);

    }

    private void FixedUpdate()
    {
        if (GameManager.PlayerPos.z > groundParts[currentGroundPart].position.z + Limits.z)
        {
            Vector3 newPos = groundParts[currentGroundPart].position;
            newPos.z = groundParts[nextGroundIndex(currentGroundPart)].position.z + Limits.z*2;
            groundParts[currentGroundPart].position = newPos;
            if (bossSpawned)
            {
                if (muliplierPlanesQueue.Count > 6)
                {
                    muliplierPlanesQueue.Dequeue().GetComponent<IDisposable>().Dispose();
                    muliplierPlanesQueue.Dequeue().GetComponent<IDisposable>().Dispose();
                }
                SpawnMultipliers();
            }
            else
            {
                // Remove comment to auto spawn gems
                //SpawnGems();
            }
            currentGroundPart++;
            if (currentGroundPart > 2)
                currentGroundPart = 0;
            if(!GameManager.Started)
                GameManager.Started = true;
        }
    }

    private int nextGroundIndex(int ind)
    {
        if (ind < 2)
            return ind + 1;
        else
            return 0;
    }

    static public Vector3 Limits
    {
        get { return limits; }
    }

    private void SpawnGems()
    {
        float boundary = (limits.x) / 2 - 1;
        float padding = limits.z / maxGemsPerBlock;
        for (float i = -limits.z/2 + (GameManager.Started ? padding : 0); i < limits.z/2; i += padding)
        {
            GameObject gem = gemPool.Pool.Get();
            gem.transform.position = new Vector3(Random.Range(-boundary, boundary), 0, groundParts[currentGroundPart].transform.position.z + i);
        }
    }
    private void SpawnMultipliers()
    {
        int curCust = Mathf.Min(GameManager.CurrentMultiplier - 1, customizations.Length - 1);
        GameObject[] multipliers = new GameObject[2];
        for (float i = 0, j = -multiplierPlane.transform.localScale.z; i < 2; i++, j += multiplierPlane.transform.localScale.z * 2)
        {
            multipliers[(int)i] = multipliersPool.Pool.Get();
            multipliers[(int)i].transform.position = new Vector3(0, multipliers[(int)i].transform.position.y,
                groundParts[currentGroundPart].position.z + 5 * j
            );
            multipliers[(int)i].GetComponent<MultiplierPlane>().GetComponent<MultiplierPlane>().ChangeCustomization(customizations[curCust]);
            muliplierPlanesQueue.Enqueue(multipliers[(int)i]);
        }
    }
    private void SpawnBoss()
    {
        IEnumerator spawn()
        {
            yield return new WaitForSeconds(2);
            bossSpawned = true;
            GameObject bossEnemy = Instantiate(carriageBoss);
            bossEnemy.SetActive(true);
            bossEnemy.GetComponent<Carriage>().enabled = true;
            bossTimer = TimersPool.Pool.Get();
            bossTimer.Duration = bossDuration;
            bossTimer.AddTimerFinishedEventListener(() =>
            {
                if (GameManager.Started)
                    GameManager.SpawnCastleEvent.Invoke();
            });
            bossTimer.Run();
        }
        StartCoroutine(spawn());
    }
    private void SpawnCastle()
    {
        GameObject castle = Instantiate(castleObject);
        castle.SetActive(true);
        float distance = World.Limits.z * 2f;
        castle.transform.position = new Vector3(0, transform.position.y, GameManager.PlayerPos.z + distance);
    }
}
