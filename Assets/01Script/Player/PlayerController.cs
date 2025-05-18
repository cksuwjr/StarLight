using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IMove movement;
    private IInputHandle[] inputHandle;

    private int hp = 3;

    private bool hittable = true;

    private event Action<int> OnChangeHp;

    public void Init()
    {
        TryGetComponent<IMove>(out movement);
        //TryGetComponent<IInputHandle>(out inputHandle);
        inputHandle = GetComponents<IInputHandle>();
        UIManager.OnPressBtnSlot1 += movement.Jump;

        OnChangeHp += UIManager.Instance.SetHpUI;
        OnChangeHp?.Invoke(hp);
    }

    private void Update()
    {
        if (inputHandle is null) return;


        Vector3 input = Vector3.zero;
        for (int i = 0; i < inputHandle.Length; i++) {
            if (inputHandle[i].GetInput() != Vector3.zero)
                input = inputHandle[i].GetInput();
        }

        movement.Move(input);


        if (Input.GetKeyDown(KeyCode.Space))
            movement.Jump();


        if (Input.GetKeyDown(KeyCode.P))
        {
            hp++;
            OnChangeHp?.Invoke(hp);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public void RemoveButtonInteraction()
    {
        UIManager.OnPressBtnSlot1 -= movement.Jump;
    }

    public void GetDamage(int damage)
    {
        if (!hittable) return;

        if (hp > 0)
        {
            SetHp(hp - damage);
            OnHit();
            if (hp <= 0)
                GameManager.Instance.GameOver();
        }
    }

    private void OnHit()
    {
        hittable = false;
        StartCoroutine("HitEffect");
    }

    private IEnumerator HitEffect()
    {
        float time = 0f;
        if (Camera.main.TryGetComponent<CameraMove>(out var cam))
            cam.ShakeCamera(1f);
        Vibration.Vibrate();

        while (time < 0.5f) {
            time += Time.deltaTime;
            UIManager.Instance.BloodScreen(time / 0.5f);
            yield return null;
        }
        time = 0f;
        while (time < 0.5f)
        {
            time += Time.deltaTime;
            UIManager.Instance.BloodScreen(1 - (time / 0.5f));
            yield return null;
        }
        hittable = true;
    }

    public void SetHp(int value)
    {
        hp = value;
        OnChangeHp?.Invoke(value);
    }

    
}
