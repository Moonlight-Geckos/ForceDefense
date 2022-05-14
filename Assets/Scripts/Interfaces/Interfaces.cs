
using System.Collections.Generic;
using UnityEngine;
public interface IHittable
{
    void GetHit(Projectile projectile);
    void Destroy();
}
public interface IDisposable
{
    void Dispose();
}
public interface IEffector
{
    void Effect(Transform effectedTF);
}
public interface IShooter
{
    public void Shoot();
}
public interface IEnemy
{
    public GameObject gameObject
    {
        get;
    }
    void OnDestroy();
}
public interface IPowerup
{
    public void Activate(Transform transform);
}
