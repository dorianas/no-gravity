using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    // 🚀 Rocket settings
    public GameObject rocketPrefab;
    public Transform rocketFirePoint1;
    public Transform rocketFirePoint2;
    public float rocketSpeed = 10f;
    public float rocketFireRate = 1f;
    public float rocketFuelCostPerShot = 4f;

    // 🔥 Fireball settings
    public GameObject fireballPrefab;
    public Transform fireballSpawnPoint;
    public float fireballCooldown = 0.5f;
    public float fireballFuelCost = 3f;
    public float fireballSpeed = 10f;

    // ⏱️ Timers
    private float nextRocketFireTime = 0f;
    private float nextFireballTime = 0f;

    // 🎨 UI Panel Color
    public Image rocketPanel;
    public Image fireballPanel;
    public Color shootColor = Color.red;
    public Color defaultColor = Color.white;
    public float colorResetTime = 0.5f;

    // 🔋 Player fuel reference
    public PlayerCrew playerCrew;

    void Update()
    {
        if (playerCrew == null) return;

        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireballTime)
        {
            FireFireball();
            nextFireballTime = Time.time + fireballCooldown;
        }

        if (Input.GetKey(KeyCode.E) && Time.time >= nextRocketFireTime)
        {
            FireRocket();
            nextRocketFireTime = Time.time + rocketFireRate;
        }

        if (Input.GetKey(KeyCode.F) && Time.time >= nextFireballTime)
        {
            FireFireball();
            nextFireballTime = Time.time + fireballCooldown;
        }
    }

    void FireRocket()
    {
        float totalFuelCost = rocketFuelCostPerShot * 2f;
        if (playerCrew.currentFuel < totalFuelCost)
        {
            Debug.Log("[PLAYER SHOOTING] Not enough fuel to fire rockets. Needed: " + totalFuelCost);
            return;
        }

        playerCrew.ConsumeFuel(totalFuelCost);
        Debug.Log("[PLAYER SHOOTING] Fuel consumed: " + totalFuelCost);

        GameObject rocket1 = Instantiate(rocketPrefab, rocketFirePoint1.position, rocketFirePoint1.rotation);
        SetRocketVelocity(rocket1, rocketFirePoint1);

        GameObject rocket2 = Instantiate(rocketPrefab, rocketFirePoint2.position, rocketFirePoint2.rotation);
        SetRocketVelocity(rocket2, rocketFirePoint2);

        ChangePanelColor(rocketPanel);
    }

    void FireFireball()
    {
        if (playerCrew.currentFuel < fireballFuelCost)
        {
            Debug.Log("[PLAYER SHOOTING] Not enough fuel to fire fireball. Needed: " + fireballFuelCost);
            return;
        }

        playerCrew.ConsumeFuel(fireballFuelCost);
        Debug.Log("[PLAYER SHOOTING] Fireball fuel consumed: " + fireballFuelCost);

        GameObject fireballGO = Instantiate(fireballPrefab, fireballSpawnPoint.position, fireballSpawnPoint.rotation);

        Rigidbody2D rb = fireballGO.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = fireballSpawnPoint.up * fireballSpeed;
        else
            Debug.LogWarning("[PLAYER SHOOTING] Fireball has no Rigidbody2D!");

        FireBallBehaviour fireball = fireballGO.GetComponent<FireBallBehaviour>();
        if (fireball != null)
        {
            fireball.SetOwner(playerCrew);
            fireball.Initialize();
            Debug.Log("[PLAYER SHOOTING] Fireball fired by: " + playerCrew.name);
        }
        else
        {
            Debug.LogWarning("[PLAYER SHOOTING] FireBallBehaviour missing on fireball prefab.");
        }

        ChangePanelColor(fireballPanel);
    }

    void SetRocketVelocity(GameObject rocket, Transform firePoint)
    {
        Rigidbody2D rb = rocket.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.up * rocketSpeed;
        }
        else
        {
            Debug.LogWarning("[PLAYER SHOOTING] Rocket has no Rigidbody2D.");
        }
    }

    void ChangePanelColor(Image panel)
    {
        if (panel != null)
        {
            panel.color = shootColor;
            Invoke(nameof(ResetPanelColor), colorResetTime);
        }
    }

    void ResetPanelColor()
    {
        if (rocketPanel != null) rocketPanel.color = defaultColor;
        if (fireballPanel != null) fireballPanel.color = defaultColor;
    }
}