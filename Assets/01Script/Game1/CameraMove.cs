using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// offset 0.05 / 6.8 / -5.44
public class CameraMove : MonoBehaviour
{
    //private Transform target;

    //public float Yaxis;
    //public float Xaxis;
    //private float rotSensitive = 3f;
    //private float dis = 8.708393f;
    ////private float dis = 5.44f;

    //private float RotationMin = -10f;
    //private float RotationMax = 80f;
    //private float smoothTime = 0.12f;

    //private Vector3 targetRotation;
    //private Vector3 currentVel;


    //private GameObject obj;

    //private void Awake()
    //{
    //    Debug.Log(Vector3.Distance(Vector3.zero, new Vector3(0.05f, 6.8f, -5.44f)));

    //    obj = GameObject.Find("Player");

    //    if (obj != null)
    //        target = obj.transform;
    //}

    //private void LateUpdate()
    //{
    //    Yaxis = Yaxis + Input.GetAxis("Mouse X") * rotSensitive;
    //    //Xaxis = Xaxis + Input.GetAxis("Mouse Y") * rotSensitive;

    //    Xaxis = Mathf.Clamp(Xaxis, RotationMin, RotationMax);

    //    targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(Xaxis, Yaxis, -0.255f), ref currentVel, smoothTime);
    //    this.transform.eulerAngles = targetRotation;

    //    transform.position = target.position - transform.forward * dis;
    //}

    private Transform target;
    private Vector3 offset;
    private GameObject obj;
    private Vector3 cameraPos;

    private Vector2 xRange = Vector2.zero;
    private Vector2 yRange = Vector2.zero; 

    private float rotateSensivity = 4.5f;

    [SerializeField] private bool cameraRotatable = false;
    [SerializeField] public float cameraYCenter = 180f;

    private void Awake()
    {
        obj = GameObject.Find("Player");

        if (obj != null)
            target = obj.transform;

        cameraPos = transform.position;
        offset = cameraPos - target.position;

        xRange = new Vector2(-75f, 34.68f);
        yRange = new Vector2(-179f, 179f);
    }

    private void LateUpdate()
    {
        cameraPos = target.position + offset;

        transform.position = cameraPos;
    }

    public void ChangeCameraRotation(Vector3 change)
    {
        if (!cameraRotatable) return;

        var camAngle = transform.rotation.eulerAngles;

        float x = camAngle.x - (change.y * rotateSensivity);
        float y = camAngle.y + (change.x * rotateSensivity * 1.5f);

        if (x < 180)
            x = Mathf.Clamp(x, -1f, xRange.y);
        else
            x = Mathf.Clamp(x, 360f + xRange.x, 360f);

        //y = Mathf.Clamp(y, cameraYCenter + yRange.x, cameraYCenter + yRange.y);

        transform.rotation = Quaternion.Euler(x, y, camAngle.z);
    }
    
    public void ChangeYCenter(float value)
    {
        cameraYCenter = value;
    }
}