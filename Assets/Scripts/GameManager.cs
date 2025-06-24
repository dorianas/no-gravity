using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }

    public void RespawnEnemy(GameObject enemyPrefab, Vector2 spawnPosition, float delay)
    {
        Debug.Log("Scheduling enemy respawn...");
        StartCoroutine(RespawnCoroutine(enemyPrefab, spawnPosition, delay));
    }

    IEnumerator RespawnCoroutine(GameObject enemyPrefab, Vector2 spawnPosition, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (enemyPrefab != null)
        {
            enemyPrefab.transform.position = spawnPosition;
            enemyPrefab.SetActive(true);

            var crewEnemy = enemyPrefab.GetComponent<EnemyCrew01>();
            if (crewEnemy != null)
            {
                crewEnemy.ResetCrew();
            }

            var basicEnemy = enemyPrefab.GetComponent<Enemy01>();
            if (basicEnemy != null)
            {
                basicEnemy.health = 100f;
            }

            Debug.Log("Enemy respawned and reset!");
        }
        else
        {
            Debug.LogError("Error: Enemy reference missing in GameManager!");
        }
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame called!");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}