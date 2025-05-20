using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchHandler : MonoBehaviour
{
    [SerializeField] private LayerMask mask;
    private Vector3 dragPos;
    private bool onDrag = false;
    private CameraMove cam;

    private float halfScreenWidth;
    private int rightFingerId;
    private Vector2 lastTouchPosition;

    private void Awake()
    {
        halfScreenWidth = Screen.width * 0.5f;
    }

    private void Start()
    {
        Camera.main.gameObject.TryGetComponent<CameraMove>(out cam);
    }

    void Update()
    {
        if (UIManager.Instance)
            if (UIManager.Instance.touchBlocking) return;

#if (UNITY_EDITOR)

        RaycastHit[] hit = null;
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Input.mousePosition.z));

            hit = Physics.RaycastAll(ray, Mathf.Infinity, mask, QueryTriggerInteraction.UseGlobal);

            for(int j = 0; j < hit.Length; j++)
            {
                if (hit[j].collider == null) continue;
                if (hit[j].collider.isTrigger) continue;

                if (hit[j].collider.TryGetComponent<IInteract>(out var interact))
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
#elif (UNITY_IOS || UNITY_ANDROID)

// ���� ��ġ�Է� ����ŭ �ݺ�
        for (int i = 0; i < Input.touchCount; i++)
        {
            // ���� ��ġ �Է� ������� ��ȸ
            Touch t = Input.GetTouch(i);
            
            // UI�� ��ġ�ϰ� �巡�� ���� ������ ȸ������ �ʵ��� �ϱ� ���� bool��
            bool isTouchingUI = EventSystem.current.IsPointerOverGameObject(t.fingerId);

            RaycastHit[] hit2 = null;
            // ��ġ �Է��� ���¿� ���� ����
            switch (t.phase)
            {
            	// ��ġ�� ���۵Ǿ��� ��
                case UnityEngine.TouchPhase.Began:

                    var ray = Camera.main.ScreenPointToRay(new Vector3(t.position.x, t.position.y, -Input.mousePosition.z));

                    hit2 = Physics.RaycastAll(ray, Mathf.Infinity, mask, QueryTriggerInteraction.UseGlobal);

                    for (int j = 0; j < hit.Length; j++)
                    {
                        if (hit2[j].collider == null) continue;
                        if (hit2[j].collider.isTrigger) continue;

                        if (hit2[j].collider.TryGetComponent<IInteract>(out var interact))
                        {
                            interact.Interaction();
                            return;
                        }
                    }




                    // ��ġ �Է��� ȭ���� �����ʿ��� �̷������
                    // ���� ������ �Է��� �̷������ �ʾҰ�
                    // UI�� ��ġ���� �ƴ϶��
                    if (t.position.x > halfScreenWidth && rightFingerId == -1 && !isTouchingUI)
                    {
                    	// ������ ��ġ �Է��� ���� ��ġ�Է��� ��ȣ�� ����
                        rightFingerId = t.fingerId;
                        // ��ġ ��ġ ����
                        lastTouchPosition = t.position;
                    }
                    break;
                // �巡�� ���� ��
                case UnityEngine.TouchPhase.Moved:
                    if (t.fingerId == rightFingerId && !isTouchingUI)
                    {
                    	// ���� ��ġ�� ���� ��ġ���� �Ÿ� ����
                        //Vector2 delta = t.position - lastTouchPosition;
                        // cinemachine�� ȸ������ �Ÿ� + �巡�� �ӵ� ��ŭ ������
                        if (cam.cameraMoving)
                            cam.cameraMoving.Moving();
                        // �ֱ� ��ġ ����
                        lastTouchPosition = t.position;
                    }
                    break;
				
                // �հ����� ������ ��
                case UnityEngine.TouchPhase.Ended:
                    if (t.fingerId == rightFingerId)
                    {
                    	// ������ ��ġ �Է� ��ȣ �ʱ�ȭ
                        rightFingerId = -1;
                        //Debug.Log("������ �հ��� ��");
                    }
                    break;
            }
        }

#endif
    }
}
