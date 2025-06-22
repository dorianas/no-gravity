using UnityEngine;
using UnityEngine.UI;

public class EnemyCrew : MonoBehaviour
{
    public float maxCrew = 42f;
    private float currentCrew;

    public GameObject crewBarUIPrefab;  // Assign the UI prefab in Inspector
    private Image crewFill;
    private GameObject uiInstance;

    void Start()
    {
        currentCrew = maxCrew;

        // Create the UI bar and parent it under the existing Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas && crewBarUIPrefab)
        {
            uiInstance = Instantiate(crewBarUIPrefab, canvas.transform);
            FollowEnemy followScript = uiInstance.GetComponent<FollowEnemy>();
            if (followScript)
                followScript.enemy = this.transform;

            crewFill = uiInstance.transform.GetChild(0).GetChild(0).GetComponent<Image>();
            UpdateCrewUI();
        }
    }

    public void TakeCrewLoss(float amount)
    {
        currentCrew -= amount;
        currentCrew = Mathf.Clamp(currentCrew, 0, maxCrew);
        UpdateCrewUI();

        if (currentCrew <= 0)
            HandleShipDestroyed();
    }

    void UpdateCrewUI()
    {
        if (crewFill != null)
            crewFill.fillAmount = currentCrew / maxCrew;
    }

    void HandleShipDestroyed()
    {
        // Optional: trigger effects, sounds, animations
        if (uiInstance) Destroy(uiInstance);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (uiInstance) Destroy(uiInstance);
    }

    // Optional: expose remaining crew
    public float GetCrewPercentage()
    {
        return currentCrew / maxCrew;
    }
}