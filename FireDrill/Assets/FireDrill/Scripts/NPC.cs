using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public enum State
    {
        Idle,
        Patrol,
        Chase,
    }

    public State state = State.Idle;
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        switch (state)
        {
            case State.Idle: UpdateIdle(); break;
            case State.Patrol: UpdatePatrol(); break;
            case State.Chase: UpdateChase(); break;
        }
    }

    GameObject player;
    private void UpdateIdle()
    {
        player = GameObject.Find("Player");

        if (player)
        {
            state = State.Patrol;
            UpdatePatrolPoint();
        }
    }

    Vector3 patrolPoint;
    void UpdatePatrolPoint(float radius = 5f)
    {
        patrolPoint = transform.position + UnityEngine.Random.insideUnitSphere* radius;
        patrolPoint.y = 0;
    }

    public float speed = 5f;
    // ����: giveupDistance�� chaseDistance���� Ŀ�� ��
    public float chaseDistance = 3f;
    public float giveupDistance = 10f;
    private void UpdatePatrol()
    {
        // ������ �̵�
        Vector3 dir = patrolPoint - transform.position;
        // transform.position += Time.deltaTime * speed * dir.normalized;
        agent.SetDestination(patrolPoint);

        // ���� �� ������ �缳��
        if (dir.magnitude <= agent.stoppingDistance)
        {
            UpdatePatrolPoint() ;
        }
        float distance = Vector3.Distance(player.transform.position, transform.position);
        // Player�� �Ÿ� 3M �̳��� �� Chase ���·� ����
        if (distance < chaseDistance)
        {
            state = State.Chase;
        }
    }

    private void UpdateChase()
    {
        // ���ΰ��� ���� �̵�
        Vector3 dir = player.transform.position - transform.position;
        // transform.position += Time.deltaTime * speed * dir.normalized;
        agent.SetDestination(player.transform.position);

        // Player���� �Ÿ��� 10M���� �ִٸ� Patrol State�� ����
        if (dir.magnitude > giveupDistance)
        {
            state = State.Patrol;
            UpdatePatrolPoint();
        }
    }
}
