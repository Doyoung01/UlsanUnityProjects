using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    PlayerInput input = null;
    CharacterController cc;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        cc = GetComponent<CharacterController>();
    }

    void Start()
    {
        
    }

    public float speed = 5f;
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        dir.Normalize();

        cc.SimpleMove(speed * Time.deltaTime * dir);
    }
}
