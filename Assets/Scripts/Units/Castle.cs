using UnityEngine;

public class Castle : MonoBehaviour
{
    private float distance;
    private void Awake()
    {
        distance = World.Limits.z * 2f;
        transform.position = new Vector3(0, transform.position.y, GameManager.PlayerPos.z + distance);
    }
}
