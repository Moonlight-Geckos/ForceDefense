using UnityEngine;

public class Speedup : MonoBehaviour, IPowerup
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
        Activate(other.transform);
    }

    public void Activate(Transform effectedTF)
    {
        PlayerControl control = effectedTF.root.GetComponent<PlayerControl>();
        control?.Speedup(speedupValue);
        Destroy(gameObject);
    }
}
