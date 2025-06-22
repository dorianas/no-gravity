using UnityEngine;

public class ShootingController : MonoBehaviour
{
    public GameObject bulletPrefab;  // Assign in Unity (Bullet Prefab)
    public float bulletSpeed = 10f;
    public Transform bulletSpawnPoint; // Empty object at player's front
    public float fireRate = 0.2f; // Editable fire rate (seconds between shots)

    private float nextFireTime = 0f; // Time control for shooting

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime) // Hold SPACE for continuous shooting
        {
            Shoot();
            nextFireTime = Time.time + fireRate; // Set next allowed fire time
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bullet.transform.Rotate(0, 0, 90); // Adjust sprite rotation
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = bulletSpawnPoint.up * bulletSpeed;
    }
}