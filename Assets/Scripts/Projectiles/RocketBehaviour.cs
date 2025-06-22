using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RocketBehaviour : MonoBehaviour
{
    [SerializeField] public float speed = 5f; // Rocket speed
    [SerializeField] public float rotateSpeed = 200f; // How fast the rocket turns toward target
    [SerializeField] public float initialForwardTime = 0.5f; // Initial straight movement duration
    [SerializeField] public float damage = 5f;
    public GameObject impactEffectPrefab;

    private Transform target;
    private Rigidbody2D rb;
    private bool isTrackingTarget = false;
    private float startTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        FindNearestEnemy();
        startTime = Time.time;

        // Start by moving straight for a short time
        rb.linearVelocity = transform.up * speed;
    }

    void FixedUpdate()
    {
        if (!isTrackingTarget && Time.time - startTime >= initialForwardTime)
        {
            isTrackingTarget = true; // Start tracking after a delay
        }

        if (isTrackingTarget && target != null)
        {
            Vector2 direction = (Vector2)target.position - rb.position;
            direction.Normalize();

            // Calculate turning amount using cross product
            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            // Apply angular velocity for smooth turning toward target
            rb.angularVelocity = -rotateAmount * rotateSpeed;

            // Move forward in the direction the rocket is facing
            rb.linearVelocity = transform.up * speed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Spawn explosion effect on impact
        if (impactEffectPrefab != null)
        {
            Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
        }

        // List of valid enemy tags
        string[] enemyTags = { "Enemy01", "Meteorite", "Planet" }; // Add more tags if needed

        foreach (string tag in enemyTags)
        {
            if (collision.gameObject.CompareTag(tag))
            {
                    // Apply damage if the object has a health script
                    Enemy01 enemy = collision.gameObject.GetComponent<Enemy01>(); // Use a generic script for all destructibles
                    if (enemy != null) enemy.TakeDamage(damage);

                break; // Stop checking once a match is found
            }
        }

        // Destroy the rocket on impact
        Destroy(gameObject);
    }
    void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy01");
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                target = enemy.transform;
            }
        }
    }
}