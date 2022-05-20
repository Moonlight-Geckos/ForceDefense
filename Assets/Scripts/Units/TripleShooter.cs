using System.Collections;
using UnityEngine;

public class TripleShooter : MonoBehaviour, IShooter, IEnemy
{
    #region Serialized

    [SerializeField]
    private ProjectilePool projectilePool;

    [Range(1f, 100f)]
    [SerializeField]
    private float projectileSpeed = 10;

    [Range(0.05f, 20f)]
    [SerializeField]
    private float Cooldown = 2;

    [SerializeField]
    private float radiusOfAttack = 20;

    [SerializeField]
    [Range(0.05f, 30f)]
    private float zDifference = 8f;

    [SerializeField]
    private bool autoShoot = true;

    #endregion

    private Transform startCenterProjectilePos;
    private Transform startLeftProjectilePos;
    private Transform endLeftProjectilePos;
    private Transform startRightProjectilePos;
    private Transform endRightProjectilePos;
    private Timer cooldownTimer;
    GameObject IEnemy.gameObject
    {
        get { return gameObject; }
    }
    private void Start()
    {
        startCenterProjectilePos = transform.Find("ProjectileStart-Center");
        startLeftProjectilePos = transform.Find("ProjectileStart-Left");
        endLeftProjectilePos = transform.Find("ProjectileEnd-Left");
        startRightProjectilePos = transform.Find("ProjectileStart-Right");
        endRightProjectilePos = transform.Find("ProjectileEnd-Right");
        GameManager.FinishGameEvent.AddListener(OnFinish);

        if (!autoShoot)
            return;
        cooldownTimer = TimersPool.Pool.Get();
        cooldownTimer.Duration = Cooldown;
        cooldownTimer.AddTimerFinishedEventListener(AutoShoot);
        cooldownTimer.Run();
    }
    private void Update()
    {
        if (!autoShoot)
            return;
        Quaternion newRotation = Quaternion.LookRotation(GameManager.PlayerPos + new Vector3(0, 0, 6) - transform.position, Vector3.up);
        transform.rotation = new Quaternion(transform.rotation.x, newRotation.y, transform.rotation.z, newRotation.w);
    }

    public void Shoot(Vector3? direction)
    {
        if (direction == null)
        {
            direction = GameManager.PlayerPos;
        }
        Vector3 Direction = (Vector3)direction;
        GameObject[] projectiles = new GameObject[3];
        projectiles[0] = projectilePool.Pool.Get(); ;
        projectiles[0].transform.position = startCenterProjectilePos.position;
        projectiles[1] = projectilePool.Pool.Get(); ;
        projectiles[1].transform.position = startLeftProjectilePos.position;
        projectiles[2] = projectilePool.Pool.Get(); ;
        projectiles[2].transform.position = startRightProjectilePos.position;

        projectiles[0].GetComponent<Rigidbody>().velocity = transform.forward.normalized * projectileSpeed;
        projectiles[1].GetComponent<Rigidbody>().velocity = (endLeftProjectilePos.position - startLeftProjectilePos.position).normalized * projectileSpeed;
        projectiles[2].GetComponent<Rigidbody>().velocity = (endRightProjectilePos.position - startRightProjectilePos.position).normalized * projectileSpeed;
        IEnumerator showTrails()
        {
            yield return new WaitForEndOfFrame();
            foreach (GameObject obj in projectiles)
            {
                TrailRenderer tr = obj.GetComponent<TrailRenderer>();
                if (tr)
                    tr.emitting = true;
            }
        }
        StartCoroutine(showTrails());
        projectiles[0].SetActive(true);
        projectiles[1].SetActive(true);
        projectiles[2].SetActive(true);
    }
    public void OnDestroy()
    {
        if (cooldownTimer != null)
            TimersPool.Pool.Release(cooldownTimer);
    }

    private void AutoShoot()
    {
        if (!autoShoot)
            return;
        Vector3 Direction = (Vector3)GameManager.PlayerPos;
        if (Vector3.Distance(Direction, transform.position) > radiusOfAttack
            || Mathf.Abs(transform.position.z - Direction.z) < zDifference
            || transform.position.z < Direction.z)
        {
            cooldownTimer.Run();
            return;
        }
        Shoot(Direction);
        cooldownTimer.Run();

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radiusOfAttack);
    }

    public void OnFinish(bool win)
    {
        autoShoot = false;
    }
}
