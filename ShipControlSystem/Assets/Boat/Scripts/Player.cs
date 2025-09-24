using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public static Player instance;
    public Rigidbody rb;
    public GameObject defaultTailTarget;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Player.instance = this;
    }

    void Start()
    {
        POINT = 0;
    }

    public float speed = 200;
    public float angle = 360;
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

    int garbageCount;
    public Tail tailFactory;
    public GameObject lastTail;
    List<Tail> tails = new List<Tail>();

    internal void AddGarbage(int addCount, Vector3 position)
    {
        garbageCount += addCount;

        Tail tail = Instantiate<Tail>(tailFactory, position, Quaternion.identity);
        
        float tailSpeed = Mathf.Max(2, 10f - garbageCount);
        tail.SetInfo(tailSpeed, lastTail);

        lastTail = tail.gameObject;

        tails.Add(tail);
    }

    int point;
    public TextMeshProUGUI textPoint;

    public int POINT
    {
        get { return point; }
        set { 
            point = value;
            textPoint.SetText(point.ToString());
        }
    }

    internal void CalcGarbagePoint()
    {
        POINT = POINT + garbageCount * garbageCount;
        // property ������� �Ʒ� �ڵ� ��� ���ʿ�
        // textPoint.SetText(point.ToString());

        garbageCount = 0;

        for (int i = 0; i < tails.Count; i++)
        {
            Destroy(tails[i].gameObject);
        }
        tails.Clear();  // ����Ʈ �޸� ����

        lastTail = defaultTailTarget;
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
