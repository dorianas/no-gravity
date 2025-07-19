using UnityEngine;

public class PixelStarfield : MonoBehaviour
{
    [Header("Star Settings")]
    public int starCount = 100;
    public Vector2 areaSize = new Vector2(100f, 100f);
    public Color starColor = Color.white;
    public float parallaxFactor = 0.2f;

    [Header("Reference Transform")]
    public Transform referencePoint; // Assign your Main Camera here in Inspector

    private Vector3 lastReferencePosition;
    private Transform[] stars;

    public float starSize = 0.05f; // Default: tiny stars

    void Start()
    {
        stars = new Transform[starCount];

        if (referencePoint != null)
            lastReferencePosition = referencePoint.position;

        for (int i = 0; i < starCount; i++)
        {
            GameObject star = new GameObject("PixelStar");
            star.transform.parent = transform;

            SpriteRenderer renderer = star.AddComponent<SpriteRenderer>();
            renderer.sprite = GeneratePixelSprite();
            renderer.color = starColor;
            renderer.sortingOrder = -10; // Push stars behind everything

            Vector2 position = new Vector2(
                Random.Range(-areaSize.x / 2, areaSize.x / 2),
                Random.Range(-areaSize.y / 2, areaSize.y / 2)
            );

            star.transform.localPosition = position;
            star.transform.localScale = Vector3.one * starSize;
            stars[i] = star.transform;
        }
    }

    void Update()
    {
        if (referencePoint == null) return;

        Vector3 delta = referencePoint.position - lastReferencePosition;
        transform.position += delta * parallaxFactor;
        lastReferencePosition = referencePoint.position;
    }

    Sprite GeneratePixelSprite()
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, 1, 1), Vector2.one * 0.5f, 1f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(transform.position, areaSize);
    }
}