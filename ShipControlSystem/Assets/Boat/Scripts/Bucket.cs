using UnityEngine;

public class Bucket : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody)
        {
            if (other.attachedRigidbody.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                // 플레이어의 grabage 개수 정산
                // if 조건문을 통해 player를 찾아냈으므로 해당 객체에서 GetComponent가 정석
                // 하지만 Player.instance를 사용하는 게 실행이 더 빠름
                Player.instance.CalcGarbagePoint();
            }
        }
    }
}
