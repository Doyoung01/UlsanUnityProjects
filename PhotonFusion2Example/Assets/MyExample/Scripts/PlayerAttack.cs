using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class PlayerAttack : NetworkBehaviour
{
    Animator anim;

    [Networked] float curHP { get; set; }
    
    public float maxHP = 10f;
    public Slider sliderHP;
    ChangeDetector changeDetector;

    void Start()
    {
        if (HasStateAuthority)
        {
            curHP = maxHP;
        }
        
        changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
        anim = GetComponentInChildren<Animator>();
    }

    public override void Render()
    {
        base.Render();
        foreach (var change in changeDetector.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(curHP):
                    sliderHP.value = curHP / maxHP;
                    break;
            }
        }
    }

    // 입력권한이 있는 클라이언트가 서버에게 공격 요청
    // 서버가 모든 클라이언트에게 공격 요청
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RPC_ServerPlayAttack()
    {
        RPC_ClientPlayAttack();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RPC_ClientPlayAttack()
    {
        anim.SetTrigger("Attack");
    }

    private void OnTriggerEnter(Collider other)
    {
        // 입력 권한이 있는 클라이언트의 dagger와 충돌 시
        if (HasInputAuthority && other.gameObject.name.Contains("dagger"))
        {
            // 모든 클라이언트의 HP -1
            RPC_ServerDamage(1f);
            other.enabled = false;
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_ServerDamage(float damage)
    {
        curHP = Mathf.Max(0, curHP - 1);
    }

    void Update()
    {
        // 입력권한이 있는 상태에서 좌클릭 시 공격 요청
        if (HasInputAuthority && Input.GetMouseButtonDown(0))
        {
            RPC_ServerPlayAttack();
        }
    }
}
