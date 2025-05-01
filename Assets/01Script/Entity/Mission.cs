using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : UIClicker
{
    void Start()
    {
        OnClick += UIManager.Instance.CloseMission;
    }
}
