

using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField]
    private ProjectilePool projectilePool;

    float startTime;

    private void Start()
    {
        startTime = Time.realtimeSinceStartup;
    }
    private void Update()
    {
        if(Time.realtimeSinceStartup - startTime > 0.5f)
        {
            startTime = Time.realtimeSinceStartup;
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject newPrj = projectilePool.Pool.Get();
        newPrj.transform.position = new Vector3(transform.position.x + Random.Range(-8f, 8f), transform.position.y, transform.position.z);
        newPrj.GetComponent<Rigidbody>().velocity = new Vector3 (0, 0, -15);
    }
}
