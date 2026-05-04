using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("UI Settings")]
    public GameObject healthBar;
    private Transform player;
    private Transform canvas;

    private Vector3 originalScale;

    [Header("Knockback Settings")]
    public float knockbackForce = 5f;
    public float upwardForce = 2f;

    private Rigidbody rb;

    void Start()
    {
        // Main health logic (Script 1)
        currentHealth = maxHealth;

        // Get Rigidbody for knockback
        rb = GetComponent<Rigidbody>();

        // UI setup (from Script 2)
        if (healthBar != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            canvas = healthBar.transform.parent.parent;

            originalScale = healthBar.transform.localScale;
            UpdateHealthBar();
        }
    }

    void Update()
    {
        // Make health bar face player (UI behavior)
        if (canvas != null && player != null)
        {
            Vector3 lookDirection = player.position - canvas.position;
            lookDirection.y = 0;
            canvas.rotation = Quaternion.LookRotation(lookDirection);
        }
    }

    public void TakeDamage(int damage)
    {
        // Main logic (Script 1)
        currentHealth -= damage;

        Debug.Log(gameObject.name + " took damage!");

        // Apply knockback
        ApplyKnockback();

        // Update UI
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void ApplyKnockback()
    {
        if (rb == null || player == null) return;

        Vector3 direction = (transform.position - player.position).normalized;

        Vector3 force = direction * knockbackForce + Vector3.up * upwardForce;

        rb.AddForce(force, ForceMode.Impulse);
    }

    void UpdateHealthBar()
    {
        if (healthBar == null) return;

        float healthPercent = (float)currentHealth / maxHealth;

        healthBar.transform.localScale = new Vector3(
            originalScale.x * healthPercent,
            originalScale.y,
            originalScale.z
        );
    }

    void Die()
    {
        Debug.Log(gameObject.name + " died!");

        gameObject.SetActive(false);
        // OR Destroy(gameObject);
    }
}