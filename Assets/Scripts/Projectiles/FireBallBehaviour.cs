using System;
using UnityEngine;

public class FireBallBehaviour : MonoBehaviour
{
    [Header("Damage Settings")]
    public float damage = 10f;

    [Header("Visuals")]
    public GameObject impactEffectPrefab;

    private MonoBehaviour owner;

    [SerializeField]
    private string[] enemyTags = { "Enemy01", "Meteorite", "Planet", "Asteroid", "Wall", "Ship" };

    private Type[] damageableTypes = { typeof(Enemy01), typeof(MeteoriteBehaviour), typeof(EnemyCrew01) };

    public void SetOwner(MonoBehaviour unit)
    {
        owner = unit;
    }

    public void Initialize()
    {
        Debug.Log("[FIREBALL] Initialize() called. Owner = " + owner?.name);
        // Fuel logic removed—owner handles that before firing
    }

    void Start()
    {
        // All logic handled externally
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (impactEffectPrefab != null)
        {
            Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
        }

        if (collision.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}