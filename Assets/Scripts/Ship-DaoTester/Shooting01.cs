using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class PlayerShooting : MonoBehaviour
{
    // Rocket settings
    public GameObject rocketPrefab;
    public Transform rocketFirePoint1;
    public Transform rocketFirePoint2;
    public float rocketSpeed = 10f;
    public float rocketFireRate = 1f;

    // Bullet settings
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 10f;
    public float bulletFireRate = 0.2f;

    private float nextRocketFireTime = 0f;
    private float nextBulletFireTime = 0f;

    // UI Panel Color Change
    public Image rocketPanel; // Assign this in Inspector
    public Image bulletPanel; // Assign this in Inspector
    public Color shootColor = Color.red; // Color when firing
    public Color defaultColor = Color.white; // Default panel color
    public float colorResetTime = 0.5f; // Duration before resetting color

    void Update()
    {
        // Fire bullets with SPACE
        if (Input.GetKey(KeyCode.Space) && Time.time >= nextBulletFireTime)
        {
            FireBullet();
            nextBulletFireTime = Time.time + bulletFireRate;
        }

        // Fire rockets with E
        if (Input.GetKey(KeyCode.E) && Time.time >= nextRocketFireTime)
        {
            FireRocket();
            nextRocketFireTime = Time.time + rocketFireRate;
        }
    }

    void FireRocket()
    {
        GameObject rocket1 = Instantiate(rocketPrefab, rocketFirePoint1.position, rocketFirePoint1.rotation);
        Rigidbody2D rb1 = rocket1.GetComponent<Rigidbody2D>();
        if (rb1 != null)
        {
            rb1.linearVelocity = rocketFirePoint1.up * rocketSpeed;
        }

        GameObject rocket2 = Instantiate(rocketPrefab, rocketFirePoint2.position, rocketFirePoint2.rotation);
        Rigidbody2D rb2 = rocket2.GetComponent<Rigidbody2D>();
        if (rb2 != null)
        {
            rb2.linearVelocity = rocketFirePoint2.up * rocketSpeed;
        }

        // **Change panel color when shooting rockets**
        ChangePanelColor(rocketPanel);
    }

    void FireBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bullet.transform.Rotate(0, 0, 90);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = bulletSpawnPoint.up * bulletSpeed;

        // **Change panel color when shooting bullets**
        ChangePanelColor(bulletPanel);
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
        if (bulletPanel != null) bulletPanel.color = defaultColor;
    }
}