using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEnterStarter : MonoBehaviour
{
    public UnityEvent OnEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnEnter?.Invoke();
            OnEnter = null;
        }
    }
}
