using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Data;
using System.Drawing;

public class InformMarker : MonoBehaviour
{
    private LineRenderer line;
    public Material lineMaterial;
    public GameObject uiPrefab;
    public GameObject iconPrefab;
    public Vector3 offset;
    private GameObject canvasUI;
    private GameObject iconUI;

    [SerializeField] private string title;
    [SerializeField] private string touchText;

    private Vector3 position;

    private bool onoff = false;

    private void ReLocatePosition()
    {
        //canvasUI.SetActive(false);
        var euler = transform.rotation.eulerAngles;

        transform.rotation = Quaternion.identity;
        //canvasUI.transform.localScale = new Vector3(1f / canvasUI.transform.lossyScale.x, 1f / canvasUI.transform.lossyScale.y, 1f / canvasUI.transform.lossyScale.z);
        canvasUI.transform.localPosition = new Vector3(offset.x * canvasUI.transform.localScale.x, offset.y * canvasUI.transform.localScale.y, offset.z * canvasUI.transform.localScale.z);

        position = canvasUI.transform.position;

        transform.rotation = Quaternion.Euler(euler);

        canvasUI.transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles);
        canvasUI.transform.position = position;

        var size = Vector3.zero;
        size.x = canvasUI.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x;
        var posOffset = new Vector3(0, 0, -0.05f);

        line.SetPosition(0, transform.position);
        line.SetPosition(1, canvasUI.transform.GetChild(1).transform.position);
        line.SetPosition(2, canvasUI.transform.GetChild(2).transform.position);
        //line.SetPosition(0, transform.position);
        //line.SetPosition(1, transform.position + offset - (size * 0.5f) + posOffset);
        //line.SetPosition(2, transform.position + offset + (size * 0.5f) + posOffset);
    }

    private void Update()
    {
        if (onoff)
            ReLocatePosition();
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



        line = gameObject.AddComponent<LineRenderer>();
        line.enabled = false;

        line.positionCount = 3;
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;

        line.material = lineMaterial;


        canvasUI = Instantiate(uiPrefab, transform);
        canvasUI.transform.localScale = new Vector3(1f / canvasUI.transform.lossyScale.x, 1f / canvasUI.transform.lossyScale.y, 1f / canvasUI.transform.lossyScale.z);
        canvasUI.SetActive(false);

        ReLocatePosition();

        

        var tmps = canvasUI.GetComponentsInChildren<TextMeshProUGUI>();

        if (tmps.Length > 1)
        {
            tmps[0].text = title;
            tmps[1].text = touchText;
        }


        var size = Vector3.zero;
        size.x = canvasUI.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x;

        var posOffset = new Vector3(0, 0, -0.05f);

        if (TryGetComponent<InteractionObject>(out var interact))
        {
            if(iconClicker) iconClicker.OnClick += interact.Interaction;

            interact.OnInteractable += (tf) =>
            {
                //ReLocatePosition();
                canvasUI.SetActive(tf);
                //canvasUI.transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles);
                onoff = tf;
                

                if (iconUI)
                {
                    iconUI.SetActive(tf);
                    line.SetPosition(0, transform.position + Vector3.right * iconUI.transform.GetChild(0).transform.localScale.x);
                }


                line.enabled = tf;
            };
        }
    }
}
