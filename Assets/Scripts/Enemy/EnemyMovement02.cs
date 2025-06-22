using UnityEngine;

public class EnemyMovement02 : MonoBehaviour
{
    [Header("Targeting & Movement")]
    public Transform target;
    public float acceleration = 5f;
    public float maxSpeed = 10f;
    public float rotationSpeed = 200f;
    public float decelerationRate = 1f;
    public float trackingRadius = 15f;

    [Header("Avoidance Settings")]
    public float avoidDistance = 5f;
    public float avoidanceStrength = 3f;
    public float raySpreadAngle = 30f;  // Spread angle between outer rays
    public int rayCount = 3;            // Should be odd (e.g. 3, 5, 7)
    public LayerMask obstacleLayer;

    private Rigidbody2D rb;
    private float thrust = 0f;
    private bool isTracking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.position);
        isTracking = distance <= trackingRadius;

        if (isTracking)
        {
            Vector2 direction = (Vector2)(target.position - transform.position);
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle - 90);

            Vector2 avoidance = CalculateAvoidance();
            if (avoidance != Vector2.zero)
            {
                float avoidAngle = Mathf.Atan2(avoidance.y, avoidance.x) * Mathf.Rad2Deg;
                Quaternion avoidRotation = Quaternion.Euler(0, 0, avoidAngle - 90);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, avoidRotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            thrust += acceleration * Time.deltaTime;
            thrust = Mathf.Clamp(thrust, 0, maxSpeed);
        }
        else
        {
            thrust -= decelerationRate * Time.deltaTime;
            thrust = Mathf.Clamp(thrust, 0, maxSpeed);
        }
    }

    void FixedUpdate()
    {
        if (isTracking)
        {
            rb.linearVelocity += (Vector2)transform.up * thrust * Time.fixedDeltaTime;
        }

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    private Vector2 CalculateAvoidance()
    {
        Vector2 forward = transform.up;
        Vector2 origin = transform.position;
        Vector2 avoidance = Vector2.zero;

        int half = rayCount / 2;
        for (int i = -half; i <= half; i++)
        {
            float angle = i * raySpreadAngle / (rayCount - 1);
            Vector2 dir = Quaternion.Euler(0, 0, angle) * forward;
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, avoidDistance, obstacleLayer);

            if (hit.collider != null)
            {
                avoidance -= dir.normalized * (avoidDistance - hit.distance) / avoidDistance;
            }
        }

        return avoidance.normalized * avoidanceStrength;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        Vector2 origin = transform.position;
        Vector2 forward = transform.up;
        int half = rayCount / 2;

        for (int i = -half; i <= half; i++)
        {
            float angle = i * raySpreadAngle / (rayCount - 1);
            Vector2 dir = Quaternion.Euler(0, 0, angle) * forward;
            Vector2 endPoint = origin + dir.normalized * avoidDistance;

            Gizmos.color = Color.red;
            Gizmos.DrawRay(origin, dir.normalized * avoidDistance);

            if (i == 0)
            {
                Gizmos.DrawSphere(endPoint, 0.1f);
            }
        }
    }
}