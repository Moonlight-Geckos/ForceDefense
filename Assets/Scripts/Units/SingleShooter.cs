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

    #endregion

    private float startTime;
    private Transform startProjectileTransform;
    private Timer cooldownTimer;

    GameObject IEnemy.gameObject
    {
        get { return gameObject; }
    }

    private void Start()
    {
        startTime = Time.realtimeSinceStartup;
        startProjectileTransform = transform.Find("ProjectileStart");
        cooldownTimer = TimersPool.Pool.Get();
        cooldownTimer.Duration = Cooldown;
        cooldownTimer.AddTimerFinishedEventListener(Shoot);
        cooldownTimer.Run();
    }
    private void Update()
    {
        Quaternion newRotation = Quaternion.LookRotation(GameManager.PlayerPos + new Vector3(0, 0, 6) - transform.position, Vector3.up);
        transform.rotation = new Quaternion(transform.rotation.x, newRotation.y, transform.rotation.z, newRotation.w);
    }

    public void Shoot()
    {
        if (Vector3.Distance(GameManager.PlayerPos, transform.position) > radiusOfAttack
            || transform.position.z < GameManager.PlayerPos.z + 5)
        {
            cooldownTimer.Run();
            return;
        }
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
