using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosLoader : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.LoadPosition();
    }
}
