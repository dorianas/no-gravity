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

    [Header("Fuel Regeneration")]
    public float fuelRegenRate = 1f;       // Seconds between ticks
    public float fuelRegenAmount = 2f;     // Amount per tick
    private float nextFuelTick = 0f;

    [Header("Death & Respawn")]
    public GameObject deathEffectPrefab;
    public float respawnTime = 3f;

    [Header("References")]
    public Rigidbody2D rb;
    public Image crewFillHUD;

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
        currentFuel = maxFuel;

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        GenerateSquares();
        UpdateCrewUI();
        UpdateFuelUI();
    }

    void Update()
    {
        if (Time.time >= nextFuelTick)
        {
            RegenerateFuel();
            nextFuelTick = Time.time + fuelRegenRate;
        }
    }

    void GenerateSquares()
    {
        if (crewSquaresContainer != null && crewSquarePrefab != null)
        {
            crewSquares = new Image[(int)maxCrew];
            for (int i = 0; i < maxCrew; i++)
                crewSquares[i] = Instantiate(crewSquarePrefab, crewSquaresContainer).GetComponent<Image>();
        }

        if (fuelSquaresContainer != null && fuelSquarePrefab != null)
        {
            fuelSquares = new Image[(int)maxFuel];
            for (int i = 0; i < maxFuel; i++)
                fuelSquares[i] = Instantiate(fuelSquarePrefab, fuelSquaresContainer).GetComponent<Image>();
        }
    }

    public void TakeDamage(float amount)
    {
        currentCrew = Mathf.Clamp(currentCrew - amount, 0, maxCrew);
        Debug.Log($"[ENEMY CREW] Crew hit! Remaining: {currentCrew}");
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

    public void ConsumeFuel(float amount)
    {
        currentFuel = Mathf.Clamp(currentFuel - amount, 0, maxFuel);
        UpdateFuelUI();
    }

    void RegenerateFuel()
    {
        if (currentFuel < maxFuel)
        {
            currentFuel = Mathf.Min(currentFuel + fuelRegenAmount, maxFuel);
            Debug.Log($"[ENEMY CREW] Regenerated fuel. Current: {currentFuel}");
            UpdateFuelUI();
        }
    }

    void UpdateCrewUI()
    {
        if (crewFillHUD != null)
            crewFillHUD.fillAmount = currentCrew / maxCrew;

        if (crewSquares == null || crewSquares.Length == 0) return;

        for (int i = 0; i < crewSquares.Length; i++)
            crewSquares[i].color = i < currentCrew ? crewActiveColor : crewInactiveColor;
    }

    public void UpdateFuelUI()
    {
        if (fuelSquares == null || fuelSquares.Length == 0) return;

        for (int i = 0; i < fuelSquares.Length; i++)
            fuelSquares[i].color = i < currentFuel ? fuelActiveColor : fuelInactiveColor;
    }
}