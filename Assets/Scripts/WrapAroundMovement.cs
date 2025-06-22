using UnityEngine;

public class WrapAroundMovement : MonoBehaviour
{
    public float xMin = -20f, xMax = 20f;
    public float yMin = -20f, yMax = 20f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rb == null) return;

        Vector2 newPosition = rb.position;

        if (newPosition.x > xMax) newPosition.x = xMin;
        else if (newPosition.x < xMin) newPosition.x = xMax;

        if (newPosition.y > yMax) newPosition.y = yMin;
        else if (newPosition.y < yMin) newPosition.y = yMax;

        rb.position = newPosition; // <- Teleport without collision issues
    }
}