using System;
using UnityEngine;

public class PostBox : InteractionObject
{
    private void Start()
    {
        OnClick += UIManager.Instance.OpenMail;
    }
}
