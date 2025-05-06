using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyRotater : MonoBehaviour
{
    private float degree;
    // Update is called once per frame
    void Update()
    {
        degree += Time.deltaTime;
        if (degree >= 360)
            degree = 0;

        RenderSettings.skybox.SetFloat("_Rotation", degree);
    }
}
