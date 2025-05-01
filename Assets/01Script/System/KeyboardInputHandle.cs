using UnityEngine;

public class KeyboardInputHandle : MonoBehaviour, IInputHandle
{
    public Vector3 GetInput()
    {
        return new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }
}
