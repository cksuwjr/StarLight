using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMovePortal : InteractionObject
{
    [SerializeField] private string sceneName;
    private void Start()
    {
        OnClick += () => GameManager.Instance.LoadScene(sceneName);
    }
}
