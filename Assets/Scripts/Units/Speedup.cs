using UnityEngine;

public class Speedup : MonoBehaviour, IPickable
{

    #region Serialized
    
    [Range(0f, 10f)]
    [SerializeField]
    private float speedupValue = 1;

    #endregion
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
        PlayerControl control = effectedTF.root.GetComponent<PlayerControl>();
        control?.Speedup(speedupValue);
        Destroy(gameObject);
    }
}
