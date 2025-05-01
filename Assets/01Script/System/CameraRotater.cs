using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotater : MonoBehaviour
{
    private CameraMove cam;
    [SerializeField] private float rotateYValue;
    private float rotateTime = 0.5f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //if (other.TryGetComponent<Movement>(out var move))
            //    move.SetMove((Start)(int)rotateYValue);
            if (Camera.main.TryGetComponent<CameraMove>(out cam))
                StartCoroutine("RotateCam");
        }
    }

    private IEnumerator RotateCam()
    {
        float timer = 0f;
        var camAngle = cam.transform.rotation.eulerAngles;
        var rotateVector = new Vector3(camAngle.x, rotateYValue, camAngle.z);

        while (timer <= rotateTime) {
            timer += Time.deltaTime;
            cam.transform.rotation = Quaternion.Euler(Vector3.Lerp(camAngle, rotateVector, timer / rotateTime));
            //cam.ChangeYCenter(Mathf.Lerp(cam.cameraYCenter, rotateYValue, timer / rotateTime));

            yield return null;
        }
    }
}
