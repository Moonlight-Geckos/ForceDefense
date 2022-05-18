using UnityEngine;

public class ShieldReflection : MonoBehaviour, IPickable
{
    bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;
        triggered = true;
        Pickup(other.transform);
    }

    public void Pickup(Transform effectedTF)
    {
        Character character = effectedTF.root.GetComponentInChildren<Character>();
        character?.ActivateReflection();
        Destroy(gameObject);
    }
}
