using UnityEngine;

public class RedDotBehavior : MonoBehaviour
{
    private SpriteRenderer sr;
    public float fadeSpeed = 2f; // Speed at which the dot fades

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Color color = sr.color;
        color.a = Mathf.Clamp01(color.a - fadeSpeed * Time.deltaTime); // Reduce alpha gradually
        sr.color = color;

        if (color.a <= 0) // If fully transparent, destroy object
        {
            Destroy(gameObject);
        }
    }
}