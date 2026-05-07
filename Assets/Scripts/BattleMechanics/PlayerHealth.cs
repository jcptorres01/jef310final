using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;

    [Header("Knockback")]
    public float knockbackForce = 5f;

    [Header("Damage UI")]
    public Image damageOverlay;
    public DamageVignetteUI damageUI;

    [Range(0, 255)]
    public float maxAlpha = 200f;

    private int currentHealth;
    private Rigidbody rb;

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();

        // START FULLY HIDDEN
        if (damageOverlay != null)
        {
            SetOverlayAlpha(0f);
            damageOverlay.gameObject.SetActive(false);
        }
    }

    public void TakeDamage(int damageAmount, Vector3 damageSource)
    {
        currentHealth -= damageAmount;

        Debug.Log("Player took damage: " + damageAmount);

        ApplyKnockback(damageSource);

        // SEND DATA TO UI SYSTEM
        if (damageUI != null)
        {
            damageUI.OnPlayerHit(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void ApplyKnockback(Vector3 damageSource)
    {
        Vector3 knockbackDirection =
            (transform.position - damageSource).normalized;

        knockbackDirection.y = 0f;

        rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
    }

    void UpdateDamageUI()
    {
        if (damageOverlay == null) return;

        // TURN ON FIRST TIME DAMAGE HAPPENS
        if (!damageOverlay.gameObject.activeSelf)
        {
            damageOverlay.gameObject.SetActive(true);
        }

        float healthPercent =
            (float)currentHealth / maxHealth;

        float missingPercent =
            1f - healthPercent;

        float alpha =
            missingPercent * maxAlpha;

        SetOverlayAlpha(alpha);
    }

    void SetOverlayAlpha(float alpha)
    {
        Color c = damageOverlay.color;
        c.a = Mathf.Clamp(alpha / 255f, 0f, maxAlpha / 255f);
        damageOverlay.color = c;
    }

    void Die()
    {
        Debug.Log("Player Died");
    }
}