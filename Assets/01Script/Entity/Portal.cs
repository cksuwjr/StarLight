using System;
using UnityEngine;

public class Portal : InteractionObject
{
    private void Start()
    {
        OnClick += UIManager.Instance.OpenChapter;
    }
}
