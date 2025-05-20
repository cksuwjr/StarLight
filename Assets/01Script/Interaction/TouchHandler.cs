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

// 들어온 터치입력 수만큼 반복
        for (int i = 0; i < Input.touchCount; i++)
        {
            // 들어온 터치 입력 순서대로 순회
            Touch t = Input.GetTouch(i);
            
            // UI를 터치하고 드래그 했을 때에는 회전하지 않도록 하기 위한 bool값
            bool isTouchingUI = EventSystem.current.IsPointerOverGameObject(t.fingerId);

            RaycastHit[] hit2 = null;
            // 터치 입력의 상태에 따른 동작
            switch (t.phase)
            {
            	// 터치가 시작되었을 때
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




                    // 터치 입력이 화면의 오른쪽에서 이루어졌고
                    // 아직 오른쪽 입력이 이루어지지 않았고
                    // UI를 터치중이 아니라면
                    if (t.position.x > halfScreenWidth && rightFingerId == -1 && !isTouchingUI)
                    {
                    	// 오른쪽 터치 입력을 현재 터치입력의 번호로 저장
                        rightFingerId = t.fingerId;
                        // 터치 위치 저장
                        lastTouchPosition = t.position;
                    }
                    break;
                // 드래그 중일 때
                case UnityEngine.TouchPhase.Moved:
                    if (t.fingerId == rightFingerId && !isTouchingUI)
                    {
                    	// 현재 위치와 이전 위치와의 거리 저장
                        //Vector2 delta = t.position - lastTouchPosition;
                        // cinemachine의 회전값에 거리 + 드래그 속도 만큼 더해줌
                        if (cam.cameraMoving)
                            cam.cameraMoving.Moving();
                        // 최근 위치 갱신
                        lastTouchPosition = t.position;
                    }
                    break;
				
                // 손가락을 떼었을 때
                case UnityEngine.TouchPhase.Ended:
                    if (t.fingerId == rightFingerId)
                    {
                    	// 오른쪽 터치 입력 번호 초기화
                        rightFingerId = -1;
                        //Debug.Log("오른쪽 손가락 끝");
                    }
                    break;
            }
        }

#endif
    }
}
