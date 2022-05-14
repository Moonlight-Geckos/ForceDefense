using UnityEngine;

public class ShieldReflection : MonoBehaviour, IPowerup
{
    bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;
        triggered = true;
        Activate(other.transform);
    }

    public void Activate(Transform effectedTF)
    {
        Character character = effectedTF.root.GetComponentInChildren<Character>();
        character?.ActivateReflection();
        Destroy(gameObject);
    }
}
