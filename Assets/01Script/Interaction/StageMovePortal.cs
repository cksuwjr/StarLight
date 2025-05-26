using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMovePortal : InteractionObject
{
    [SerializeField] private string sceneName;

    [SerializeField] private bool isClicker = false;

    private void Start()
    {
        if (isClicker)
        {
            OnClick += () => GameManager.Instance.SavePosition();
            OnClick += () => GameManager.Instance.LoadScene(sceneName);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
            GameManager.Instance.LoadScene(sceneName);
    }
}
