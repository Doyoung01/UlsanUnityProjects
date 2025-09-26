using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ParticleSystem ps;

    public void StartPowder()
    {
        ps.Stop();
        ps.Play();
    }

    public void StopPowder()
    {
        ps.Stop();
    }
}
