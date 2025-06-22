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
    public Image crewFillHUD; // Drag your green UI fill bar here in Inspector

    void Start()
    {
        currentCrew = maxCrew;

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

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
    }
}