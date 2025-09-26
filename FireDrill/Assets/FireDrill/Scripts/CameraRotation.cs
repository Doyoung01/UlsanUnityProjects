using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour
{
    // �θ� ������Ʈ�� PlayerInput Component�� get
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

    float rx, ry;   // ������, ȸ���� �����̹Ƿ� rx�� my�� ���� ��
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
        // ���Ϸ� �ޱ� �̿�
        // transform.eulerAngles = new Vector3(rx, ry, 0);
        // player.eulerAngles = new Vector3(0, ry, 0);

        // ����� �̿�
        // ī�޶� ȸ�� ����ó��
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(rx, 0, 0), 1);
        
        // �÷��̾� �� ȸ�� ����ó��
        player.rotation = Quaternion.Lerp(player.rotation, Quaternion.Euler(0, ry, 0), 1);
    }
}
