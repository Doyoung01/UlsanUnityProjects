using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour
{
    // 부모 오브젝트의 PlayerInput Component를 get
    InputAction mouseAction;
    public Transform player;

    private void Awake()
    {
        var input = GetComponentInParent<PlayerInput>();
        mouseAction = input.actions["Mouse"];
    }

    void Start()
    {

    }

    float rx, ry;   // 누적값, 회전축 기준이므로 rx에 my를 합할 것
    public float rotSpeed = 10f;
    void Update()
    {
        var mouseDelta = mouseAction.ReadValue<Vector2>();
        //float mx = Input.GetAxis("Mouse X");
        //float my = Input.GetAxis("Mouse Y");

        rx -= mouseDelta.y * Time.deltaTime * rotSpeed;
        ry += mouseDelta.x * Time.deltaTime * rotSpeed;

        rx = Mathf.Clamp(rx, -60, 40);
    }

    private void FixedUpdate()
    {
        // 오일러 앵글 이용
        // transform.eulerAngles = new Vector3(rx, ry, 0);
        // player.eulerAngles = new Vector3(0, ry, 0);

        // 사원수 이용
        // 카메라 회전 보간처리
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(rx, 0, 0), 1);
        
        // 플레이어 몸 회전 보간처리
        player.rotation = Quaternion.Lerp(player.rotation, Quaternion.Euler(0, ry, 0), 1);
    }
}
