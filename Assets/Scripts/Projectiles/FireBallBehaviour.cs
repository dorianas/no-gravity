using System;
using UnityEngine;

public class FireBallBehaviour : MonoBehaviour
{
    public float damage = 10f;
    public GameObject impactEffectPrefab;

    [SerializeField] private string[] enemyTags = { "Enemy01", "Meteorite", "Planet", "Asteroid", "Wall", "Ship" };

    // List the component types that can take damage
    private Type[] damageableTypes = { typeof(Enemy01), typeof(MeteoriteBehaviour) };

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (impactEffectPrefab != null)
        {
            Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
        }

        foreach (string tag in enemyTags)
        {
            if (collision.gameObject.CompareTag(tag))
            {
                foreach (Type type in damageableTypes)
                {
                    var component = collision.gameObject.GetComponent(type);
                    if (component != null)
                    {
                        // Use reflection to call the TakeDamage method
                        var method = type.GetMethod("TakeDamage");
                        if (method != null)
                        {
                            method.Invoke(component, new object[] { damage });
                        }

                        break; // Stop after the first valid component
                    }
                }

                break; // Stop after tag match
            }
        }

        Destroy(gameObject);
    }
}