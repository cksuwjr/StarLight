using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// offset 0.05 / 6.8 / -5.44
public class CameraMove : MonoBehaviour
{
    public bool rotateCam = true;
    public CameraMoving cameraMoving;
    [SerializeField] private float distance = 1.376f;
    private bool shakeCam = false;

    public void Init()
    {
        if (!rotateCam) return;
        Debug.Log(Vector3.Distance(transform.position, GameManager.Instance.Player.transform.position));

        var o = new GameObject();
        cameraMoving = o.AddComponent<CameraMoving>();
        o.name = "TPP Camera";
        //o.transform.position = transform.position;
        //transform.SetParent(o.transform);

        cameraMoving.realCamera = transform;

        cameraMoving.Init(distance);
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
        //if (rotateCam) return;

        obj = GameObject.Find("Player");

        if (obj != null)
            target = obj.transform;

        cameraPos = transform.position;
        offset = cameraPos - target.position;
    }

    private void LateUpdate()
    {
        if (rotateCam) return;
        if (shakeCam) return;

        cameraPos = target.position + offset;

        transform.position = cameraPos;
    }

    public void ShakeCamera(float time)
    {
        if (target == null)
            target = GameManager.Instance.Player.transform;

        StartCoroutine("ShakingCamera", time);
    }

    private IEnumerator ShakingCamera(float duration)
    {
        float halfDuration = duration / 2;
        float elapsed = 0f;
        float tick = Random.Range(-10f, 10f);

        float m_roughness = 10;
        float m_magnitude = 10;

        Vector3 originPos = transform.position;

        shakeCam = true;
        if (cameraMoving) cameraMoving.Shaking(true);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime / halfDuration;

            tick += Time.deltaTime * m_roughness;

            transform.position = target.transform.position + offset + new Vector3(
                Mathf.PerlinNoise(tick, 0) - .5f,
                Mathf.PerlinNoise(0, tick) - .5f,
                0f) * m_magnitude * Mathf.PingPong(elapsed, halfDuration);
            yield return null;
        }

        shakeCam = false;
        transform.position = originPos;
        if (cameraMoving) cameraMoving.Shaking(false);
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