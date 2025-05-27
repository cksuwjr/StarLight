using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionObject : MonoBehaviour, IInteract
{
    protected event Action OnClick;
    public bool interactable = true;
    public bool nonSave = false;
    public event Action<bool> OnInteractable;

    bool usable = true;
    [SerializeField] private UnityEvent OnClickUnityEvent;
    [SerializeField] private UnityEvent OnClickUnityEventOnce;


    private void Start()
    {
        if (nonSave) return;

        if (PlayerPrefs.GetInt(gameObject.name, 0) != 0)
        {
            if(ScenarioManager.Instance)
                ScenarioManager.Instance.AddStar();
            OnClickUnityEventOnce = null;
        }
    }

    public void Interaction()
    {
        if (!interactable) return;
        Debug.Log("인터랙션");
        interactable = false;
        OnInteractable?.Invoke(false);
        OnClickUnityEvent?.Invoke();

        if (ScenarioManager.Instance)
        {
            ScenarioManager.Instance.OnStoryEnd += () =>
            {
                OnClickUnityEventOnce?.Invoke();
                OnClickUnityEventOnce = null;
            };
        }
        OnClick?.Invoke();

        PlayerPrefs.SetInt(gameObject.name, 1);
        PlayerPrefs.Save();
        interactable = true;
        OnInteractable?.Invoke(true);
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
