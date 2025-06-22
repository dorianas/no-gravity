using UnityEngine;
using UnityEngine.UI;

public class EnemyCrew01 : MonoBehaviour, IDamageable
{
    [Header("Crew Attributes")]
    public float maxCrew = 50f;
    private float currentCrew;

    [Header("Fuel Attributes")]
    public float maxFuel = 50f;
    public float currentFuel;

    [Header("Death & Respawn")]
    public GameObject deathEffectPrefab;
    public float respawnTime = 3f;

    [Header("References")]
    public Rigidbody2D rb;
    public Image crewFillHUD; // Optional linear bar

    [Header("Square-Based Crew UI")]
    public Transform crewSquaresContainer;
    public GameObject crewSquarePrefab;
    private Image[] crewSquares;

    [Header("Square-Based Fuel UI")]
    public Transform fuelSquaresContainer;
    public GameObject fuelSquarePrefab;
    private Image[] fuelSquares;

    [Header("Square Colors")]
    public Color crewActiveColor = Color.green;
    public Color crewInactiveColor = Color.gray;
    public Color fuelActiveColor = Color.yellow;
    public Color fuelInactiveColor = Color.gray;

    void Start()
    {
        currentCrew = maxCrew;
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        // Create crew squares
        if (crewSquaresContainer != null && crewSquarePrefab != null)
        {
            crewSquares = new Image[(int)maxCrew];
            for (int i = 0; i < maxCrew; i++)
            {
                GameObject square = Instantiate(crewSquarePrefab, crewSquaresContainer);
                crewSquares[i] = square.GetComponent<Image>();
            }
        }

        // Create fuel squares
        if (fuelSquaresContainer != null && fuelSquarePrefab != null)
        {
            fuelSquares = new Image[(int)maxFuel];
            for (int i = 0; i < maxFuel; i++)
            {
                GameObject square = Instantiate(fuelSquarePrefab, fuelSquaresContainer);
                fuelSquares[i] = square.GetComponent<Image>();
            }
        }

        UpdateCrewUI();
        UpdateFuelUI();
    }

    public void TakeDamage(float amount)
    {
        currentCrew = Mathf.Clamp(currentCrew - amount, 0, maxCrew);
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

        for (int i = 0; i < crewSquares.Length; i++)
            crewSquares[i].color = crewInactiveColor;

        int filled = Mathf.FloorToInt(currentCrew);
        int rows = crewSquares.Length / 2;
        int index = 0;

        for (int row = 0; row < rows; row++)
        {
            int leftIndex = row * 2;
            int rightIndex = row * 2 + 1;

            if (index < filled && leftIndex < crewSquares.Length)
            {
                crewSquares[leftIndex].color = crewActiveColor;
                index++;
            }

            if (index < filled && rightIndex < crewSquares.Length)
            {
                crewSquares[rightIndex].color = crewActiveColor;
                index++;
            }
        }
    }

    public void UpdateFuelUI()
    {
        if (fuelSquares == null || fuelSquares.Length == 0) return;

        for (int i = 0; i < fuelSquares.Length; i++)
            fuelSquares[i].color = fuelInactiveColor;

        int filled = Mathf.FloorToInt(currentFuel);
        int rows = fuelSquares.Length / 2;
        int index = 0;

        for (int row = 0; row < rows; row++)
        {
            int leftIndex = row * 2;
            int rightIndex = row * 2 + 1;

            if (index < filled && leftIndex < fuelSquares.Length)
            {
                fuelSquares[leftIndex].color = fuelActiveColor;
                index++;
            }

            if (index < filled && rightIndex < fuelSquares.Length)
            {
                fuelSquares[rightIndex].color = fuelActiveColor;
                index++;
            }
        }
    }

    public void ConsumeFuel(float amount)
    {
        currentFuel = Mathf.Clamp(currentFuel - amount, 0, maxFuel);
        UpdateFuelUI();
    }
}