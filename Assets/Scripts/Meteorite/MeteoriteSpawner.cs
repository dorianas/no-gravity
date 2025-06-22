using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeteoriteSpawner : MonoBehaviour
{
    [Header("Meteorite Prefabs")]
    public GameObject[] meteoritePrefabs; // Assign multiple meteorite types in Inspector

    [Header("Spawning Settings")]
    public int maxMeteorites = 10;          // Max number of meteorites at any time
    public float respawnDelay = 10f;        // Delay (in seconds) before destroyed meteorites are replaced

    [Header("Spawn Area")]
    public Vector2 spawnAreaMin = new Vector2(-10f, -5f); // Bottom-left corner
    public Vector2 spawnAreaMax = new Vector2(10f, 5f);   // Top-right corner

    private List<GameObject> activeMeteorites = new List<GameObject>();

    void Start()
    {
        // Spawn initial wave
        for (int i = 0; i < maxMeteorites; i++)
        {
            SpawnMeteorite();
        }
    }

    public void SpawnMeteorite()
    {
        // Random position within spawn area
        Vector2 pos = new Vector2(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );

        // Choose a random prefab
        int index = Random.Range(0, meteoritePrefabs.Length);
        GameObject m = Instantiate(meteoritePrefabs[index], pos, Quaternion.identity);

        // Track this meteorite
        activeMeteorites.Add(m);

        // Assign spawner reference so it can report its own death
        MeteoriteBehaviour mb = m.GetComponent<MeteoriteBehaviour>();
        if (mb != null)
        {
            mb.spawner = this;
        }
    }

    // Called by meteorites when they’re destroyed
    public void HandleMeteoriteDestroyed(GameObject meteorite)
    {
        activeMeteorites.Remove(meteorite);
        StartCoroutine(RespawnAfterDelay(respawnDelay));
    }

    private IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (activeMeteorites.Count < maxMeteorites)
        {
            SpawnMeteorite();
        }
    }
}