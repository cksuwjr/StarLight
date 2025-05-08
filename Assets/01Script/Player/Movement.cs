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

        //PlayerPrefs.DeleteAll();
    }

    public void Move(Vector3 direction)
    {
        if (!movable) return;
        anim.SetBool("Move", false);
        if (direction == Vector3.zero) return;
        anim.SetBool("Move", true);
        Vector3 dir = direction;
        if (cam && cam.enabled && cam.cameraRotatable)
        {
            var camAngle = Camera.main.transform.eulerAngles;

            dir = Quaternion.Euler(camAngle) * direction;
        }
        dir.y = 0;

        transform.forward = dir.normalized;


        rb.velocity = dir.normalized * speed;
    }

    public void Jump()
    {
        if (!rb) return;

        if (isGrounded)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(jump * Vector3.up, ForceMode.Impulse);
        }
    }

    private void Update()
    {
        if (Physics.OverlapSphere(groundCheck.position, 0.2f, checkLayer).Length > 0)
            isGrounded = true;
        else
            isGrounded = false;
    }

}
