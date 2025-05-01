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
        transform.position += speed * Time.deltaTime * direction;
    }

    public void Init(float speed)
    {
        this.speed = speed;
        rigid.velocity = Vector3.zero;
        Invoke("DestorySelf", 15f);
    }

    public void DestorySelf()
    {
        ReturnToPool();
    }
}
