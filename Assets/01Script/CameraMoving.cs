using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    public Transform objectTofollow;
    public float followSpeed = 1000f;
    public float sensivity = 400f;
    public float clampAngle = 30f;

    private float rotX;
    private float rotY;

    public Transform realCamera;
    public Vector3 dirNomalized;
    public Vector3 finalDir;
    public float minDistance = 0;
    public float maxDistance = 1.376f; // 1.376 , 2.5
    public float finalDistance;

    public float smoothness = 10f;

    private bool isInit = false;
    private bool isShaking = false;

    public void Init(float distance)
    {
        maxDistance = distance;

        isInit = true;

        objectTofollow = GameManager.Instance.Player.transform;

        transform.position = GameManager.Instance.Player.transform.position;


        var cam = Camera.main;
        cam.transform.SetParent(transform);

        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;

        dirNomalized = realCamera.localPosition.normalized;
        finalDistance = realCamera.localPosition.magnitude;
    }

    public void Moving()
    {
        rotX -= Input.GetAxis("Mouse Y") * sensivity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * sensivity * Time.deltaTime;

        //rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        rotX = Mathf.Clamp(rotX, -30, 50);
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot;
    }

    public void Shaking(bool tf)
    {
        isShaking = tf;
    }

    //private void Update()
    //{
    //    rotX -= Input.GetAxis("Mouse Y") * sensivity * Time.deltaTime;
    //    rotY += Input.GetAxis("Mouse X") * sensivity * Time.deltaTime;

    //    //rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
    //    rotX = Mathf.Clamp(rotX, -30, 50);
    //    Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
    //    transform.rotation = rot;
    //}

    private void LateUpdate()
    {
        if (!isInit) return;

        transform.position = Vector3.MoveTowards(transform.position, objectTofollow.position, followSpeed * Time.deltaTime);

        finalDir = transform.TransformPoint(dirNomalized * maxDistance);

        RaycastHit hit;

        if(Physics.Linecast(transform.position, finalDir, out hit))
        {
            finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            finalDistance = maxDistance;
        }

        if (isShaking) return;

        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNomalized * finalDistance, Time.deltaTime * smoothness);
    }
}
