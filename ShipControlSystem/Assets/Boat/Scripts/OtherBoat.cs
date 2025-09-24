using Unity.VisualScripting;
using UnityEngine;

using Random = UnityEngine.Random;

public class OtherBoat : MonoBehaviour
{
    public float speed = 5;
    Vector3 dir;

    void Start()
    {
        int value = Random.Range(0, 10);

        if (value < 3)
        {
            GameObject player = Player.instance.gameObject;
            dir = player.transform.position - this.transform.position;
            dir.Normalize();
            transform.forward = dir;
        } else
        {
            // 30% Ȯ���� �÷��̾� ����, �������� �չ������� ����
            dir = transform.forward;
        }
    }

    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;

        
    }

    private void OnTriggerEnter(Collider other)
    {
        // �浹 ��ü�� Player�ΰ�?
        if (other.attachedRigidbody)
        {
            if (other.attachedRigidbody.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                ResultManager.instance.ShowResultUI();
            }
        }
    }
}
