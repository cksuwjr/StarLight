using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    [SerializeField] private bool testMode; 
    private void Awake()
    {
        if (testMode) return;
        if(TryGetComponent<Camera>(out var camera))
        {
            var cameraRect = camera.rect;

            float xRatio = 9;
            float yRatio = 16;

            float scale_height = ((float)Screen.width / Screen.height) / (yRatio / xRatio);
            float scale_width = 1f / scale_height;

            //Debug.Log($"���� width {Screen.width}, ���� height {Screen.height}");
            //Debug.Log($"scale_width {scale_width}, scale_height {scale_height}");


            if (scale_height < 1f)
            {
                cameraRect.height = scale_height;
                cameraRect.y = (1f - scale_height) / 2f;
            }
            else
            {
                cameraRect.width = scale_width;
                cameraRect.x = (1f - scale_width) / 2f;
            }
            //Debug.Log($"ī�޶� width {cameraRect.x}, ���� height {cameraRect.height}");
            camera.rect = cameraRect;
        }
    }
}
