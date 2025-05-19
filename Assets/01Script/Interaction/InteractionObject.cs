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

    bool usable = true;
    [SerializeField] private UnityEvent OnClickUnityEvent;

    private void Start()
    {
        if (PlayerPrefs.GetInt(gameObject.name, 0) != 0)
        {
            usable = false;
            if (destroy) Destroy(gameObject);
            interactable = false;
            OnInteractable?.Invoke(false);
            ScenarioManager.Instance.AddStar();
            Destroy(this);
        }
    }

    public void Interaction()
    {
        if (!interactable) return;
        Debug.Log("인터랙션");
        interactable = false;
        OnInteractable?.Invoke(false);
        OnClickUnityEvent?.Invoke();
        OnClick?.Invoke();

        if (oneOff)
        {
            PlayerPrefs.SetInt(gameObject.name, 1);
            PlayerPrefs.Save();
        }
        else
        {
            //if (ScenarioManager.Instance)
            //{
            //    ScenarioManager.Instance.OnStoryEnd += () => { interactable = true; OnInteractable?.Invoke(true); };
            //}
            //else
            {
                interactable = true;
                OnInteractable?.Invoke(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!usable) return;

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
