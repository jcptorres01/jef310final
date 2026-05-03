using UnityEngine;
using System.Collections.Generic;

public class HitBoxCollision : MonoBehaviour
{
    public int damage = 1;

    private HashSet<GameObject> hitObjects = new HashSet<GameObject>();

    private void OnEnable()
    {
        hitObjects.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hitObjects.Contains(other.gameObject))
            return;

        hitObjects.Add(other.gameObject);

        // BREAKABLE
        if (other.CompareTag("BreakableObject"))
        {
            BreakableObjectBehvaior breakable = other.GetComponent<BreakableObjectBehvaior>();

            if (breakable != null)
            {
                breakable.TakeHit(damage);
            }
        }

        // ENEMY
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemy = other.GetComponentInChildren<EnemyHealth>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}