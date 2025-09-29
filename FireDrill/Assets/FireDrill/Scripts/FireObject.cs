using UnityEngine;

public class FireObject : MonoBehaviour
{
    // ³»±¸µµ
    public int maxDurability = 300;
    public int durability;
    ParticleSystem[] particles;

    void Start()
    {
        durability = maxDurability;
        particles = GetComponentsInChildren<ParticleSystem>();
    }

    public void TakeDamage()
    {
        durability--;
        if (durability <= 0)
        {
            Destroy(gameObject);
        }
        foreach (var ps in particles)
        {
            var em = ps.emission;
            // float per = durability / (float)maxDurability;
            em.rateOverTime = Mathf.Lerp(em.rateOverTime.constant, 0f, 0.01f);
        }
    }

    void Update()
    {
        
    }
}
