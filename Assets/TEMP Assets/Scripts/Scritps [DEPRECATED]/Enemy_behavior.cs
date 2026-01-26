using UnityEngine;

public class Enemy_behavior : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float enemyVelocity;

    [SerializeField]
    private Rigidbody2D enemyRb;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float minDistance;

    public void Update()
    {
        Vector2 targetPosition = this.target.position;
        Vector2 currentPosition = this.transform.position;

        float distance = Vector2.Distance(currentPosition, targetPosition);

        // Se a distância entre inimigo e jogador for maior ou igual à distância mínima que precisa percorrer,
        // o inimigo continua se movendo
        if (distance >= this.minDistance)
        {
            Vector2 direction = (targetPosition - currentPosition).normalized;

            this.enemyRb.linearVelocity = (this.enemyVelocity * direction);
            // enemyVelocity = 2;
            // se direção = (1, -1) -> (2, -2)

            if (this.enemyRb.linearVelocity.x > 0) this.spriteRenderer.flipX = false; // olhando pra direita
            else if (this.enemyRb.linearVelocity.x < 0) this.spriteRenderer.flipX = true; // olhando pra esquerda
        }
        // Caso contrário ele para de se movimentar :D
        else this.enemyRb.linearVelocity = Vector2.zero; // para de se movimentar
        
    }
}
