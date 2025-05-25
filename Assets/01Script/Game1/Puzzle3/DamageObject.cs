using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    public int damage = 1;

    public bool isAlwaysDamage = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance.Player.TryGetComponent<PlayerController>(out var da))
            {
                if (isAlwaysDamage)
                    da.FixDamage(damage);
                else
                    da.GetDamage(damage);
            }
        }
    }
}
