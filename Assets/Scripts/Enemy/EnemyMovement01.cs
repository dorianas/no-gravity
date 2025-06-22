using UnityEngine;

public class EnemyMovement01 : MonoBehaviour
{
    public Transform target; // Assign the player here
    public float acceleration = 5f;
    public float maxSpeed = 10f;
    public float rotationSpeed = 200f;
    public float decelerationRate = 1f;

    public float trackingRadius = 15f;  // Starts moving toward player in this range

    private Rigidbody2D rb;
    private float thrust = 0f;
    private bool isTracking = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //rb.angularVelocity = 0f;
        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.position);
        isTracking = distance <= trackingRadius;

        if (isTracking)
        {
            // Rotate toward player
            Vector2 direction = (Vector2)(target.position - transform.position);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float step = rotationSpeed * Time.deltaTime;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90); // Adjust if needed
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);

            // Accelerate
            thrust += acceleration * Time.deltaTime;
            thrust = Mathf.Clamp(thrust, 0, maxSpeed);
        }
        else
        {
            // Decelerate if player is out of range
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
}