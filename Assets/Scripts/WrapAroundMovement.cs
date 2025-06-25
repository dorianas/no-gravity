using UnityEngine;

public class WrapAroundMovement : MonoBehaviour
{
    private const float xMin = -50f;
    private const float xMax = 14.5f;
    private const float yMin = -20f;
    private const float yMax = 20f;

    public float XMin => xMin;
    public float XMax => xMax;
    public float YMin => yMin;
    public float YMax => yMax;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("WrapAroundMovement requires a Rigidbody2D component.");
        }
    }

    void Update()
    {
        if (rb == null) return;

        Vector2 pos = rb.position;

        if (pos.x > xMax) pos.x = xMin;
        else if (pos.x < xMin) pos.x = xMax;

        if (pos.y > yMax) pos.y = yMin;
        else if (pos.y < yMin) pos.y = yMax;

        rb.position = pos;

        DrawRuntimeBounds();
    }

    private void DrawRuntimeBounds()
    {
        Debug.DrawLine(new Vector3(xMin, yMin), new Vector3(xMax, yMin), Color.green);
        Debug.DrawLine(new Vector3(xMax, yMin), new Vector3(xMax, yMax), Color.green);
        Debug.DrawLine(new Vector3(xMax, yMax), new Vector3(xMin, yMax), Color.green);
        Debug.DrawLine(new Vector3(xMin, yMax), new Vector3(xMin, yMin), Color.green);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector3(xMin, yMin), new Vector3(xMax, yMin));
        Gizmos.DrawLine(new Vector3(xMax, yMin), new Vector3(xMax, yMax));
        Gizmos.DrawLine(new Vector3(xMax, yMax), new Vector3(xMin, yMax));
        Gizmos.DrawLine(new Vector3(xMin, yMax), new Vector3(xMin, yMin));
    }
}