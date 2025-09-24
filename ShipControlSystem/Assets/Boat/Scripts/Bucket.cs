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
                // �÷��̾��� grabage ���� ����
                // if ���ǹ��� ���� player�� ã�Ƴ����Ƿ� �ش� ��ü���� GetComponent�� ����
                // ������ Player.instance�� ����ϴ� �� ������ �� ����
                Player.instance.CalcGarbagePoint();
            }
        }
    }
}
