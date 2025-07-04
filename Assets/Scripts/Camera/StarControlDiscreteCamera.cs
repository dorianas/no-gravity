using UnityEngine;

public class StarControlDiscreteCamera : MonoBehaviour
{
    public Transform targetA;
    public Transform targetB;

    [Header("Wrap Settings")]
    public float wrapXMin = -25f;
    public float wrapXMax = 25f;

    [Header("Zoom Levels")]
    public float zoomClose = 6f;
    public float zoomMid = 9f;
    public float zoomFar = 12f;

    [Header("Screen Thresholds (viewport units)")]
    public float midThreshold = 0.6f;
    public float farThreshold = 0.95f;
    public float hysteresisPadding = 0.05f;

    [Header("Movement")]
    public float moveSmoothSpeed = 10f;

    [Header("Camera Clamp Per Zoom")]
    public float maxX_Close = 20f;
    public float maxY_Close = 15f;
    public float maxX_Mid = 28f;
    public float maxY_Mid = 20f;
    public float maxX_Far = 30f;
    public float maxY_Far = 24f;

    private enum ZoomState { Close, Mid, Far }
    private ZoomState currentState = ZoomState.Close;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (targetA == null || targetB == null) return;

        float wrapWidth = wrapXMax - wrapXMin;

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

        Vector3 desiredPos = new Vector3(midpoint.x, midpoint.y, transform.position.z);

        // Choose clamp limits based on zoom level
        float clampX = maxX_Far;
        float clampY = maxY_Far;
        switch (currentState)
        {
            case ZoomState.Close:
                clampX = maxX_Close;
                clampY = maxY_Close;
                break;
            case ZoomState.Mid:
                clampX = maxX_Mid;
                clampY = maxY_Mid;
                break;
            case ZoomState.Far:
                clampX = maxX_Far;
                clampY = maxY_Far;
                break;
        }

        // Clamp camera position
        desiredPos.x = Mathf.Clamp(desiredPos.x, -clampX, clampX);
        desiredPos.y = Mathf.Clamp(desiredPos.y, -clampY, clampY);
        transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * moveSmoothSpeed);

        // Zoom decision logic
        Vector3 screenPosA = cam.WorldToViewportPoint(targetA.position);
        Vector3 screenPosB = cam.WorldToViewportPoint(targetB.position);
        float screenDistance = Vector2.Distance(screenPosA, screenPosB);
        bool offscreen = IsOffscreen(screenPosA) || IsOffscreen(screenPosB);

        switch (currentState)
        {
            case ZoomState.Close:
                if (screenDistance > midThreshold + hysteresisPadding) currentState = ZoomState.Mid;
                break;
            case ZoomState.Mid:
                if (offscreen || screenDistance > farThreshold + hysteresisPadding)
                    currentState = ZoomState.Far;
                else if (screenDistance < midThreshold - hysteresisPadding)
                    currentState = ZoomState.Close;
                break;
            case ZoomState.Far:
                if (screenDistance < midThreshold - hysteresisPadding)
                    currentState = ZoomState.Close;
                else if (!offscreen && screenDistance < farThreshold - hysteresisPadding)
                    currentState = ZoomState.Mid;
                break;
        }

        cam.orthographicSize = GetZoomValue(currentState);
    }

    private float GetZoomValue(ZoomState state)
    {
        switch (state)
        {
            case ZoomState.Far: return zoomFar;
            case ZoomState.Mid: return zoomMid;
            default: return zoomClose;
        }
    }

    private bool IsOffscreen(Vector3 vp) => vp.x < 0 || vp.x > 1 || vp.y < 0 || vp.y > 1;

    void OnDrawGizmosSelected()
    {
        if (cam == null) cam = Camera.main;

        // Draw Zoom Area Rectangles
        Gizmos.color = Color.red;
        DrawZoomAreaGizmo(zoomFar);
        Gizmos.color = Color.magenta;
        DrawZoomAreaGizmo(zoomMid);
        Gizmos.color = Color.green;
        DrawZoomAreaGizmo(zoomClose);

        // Draw Clamp Boundaries
        Gizmos.color = Color.red;
        DrawClampGizmo(maxX_Far, maxY_Far);
        Gizmos.color = Color.magenta;
        DrawClampGizmo(maxX_Mid, maxY_Mid);
        Gizmos.color = Color.green;
        DrawClampGizmo(maxX_Close, maxY_Close);
    }

    private void DrawZoomAreaGizmo(float zoomLevel)
    {
        float height = 2f * zoomLevel;
        float width = height * (cam != null ? cam.aspect : 1.77f);

        Vector3 center = transform.position;
        Vector3 tl = center + new Vector3(-width / 2f, height / 2f);
        Vector3 tr = center + new Vector3(width / 2f, height / 2f);
        Vector3 bl = center + new Vector3(-width / 2f, -height / 2f);
        Vector3 br = center + new Vector3(width / 2f, -height / 2f);

        Gizmos.DrawLine(tl, tr);
        Gizmos.DrawLine(tr, br);
        Gizmos.DrawLine(br, bl);
        Gizmos.DrawLine(bl, tl);
    }

    private void DrawClampGizmo(float x, float y)
    {
        Vector3 center = Vector3.zero;
        Vector3 tl = center + new Vector3(-x, y);
        Vector3 tr = center + new Vector3(x, y);
        Vector3 bl = center + new Vector3(-x, -y);
        Vector3 br = center + new Vector3(x, -y);

        Gizmos.DrawLine(tl, tr);
        Gizmos.DrawLine(tr, br);
        Gizmos.DrawLine(br, bl);
        Gizmos.DrawLine(bl, tl);
    }
}