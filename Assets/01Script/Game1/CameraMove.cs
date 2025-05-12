using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// offset 0.05 / 6.8 / -5.44
public class CameraMove : MonoBehaviour
{
    public bool rotateCam = true;
    public CameraMoving cameraMoving;

    public void Init()
    {
        if (!rotateCam) return;

        var o = new GameObject();
        cameraMoving = o.AddComponent<CameraMoving>();
        o.name = "TPP Camera";
        //o.transform.position = transform.position;
        //transform.SetParent(o.transform);

        cameraMoving.realCamera = transform;

        cameraMoving.Init();
    }

    private Transform target;
    private GameObject obj;
    private Vector3 offset;
    private Vector3 cameraPos;

    //private Vector2 xRange = Vector2.zero;
    //private Vector2 yRange = Vector2.zero; 

    //private float rotateSensivity = 4.5f;

    //public bool cameraRotatable = false;

    private void Awake()
    {
        if (rotateCam) return;

        obj = GameObject.Find("Player");

        if (obj != null)
            target = obj.transform;

        cameraPos = transform.position;
        offset = cameraPos - target.position;
    }

    private void LateUpdate()
    {
        if (rotateCam) return;

        cameraPos = target.position + offset;

        transform.position = cameraPos;
    }

    //public void ChangeCameraRotation(Vector3 change)
    //{
    //    if (!cameraRotatable) return;

    //    var camAngle = transform.rotation.eulerAngles;

    //    float x = camAngle.x - (change.y * rotateSensivity);
    //    float y = camAngle.y + (change.x * rotateSensivity * 1.5f);

    //    if (x < 180)
    //        x = Mathf.Clamp(x, -1f, xRange.y);
    //    else
    //        x = Mathf.Clamp(x, 360f + xRange.x, 360f);

    //    transform.rotation = Quaternion.Euler(x, y, camAngle.z);
    //}
}