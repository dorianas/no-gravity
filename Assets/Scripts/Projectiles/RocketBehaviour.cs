using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RocketBehaviour : MonoBehaviour
{
    [Header("Rocket Settings")]
    [SerializeField] public float speed = 5f;
    [SerializeField] public float rotateSpeed = 200f;
    [SerializeField] public float initialForwardTime = 0.5f;
    [SerializeField] public float damage = 5f;

    [Header("Effects")]
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

        // Initial forward thrust
        rb.linearVelocity = transform.up * speed;
    }

    void FixedUpdate()
    {
        if (!isTrackingTarget && Time.time - startTime >= initialForwardTime)
        {
            isTrackingTarget = true;
        }

        if (isTrackingTarget && target != null)
        {
            Vector2 direction = (Vector2)target.position - rb.position;
            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            rb.angularVelocity = -rotateAmount * rotateSpeed;
            rb.linearVelocity = transform.up * speed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (impactEffectPrefab != null)
        {
            Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
        }

        // If the hit object is damageable, apply damage
        if (collision.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
        }

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