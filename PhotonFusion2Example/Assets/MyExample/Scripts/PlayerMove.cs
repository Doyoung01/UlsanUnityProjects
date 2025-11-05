using Fusion;
using System;
using TMPro;
using UnityEngine;

public class PlayerMove : NetworkBehaviour
{
    public float speed = 3f;
    public float rotSpeed = 200f;

    public GameObject cameraRig;
    public Transform body;
    public Animator anim;
    NetworkCharacterController cc;


    void Start()
    {
    }

    public TextMeshProUGUI textNickname;
    [Networked] string myNickname { get; set; }
    public override void Spawned()
    {
        base.Spawned();
        cc = GetComponent<NetworkCharacterController>();

        // 입력 권한이 있는 플레이어만 조작하기 위한 코드
        if (!HasInputAuthority)
        {
            // 입력 권한이 없는 카메라는 모두 OFF
            cameraRig.transform.GetChild(0).gameObject.SetActive(false);
            textNickname.SetText(myNickname);
        }
        else
        {
            myNickname = ConnManager.instance.userNickname;
            textNickname.SetText(ConnManager.instance.userNickname);
            textNickname.color = Color.blue;
            RPC_ServerSetNickname(ConnManager.instance.userNickname);
        }
    }

    // 입력권한이 있는 클라이언트가 서버에게 이름 변경 요청
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_ServerSetNickname(string nickname)
    {
        myNickname = nickname;
        RPC_ClientSetNickname(nickname);
    }
    
    // 요청 받은 서버가 다른 모든 클라이언트에게 이름 변경 요청
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_ClientSetNickname(string nickname)
    {
        textNickname.SetText(nickname);
    }

    // 서버와 동기화하는 업데이트 주기
    // 기존 Update()보다 주기가 김
    // Time.deltaTime 대신 Runner.DeltaTime 사용
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        Move();
        Rotation();
    }

    // Fusion에서 화면을 그리는 함수
    public override void Render()
    {
        base.Render();
        anim.SetFloat("Speed", magnitude);
    }

    void Update()
    {
    }

    [Networked] float magnitude { get; set; }

    private void Move()
    {
        if (!GetInput(out Vector3 direction))
        {
            return;
        }
        direction.Normalize();

        direction = cameraRig.transform.TransformDirection(direction);

        cc.Move(Runner.DeltaTime * speed * direction);

        magnitude = direction.magnitude;

        if (magnitude > 0)
        {
            body.rotation = Quaternion.LookRotation(direction);
        }
    }

    // 카메라 회전
    private void Rotation()
    {
        if (GetInput(out NetworkInputData data))
        {
            float mx = data.mouseX;
            cameraRig.transform.eulerAngles += rotSpeed * Runner.DeltaTime * new Vector3(0, mx, 0);
        }
    }

    bool GetInput(out Vector3 dir)
    {
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");
        //dir = new Vector3(h, 0, v);

        if (GetInput(out NetworkInputData data))
        {
            dir = data.direction;
        }
        else
        {
            dir = Vector3.zero;
        }

        return true;
    }
}
