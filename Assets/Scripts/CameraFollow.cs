using UnityEngine;

public class CameraFollowDynamic : MonoBehaviour
{
    public Transform targetA; // Assign Ship01
    public Transform targetB; // Assign Enemy01
    public Vector3 offset = new Vector3(0, 0, -10f);

    [Header("Zoom Settings")]
    public float zoomLimiter = 40f;      // Affects zoom range based on distance
    public float minZoom = 5f;           // Closest zoom
    public float maxZoom = 20f;          // Furthest zoom (match wrap bounds)
    public float zoomSmoothTime = 0.2f;  // Smaller = faster zoom

    [Header("Movement Smoothing")]
    public float moveSpeed = 5f;

    private Camera cam;
    private float zoomVelocity = 0f;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (targetA == null || targetB == null)
            return;

        // Center between targets
        Vector3 centerPoint = (targetA.position + targetB.position) / 2f;
        Vector3 desiredPosition = centerPoint + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * moveSpeed);

        // Zoom adjustment based on distance
        float distance = Vector3.Distance(targetA.position, targetB.position);
        float t = Mathf.Clamp01(distance / zoomLimiter);
        float desiredZoom = Mathf.Lerp(minZoom, maxZoom, t);

        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, desiredZoom, ref zoomVelocity, zoomSmoothTime);
    }
}