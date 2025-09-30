using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("FireObject"))
        {
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }


    void Update()
    {
        
    }
}
