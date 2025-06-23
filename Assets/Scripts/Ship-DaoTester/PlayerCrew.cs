using UnityEngine;
using UnityEngine.UI;

public class PlayerCrew : MonoBehaviour, IDamageable
{
    [Header("Crew Settings")]
    public float maxCrew = 50f;
    private float currentCrew;

    [Header("Fuel Settings")]
    public float maxFuel = 50f;
    public float currentFuel;

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
        currentFuel = Mathf.Clamp(currentFuel - amount, 0, maxFuel);
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
}