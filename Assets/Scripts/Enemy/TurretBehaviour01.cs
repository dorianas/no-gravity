using UnityEngine;

public class TurretBehaviour01 : MonoBehaviour
{
    [Header("Targeting Settings")]
    public Transform target;
    public float rotationSpeed = 200f;
    public float trackingRadius = 15f;

    private bool isTracking = false;

    void Update()
    {
        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.position);
        isTracking = distance <= trackingRadius;

        if (isTracking)
        {
            // 🔮 Predict future position
            Vector2 targetPos = target.position;
            Vector2 targetVelocity = Vector2.zero;

            Rigidbody2D targetRb = target.GetComponent<Rigidbody2D>();
            if (targetRb != null)
                targetVelocity = targetRb.linearVelocity;

            float estimatedTime = distance / 5f; // Adjust based on projectile speed
            Vector2 predictedPosition = targetPos + targetVelocity * estimatedTime;
            Vector2 direction = predictedPosition - (Vector2)transform.position;

            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle - 90);

            // 🧭 Rotate turret toward target
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
    void OnDrawGizmos()
    {
        // Draw the tracking radius in the Scene view
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, trackingRadius);
    }
}