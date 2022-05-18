
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
    public void Shoot(Vector3? direction);
}
public interface IEnemy
{
    public GameObject gameObject
    {
        get;
    }
    void OnDestroy();
}
public interface IPickable
{
    public void Pickup(Transform transform);
}
