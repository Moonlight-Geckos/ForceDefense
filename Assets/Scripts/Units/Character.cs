using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour, IHittable
{

    #region Serialized

    [Range(1, 100)]
    [SerializeField]
    private float maxHealth = 20f;

    [Range(0.10f, 1f)]
    [SerializeField]
    private float hitAnimationLength = 0.2f;

    [Range(0.0f, 6f)]
    [SerializeField]
    private float hitDuration = 2f;

    [SerializeField]
    private ParticlePool characterExplosionPool;

    [SerializeField]
    private GameObject shieldsTransform;

    [SerializeField]
    private HitglowEffect hitEffect;

    [SerializeField]
    private HealthBar healthBar;

    #endregion
    private Shield[] shields;
    [SerializeField]
    private float health = 100f;

    private void Awake()
    {
        shields = shieldsTransform.GetComponentsInChildren<Shield>();
        foreach (Shield shield in shields)
        {
            shield.Initialize();
        }
        health = maxHealth;
    }
    private void Start()
    {
        foreach (Shield shield in shields)
        {
            shield.Initialize();
            if (shield.Positioning == ShieldPositioning.Center)
                shield.gameObject.SetActive(true);
            else
                shield.gameObject.SetActive(false);
        }
    }

    public void GetHit(Projectile projectile)
    {
        if (health <= 0)
        {
            Destroy();
            return;
        }
        health -= projectile.Damage;
        projectile.Explode();
        healthBar?.UpdateValue(health/maxHealth);
        hitEffect?.HitActivate(hitAnimationLength, hitDuration, null);
    }
    public void Destroy()
    {
        Camera.main.transform.parent = null;
        characterExplosionPool?.createItem(transform);
        GetComponent<BoxCollider>().enabled = false;
        healthBar?.ResetEffect();
        hitEffect?.Reset();
        GameManager.FinishGameEvent.Invoke(false);
    }

    public void ReplenishShield()
    {
        foreach (Shield shield in shields)
        {
            if (shield.gameObject.activeSelf && shield.Health < shield.MaxHealth)
            {
                shield.Replenish();
                return;
            }
        }
        List<Shield> randomShieldList = new List<Shield>();
        foreach (Shield shield in shields)
        {
            if (!shield.gameObject.activeSelf)
            {
                randomShieldList.Add(shield);
            }
        }
        if (randomShieldList.Count == 0)
            return;
        Shield newSh = randomShieldList[Random.Range(0, randomShieldList.Count)];
        if (randomShieldList.Count == 3)
            newSh.transform.parent.rotation = new Quaternion(0,0,0,0);
        newSh.ResetShield();
        newSh.gameObject.SetActive(true);
    }

    public void ActivateReflection()
    {
        foreach (Shield shield in shields)
        {
            if (shield.gameObject.activeSelf)
            {
                shield.ActivateReflection();
            }
        }
    }
}
