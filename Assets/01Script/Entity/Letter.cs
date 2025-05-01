using System;
using UnityEngine;

public class Letter : UIClicker
{
    private void Start()
    {
        OnClick += UIManager.Instance.CloseMail;
        OnClick += UIManager.Instance.OpenMission;
    }
}
