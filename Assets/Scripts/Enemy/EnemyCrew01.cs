using UnityEngine;
using UnityEngine.UI;

public class EnemyCrew01 : MonoBehaviour, IDamageable
{
    [Header("Crew Attributes")]
    public float maxCrew = 50f;
    private float currentCrew;

    [Header("Death & Respawn")]
    public GameObject deathEffectPrefab;
    public float respawnTime = 3f;

    [Header("References")]
    public Rigidbody2D rb;
    public Image crewFillHUD; // Linear bar (optional)

    [Header("Square-Based UI")]
    public Transform crewSquaresContainer; // Grid Layout Group with 2 rows
    public GameObject crewSquarePrefab;    // A tiny green square prefab
    private Image[] crewSquares;

    void Start()
    {
        currentCrew = maxCrew;

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        // Create square indicators
        if (crewSquaresContainer != null && crewSquarePrefab != null)
        {
            crewSquares = new Image[(int)maxCrew];

            for (int i = 0; i < maxCrew; i++)
            {
                GameObject square = Instantiate(crewSquarePrefab, crewSquaresContainer);
                crewSquares[i] = square.GetComponent<Image>();
            }
        }

        UpdateCrewUI();
    }

    public void TakeDamage(float amount)
    {
        currentCrew -= amount;
        currentCrew = Mathf.Clamp(currentCrew, 0, maxCrew);

        Debug.Log($"Crew hit! Remaining: {currentCrew}");

        UpdateCrewUI();

        if (currentCrew <= 0f)
        {
            if (deathEffectPrefab)
                Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);

            GameManager.instance.RespawnEnemy(gameObject, transform.position, respawnTime);
            gameObject.SetActive(false);
        }
    }

    public void ResetCrew()
    {
        currentCrew = maxCrew;
        UpdateCrewUI();
    }

    void UpdateCrewUI()
    {
        if (crewFillHUD != null)
            crewFillHUD.fillAmount = currentCrew / maxCrew;

        if (crewSquares == null || crewSquares.Length == 0) return;

        // Reset all to gray
        for (int i = 0; i < crewSquares.Length; i++)
            crewSquares[i].color = Color.gray;

        int filled = Mathf.FloorToInt(currentCrew);

        int rows = crewSquares.Length / 2; // since we have 2 columns
        int index = 0;

        for (int row = 0; row < rows; row++)
        {
            // Left then right square in this row
            int leftIndex = row * 2;
            int rightIndex = row * 2 + 1;

            if (index < filled && leftIndex < crewSquares.Length)
            {
                crewSquares[leftIndex].color = Color.green;
                index++;
            }

            if (index < filled && rightIndex < crewSquares.Length)
            {
                crewSquares[rightIndex].color = Color.green;
                index++;
            }
        }
    }
}