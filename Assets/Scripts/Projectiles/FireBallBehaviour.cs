using System;
using UnityEngine;

public class FireBallBehaviour : MonoBehaviour
{
    public float damage = 10f;
    public GameObject impactEffectPrefab;

    [SerializeField] private string[] enemyTags = { "Enemy01", "Meteorite", "Planet", "Asteroid", "Wall", "Ship" };

    // List the component types that can take damage
    private Type[] damageableTypes = { typeof(Enemy01), typeof(MeteoriteBehaviour), typeof(EnemyCrew01) };

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