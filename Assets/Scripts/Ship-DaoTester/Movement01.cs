using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float acceleration = 5f;
    [SerializeField] public float maxSpeed = 1f;
    [SerializeField] public float rotationSpeed = 200f;
    [SerializeField] public float decelerationRate = 1f;

    public GameObject redDotPrefab;
    public Transform dotSpawnPoint;
    [SerializeField] public float spawnInterval = 0.5f;
    private float lastSpawnTime;
    private Rigidbody2D rb;

    private float thrust = 0f;
    private bool isMoving = false;

    public TextMeshProUGUI speedDisplay; // **Ensure this is assigned in Unity Inspector!**

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Rotate player with A/D keys
        float rotationInput = Input.GetAxisRaw("Horizontal");
        transform.Rotate(0, 0, -rotationInput * rotationSpeed * Time.deltaTime);

        // Handle movement with W key
        if (Input.GetKey(KeyCode.W))
        {
            thrust += acceleration * Time.deltaTime;
            thrust = Mathf.Clamp(thrust, 0, maxSpeed);
            isMoving = true;

            // Spawn dots at separate spawn point
            if (Time.time - lastSpawnTime > spawnInterval)
            {
                Instantiate(redDotPrefab, dotSpawnPoint.position, Quaternion.identity);
                lastSpawnTime = Time.time;
            }
        }
        else
        {
            isMoving = false;
        }

        // Apply braking with S
        if (Input.GetKey(KeyCode.S))
        {
            thrust -= decelerationRate * Time.deltaTime;
            thrust = Mathf.Clamp(thrust, 0, maxSpeed);
        }

        // **Re-added Speed Display Update**
        if (speedDisplay != null)
        {
            float actualSpeed = rb.linearVelocity.magnitude; // Get real-time speed
            speedDisplay.fontSize = 10f; // Adjust the value as needed
            speedDisplay.text = $"Speed: {actualSpeed:F2}\nRotation: {transform.eulerAngles.z:F2}";
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            Vector2 desiredVelocity = (Vector2)transform.up * thrust;

            // Smoothly adjust current velocity toward desired maxSpeed direction
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, desiredVelocity, acceleration * Time.fixedDeltaTime);
        }
    }
}