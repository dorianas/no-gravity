using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlanetGravityField : MonoBehaviour
{
    public enum GravityFalloffType
    {
        Linear,         // Gradual, gamey
        InverseLinear,  // More dramatic near center
        InverseSquare,  // Realistic space physics
        Constant        // Uniform pull everywhere in the zone
    }

    [Header("Gravity Settings")]
    public float gravityMin = 2f;
    public float gravityMax = 10f;
    public float gravityRadius = 4f;
    public GravityFalloffType falloffType = GravityFalloffType.Linear;
    public string[] affectedTags = { "Ship", "Enemy01", "Meteorite" };

    private void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, gravityRadius);

        foreach (Collider2D col in colliders)
        {
            foreach (string tag in affectedTags)
            {
                if (col.CompareTag(tag))
                {
                    Rigidbody2D rb = col.attachedRigidbody;
                    if (rb != null)
                    {
                        Vector2 toCenter = (Vector2)(transform.position - col.transform.position);
                        float distance = toCenter.magnitude;

                        if (distance > gravityRadius || distance < 0.01f)
                            continue;

                        float gravity = CalculateGravity(distance);
                        Vector2 force = toCenter.normalized * gravity * rb.mass * Time.fixedDeltaTime;

                        rb.AddForce(force, ForceMode2D.Force);
                    }
                }
            }
        }
    }

    private float CalculateGravity(float distance)
    {
        float t = Mathf.Clamp01(1f - (distance / gravityRadius));

        switch (falloffType)
        {
            case GravityFalloffType.Linear:
                return Mathf.Lerp(gravityMin, gravityMax, t);

            case GravityFalloffType.InverseLinear:
                return Mathf.Lerp(gravityMin, gravityMax, 1f / Mathf.Max(distance, 0.1f));

            case GravityFalloffType.InverseSquare:
                float g = gravityMax / (distance * distance);
                return Mathf.Clamp(g, gravityMin, gravityMax);

            case GravityFalloffType.Constant:
                return gravityMax;

            default:
                return gravityMax;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, gravityRadius);

#if UNITY_EDITOR
        int steps = 30;
        float stepSize = gravityRadius / steps;

        for (int i = 0; i <= steps; i++)
        {
            float dist = i * stepSize;
            float gravity = CalculateGravity(dist);
            float height = gravity * 0.1f;

            Vector3 from = transform.position + Vector3.right * dist;
            Vector3 to = from + Vector3.up * height;

            Handles.color = Color.cyan;
            Handles.DrawLine(from, to);
        }
#endif
    }
}