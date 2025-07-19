using UnityEngine;
using System.Collections.Generic;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab;
    public int spawnCount = 10;
    public Vector2 areaSize = new Vector2(5f, 5f);
    public LayerMask planetLayer;
    public float minSpacing = 0.5f;  // Minimum distance between bubbles

    private List<Vector2> placedPositions = new List<Vector2>();

    void Start()
    {
        int spawned = 0;
        int attempts = 0;
        while (spawned < spawnCount && attempts < 1000)
        {
            attempts++;

            Vector2 spawnPos = new Vector2(
                Random.Range(-areaSize.x / 2, areaSize.x / 2),
                Random.Range(-areaSize.y / 2, areaSize.y / 2)
            );
            Vector2 worldPos = (Vector2)transform.position + spawnPos;

            // Check for planet overlap
            bool hitsPlanet = Physics2D.OverlapCircle(worldPos, 0.3f, planetLayer);

            // Check for spacing with other bubbles
            bool tooCloseToBubble = false;
            foreach (Vector2 pos in placedPositions)
            {
                if (Vector2.Distance(pos, worldPos) < minSpacing)
                {
                    tooCloseToBubble = true;
                    break;
                }
            }

            if (!hitsPlanet && !tooCloseToBubble)
            {
                GameObject bubble = Instantiate(bubblePrefab, worldPos, Quaternion.identity);
                bubble.GetComponent<Animator>().enabled = false;
                placedPositions.Add(worldPos);
                spawned++;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, areaSize);
    }
}