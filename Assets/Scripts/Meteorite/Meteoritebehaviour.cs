using UnityEngine;

public class MeteoriteBehaviour : MonoBehaviour
{
    public float minSpeed = 0.5f;
    public float maxSpeed = 2f;

    public float minSpin = -50f;
    public float maxSpin = 50f;

    [HideInInspector]
    public MeteoriteSpawner spawner; // Assigned dynamically at spawn

    public float health = 3f;
    public GameObject deathEffect;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        LaunchMeteor();
    }

    void LaunchMeteor()
    {
        // Random direction + speed
        Vector2 dir = Random.insideUnitCircle.normalized;
        float speed = Random.Range(minSpeed, maxSpeed);
        rb.linearVelocity = dir * speed;

        // Optional spin
        rb.angularVelocity = Random.Range(minSpin, maxSpin);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Planet"))
        {
            Die();
        }
        else if (collision.collider.CompareTag("Fireball"))
        {
            TakeDamage(1f); // Or damage based on bullet properties
        }
    }
    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0f)
        {
            Die();
        }
    }
    void Die()
    {
        // Optional: spawn explosion VFX
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        if (spawner != null)
        {
            spawner.HandleMeteoriteDestroyed(gameObject);
        }

        Destroy(gameObject);
    }
}