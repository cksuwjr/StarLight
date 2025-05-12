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


    public bool movable { get; protected set; }

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
        n.y = -80f;
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
        if (!movable) return;
        anim.SetBool("Move", false);
        rb.velocity = new Vector3(0, rb.velocity.y, 0);

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

        if (Physics.OverlapSphere(groundCheck.position, 0.02f, checkLayer).Length > 0)
        {
            isGrounded = true;
            anim.SetBool("Jump", false);
        }
        else
            isGrounded = false;
    }

}
