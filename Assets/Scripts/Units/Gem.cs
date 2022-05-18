using UnityEngine;

public class Gem : MonoBehaviour, IPickable
{
    [SerializeField]
    private ParticlePool pickupParticlePool;
    public void Pickup(Transform transform)
    {
        pickupParticlePool.createItem(transform);
        GameManager.AddGem();
        IDisposable disposable;
        TryGetComponent<IDisposable>(out disposable);
        if (disposable != null)
            GetComponent<IDisposable>().Dispose();
        else Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Shield test;
        if (other.transform.TryGetComponent<Shield>(out test))
            return;
        Pickup(other.transform);
    }
}
