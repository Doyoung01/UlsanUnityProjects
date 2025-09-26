using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour
{
    // �θ� ������Ʈ�� PlayerInput Component�� get
    InputAction mouseAction;

    private void Awake()
    {
        var input = GetComponentInParent<PlayerInput>();
        mouseAction = input.actions["Mouse"];
    }

    void Start()
    {

    }

    float rx, ry;   // ������, ȸ���� �����̹Ƿ� rx�� my�� ���� ��
    public float rotSpeed = 10f;
    void Update()
    {
        var mouseDelta = mouseAction.ReadValue<Vector2>();
        //float mx = Input.GetAxis("Mouse X");
        //float my = Input.GetAxis("Mouse Y");

        rx -= mouseDelta.y * Time.deltaTime * rotSpeed;
        ry += mouseDelta.x * Time.deltaTime * rotSpeed;

        rx = Mathf.Clamp(rx, -60, 90);

        // ���Ϸ� �ޱ� �̿�
        transform.eulerAngles = new Vector3(rx, ry, 0);
        // ����� �̿�
        // transform.rotation = Quaternion.Euler(rx, ry, 0);
    }
}
