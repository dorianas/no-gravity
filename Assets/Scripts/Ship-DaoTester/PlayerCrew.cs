using UnityEngine;
using UnityEngine.UI;

public class PlayerCrew : MonoBehaviour, IDamageable
{
    [Header("Crew Settings")]
    public float maxCrew = 50f;
    private float currentCrew;

    [Header("Fuel Settings")]
    public float maxFuel = 100f;
    public float currentFuel = 50f;

    [Header("Fuel Regeneration")]
    public float fuelRegenRate = 1f;      // How often to regenerate (seconds per tick)
    public float fuelRegenAmount = 2f;    // How much fuel to add per tick
    private float nextFuelTick = 0f;

    [Header("UI Settings")]
    public Image crewBarHUD;
    public Transform crewSquaresContainer;
    public GameObject crewSquarePrefab;
    private Image[] crewSquares;

    public Transform fuelSquaresContainer;
    public GameObject fuelSquarePrefab;
    private Image[] fuelSquares;

    [Header("Colors")]
    public Color crewActiveColor = Color.green;
    public Color crewInactiveColor = Color.gray;
    public Color fuelActiveColor = Color.yellow;
    public Color fuelInactiveColor = Color.gray;

    void Start()
    {
        currentCrew = maxCrew;
        currentFuel = maxFuel;

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

    public void ConsumeFuel(float amount)
    {
        float before = currentFuel;
        float after = Mathf.Clamp(currentFuel - amount, 0, maxFuel);

        Debug.Log($"[PLAYER CREW] Consuming fuel: {amount}");
        Debug.Log($"[PLAYER CREW] Fuel before: {before}, after: {after}");

        currentFuel = after;
        UpdateFuelUI();
    }

    public void TakeDamage(float amount)
    {
        currentCrew = Mathf.Clamp(currentCrew - amount, 0, maxCrew);
        UpdateCrewUI();
    }

    public void UpdateCrewUI()
    {
        if (crewBarHUD != null)
            crewBarHUD.fillAmount = currentCrew / maxCrew;

        if (crewSquares == null) return;

        for (int i = 0; i < crewSquares.Length; i++)
            crewSquares[i].color = i < currentCrew ? crewActiveColor : crewInactiveColor;
    }

    public void UpdateFuelUI()
    {
        if (fuelSquares == null) return;

        for (int i = 0; i < fuelSquares.Length; i++)
            fuelSquares[i].color = i < currentFuel ? fuelActiveColor : fuelInactiveColor;
    }

    void RegenerateFuel()
    {
        if (currentFuel < maxFuel)
        {
            currentFuel = Mathf.Min(currentFuel + fuelRegenAmount, maxFuel);
            Debug.Log($"[FUEL] Regenerated. Current fuel: {currentFuel}");
            UpdateFuelUI();
        }
    }
}