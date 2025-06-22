using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    void Awake()
    {
        instance = this; // Assign global instance
    }

    public void RespawnEnemy(GameObject enemyPrefab, Vector2 spawnPosition, float delay)
    {
        Debug.Log("Scheduling Enemy01 respawn...");
        StartCoroutine(RespawnCoroutine(enemyPrefab, spawnPosition, delay));
    }

    IEnumerator RespawnCoroutine(GameObject enemyPrefab, Vector2 spawnPosition, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (enemyPrefab != null)
        {
            enemyPrefab.SetActive(true); // Reactivate enemy
            Enemy01 enemyScript = enemyPrefab.GetComponent<Enemy01>();
            if (enemyScript != null)
            {
                enemyScript.health = 100f; // Reset health
            }
            Debug.Log("Enemy01 respawned successfully!");
        }
        else
        {
            Debug.LogError("Error: Enemy01 reference missing in GameManager!");
        }
    }
}