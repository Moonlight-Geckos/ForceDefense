using UnityEngine;

public class ShieldReplenish : MonoBehaviour,IPickable
{
    bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;
        triggered = true;
        Pickup(other.transform);
    }

    public void Pickup(Transform effected)
    {
        Character character = effected.transform.root.GetComponentInChildren<Character>();
        character?.ReplenishShield();
        Destroy(gameObject);
    }
}
