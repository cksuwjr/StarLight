using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionObject : MonoBehaviour, IInteract
{
    protected event Action OnClick;
    public bool interactable = true;
    public bool oneOff = false;
    public bool destroy = false;
    public event Action<bool> OnInteractable;

    [SerializeField] private UnityEvent OnClickUnityEvent;

    private void Start()
    {
        if (PlayerPrefs.GetInt(gameObject.name, 0) != 0)
        {
            interactable = false;
            OnInteractable?.Invoke(false);
            ScenarioManager.Instance.AddStar();
            if (destroy) Destroy(gameObject);
            Destroy(this);
        }
    }

    public void Interaction()
    {
        if (!interactable) return;

        OnClickUnityEvent?.Invoke();
        OnClick?.Invoke();

        if (oneOff)
        {
            interactable = false;
            OnInteractable?.Invoke(false);
            PlayerPrefs.SetInt(gameObject.name, 1);
            PlayerPrefs.Save();
            Destroy(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactable = true;
            OnInteractable?.Invoke(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactable = false;
            OnInteractable?.Invoke(false);
        }
    }
}
