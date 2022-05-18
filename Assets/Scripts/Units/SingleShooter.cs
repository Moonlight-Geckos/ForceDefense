using System.Collections;
using UnityEngine;

public class SingleShooter : MonoBehaviour, IShooter, IEnemy
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

    private Transform startProjectileTransform;
    private Timer cooldownTimer;

    GameObject IEnemy.gameObject
    {
        get { return gameObject; }
    }

    private void Start()
    {
        startProjectileTransform = transform.Find("ProjectileStart");
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
        GameObject newPrj = projectilePool.Pool.Get();
        newPrj.transform.position = startProjectileTransform.position;
        newPrj.GetComponent<Rigidbody>().velocity = transform.forward.normalized * projectileSpeed;
        newPrj.transform.rotation = Quaternion.LookRotation(newPrj.GetComponent<Rigidbody>().velocity, Vector3.up);
        IEnumerator showTrails()
        {
            yield return new WaitForEndOfFrame();
            TrailRenderer tr = newPrj.GetComponent<TrailRenderer>();
            if (tr)
                tr.emitting = true;
        }
        StartCoroutine(showTrails());
    }

    private void AutoShoot()
    {
        Vector3 Direction = GameManager.PlayerPos;
        if (autoShoot && Vector3.Distance(Direction, transform.position) > radiusOfAttack
            || Mathf.Abs(transform.position.z - Direction.z) < zDifference
            || transform.position.z < Direction.z)
        {
            cooldownTimer.Run();
            return;
        }
        Shoot(Direction);
        cooldownTimer.Run();

    }
    public void OnDestroy()
    {
        if(cooldownTimer != null)
            TimersPool.Pool.Release(cooldownTimer);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radiusOfAttack);
    }
}
