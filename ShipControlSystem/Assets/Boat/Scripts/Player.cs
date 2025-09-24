using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {

    }

    public float speed = 20;
    public float angle = 180;
    Vector3 velocity;
    void Update()
    {
        // ����ڰ� �Է��ϸ� �����¿�� �̵��ϰ�ʹ�.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = transform.forward * v;
        velocity += dir * speed * Time.deltaTime;
        //transform.Rotate(Vector3.up, h * angle * Time.deltaTime);
        Quaternion q = Quaternion.Euler(Vector3.up * h * angle * Time.deltaTime);
        rb.MoveRotation(rb.transform.rotation * q);
        // ������ġ P = P0 + vt
        // �����̵�
        //transform.position = transform.position + velocity * Time.deltaTime;
        // rb�� �̿��� �̵�
        rb.MovePosition(rb.transform.position + velocity * Time.deltaTime);

        velocity = Vector3.Lerp(velocity, Vector3.zero, Time.deltaTime * 6);
    }
}





// using UnityEngine;

//public class Player : MonoBehaviour
//{
//    public Rigidbody rb;

//    public float speed = 20;
//    public float angle = 180;
//    Vector3 velocity;
//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//    }

//    void Update()
//    {
//        float h = Input.GetAxis("Horizontal");  // ȸ��
//        float v = Input.GetAxis("Vertical");    // ���� �̵�

//        Vector3 dir = transform.forward * v;

//        velocity += dir * speed * Time.deltaTime;

//        // y�� ���� ȸ�� - ī�޶� +y�࿡ ��ġ
//        // transform.Rotate(Vector3.up * h * angle * Time.deltaTime);
//        // transform.position += velocity * Time.deltaTime;

//        Quaternion q = Quaternion.Euler(Vector3.up * h * angle * Time.deltaTime);

//        rb.MovePosition(rb.transform.position + velocity * Time.deltaTime);
//        // ���� rotation * �ٲٰ� ���� ����
//        rb.MoveRotation(rb.transform.rotation * q);

//        velocity = Vector3.Lerp(velocity, Vector3.zero, Time.deltaTime * 6);
//    }
//}
