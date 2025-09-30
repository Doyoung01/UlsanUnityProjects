using System;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public enum State
    {
        Cough,
        Getup,
        Idle,
        Patrol,
        Chase,
    }

    public State state = State.Cough;
    NavMeshAgent agent;
    Animator anim;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        switch (state)
        {
            case State.Cough: UpdateCough(); break;
            case State.Getup: UpdateGetup(); break;
            case State.Idle: UpdateIdle(); break;
            case State.Patrol: UpdatePatrol(); break;
            case State.Chase: UpdateChase(); break;
        }
    }

    public void Getup()
    {
        anim.SetTrigger("Getup");
    }

    private void UpdateCough()
    {
    }

    private void UpdateGetup()
    {
    }

    GameObject player;
    private void UpdateIdle()
    {
        player = GameObject.Find("Player");

        if (player)
        {
            SetState(State.Patrol);
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
        if (distance <= chaseDistance)
        {
            SetState(State.Chase);
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
            SetState(State.Patrol);
        }
    }

    public void SetState(State next)
    {
        state = next;
        if (next == State.Patrol)
        {
            UpdatePatrolPoint();
        }
        if (next == State.Chase || next == State.Patrol)
        {
            anim.SetTrigger("Walking");
        }
    }
}
