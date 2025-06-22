using UnityEngine;

public class Enemy01Behaviour : MonoBehaviour
{
    // Enemy attributes
    public float health = 50f;
    public GameObject deathEffectPrefab;
    public float respawnTime = 3f;

    // Homing AI
    public float rotateSpeed = 3f;
    public float speed = 2f;
    public float trackingRadius = 15f;  // New tracking radius
    public float shootingRadius = 10f;  // Separate shooting radius
    public float initialForwardTime = 1f; // Delay before homing starts

    private Transform player;
    private Rigidbody2D rb;
    private bool isTrackingTarget = false;
    private float startTime;

    // Shooting Mechanics
    public GameObject blastPrefab; // Assign `Blast01` in Inspector
    public Transform blasterSpawnPoint; // Assign `BlasterSpawnPoint`
    public float blastSpeed = 5f; // Editable speed in Inspector
    public float fireRate = 2f; // Shots per second
    private float nextFireTime = 0f;

    public GameObject redDotPrefab;
    public Transform dotSpawnPoint; // Assign spawn point in Inspector
    public float spawnInterval = 2f; // Adjust interval time in Inspector
    private float lastSpawnTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) Debug.LogError("Rigidbody2D missing on Enemy01!");

        GameObject playerObj = GameObject.FindGameObjectWithTag("Ship");
        if (playerObj != null) player = playerObj.transform;

        startTime = Time.time; // Track spawn time
    }

    void FixedUpdate()
    {
        if (player == null || rb == null) return;

        if (!isTrackingTarget && Time.time - startTime >= initialForwardTime)
        {
            isTrackingTarget = true;
        }

        if (isTrackingTarget && Vector2.Distance(transform.position, player.position) <= trackingRadius)
        {
            // Calculate direction to player
            Vector2 direction = (Vector2)player.position - rb.position;
            direction.Normalize();

            // Rotate smoothly towards the player
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);

            // Apply forward movement
            rb.linearVelocity = direction * speed;
        }
    }

    void Update()
    {
        //Spawn red dots
        if (Time.time - lastSpawnTime > spawnInterval)
        {
            Instantiate(redDotPrefab, dotSpawnPoint.position, Quaternion.identity);
            lastSpawnTime = Time.time;
        }
        // Enemy shoots only if the player is within shooting radius
        if (isTrackingTarget && player != null && Vector2.Distance(transform.position, player.position) <= shootingRadius)
        {
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }

    void Shoot()
    {
        if (blastPrefab != null && blasterSpawnPoint != null)
        {
            GameObject blast = Instantiate(blastPrefab, blasterSpawnPoint.position, blasterSpawnPoint.rotation);
            Rigidbody2D blastRb = blast.GetComponent<Rigidbody2D>();

            if (blastRb != null)
            {
                blastRb.linearVelocity = blasterSpawnPoint.up * blastSpeed; // Fire forward
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fireball"))
        {
            if (collision.gameObject.TryGetComponent(out FireBallBehaviour fireball))
            {
                TakeDamage(fireball.damage);
            }
            Destroy(collision.gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"Enemy01 hit! Health remaining: {health}");

        if (health <= 0)
        {
            if (deathEffectPrefab != null)
            {
                Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            }
            GameManager.instance.RespawnEnemy(gameObject, transform.position, respawnTime);
            gameObject.SetActive(false);
        }
    }
    void OnDrawGizmosSelected()
    {
        if (player == null) return;

        // Set Gizmo color for tracking radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, trackingRadius);

        // Set Gizmo color for shooting radius
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, shootingRadius);
    }
}