using UnityEngine;

[RequireComponent(typeof(EnemyCrew01))]
public class EnemyRocketShooter : MonoBehaviour
{
    public GameObject rocketPrefab;
    public Transform rocketFirePoint1;
    public Transform rocketFirePoint2;
    public float rocketSpeed = 10f;
    public float rocketFireRate = 1f;
    public float rocketFuelCostPerShot = 4f;
    public float shootingRadius = 12f;

    private float nextRocketFireTime = 0f;
    private Transform player;
    private EnemyCrew01 selfCrew;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Ship");
        if (playerObj != null)
            player = playerObj.transform;

        selfCrew = GetComponent<EnemyCrew01>();
    }

    void Update()
    {
        if (player == null || selfCrew == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= shootingRadius && Time.time >= nextRocketFireTime)
        {
            FireRocketBurst();
            nextRocketFireTime = Time.time + 1f / rocketFireRate;
        }
    }

    void FireRocketBurst()
    {
        if (rocketPrefab == null || rocketFirePoint1 == null || rocketFirePoint2 == null) return;

        float totalFuelCost = rocketFuelCostPerShot * 2f;
        if (selfCrew.currentFuel < totalFuelCost)
        {
            Debug.Log("[ENEMY ROCKET SHOOTER] Not enough fuel to fire rockets. Needed: " + totalFuelCost);
            return;
        }

        selfCrew.ConsumeFuel(totalFuelCost);
        Debug.Log("[ENEMY ROCKET SHOOTER] Fuel consumed: " + totalFuelCost);

        GameObject rocket1 = Instantiate(rocketPrefab, rocketFirePoint1.position, rocketFirePoint1.rotation);
        Rigidbody2D rb1 = rocket1.GetComponent<Rigidbody2D>();
        if (rb1 != null)
            rb1.linearVelocity = rocketFirePoint1.up * rocketSpeed;

        GameObject rocket2 = Instantiate(rocketPrefab, rocketFirePoint2.position, rocketFirePoint2.rotation);
        Rigidbody2D rb2 = rocket2.GetComponent<Rigidbody2D>();
        if (rb2 != null)
            rb2.linearVelocity = rocketFirePoint2.up * rocketSpeed;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, shootingRadius);
    }
}