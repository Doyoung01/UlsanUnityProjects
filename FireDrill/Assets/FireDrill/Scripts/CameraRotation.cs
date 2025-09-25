using Unity.Hierarchy;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    void Start()
    {
        
    }

    float rx, ry;   // 누적값, 회전축 기준이므로 rx에 my를 합할 것
    public float rotSpeed = 10f;
    void Update()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        rx -= my * Time.deltaTime * rotSpeed;
        ry += mx * Time.deltaTime * rotSpeed;

        rx = Mathf.Clamp(rx, -60, 90);

        // 오일러 앵글 이용
        transform.eulerAngles = new Vector3(rx, ry, 0);
        // 사원수 이용
        // transform.rotation = Quaternion.Euler(rx, ry, 0);
    }
}
