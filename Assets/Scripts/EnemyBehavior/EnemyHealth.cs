using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log(gameObject.name + " took damage!");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " died!");

        gameObject.SetActive(false);

        // OR Destroy(gameObject);
    }
}