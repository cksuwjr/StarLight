using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMovePortal : InteractionObject
{
    [SerializeField] private string sceneName;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
            GameManager.Instance.LoadScene(sceneName);
    }
}
