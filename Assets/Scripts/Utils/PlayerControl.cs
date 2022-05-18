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

    [SerializeField]
    private ParticlePool speedupParticlePool;

    [SerializeField]
    private Transform shieldsTransform;

    [SerializeField]
    private Transform characterTransform;

    [SerializeField]
    private Transform camerasTransform;

    #endregion

    private Joystick joystick;
    private Rigidbody mainRigidbody;
    private Animator animator;
    private Animator cameraAnimator;

    private void Awake()
    {
        joystick = FindObjectOfType<Joystick>();
        mainRigidbody = GetComponent<Rigidbody>();
        animator = characterTransform.GetComponent<Animator>();
        cameraAnimator = camerasTransform.GetComponent<Animator>();
        GameManager.FinishGameEvent.AddListener(finishGame);
        GameManager.SpawnBossEvent.AddListener(BossMode);
    }
    private void Start()
    {
        mainRigidbody.velocity = new Vector3(0, 0, passiveVerticalSpeed);
    }

    private void Update()
    {
        float horizontalMove = joystick.Horizontal;
        float verticalMove = joystick.Vertical;

        if (verticalMove < 0)
        {
            verticalMove *= 0.5f;
        }

        Vector3 newVelocity = new Vector3(
            horizontalMove * horizontalSpeed,
            0,
            (additiveVerticalSpeed * verticalMove) + passiveVerticalSpeed
        );

        mainRigidbody.velocity = newVelocity;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -World.Limits.x/2, World.Limits.x/2),
            transform.position.y,
            transform.position.z
        );

        float sign = (joystick.Direction.x < new Vector2(0, 1).x) ? -1.0f : 1.0f;
        float angle = Vector3.Angle(joystick.Direction, new Vector2(0, 1)) * sign;

        //Clamp angle for character only
        characterTransform.rotation = Quaternion.Slerp(
            characterTransform.rotation,
            Quaternion.AngleAxis(Mathf.Clamp(angle, -35f, 35f), Vector3.up),
            Time.deltaTime * rotationSpeed);

        if (joystick.Horizontal == 0 && joystick.Vertical == 0)
            return;
        shieldsTransform.rotation = Quaternion.Slerp(
            shieldsTransform.rotation,
            Quaternion.AngleAxis(angle, Vector3.up),
            Time.deltaTime * rotationSpeed);

    }

    public void Speedup(float addition)
    {
        Transform t = speedupParticlePool.createItem(transform);
        t.parent = transform;
        horizontalSpeed += addition;
        passiveVerticalSpeed += addition;
        additiveVerticalSpeed += addition;
        animator.speed += 0.05f;
    }

    private void finishGame(bool win)
    {
        mainRigidbody.velocity = Vector3.zero;
        animator.speed = 1;
        if (win)
        {
            animator.SetBool("Dance", true);
            cameraAnimator.SetBool("Win", true);
        }
        else
            animator.SetBool("Dead", true);
        this.enabled = false;
    }

    private void BossMode()
    {
        cameraAnimator.SetBool("Boss", true);
    }
}
