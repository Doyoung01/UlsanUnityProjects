using Unity.Hierarchy;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    void Start()
    {
        
    }

    float rx, ry;   // ������, ȸ���� �����̹Ƿ� rx�� my�� ���� ��
    public float rotSpeed = 10f;
    void Update()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        rx -= my * Time.deltaTime * rotSpeed;
        ry += mx * Time.deltaTime * rotSpeed;

        rx = Mathf.Clamp(rx, -60, 90);

        // ���Ϸ� �ޱ� �̿�
        transform.eulerAngles = new Vector3(rx, ry, 0);
        // ����� �̿�
        // transform.rotation = Quaternion.Euler(rx, ry, 0);
    }
}
