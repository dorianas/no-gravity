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

    [Header("Blast Fuel Cost")]
    public float fuelCost = 5f;

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

        if (selfCrew.currentFuel < fuelCost)
        {
            Debug.Log($"{gameObject.name} tried to fire but is out of fuel!");
            return;
        }

        selfCrew.currentFuel -= fuelCost;
        selfCrew.SendMessage("UpdateFuelUI", SendMessageOptions.DontRequireReceiver);

        // Spawn blast and temporarily parent to this ship so FireBall can auto-detect EnemyCrew01
        GameObject blast = Instantiate(blastPrefab, blasterSpawnPoint.position, blasterSpawnPoint.rotation, transform);

        Rigidbody2D rb = blast.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = blasterSpawnPoint.up * blastSpeed;

        // Optional: Detach blast after assignment if you don’t want to keep it under this object
        blast.transform.parent = null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, shootingRadius);
    }
}