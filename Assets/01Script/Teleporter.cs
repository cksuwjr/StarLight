using UnityEngine;
using UnityEngine.Events;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Vector3 Destination;

    public UnityEvent OnTeleport;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = Destination;
            OnTeleport?.Invoke();
        }
    }
}
