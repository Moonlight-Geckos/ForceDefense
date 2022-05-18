using System.Collections;
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
        IEnumerator destroy()
        {
            yield return new WaitForSeconds(4);
            if(gameObject != null)
                Destroy(gameObject);
        }
        StartCoroutine(destroy());
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
