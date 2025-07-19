using UnityEngine;

public class BubbleBehavior : MonoBehaviour
{
    public string playerTag = "Ship";   // Tag can be set in Inspector
    public float moveSpeed = 2f;

    private Animator animator;
    private Transform player;
    private bool isCollecting = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isCollecting && other.CompareTag(playerTag))
        {
            isCollecting = true;
            player = other.transform;
            animator.enabled = true;
            animator.Play("bubble_6");
        }
    }

    void Update()
    {
        if (isCollecting && player != null)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                moveSpeed * Time.deltaTime
            );
        }
    }

    public void OnBubbleCollected()
    {
        Debug.Log("Bubble collected!");
        Destroy(gameObject);
    }
}