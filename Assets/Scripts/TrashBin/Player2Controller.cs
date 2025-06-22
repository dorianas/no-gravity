using UnityEngine;
using TMPro;

public class Player2Controller : MonoBehaviour
{
    [SerializeField] public float acceleration = 5f;
    [SerializeField] public float maxSpeed = 10f;
    [SerializeField] public float rotationSpeed = 200f;
    [SerializeField] public float decelerationRate = 1f;

    public GameObject fireballPrefab; // Assign fireball prefab in Unity
    public Transform fireballSpawnPoint; // Fireball spawn location

    public GameObject rocketPrefab; // Assign rocket prefab in Unity
    public Transform rocketFirePoint1; // First rocket spawn position
    public Transform rocketFirePoint2; // Second rocket spawn position
    public float rocketSpeed = 10f;

    private Rigidbody2D rb;
    private float thrust = 0f;
    private bool isMoving = false;

    public TextMeshProUGUI speedDisplay;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Rotate player using Left/Right Arrow Keys
        float rotationInput = Input.GetAxisRaw("Horizontal");
        transform.Rotate(0, 0, -rotationInput * rotationSpeed * Time.deltaTime);

        // Move forward with Up Arrow
        if (Input.GetKey(KeyCode.UpArrow))
        {
            thrust += acceleration * Time.deltaTime;
            thrust = Mathf.Clamp(thrust, 0, maxSpeed);
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        // Brake with Down Arrow
        if (Input.GetKey(KeyCode.DownArrow))
        {
            thrust -= decelerationRate * Time.deltaTime;
            thrust = Mathf.Clamp(thrust, 0, maxSpeed);
        }

        // Fire Fireball with Right Control
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            FireFireball();
        }

        // Fire Rockets with Right Shift
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            FireRocket();
        }

        // Update UI display
        if (speedDisplay != null)
        {
            speedDisplay.text = $"Speed: {thrust:F2}\nVelocity: {rb.linearVelocity.magnitude:F2}\nRotation: {transform.eulerAngles.z:F2}";
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            rb.linearVelocity += (Vector2)transform.up * thrust * Time.deltaTime;
        }

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    void FireFireball()
    {
        Instantiate(fireballPrefab, fireballSpawnPoint.position, fireballSpawnPoint.rotation);
    }

    void FireRocket()
    {
        GameObject rocket1 = Instantiate(rocketPrefab, rocketFirePoint1.position, rocketFirePoint1.rotation);
        Rigidbody2D rb1 = rocket1.GetComponent<Rigidbody2D>();
        if (rb1 != null)
        {
            rb1.linearVelocity = rocketFirePoint1.up * rocketSpeed;
        }

        GameObject rocket2 = Instantiate(rocketPrefab, rocketFirePoint2.position, rocketFirePoint2.rotation);
        Rigidbody2D rb2 = rocket2.GetComponent<Rigidbody2D>();
        if (rb2 != null)
        {
            rb2.linearVelocity = rocketFirePoint2.up * rocketSpeed;
        }
    }
}