using UnityEngine;

public class BoatMaker : MonoBehaviour
{
    // Spawn ��� ����
    public Transform[] spawns;

    // ��Ʈ�� �ֱ������� ����� �÷��̾� ��ġ�� ��ġ
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
