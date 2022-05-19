using UnityEngine;

public class Barrier : MonoBehaviour
{
    private PlayerControl playerControl;
    private void OnCollisionEnter(Collision collision)
    {
        playerControl = collision.transform.GetComponent<PlayerControl>();
        playerControl?.HorizontalBoost(true);
    }
    private void OnCollisionExit(Collision collision)
    {
        playerControl?.HorizontalBoost(false);
    }
}
