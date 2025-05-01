using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickInputHandle : MonoBehaviour, IInputHandle
{
    private Joystick joystick;

    private void Awake()
    {
        GameObject.Find("JoyStick").TryGetComponent<Joystick>(out joystick);
    }

    public Vector3 GetInput()
    {
        return new Vector3(joystick.Horizontal, 0, joystick.Vertical);
    }
}
