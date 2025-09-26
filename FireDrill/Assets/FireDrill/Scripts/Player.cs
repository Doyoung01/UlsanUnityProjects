using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    CharacterController cc;
    InputAction moveAction;
    InputAction jumpAction;

    public float gravity = -9.81f;
    public float jumpPower = 20f;
    float yVelocity;
    int jumpCount = 0;
    public int maxJumpCount = 1;
    Animator anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        anim.SetTrigger("Idle");

        var input = GetComponentInParent<PlayerInput>();
        moveAction = input.actions["Move"];
        jumpAction = input.actions["Jump"];

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

    void Start()
    {
        
    }

    public float speed = 5f;

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

        if (Mathf.Abs(v) > 0.1f)
        {
            anim.SetTrigger("Walking");
        }
        else
        {
            anim.SetTrigger("Idle");
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
