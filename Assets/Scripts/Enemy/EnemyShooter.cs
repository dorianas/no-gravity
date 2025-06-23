using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject blastPrefab;
    public Transform blasterSpawnPoint;
    public float blastSpeed = 5f;
    public float fireRate = 2f;
    private float nextFireTime = 0f;

    public float shootingRadius = 10f;
    public float fireballFuelCost = 3f; // 🔋 Add this!

    [Header("Red Dot Trail")]
    public GameObject redDotPrefab;
    public Transform dotSpawnPoint;
    public float spawnInterval = 2f;
    private float lastSpawnTime = 0f;

    private Transform player;
    private EnemyCrew01 selfCrew;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Ship");
        if (playerObj != null)
            player = playerObj.transform;

        selfCrew = GetComponent<EnemyCrew01>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= shootingRadius && Time.time >= nextFireTime)
        {
            TryShoot();
            nextFireTime = Time.time + 1f / fireRate;
        }

        if (Time.time - lastSpawnTime >= spawnInterval && redDotPrefab != null && dotSpawnPoint != null)
        {
            Instantiate(redDotPrefab, dotSpawnPoint.position, Quaternion.identity);
            lastSpawnTime = Time.time;
        }
    }

    void TryShoot()
    {
        if (blastPrefab == null || blasterSpawnPoint == null || selfCrew == null) return;

        if (selfCrew.currentFuel < fireballFuelCost)
        {
            Debug.Log("[ENEMY SHOOTER] Not enough fuel to launch fireball.");
            return;
        }

        selfCrew.ConsumeFuel(fireballFuelCost);
        Debug.Log("[ENEMY SHOOTER] Fireball fuel consumed: " + fireballFuelCost);

        GameObject blast = Instantiate(blastPrefab, blasterSpawnPoint.position, blasterSpawnPoint.rotation);

        FireBallBehaviour fireball = blast.GetComponent<FireBallBehaviour>();
        if (fireball != null)
        {
            fireball.SetOwner(selfCrew);
            fireball.Initialize(); // Logging only
        }

        Rigidbody2D blastRb = blast.GetComponent<Rigidbody2D>();
        if (blastRb != null)
            blastRb.linearVelocity = blasterSpawnPoint.up * blastSpeed;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, shootingRadius);
    }
}