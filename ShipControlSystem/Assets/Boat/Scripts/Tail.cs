using System;
using UnityEngine;

public class Tail : MonoBehaviour
{
    public float speed = 5f;

    GameObject target;

    internal void SetInfo(float tailSpeed, GameObject newTarget)
    {
        target = newTarget;
        speed = tailSpeed;
    }

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 dir = target.transform.position - this.transform.position;
        dir.Normalize();

        transform.position += dir * speed * Time.deltaTime;
    }
}
