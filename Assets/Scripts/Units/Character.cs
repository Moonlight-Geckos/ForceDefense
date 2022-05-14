using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour, IHittable
{

    #region Serialized

    [Range(1, 100)]
    [SerializeField]
    private float health = 100;

    [SerializeField]
    private Color hitColor = Color.red;

    [Range(0.10f, 1f)]
    [SerializeField]
    private float animationLength = 0.2f;

    [SerializeField]
    private ParticlePool characterExplosionPool;

    #endregion

    private Material[] materials;
    private Color[] originalColors;
    private Shield[] shields;
    private Timer shieldHitTimer;

    private void Awake()
    {
        materials = GetComponent<MeshRenderer>().materials;
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }
        shields = transform.GetComponentsInChildren<Shield>();
        foreach (Shield shield in shields)
        {
            shield.Initialize();
        }
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
        if(health == 0) return;

        health-=projectile.Damage;
        projectile.Explode();
        if (health <= 0)
        {
            Destroy();
            return;
        }
        if (shieldHitTimer == null)
        {
            shieldHitTimer = TimersPool.Pool.Get();
            shieldHitTimer.Duration = animationLength;
            shieldHitTimer.AddTimerFinishedEventListener(ResetColors);
            ChangeColor(hitColor);
            shieldHitTimer.Run();
        }

        if (shieldHitTimer.Running)
            shieldHitTimer.Refresh();
        else
        {
            ChangeColor(hitColor);
            shieldHitTimer.Run();
        }
    }

    public void ChangeColor(Color color)
    {
        foreach (Material mat in materials)
        {
            mat.SetColor("_Color", color);
        }
    }
    public void ResetColors()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetColor("_Color", originalColors[i]);
        }
    }
    public void Destroy()
    {
        Camera.main.transform.parent = null;
        transform.root.gameObject.SetActive(false);
        characterExplosionPool?.createItem(transform);
        GameManager.FinishGame();
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
