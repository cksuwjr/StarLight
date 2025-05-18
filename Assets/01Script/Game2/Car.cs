using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : PoolObject
{
    private Rigidbody rb;
    private Rigidbody playerRb;

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

        if (other.CompareTag("Player"))
        {
            if (playerRb == null)
                other.TryGetComponent<Rigidbody>(out playerRb);
            if (playerRb)
                Hit();
        }
    }

    private void Hit()
    {
        playerRb.GetComponent<Movement>().CC(0.5f);
        playerRb.transform.position = new Vector3(23f, -1.183f, 110f);
    }
}
