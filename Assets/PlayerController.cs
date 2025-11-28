using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float attackRange = 3.0f; // range de ataque
    public LayerMask enemyLayer; // Para o Raycast só bater nos inimigos

    void Update()
    {
        // Verifica se o teclado existe para evitar erros
        if (Keyboard.current == null) return;

        // Checa se a tecla foi pressionada neste frame
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            AttemptAttack(Vector2.left);
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            AttemptAttack(Vector2.right);
        }
    }

    void AttemptAttack(Vector2 direction)
    {
        // Lança um raio na direção apertada
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, attackRange, enemyLayer);

        // Debug visual para ver a linha na cena
        Debug.DrawRay(transform.position, direction * attackRange, Color.red, 0.5f);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                // LÓGICA CORE:
                // 1. Pega a posição do inimigo
                Vector3 enemyPos = hit.collider.transform.position;

                // 2. Destroi o inimigo
                Destroy(hit.collider.gameObject);

                // 3. Teleporta o player para a posição onde o inimigo estava
                transform.position = enemyPos;

                Debug.Log("Inimigo atingido!");
            }
        }
        else
        {
            // Opcional: Feedback de erro (ex: som de "miss")
            Debug.Log("Errou o golpe!");
        }
    }
}
