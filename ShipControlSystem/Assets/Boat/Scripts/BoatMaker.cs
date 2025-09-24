using UnityEngine;

public class BoatMaker : MonoBehaviour
{
    // Spawn 목록 생성
    public Transform[] spawns;

    // 보트를 주기적으로 만들어 플레이어 위치에 배치
    float curTime;
    public float makeTime = 2;
    public GameObject boatFactory;

    int previdx;

    void Start()
    {
        
    }

    void Update()
    {
        curTime += Time.deltaTime;
        if (curTime > makeTime)
        {
            curTime = 0;
            int idx = Random.Range(0, spawns.Length);

            if(idx == previdx)
            {
                idx = (idx + 1) % spawns.Length;
            }

            Transform t = spawns[idx].transform;
            Instantiate(boatFactory, t.position, t.rotation);
            previdx = idx;
        }
    }
}
