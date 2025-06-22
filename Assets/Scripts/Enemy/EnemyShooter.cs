using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject blastPrefab;
    public Transform blasterSpawnPoint;
    public float blastSpeed = 5f;

    public float fireRate = 2f;
    private float nextFireTime = 0f;

    public float shootingRadius = 10f;

    [Header("Red Dot Trail")]
    public GameObject redDotPrefab;
    public Transform dotSpawnPoint;
    public float spawnInterval = 2f;
    private float lastSpawnTime = 0f;

    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Ship");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // 🔫 Shooting Logic
        if (distance <= shootingRadius && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }

        // 🔴 Red Dot Trail
        if (Time.time - lastSpawnTime >= spawnInterval && redDotPrefab != null && dotSpawnPoint != null)
        {
            Instantiate(redDotPrefab, dotSpawnPoint.position, Quaternion.identity);
            lastSpawnTime = Time.time;
        }
    }

    void Shoot()
    {
        if (blastPrefab == null || blasterSpawnPoint == null) return;

        GameObject blast = Instantiate(blastPrefab, blasterSpawnPoint.position, blasterSpawnPoint.rotation);
        Rigidbody2D rb = blast.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = blasterSpawnPoint.up * blastSpeed;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, shootingRadius);
    }
}