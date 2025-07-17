using System.Collections;
using UnityEngine;

public enum StartMove
{
    Forward = 0,
    Right = 90,
    Backward = 180,
    Left = 270,
}

public class Movement : MonoBehaviour, IMove
{
    private Rigidbody rb;
    private CapsuleCollider cc;
    private Animator anim;

    private CameraMove cam;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask checkLayer;

    [SerializeField] private float speed = 3f;      
    [SerializeField] private float jumpPower = 15f;
    [SerializeField] private float gravityY = -80f;

    public bool movable { get; protected set; } = true;

    private bool notBinded = true;
    private bool isGrounded = true;
    private bool jumpCheck = true;

    private float walkSoundTimer;

    private Vector3 camAngle;
    private Vector3 inputDirection = Vector3.zero;

    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip jumpSound;

    private void Awake()
    {
        TryGetComponent(out rb);
        TryGetComponent(out cc);
        anim = GetComponentInChildren<Animator>();

        // ���� �߷� ����
        Physics.gravity = new Vector3(0, gravityY, 0);
    }

    private void Start()
    {
        Camera.main.TryGetComponent(out cam);
        camAngle = cam.transform.rotation.eulerAngles;
    }

    public void Move(Vector3 direction)
    {
        if (!movable || !notBinded || UIManager.Instance.touchBlocking)
        {
            inputDirection = Vector3.zero;
            return;
        }

        inputDirection = direction;
    }

    private void FixedUpdate()
    {
        // ���� üũ
        isGrounded = Physics.OverlapSphere(groundCheck.position, 0.14f, checkLayer).Length > 0;
        anim.SetBool("Jump", !isGrounded);

        // �Է� ���� ��� �ӵ� 0 ó�� �� �ִϸ��̼� ����
        if (!movable || !notBinded || inputDirection == Vector3.zero)
        {
            anim.SetBool("Move", false);
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            return;
        }

        // ī�޶� ���� �������� ȸ�� ����
        Vector3 moveDir = inputDirection;
        if (cam && cam.rotateCam)
        {
            camAngle = cam.transform.eulerAngles;
            moveDir = Quaternion.Euler(0, camAngle.y, 0) * moveDir;
        }

        moveDir.y = 0;
        moveDir.Normalize();

        // ȸ�� ó��
        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 500f * Time.fixedDeltaTime);

        // �ӵ� ��� �� ����
        Vector3 desiredVelocity = moveDir * speed;
        desiredVelocity.y = rb.velocity.y;
        Vector3 velocityDiff = desiredVelocity - rb.velocity;

        rb.AddForce(velocityDiff, ForceMode.VelocityChange);

        // �ִϸ��̼�
        anim.SetBool("Move", true);

        // �ȴ� �Ҹ�
        if (walkSound)
        {
            walkSoundTimer += Time.fixedDeltaTime;
            if (walkSoundTimer > walkSound.length)
            {
                SoundManager.Instance.PlaySound(walkSound, 0.4f);
                walkSoundTimer = 0f;
            }
        }
    }

    public void Jump()
    {
        if (!rb || !jumpCheck || !isGrounded) return;

        jumpCheck = false;
        isGrounded = false;

        anim.SetBool("Jump", true);
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

        if (jumpSound)
            SoundManager.Instance.PlaySound(jumpSound, 0.2f);

        Invoke(nameof(JumpCheckOn), 0.2f);
    }

    private void JumpCheckOn()
    {
        jumpCheck = true;
    }

    public void CC(float time)
    {
        StartCoroutine(CCApply(time));
    }

    private IEnumerator CCApply(float time)
    {
        notBinded = false;
        yield return new WaitForSeconds(time);
        notBinded = true;
    }
}