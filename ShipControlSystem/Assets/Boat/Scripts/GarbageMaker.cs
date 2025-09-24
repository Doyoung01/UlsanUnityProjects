using System.Collections;
using UnityEngine;

public class GarbageMaker : MonoBehaviour
{
    // ���� �ð�(1s)���� garbage ���忡�� ������ ��ġ�� ��ü ����
    void Start()
    {
        StartCoroutine(IELoop());
    }

    public GameObject garbageFactory;
    public float makeTime = 1f;

    IEnumerator IELoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(makeTime);
            Vector3 point = transform.position + Random.insideUnitSphere * 10f;
            // y position�� ������ 0���� ���� -> �谡 garbage�� ������ �� �ִ� ���̷� ����
            point.y = 0;
            
            // ��Ŭ���� �Ÿ� ���� �̿� �� �Ʒ� �ڵ� ���
            // float dist = Vector3.Distance(point, Player.instance.transform.position);
            Vector3 dir = point - Player.instance.transform.position;
            if(dir.magnitude < 2)
            {
                point += dir * 2;
            }
            point.y = 0;

            Instantiate(garbageFactory, point, Quaternion.identity);
        }
    }

    void Update()
    {
        
    }
}
