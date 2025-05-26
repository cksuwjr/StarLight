using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadWrite : InteractionObject
{
    private void Start()
    {
        OnClick += UIManager.Instance.OpenReadWrite;
    }
}
