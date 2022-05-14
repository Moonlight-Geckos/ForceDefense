using UnityEngine;

public class ShieldReplenish : MonoBehaviour,IPowerup
{
    bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;
        triggered = true;
        Activate(other.transform);
    }

    public void Activate(Transform effected)
    {
        Character character = effected.transform.root.GetComponentInChildren<Character>();
        character.ReplenishShield();
        Destroy(gameObject);
    }
}
