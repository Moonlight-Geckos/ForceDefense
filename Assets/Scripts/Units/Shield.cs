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

    [SerializeField]
    private Color hitColor = Color.red;

    [SerializeField]
    private Color reflectionColor = Color.blue;

    [Range(0.10f, 0.90f)]
    [SerializeField]
    private float shrinkBy = 0.30f;

    [Range(0.10f, 1f)]
    [SerializeField]
    private float animationLength = 0.2f;

    [SerializeField]
    private ParticlePool shieldExplosionParticlePool;

    #endregion

    #region Private

    private float health;
    private Material[] materials;
    private Color[] originalColors;
    bool isReflecting = false;
    private Vector3 defaultScale = Vector3.one;
    private ShieldPositioning positioning;
    private Timer shieldReflectionTimer;
    private Timer shieldHitTimer;

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
        materials = GetComponent<MeshRenderer>().materials;
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }
        defaultScale = transform.localScale;
        health = maxHealth;
    }
    public void GetHit(Projectile projectile)
    {
        if(health == 0) return;
        if (IsReflecting)
        {
            projectile.Reflect(transform.forward);
        }
        else
        {
            health -= projectile.Damage;
            if (health == 0)
            {
                Destroy();
                return;
            }
            void activate()
            {
                transform.localScale -= transform.localScale * shrinkBy;
                ChangeColor(hitColor);
                shieldHitTimer.Run();
            }
            if (shieldHitTimer == null)
            {
                shieldHitTimer = TimersPool.Pool.Get();
                shieldHitTimer.Duration = animationLength;
                shieldHitTimer.AddTimerFinishedEventListener(ResetColors);
                activate();
            }

            if (shieldHitTimer.Running)
                shieldHitTimer.Refresh();
            else
            {
                activate();
            }
            projectile.Explode();
        }
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
        shieldExplosionParticlePool?.createItem(transform);
        ResetShield();
    }

    public void ResetShield()
    {
        transform.localScale = defaultScale; 
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetColor("_Color", originalColors[i]);
        }
    }

    public void Replenish()
    {
        health += Mathf.Min(MaxHealth / 2, MaxHealth);
        transform.localScale += transform.localScale * shrinkBy;
    }

    public void ActivateReflection()
    {
        void activate() {
            isReflecting = true;
            ChangeColor(reflectionColor);
            shieldReflectionTimer.Run();
        }

        if (shieldReflectionTimer == null)
        {
            shieldReflectionTimer = TimersPool.Pool.Get();
            shieldReflectionTimer.Duration = 2f;
            shieldReflectionTimer.AddTimerFinishedEventListener(stopReflection);
            activate();
        }

        if (shieldReflectionTimer.Running)
            shieldReflectionTimer.Refresh();
        else
            activate();
    }

    private void stopReflection()
    {
        isReflecting = false;
        ResetColors();
    }
    private void ChangeColor(Color color)
    {
        foreach (Material mat in materials)
        {
            mat.color = color;
        }
    }
    private void ResetColors()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
    }
    #endregion
}
