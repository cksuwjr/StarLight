using System.Collections;
using Unity.VisualScripting;
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

    [SerializeField] private float speed = 50f;
    [SerializeField] private float jump = 15f;

    [SerializeField] private float gravityY = -80f;

    public bool movable { get; protected set; }

    private bool notBinded = true;

    private bool isGrounded;
    private bool jumpCheck = true;

    private Vector3 camAngle;


    private void Awake()
    {
        movable = true;
        TryGetComponent<Rigidbody>(out rb);
        TryGetComponent<CapsuleCollider>(out cc);
        anim = GetComponentInChildren<Animator>();
       
        var n = Physics.gravity;
        n.y = gravityY;
        Physics.gravity = n;
    }


    private void Start()
    {
        Camera.main.TryGetComponent<CameraMove>(out cam);

        camAngle = cam.transform.rotation.eulerAngles;
        //PlayerPrefs.DeleteAll();
    }

    public void Move(Vector3 direction)
    {
        anim.SetBool("Move", false);
        if (!movable) return;
        if (!notBinded) return;

        rb.velocity = new Vector3(0, rb.velocity.y, 0);

        if (UIManager.Instance.touchBlocking) return;
        if (direction == Vector3.zero) return;
        anim.SetBool("Move", true);
        Vector3 dir = direction;
        if (cam.rotateCam)
        {
            camAngle = Camera.main.transform.eulerAngles;

            dir = Quaternion.Euler(camAngle) * direction;
        }
        //dir.y = 0;

        dir.Normalize();
        dir.y = 0;
        transform.forward = dir;

        dir *= speed;
        dir.y = rb.velocity.y;
        rb.velocity = dir;
    }

    public void Jump()
    {
        if (!rb) return;
        if (!jumpCheck) return;

        if (isGrounded)
        {
            isGrounded = false;
            jumpCheck = false;

            anim.SetBool("Jump", true);
            rb.velocity = Vector3.zero;
            rb.AddForce(jump * Vector3.up, ForceMode.Impulse);
            Invoke("JumpCheckOn", 0.2f);
        }
    }

    private void JumpCheckOn()
    {
        jumpCheck = true;
    }

    private void Update()
    {
        if (!jumpCheck) return;

        if (Physics.OverlapSphere(groundCheck.position, 0.08f, checkLayer).Length > 0)
        {
            isGrounded = true;
            anim.SetBool("Jump", false);
        }
        else
            isGrounded = false;
    }

    public void CC(float time)
    {
        StartCoroutine("CCApply", time);
    }

    private IEnumerator CCApply(float time)
    {
        float timer = 0f;
        notBinded = false;

        while (timer < time)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        notBinded = true;
    }
}
