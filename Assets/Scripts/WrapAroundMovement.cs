using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WrapAroundMovement : MonoBehaviour
{
    public enum WrapMode { WorldBounds, CameraView }
    public WrapMode wrapMode = WrapMode.WorldBounds;

    [Header("World Wrap Area (for WorldBounds mode)")]
    [SerializeField] private float xMin = -25f;
    [SerializeField] private float xMax = 25f;
    [SerializeField] private float yMin = -20f;
    [SerializeField] private float yMax = 20f;

    public float XMin => xMin;
    public float XMax => xMax;
    public float YMin => yMin;
    public float YMax => yMax;
    public float WrapWidth => xMax - xMin;
    public float WrapHeight => yMax - yMin;

    private Camera cam;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    void Update()
    {
        if (rb == null) return;

        Vector2 pos = rb.position;
        bool wrapped = false;

        if (wrapMode == WrapMode.WorldBounds)
        {
            if (pos.x > xMax) pos.x = xMin;
            else if (pos.x < xMin) pos.x = xMax;

            if (pos.y > yMax) pos.y = yMin;
            else if (pos.y < yMin) pos.y = yMax;
        }
        else if (wrapMode == WrapMode.CameraView && cam != null)
        {
            float camHeight = 2f * cam.orthographicSize;
            float camWidth = camHeight * cam.aspect;

            float left = cam.transform.position.x - camWidth / 2f;
            float right = cam.transform.position.x + camWidth / 2f;
            float bottom = cam.transform.position.y - camHeight / 2f;
            float top = cam.transform.position.y + camHeight / 2f;

            if (pos.x < left) { pos.x = right; wrapped = true; }
            else if (pos.x > right) { pos.x = left; wrapped = true; }

            if (pos.y < bottom) { pos.y = top; wrapped = true; }
            else if (pos.y > top) { pos.y = bottom; wrapped = true; }

            DrawBounds(left, right, bottom, top, Color.magenta);
        }

        if (wrapped || wrapMode == WrapMode.WorldBounds)
            rb.position = pos;
    }

    private void DrawBounds(float xMin, float xMax, float yMin, float yMax, Color color)
    {
        Vector3 bl = new Vector3(xMin, yMin);
        Vector3 br = new Vector3(xMax, yMin);
        Vector3 tr = new Vector3(xMax, yMax);
        Vector3 tl = new Vector3(xMin, yMax);

        Debug.DrawLine(bl, br, color);
        Debug.DrawLine(br, tr, color);
        Debug.DrawLine(tr, tl, color);
        Debug.DrawLine(tl, bl, color);
    }

    void OnDrawGizmos()
    {
        if (wrapMode == WrapMode.WorldBounds)
        {
            Gizmos.color = Color.cyan;
            DrawBounds(xMin, xMax, yMin, yMax, Gizmos.color);
        }
    }
}