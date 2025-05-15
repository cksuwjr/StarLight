using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : PoolObject
{
    private Rigidbody rb;

    private void Awake()
    {
        TryGetComponent<Rigidbody>(out rb);    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destroyer"))
        {
            ReturnToPool();
            rb.velocity = Vector3.zero;
        }
    }
}
