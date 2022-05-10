using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour
{
    #region Serialized

    [Range(1f, 20f)]
    [SerializeField]
    float passiveVerticalSpeed = 7;

    [Range(1f, 20f)]
    [SerializeField]
    float additiveVerticalSpeed = 8;

    [Range(1f, 20f)]
    [SerializeField]
    float horizontalSpeed = 4;

    [Range(1f, 20f)]
    [SerializeField]
    float rotationSpeed = 4;
    #endregion

    Joystick joystick;
    Rigidbody mainRigidbody;
    private Transform character;
    private void Awake()
    {
        joystick = FindObjectOfType<Joystick>();
        mainRigidbody = GetComponent<Rigidbody>();
        character = transform.Find("Character");
        mainRigidbody.velocity = new Vector3(0, 0, passiveVerticalSpeed);
    }

    private void Update()
    {
        float horizontalMove = joystick.Horizontal;
        float verticalMove = joystick.Vertical;

        Vector3 newVelocity = new Vector3(
            horizontalMove * horizontalSpeed,
            0,
            (additiveVerticalSpeed * verticalMove) + passiveVerticalSpeed
        );

        mainRigidbody.velocity = newVelocity;

        if (joystick.Horizontal == 0 && joystick.Vertical == 0)
            return;
        float sign = (joystick.Direction.x < new Vector2(0, 1).x) ? -1.0f : 1.0f;
        float angle = Vector3.Angle(joystick.Direction, new Vector2(0, 1)) * sign;
        character.rotation = Quaternion.Slerp(
            character.rotation,
            Quaternion.AngleAxis(angle, Vector3.up),
            Time.deltaTime * rotationSpeed);
    }
}
