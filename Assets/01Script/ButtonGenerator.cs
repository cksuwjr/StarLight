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
        if (GUILayout.Button("����1"))
        {
            onOFF.Setting1();
        }

        if (GUILayout.Button("����2"))
        {
            onOFF.Setting2();
        }
    }
}
