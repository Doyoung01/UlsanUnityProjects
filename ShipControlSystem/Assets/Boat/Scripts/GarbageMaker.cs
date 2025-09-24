using System.Collections;
using UnityEngine;

public class GarbageMaker : MonoBehaviour
{
    // 일정 시간(1s)마다 garbage 공장에서 랜덤한 위치에 객체 생성
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
            // y position을 강제로 0으로 설정 -> 배가 garbage를 수집할 수 있는 높이로 설정
            point.y = 0;
            
            // 유클리드 거리 공식 이용 시 아래 코드 사용
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
