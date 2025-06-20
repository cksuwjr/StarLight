using System;
using UnityEngine;

public class ClearZone : MonoBehaviour
{
    public Action OnEnter;

    public void ClearEvent()
    {
        OnEnter = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnEnter?.Invoke();
        }
    }
}
