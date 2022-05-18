using UnityEngine;

public class Carriage : MonoBehaviour, IEnemy
{
    [SerializeField]
    private float desiredZPosition = 30;

    [SerializeField]
    [Range(0.5f, 10f)]
    private float animationSpeed = 2f;

    [SerializeField]
    [Range(0.5f, 10f)]
    private float shootCooldown = 2f;

    [SerializeField]
    private ParticlePool explosionParticlePool;

    private bool isShooting = false;
    private float distance;
    private float startTime;
    private SingleShooter[] shooters = new SingleShooter[3];
    private bool initialized = false;
    public void Initialize()
    {
        initialized = true;
        distance = World.Limits.z * 4;
        transform.position = new Vector3(0, transform.position.y, GameManager.PlayerPos.z + distance);
        shooters = GetComponentsInChildren<SingleShooter>();
        GameManager.SpawnCastleEvent.AddListener(Kill);
        startTime = Time.timeSinceLevelLoad;
    }

    private void Update()
    {
        if (!initialized)
            Initialize();
        transform.position = new Vector3(0, transform.position.y, GameManager.PlayerPos.z +  distance);
        if (distance > desiredZPosition + 0.2f)
            distance = Mathf.Lerp(distance, desiredZPosition,Time.deltaTime / animationSpeed);
        else
        {
            if (!isShooting)
            {
                startTime = Time.timeSinceLevelLoad;
            }
            isShooting = true;
        }
        if (isShooting)
        {
            if(Time.timeSinceLevelLoad - startTime > shootCooldown)
            {
                int f = Random.Range(0, 3);
                int s = Random.Range(0, 3);
                int maxRandoms = 10;
                while(s == f && maxRandoms > 0)
                {
                    s = Random.Range(0, 2);
                    maxRandoms--;
                }
                if(s == f)
                {
                    if (f < 2)
                        s = f + 1;
                    else
                        s = f - 1;
                }
                shooters[f].Shoot(shooters[f].transform.forward);
                shooters[s].Shoot(shooters[s].transform.forward);
                startTime = Time.timeSinceLevelLoad;
            }
        }
    }

    void Kill()
    {
        explosionParticlePool.createItem(transform);
        Destroy(gameObject);
    }

    void IEnemy.OnDestroy()
    {
        //Nothing to do yet
    }
}
