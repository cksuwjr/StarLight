using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoNode : PoolObject
{
    private float speed = 0;
    public int id = -1;
    public void Init(int id, float speed)
    {
        this.id = id;
        this.speed = speed;
    }

    private void Update()
    {
        transform.position += Vector3.down * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destroyer"))
        {
            ReturnToPool();
        }
    }
}
