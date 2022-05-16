
using UnityEngine;

public class PowerupGlow : MonoBehaviour
{
    Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        Vector3 position = (mainCamera.transform.forward) + transform.parent.position + (new Vector3(0, 2));
        transform.position = position;
    }
}
