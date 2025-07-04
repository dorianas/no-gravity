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
    [Range(0f, 0.5f)]
    public float rightPaddingPercent = 0.2f;

    private Camera cam;
    private float zoomVelocity = 0f;
    private WrapAroundMovement wrap;

    void Start()
    {
        cam = Camera.main;

        // Apply screen rect to reserve UI space on the right
        cam.rect = new Rect(0f, 0f, 1f - rightPaddingPercent, 1f);

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

        float wrapWidth = wrap.XMax - wrap.XMin;

        // --- STEP 1: Wrap-aware position correction for midpoint ---
        Vector3 aPos = targetA.position;
        Vector3 bPos = targetB.position;

        float dx = Mathf.Abs(aPos.x - bPos.x);
        if (dx > wrapWidth / 2f)
        {
            if (aPos.x > bPos.x) aPos.x -= wrapWidth;
            else bPos.x -= wrapWidth;
        }

        Vector3 midpoint = (aPos + bPos) / 2f;
        if (midpoint.x < wrap.XMin) midpoint.x += wrapWidth;

        // --- STEP 2: Zoom calculation using X and Y extents separately ---
        float xSpan = Mathf.Abs(aPos.x - bPos.x);
        float ySpan = Mathf.Abs(aPos.y - bPos.y);

        float zoomX = xSpan / cam.aspect / 2f;
        float zoomY = ySpan / 2f;
        float requiredZoom = Mathf.Max(zoomX, zoomY);
        float targetZoom = Mathf.Clamp(requiredZoom, minZoom, maxZoom);
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoom, ref zoomVelocity, zoomSmoothTime);

        // --- STEP 3: Viewport clamping to visible world bounds ---
        float vertExtent = cam.orthographicSize;
        float horzExtent = vertExtent * cam.aspect;
        float uiWorldOffset = horzExtent * rightPaddingPercent;

        float minX = wrap.XMin + horzExtent;
        float maxX = wrap.XMax - horzExtent - uiWorldOffset;
        float minY = wrap.YMin + vertExtent;
        float maxY = wrap.YMax - vertExtent;

        float clampedX = Mathf.Clamp(midpoint.x, minX, maxX);
        float clampedY = Mathf.Clamp(midpoint.y, minY, maxY);

        Vector3 desiredPosition = new Vector3(clampedX, clampedY, offset.z);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * moveSpeed);
    }
}