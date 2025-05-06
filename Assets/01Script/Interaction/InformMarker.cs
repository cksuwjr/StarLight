using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Data;
using System.Drawing;
using static UnityEditor.PlayerSettings;

public class InformMarker : MonoBehaviour
{
    public GameObject uiPrefab;
    public GameObject iconPrefab;
    public Vector2 offset;
    private GameObject iconUI;

    private GameObject field;

    [SerializeField] private string title;
    [SerializeField] private string touchText;

    private Vector3 position;

    private bool onoff = false;

    private void ReLocate()
    {
        var local = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);

        UIManager.Instance.LocateInformText(local, offset);
    }

    private void LateUpdate()
    {
        if (onoff)
            ReLocate();
    }

    private void Awake()
    {
        if (!uiPrefab) return;

        UIClicker iconClicker = null;

        if (iconPrefab)
        {
            iconUI = Instantiate(iconPrefab, transform);
            iconUI.SetActive(false);
            iconClicker = iconUI.GetComponentInChildren<UIClicker>();
        }


        if (TryGetComponent<InteractionObject>(out var interact))
        {
            if(iconClicker) iconClicker.OnClick += interact.Interaction;

            interact.OnInteractable += (tf) =>
            {
                if (tf)
                    UIManager.Instance.OpenInformText(title, touchText);
                else
                    UIManager.Instance.CloseInformText();

                onoff = tf;
                

                if (iconUI)
                    iconUI.SetActive(tf);

            };
        }
    }
}
