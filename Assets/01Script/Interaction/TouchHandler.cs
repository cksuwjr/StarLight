using UnityEditor;
using UnityEngine;

public class TouchHandler : MonoBehaviour
{
    [SerializeField] private LayerMask mask;
    private Vector3 dragPos;
    private bool onDrag = false;
    private CameraMove cam;
    private void Start()
    {
        Camera.main.gameObject.TryGetComponent<CameraMove>(out cam);
    }

    void Update()
    {
        if (UIManager.Instance)
            if (UIManager.Instance.touchBlocking) return;

        RaycastHit[] hit = null;
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Input.mousePosition.z));

            hit = Physics.RaycastAll(ray, Mathf.Infinity, mask, QueryTriggerInteraction.UseGlobal);

            for(int i = 0; i < hit.Length; i++)
            {
                if (hit[i].collider == null) continue;
                if (hit[i].collider.isTrigger) continue;

                if (hit[i].collider.TryGetComponent<IInteract>(out var interact))
                {
                    interact.Interaction();
                    return;
                }
            }
        }



        if (Input.GetMouseButtonDown(0))
        {
            if (Input.mousePosition.x > Screen.width * 0.194f && Input.mousePosition.x < Screen.width * 0.83f)
            {
                onDrag = true;
                dragPos = Input.mousePosition;
            }
        }
        if (onDrag)
        {
            if(cam)
                if (cam.cameraMoving)
                    cam.cameraMoving.Moving();
            //if (onDrag)
            //    cam.ChangeCameraRotation(new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0));
        }

        if (Input.GetMouseButtonUp(0))
            onDrag = false;

        //for(int i = 0; i < Input.touchCount; i++)
        //{
        //    if(Input.GetTouch(i).phase == TouchPhase.Moved)
        //    {

        //    }
        //}

        // For Mobile

        //if (Input.touchCount > 0)
        //{
        //    var touch = Input.GetTouch(0);

        //    if (touch.phase == TouchPhase.Began)
        //    {
        //        var ray = Camera.main.ScreenPointToRay(touch.position);
        //        RaycastHit hit;

        //        Physics.Raycast(ray, out hit);
        //        if (hit.collider == null) return;

        //        if (hit.collider.TryGetComponent<IInteract>(out var interact))
        //            interact.Interaction();
        //    }
        //}
    }
}
