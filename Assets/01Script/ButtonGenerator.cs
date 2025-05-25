using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OnOFF))]
public class ButtonGenerator : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        OnOFF onOFF = (OnOFF)target;
        if (GUILayout.Button("세팅1"))
        {
            onOFF.Setting1();
        }

        if (GUILayout.Button("세팅2"))
        {
            onOFF.Setting2();
        }
    }
}
