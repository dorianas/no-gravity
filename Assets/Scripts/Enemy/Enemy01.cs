using UnityEngine;

public class Enemy01 : MonoBehaviour, IDamageable
{
    [Header("Attributes")]
    public float health = 50f;
    public GameObject deathEffectPrefab;
    public float respawnTime = 3f;

    [Header("References")]
    public Rigidbody2D rb;

    void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (rb == null)
            Debug.LogError("Rigidbody2D missing on Enemy01!");
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log($"Enemy01 hit! Health remaining: {health}");

        if (health <= 0f)
        {
            if (deathEffectPrefab != null)
            {
                Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            }

            // Assuming GameManager handles pooling or respawn logic
            GameManager.instance.RespawnEnemy(gameObject, transform.position, respawnTime);
            gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Optional logic if fireballs or other sources should damage this directly
        if (collision.gameObject.CompareTag("Fireball"))
        {
            if (collision.gameObject.TryGetComponent(out FireBallBehaviour fireball))
            {
                TakeDamage(fireball.damage);
            }
            Destroy(collision.gameObject);
        }
    }
}