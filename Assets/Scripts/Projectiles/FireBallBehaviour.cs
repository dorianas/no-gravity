using System;
using UnityEngine;

public class FireBallBehaviour : MonoBehaviour
{
    [Header("Damage Settings")]
    public float damage = 10f;

    [Header("Fuel Settings")]
    public float fuelCost = 4f;

    [Header("Visuals")]
    public GameObject impactEffectPrefab;

    private EnemyCrew01 owner;

    [SerializeField]
    private string[] enemyTags = { "Enemy01", "Meteorite", "Planet", "Asteroid", "Wall", "Ship" };

    private Type[] damageableTypes = { typeof(Enemy01), typeof(MeteoriteBehaviour), typeof(EnemyCrew01) };

    void Start()
    {
//       if (owner == null)
//       {
//           Debug.LogWarning("Fireball has no owner assigned!");
//            Destroy(gameObject);
//            return;
//       }

//        if (owner.currentFuel < fuelCost)
//        {
//            Debug.Log($"{owner.name} cannot fire fireball – insufficient fuel.");
//            Destroy(gameObject);
//            return;
//        }

//        owner.ConsumeFuel(fuelCost);
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
    public void SetOwner(EnemyCrew01 enemy)
    {
        owner = enemy;
    }

}