using UnityEngine;
using UnityEngine.UI;

public class DamageVignetteUI : MonoBehaviour
{
    [Header("Blood Sprites (random pool)")]
    public Sprite[] bloodSprites;

    [Header("UI Reference")]
    public Image bloodImage;

    [Header("Transparency")]
    public float maxAlpha = 200f;

    private void Start()
    {
        if (bloodImage != null)
        {
            bloodImage.gameObject.SetActive(false);
            SetAlpha(0f);
        }
    }

    public void OnPlayerHit(int currentHealth, int maxHealth)
    {
        if (bloodImage == null || bloodSprites.Length == 0)
            return;

        // TURN ON ON FIRST HIT
        if (!bloodImage.gameObject.activeSelf)
        {
            bloodImage.gameObject.SetActive(true);
        }

        // RANDOM BLOOD IMAGE EVERY HIT
        int randomIndex =
            Random.Range(0, bloodSprites.Length);

        bloodImage.sprite =
            bloodSprites[randomIndex];

        // DAMAGE-BASED TRANSPARENCY (still flexible)
        float damagePercent =
            1f - ((float)currentHealth / maxHealth);

        float alpha =
            damagePercent * maxAlpha;

        SetAlpha(alpha);
    }

    void SetAlpha(float alpha)
    {
        Color c = bloodImage.color;
        c.a = Mathf.Clamp(alpha / 255f, 0f, maxAlpha / 255f);
        bloodImage.color = c;
    }
}