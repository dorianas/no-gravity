using UnityEngine;

public class StarControlCamera : MonoBehaviour
{
    public Transform targetA;
    public Transform targetB;

    [Header("Wrap Settings")]
    public float wrapXMin = -25f;
    public float wrapXMax = 25f;
    public float wrapYMin = -20f;
    public float wrapYMax = 20f;

    [Header("Zoom Settings")]
    public float zoomLimiter = 40f;
    public float minZoom = 6f;
    public float maxZoom = 20f;
    public float zoomSmoothTime = 0.1f;

    [Header("Camera Smoothing")]
    public float moveSmoothSpeed = 10f;

    private Camera cam;
    private float zoomVelocity;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (targetA == null || targetB == null) return;

        float wrapWidth = wrapXMax - wrapXMin;

        // -- WRAP-AWARE POSITION CORRECTION --
        Vector3 aPos = targetA.position;
        Vector3 bPos = targetB.position;

        float dx = Mathf.Abs(aPos.x - bPos.x);
        if (dx > wrapWidth / 2f)
        {
            if (aPos.x > bPos.x) aPos.x -= wrapWidth;
            else bPos.x -= wrapWidth;
        }

        Vector3 midpoint = (aPos + bPos) / 2f;
        if (midpoint.x < wrapXMin) midpoint.x += wrapWidth;

        // -- ZOOM CALCULATION --
        float xSpan = Mathf.Abs(aPos.x - bPos.x);
        float ySpan = Mathf.Abs(aPos.y - bPos.y);

        float zoomX = xSpan / cam.aspect / 2f;
        float zoomY = ySpan / 2f;
        float requiredZoom = Mathf.Max(zoomX, zoomY);
        float targetZoom = Mathf.Clamp(requiredZoom, minZoom, maxZoom);

        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoom, ref zoomVelocity, zoomSmoothTime);

        // -- CAMERA POSITION --
        Vector3 desiredPosition = new Vector3(midpoint.x, midpoint.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * moveSmoothSpeed);
    }
}