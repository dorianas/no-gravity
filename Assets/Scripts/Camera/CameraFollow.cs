using UnityEngine;

public class CameraFollowDynamic : MonoBehaviour
{
    public Transform targetA;
    public Transform targetB;
    public Vector3 offset = new Vector3(0, 0, -10f);

    [Header("Zoom Settings")]
    public float zoomLimiter = 40f;
    public float minZoom = 5f;
    public float maxZoom = 20f;
    public float zoomSmoothTime = 0.2f;

    [Header("Movement Smoothing")]
    public float moveSpeed = 5f;

    [Header("UI Edge Padding")]
    [Range(-0.5f, 0.5f)]
    public float rightPaddingPercent = 0.2f; // Portion of screen width reserved for UI

    private Camera cam;
    private float zoomVelocity = 0f;
    private WrapAroundMovement wrap;

    void Start()
    {
        cam = Camera.main;
        float uiWidthPercent = rightPaddingPercent; // e.g. 0.2f for 20%
        cam.rect = new Rect(0f, 0f, 1f - uiWidthPercent, 1f);

        if (targetA != null)
        {
            wrap = targetA.GetComponent<WrapAroundMovement>();
            if (wrap == null)
                Debug.LogWarning("WrapAroundMovement not found on targetA.");
        }
    }

    void LateUpdate()
    {
        if (targetA == null || targetB == null || cam == null || wrap == null)
            return;

        // --- 1. Calculate bounding box between targets ---
        Bounds targetBounds = new Bounds(targetA.position, Vector3.zero);
        targetBounds.Encapsulate(targetB.position);

        // --- 2. Dynamic zoom based on spread ---
        float maxDistance = targetBounds.size.magnitude;
        float t = Mathf.Clamp01(maxDistance / zoomLimiter);
        float targetZoom = Mathf.Lerp(minZoom, maxZoom, t);
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoom, ref zoomVelocity, zoomSmoothTime);

        // --- 3. Camera extents ---
        float vertExtent = cam.orthographicSize;
        float horzExtent = vertExtent * cam.aspect;

        // --- 4. Right UI padding in world units ---
        float uiWorldOffset = horzExtent * rightPaddingPercent;

        // --- 5. Define a “safe zone” camera center window ---
        float minX = wrap.XMin + horzExtent;
        float maxX = wrap.XMax - horzExtent - uiWorldOffset;

        float minY = wrap.YMin + vertExtent;
        float maxY = wrap.YMax - vertExtent;

        // --- 6. Clamp camera center (instead of full viewport) to keep view on-screen ---
        Vector3 midpoint = targetBounds.center;
        float clampedX = Mathf.Clamp(midpoint.x, minX, maxX);
        float clampedY = Mathf.Clamp(midpoint.y, minY, maxY);
        Vector3 desiredPosition = new Vector3(clampedX, clampedY, offset.z);

        // --- 7. Smooth follow toward target window ---
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * moveSpeed);
    }
}