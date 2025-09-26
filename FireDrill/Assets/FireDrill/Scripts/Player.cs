using JetBrains.Annotations;
using System;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    CharacterController cc;
    InputAction moveAction, jumpAction, runAction, eventAction, putAction, actionAction;

    public float gravity = -9.81f;
    public float jumpPower = 20f;
    float yVelocity;
    int jumpCount = 0;
    public int maxJumpCount = 1;
    Animator anim;
    bool bRun;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();

        var input = GetComponentInParent<PlayerInput>();
        moveAction = input.actions["Move"];
        jumpAction = input.actions["Jump"];
        runAction = input.actions["Run"];
        eventAction = input.actions["Event"];
        putAction = input.actions["Put"];
        actionAction = input.actions["Action"];

        eventAction.performed += OnMyEvent;
        putAction.performed += c => Put();
        actionAction.performed += c =>
        {
            if (grabObject)
            {
                grabObject.StartPowder();
            }
        };

        actionAction.canceled += c =>
        {
            if (grabObject)
            {
                grabObject.StopPowder();
            }
        };

        bRun = false;
        runAction.performed += (context) => { bRun = true; };
        runAction.canceled += (context) => { bRun = false; };

        jumpAction.performed += (context) =>
        {
            // 스페이스바를 누를 시 점프
            if (jumpCount < maxJumpCount)
            {
                yVelocity = jumpPower;
                jumpCount++;
            }
        };

        cc = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnMyEvent(InputAction.CallbackContext context)
    {
        // 앞에 있는 충돌체 검사
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        // Raycast 이용
        // bool bHit = Physics.Raycast(ray, out RaycastHit hitInfo, 10);

        // SphereCast 이용
        bool bHit = Physics.SphereCast(ray, 1, out RaycastHit hitInfo, 15);

        if (bHit)
        {
            // 이름에 Door 포함 시 Animator Component를 get
            if (hitInfo.transform.tag.Equals("Door"))
            {
                var anim = hitInfo.transform.GetComponentInChildren<Animator>();
                if (anim)
                {
                    anim.Play("Opening");
                }
            }
            // 해당 충돌체가 소화기일 경우 소화기를 잡음
            else if (hitInfo.transform.tag.Equals("FireExtinguisher"))
            {
                print("잡았다");
                Grab(ref hitInfo);
            }
        }
    }

    void Grab(ref RaycastHit hitInfo)
    {
        hitInfo.rigidbody.isKinematic = true;
        hitInfo.transform.parent = fireExtTarget;
        hitInfo.transform.localPosition = Vector3.zero;
        hitInfo.transform.localRotation = Quaternion.identity;

        grabObject = hitInfo.transform.GetComponent<FireExtinguisher>();
        print(grabObject);
    }
    
    void Put()
    {
        if (grabObject)
        {
            grabObject.GetComponent<Rigidbody>().isKinematic = false;
            grabObject.transform.parent = null;
            grabObject = null;
        }
    }

    FireExtinguisher grabObject;
    public Transform fireExtTarget;

    void Start()
    {
        
    }
    float walkSpeed = 3f, runSpeed = 6f;
    float speed;

    private void FixedUpdate()
    {
        // 땅에 닿았을 시 jumpCount를 0으로 초기화
        if (cc.isGrounded)
        {
            jumpCount = 0;
        }
    }

    void Update()
    {
        var move = moveAction.ReadValue<Vector2>();

        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");

        float h = move[0];
        float v = move[1];

        speed = walkSpeed;
        if (bRun)
        {
            anim.SetFloat("h", h * 2);
            anim.SetFloat("v", v * 2);
            speed = runSpeed;
        }
        else
        {
            anim.SetFloat("h", h);
            anim.SetFloat("v", v);
        }


        yVelocity += gravity * Time.deltaTime;


        Vector3 dir = new Vector3(h, 0, v);
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        dir.Normalize();

        Vector3 velocity = dir * speed;
        velocity.y = yVelocity;

        // 이 부분은 FixedUㅔdate에 작성하는 게 좋음
        cc.Move(Time.deltaTime * velocity);
    }
}
