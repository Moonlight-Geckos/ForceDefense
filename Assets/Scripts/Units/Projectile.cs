using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region Serialized

    [SerializeField]
    private float damage = 1;
    [SerializeField]
    private ParticlePool particlePool;
    [SerializeField]
    private ProjectileType projectileType;

    #endregion

    Rigidbody rb;
    bool reflected = true;
    bool triggered=false;
    public float Damage
    {
        get { return damage; }
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (triggered)
            return;
        triggered = true;
        IHittable hittable = collider.GetComponent<IHittable>();
        if (hittable != null)
            hittable.GetHit(this);
        else
            Explode();
    }

    public void Reflect(Vector3 direction)
    {
        if (reflected)
            return;
        triggered = false;
        if(rb == null)
            rb = GetComponent<Rigidbody>();
        Vector3 newVel = Vector3.Reflect(rb.velocity, direction)*2;
        rb.velocity = newVel;
        transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.up);
        gameObject.layer = LayerMask.NameToLayer("Character");
        reflected = true;
    }

    public void Explode()
    {
        particlePool?.createItem(transform);
        GetComponent<IDisposable>().Dispose();
    }
    void OnEnable()
    {
        triggered = false;
        reflected = false;
    }
}
