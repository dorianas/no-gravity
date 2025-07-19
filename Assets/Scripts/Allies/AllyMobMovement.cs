using UnityEngine;

public class AllyMobMovement : MonoBehaviour
{
    [Header("Path Settings")]
    public Transform[] waypoints;
    public float baseSpeed = 1f;           // Original speed set in Inspector
    public float slowedSpeed = 0.5f;       // Not used directly anymore
    private int currentIndex = 0;

    [Header("Enemy Detection")]
    public float detectionRadius = 8f;
    private Transform currentEnemy;

    private float currentSpeed = 0f;

    void Update()
    {
        DetectNearestEnemy();

        if (currentEnemy != null)
        {
            // Gradually slow down
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, Time.deltaTime * 3f);

            // Face the enemy
            Vector3 directionToEnemy = currentEnemy.position - transform.position;
            transform.up = directionToEnemy.normalized;
        }
        else
        {
            // Resume base speed smoothly
            currentSpeed = Mathf.Lerp(currentSpeed, baseSpeed, Time.deltaTime * 2f);

            if (waypoints.Length == 0) return;

            Transform targetPoint = waypoints[currentIndex];
            Vector3 direction = targetPoint.position - transform.position;

            if (direction.magnitude < 0.1f)
            {
                currentIndex = (currentIndex + 1) % waypoints.Length;
                return;
            }

            transform.position += direction.normalized * currentSpeed * Time.deltaTime;
            transform.up = direction.normalized;
        }
    }

    void DetectNearestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        currentEnemy = null;

        float closestDistance = Mathf.Infinity;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy01"))
            {
                float dist = Vector2.Distance(transform.position, hit.transform.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    currentEnemy = hit.transform;
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        // Waypoint visualization
        Gizmos.color = Color.green;
        if (waypoints != null)
        {
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (waypoints[i] != null)
                    Gizmos.DrawSphere(waypoints[i].position, 0.2f);

                if (i < waypoints.Length - 1 && waypoints[i] != null && waypoints[i + 1] != null)
                    Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
        }

        // Detection radius gizmo
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}