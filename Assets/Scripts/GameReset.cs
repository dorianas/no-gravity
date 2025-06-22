using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // If using TextMeshPro for UI updates

public class GameReset : MonoBehaviour
{
    public TextMeshProUGUI resetText; // Optional UI message for reset status

    public void ResetGame()
    {
        Debug.Log("Game is resetting...");

        if (resetText != null)
        {
            resetText.text = "Game Restarting..."; // Show feedback before reload
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current scene
    }
}