using UnityEngine;

public class TankEnemy : Enemy
{
    [Header("Efeitos Visuais")]
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float damageFlashDuration = 0.1f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    protected override void Start()
    {
        speed = 1.0f;
        maxHealth = 3;
        scoreValue = 50;

        // Pega o sprite renderer para feedback visual
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) originalColor = spriteRenderer.color;

        base.Start();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (spriteRenderer != null && currentHealth > 0) StartCoroutine(DamageFlash());

    }

    private System.Collections.IEnumerator DamageFlash()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(damageFlashDuration);
        spriteRenderer.color = originalColor;
    }
}