using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Atributos do Inimigo")]
    [SerializeField] protected float speed = 2.0f;
    [SerializeField] protected int maxHealth = 1;
    [SerializeField] protected int scoreValue = 10;

    protected int currentHealth;
    protected Transform player;
    protected Vector3 currentScale;

    protected virtual void Start()
    {
        // Inicializa a vida
        currentHealth = maxHealth;

        // Encontra o player na cena
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
        else Debug.LogWarning($"Player não encontrado para o inimigo {gameObject.name}");

        // Salva a escala original
        currentScale = transform.localScale;
    }

    protected virtual void Update()
    {
        if (player != null)
        {
            MoveTowardsPlayer();
            FlipSprite();
        }
    }

    // Move o inimigo em direção ao player
    protected virtual void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );
    }

    // Vira o sprite do inimigo na direção do player
    protected virtual void FlipSprite()
    {
        currentScale = transform.localScale;

        // Vira para a direita
        if (player.position.x > transform.position.x) currentScale.x = -Mathf.Abs(currentScale.x);
        // Vira para a esquerda
        else currentScale.x = Mathf.Abs(currentScale.x);

        transform.localScale = currentScale;
    }

    // Causa dano ao inimigo e verifica se ele é derrotado
    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) Die();
    }

    // Elimina inimigo e adiciona pontuação
    protected virtual void Die()
    {
        if (ScoreManager.instance != null) ScoreManager.instance.AddScore(scoreValue);
        Destroy(gameObject);
    }

    // Retorna o valor de pontuação deste inimigo
    public int GetScoreValue() { return scoreValue; }

    // Retorna a vida atual do inimigo
    public int GetCurrentHealth() { return currentHealth; }

    // Retorna a vida máxima do inimigo
    public int GetMaxHealth() { return maxHealth; }
}