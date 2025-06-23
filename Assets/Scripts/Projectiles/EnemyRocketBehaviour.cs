using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyRocketBehaviour : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] public float speed = 5f;
    [SerializeField] public float rotateSpeed = 200f;
    [SerializeField] public float initialForwardTime = 0.5f;

    [Header("Damage Settings")]
    [SerializeField] public float damage = 5f;
    public GameObject impactEffectPrefab;

    [Header("Targeting")]
    public string trackingTag = "Enemy01";             // 🧭 Tag to search for nearest target
    public string[] damageableTags = { "Enemy01" };    // 💥 Tags that will trigger damage

    private Transform target;
    private Rigidbody2D rb;
    private bool isTrackingTarget = false;
    private float startTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        FindNearestTarget();
        startTime = Time.time;
        rb.linearVelocity = transform.up * speed;
    }

    void FixedUpdate()
    {
        if (!isTrackingTarget && Time.time - startTime >= initialForwardTime)
            isTrackingTarget = true;

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

        foreach (string tag in damageableTags)
        {
            if (collision.gameObject.CompareTag(tag))
            {
                var damageable = collision.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damage); // 🔄 Use interface for flexibility
                }
                break;
            }
        }

        Destroy(gameObject);
    }

    void FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(trackingTag);
        float closestDistance = Mathf.Infinity;

        foreach (GameObject obj in targets)
        {
            float distance = Vector2.Distance(transform.position, obj.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                target = obj.transform;
            }
        }
    }
}