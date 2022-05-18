using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class WorldTriggers : MonoBehaviour
{
    [SerializeField]
    private UnityEvent Action;

    bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;
        triggered = true;
        Action.Invoke();
        Destroy(gameObject);
    }

    public void SpawnBoss()
    {
        GameManager.SpawnBossEvent.Invoke();
    }
    public void WinGame()
    {
        GameManager.FinishGameEvent.Invoke(true);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, GetComponent<BoxCollider>().bounds.size);
    }
}
