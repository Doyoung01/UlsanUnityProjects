using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    CharacterController cc;
    InputAction moveAction, jumpAction, runAction, eventAction, putAction, actionAction, throwAction;

    public float gravity = -9.81f;
    public float jumpPower = 20f;
    float yVelocity;
    int jumpCount = 0;
    public int maxJumpCount = 1;
    Animator anim;
    bool bRun;
    public GameObject ballFactory;
    public Transform ballPoint;

    float curHP;
    public float maxHP = 10f;
    public Slider sliderHP;
    public Image imageDamage;
    bool bDamageEffect;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("FireObject"))
        {
            curHP--;
            sliderHP.value = curHP;
            if (curHP <= 0)
            {
                ResultManager.Instance.ShowUI();
                ResultManager.Instance.failureUI.SetActive(true);
            }

            if (!bDamageEffect)
            {
                bDamageEffect = true;
                StartCoroutine(IEDamage());
            }
        }
        else if (hit.collider.name == "Goal")
        {
            print("����");
            ResultManager.Instance.ShowUI();
            ResultManager.Instance.successUI.SetActive(true);
        }
    }

    IEnumerator IEDamage()
    {
        Color c = imageDamage.color;
        
        for (float t = 0.75f; t > 0; t -= Time.deltaTime * 2)
        {
            c.a = t;
            imageDamage.color = c;
            yield return new WaitForSeconds(Time.deltaTime * 2);
        }
        c.a = 0;
        imageDamage.color = c;
        bDamageEffect = false;
    }

    private void Awake()
    {
        sliderHP.minValue = 0;
        sliderHP.maxValue = maxHP;
        sliderHP.value = maxHP;
        curHP = maxHP;

        anim = GetComponentInChildren<Animator>();

        var input = GetComponentInParent<PlayerInput>();
        moveAction = input.actions["Move"];
        jumpAction = input.actions["Jump"];
        runAction = input.actions["Run"];
        eventAction = input.actions["Event"];
        putAction = input.actions["Put"];
        actionAction = input.actions["Action"];
        throwAction = input.actions["ThrowBall"];

        throwAction.performed += c =>
        {
            Instantiate(ballFactory, ballPoint.position, ballPoint.rotation);
        };

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
            // �����̽��ٸ� ���� �� ����
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
        // �տ� �ִ� �浹ü �˻�
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        // Raycast �̿�
        // bool bHit = Physics.Raycast(ray, out RaycastHit hitInfo, 10);

        // SphereCast �̿�
        bool bHit = Physics.SphereCast(ray, 1, out RaycastHit hitInfo, 15);

        if (bHit)
        {
            // �̸��� Door ���� �� Animator Component�� get
            if (hitInfo.transform.tag.Equals("Door"))
            {
                var anim = hitInfo.transform.GetComponentInChildren<Animator>();
                if (anim)
                {
                    anim.Play("Opening");
                }
            }
            // �ش� �浹ü�� ��ȭ���� ��� ��ȭ�⸦ ����
            else if (hitInfo.transform.tag.Equals("FireExtinguisher"))
            {
                print("��Ҵ�");
                Grab(ref hitInfo);
            }
            else if (hitInfo.transform.root.tag.Equals("NPC"))
            {
                print("����̴�");
                var npc = hitInfo.transform.GetComponent<NPC>();
                if (npc)
                {
                    npc.Getup();
                }
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
        // ���� ����� �� jumpCount�� 0���� �ʱ�ȭ
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

        // �� �κ��� FixedU��date�� �ۼ��ϴ� �� ����
        cc.Move(Time.deltaTime * velocity);
    }
}
