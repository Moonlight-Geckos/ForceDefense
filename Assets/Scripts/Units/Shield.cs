using System.Collections;
using UnityEngine;

public enum ShieldPositioning
{
    Center,
    Left,
    Right
}
public class Shield : MonoBehaviour, IHittable
{
    #region Serialized

    [Range(1, 100)]
    [SerializeField]
    private float maxHealth = 4;

    [Range(0.10f, 0.90f)]
    [SerializeField]
    private float shrinkBy = 0.30f;

    [Header("Hittable")]

    [Range(0.10f, 0.90f)]
    [SerializeField]
    private float hitAnimationLength = 0.30f;

    [Range(0.0f, 6f)]
    [SerializeField]
    private float hitDuration = 2f;

    [SerializeField]
    private Color hitColor;

    [Space(20)]

    [Header("Reflection")]

    [Range(0.10f, 0.90f)]
    [SerializeField]
    private float reflectAnimationLength = 0.30f;

    [Range(0.0f, 6f)]
    [SerializeField]
    private float reflectionDuration = 2f;

    [SerializeField]
    private Color reflectionColor;

    [Space(20)]

    [SerializeField]
    private ParticlePool shieldExplosionParticlePool;

    [SerializeField]
    private HitglowEffect shieldGlowEffect;


    #endregion

    #region Private

    private float health;
    bool isReflecting = false;
    private Vector3 defaultScale = Vector3.one;
    private ShieldPositioning positioning;
    private Timer shieldReflectionTimer;

    #endregion

    #region Properties

    public float Health
    {
        get { return health; }
    }
    public float MaxHealth
    {
        get { return maxHealth; }
    }
    public ShieldPositioning Positioning
    {
        get { return positioning; }
    }

    #endregion

    #region Methods
    public bool IsReflecting
    {
        get { return isReflecting; }
        set { isReflecting = value; }
    }
    public void Initialize()
    {
        positioning = ShieldPositioning.Center;
        if(transform.localPosition.x < 0)
            positioning = ShieldPositioning.Left;
        else if(transform.localPosition.x > 0)
            positioning = ShieldPositioning.Right;
        defaultScale = transform.localScale; 
        health = maxHealth;
    }
    public void GetHit(Projectile projectile)
    {
        if(health <= 0) return;

        if (IsReflecting)
        {
            projectile.Reflect(transform.forward);
        }
        else
        {
            health -= projectile.Damage;
            projectile.Explode();
            if (health <= 0)
            {
                Destroy();
                return;
            }
            transform.localScale -= transform.localScale * shrinkBy;
            shieldGlowEffect?.HitActivate(hitAnimationLength, hitDuration, hitColor);
        }
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
        shieldExplosionParticlePool?.createItem(transform);
        shieldGlowEffect?.Reset();
        ResetShield();
    }

    public void ResetShield()
    {
        health = MaxHealth;
        transform.localScale = defaultScale;
        shieldGlowEffect?.Reset();
        shieldGlowEffect?.Reset();
    }

    public void Replenish()
    {
        health += MaxHealth / 2f;
        health = Mathf.Min(health, MaxHealth);
        Vector3 newScale = transform.localScale + (transform.localScale * 2 * shrinkBy);

        newScale.x = Mathf.Min(newScale.x, defaultScale.x);
        newScale.y = Mathf.Min(newScale.y, defaultScale.y);
        newScale.z = Mathf.Min(newScale.z, defaultScale.z);
        transform.localScale = newScale;
    }

    public void ActivateReflection()
    {
        void activate() {
            isReflecting = true;
            shieldGlowEffect?.HitActivate(reflectAnimationLength, reflectionDuration, reflectionColor);
            shieldReflectionTimer.Run();
        }

        if (shieldReflectionTimer == null)
        {
            shieldReflectionTimer = TimersPool.Pool.Get();
            shieldReflectionTimer.Duration = reflectionDuration;
            shieldReflectionTimer.AddTimerFinishedEventListener(stopReflection);
            activate();
        }

        if (shieldReflectionTimer.Running)
        {
            shieldReflectionTimer.Refresh();
            shieldGlowEffect?.HitActivate(reflectAnimationLength, reflectionDuration, reflectionColor);
        }
        else
            activate();
    }

    private void stopReflection()
    {
        isReflecting = false;
    }
    #endregion
}
