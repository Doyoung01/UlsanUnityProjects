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
    // 주의: giveupDistance는 chaseDistance보다 커야 함
    public float chaseDistance = 3f;
    public float giveupDistance = 10f;
    private void UpdatePatrol()
    {
        // 목적지 이동
        Vector3 dir = patrolPoint - transform.position;
        // transform.position += Time.deltaTime * speed * dir.normalized;
        agent.SetDestination(patrolPoint);

        // 도착 시 목적지 재설정
        if (dir.magnitude <= agent.stoppingDistance)
        {
            UpdatePatrolPoint() ;
        }
        float distance = Vector3.Distance(player.transform.position, transform.position);
        // Player와 거리 3M 이내일 시 Chase 상태로 전이
        if (distance < chaseDistance)
        {
            state = State.Chase;
        }
    }

    private void UpdateChase()
    {
        // 주인공을 향해 이동
        Vector3 dir = player.transform.position - transform.position;
        // transform.position += Time.deltaTime * speed * dir.normalized;
        agent.SetDestination(player.transform.position);

        // Player와의 거리가 10M보다 멀다면 Patrol State로 전이
        if (dir.magnitude > giveupDistance)
        {
            state = State.Patrol;
            UpdatePatrolPoint();
        }
    }
}
