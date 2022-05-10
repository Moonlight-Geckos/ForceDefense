using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private ParticlePool particlePool;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.root.gameObject.tag != "Player")
            return;
        try
        {
            collider.GetComponent<IDamagable>().Damage();
            GetComponent<IDisposable>().Dispose();
            particlePool.createItem(transform.position);
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
            Destroy(gameObject);
        }
    }
}
