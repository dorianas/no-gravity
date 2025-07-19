using UnityEngine;

public class StarLayer : MonoBehaviour
{
    public int starCount = 100;
    public Vector2 areaSize = new Vector2(50, 50);
    public GameObject starPrefab;
    public float scrollSpeed = 0.2f; // Tweak per layer

    private Transform[] stars;

    void Start()
    {
        stars = new Transform[starCount];
        for (int i = 0; i < starCount; i++)
        {
            Vector2 position = new Vector2(
                Random.Range(-areaSize.x / 2, areaSize.x / 2),
                Random.Range(-areaSize.y / 2, areaSize.y / 2)
            );
            GameObject star = Instantiate(starPrefab, transform);
            star.transform.localPosition = position;
            stars[i] = star.transform;
        }
    }

    void Update()
    {
        foreach (Transform star in stars)
        {
            star.Translate(Vector2.left * scrollSpeed * Time.deltaTime);

            if (star.localPosition.x < -areaSize.x / 2)
            {
                star.localPosition = new Vector2(areaSize.x / 2, Random.Range(-areaSize.y / 2, areaSize.y / 2));
            }
        }
    }
}