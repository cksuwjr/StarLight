using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogMove : PoolObject, IMove
{
    private Rigidbody rigid;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float speed;


    private void Awake()
    {
        TryGetComponent<Rigidbody>(out rigid);
    }

    private void Update()
    {
        Move(direction);    
    }

    public void Jump()
    {
        throw new System.NotImplementedException();
    }

    public void Move(Vector3 direction)
    {
        rigid.velocity = new Vector3(0, rigid.velocity.y, 0);
        transform.position += speed * Time.deltaTime * direction;
        transform.Rotate(new Vector3(0, -500 * Time.deltaTime, 0));
    }

    public void Init(float speed)
    {
        this.speed = speed;
        rigid.velocity = Vector3.zero;
    }

    public void DestorySelf()
    {
        ReturnToPool();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destroyer"))
        {
            ReturnToPool();
            rigid.velocity = Vector3.zero;
        }

    }
}
