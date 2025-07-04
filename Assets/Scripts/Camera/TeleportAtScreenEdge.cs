using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TeleportAtScreenEdge : MonoBehaviour
{
    private Camera cam;
    private float camHalfHeight;
    private float camHalfWidth;

    private float zoomFar;

    void Start()
    {
        cam = Camera.main;

        var camController = cam.GetComponent<StarControlDiscreteCamera>();
        if (camController != null)
        {
            zoomFar = camController.zoomFar;
        }
        else
        {
            Debug.LogWarning("StarControlDiscreteCamera not found on main camera — TeleportAtScreenEdge will always be disabled.");
        }
    }

    void LateUpdate()
    {
        if (cam == null || Mathf.Abs(cam.orthographicSize - zoomFar) > 0.01f) return;

        camHalfHeight = cam.orthographicSize;
        camHalfWidth = camHalfHeight * cam.aspect;

        Vector3 pos = transform.position;

        float left = cam.transform.position.x - camHalfWidth;
        float right = cam.transform.position.x + camHalfWidth;
        float bottom = cam.transform.position.y - camHalfHeight;
        float top = cam.transform.position.y + camHalfHeight;

        bool teleported = false;

        if (pos.x < left)
        {
            pos.x = right;
            teleported = true;
        }
        else if (pos.x > right)
        {
            pos.x = left;
            teleported = true;
        }

        if (pos.y < bottom)
        {
            pos.y = top;
            teleported = true;
        }
        else if (pos.y > top)
        {
            pos.y = bottom;
            teleported = true;
        }

        if (teleported)
            GetComponent<Rigidbody2D>().position = pos;
    }
}