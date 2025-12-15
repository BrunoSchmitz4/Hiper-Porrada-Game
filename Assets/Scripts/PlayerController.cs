using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float attackRange = 3.0f; // range de ataque
    public LayerMask enemyLayer; // Para o Raycast só bater nos inimigos

    private Vector3 currentScale;//Essa variavel guarda a escala atual do objeto

    void Start()
    {
        currentScale = transform.localScale; // Inicializa a variável coma  escala que está no Inspector agora.
    }

    void Update()
    {
        if (Keyboard.current == null) return; // Verifica se o teclado está disponível

        // Checa se a tecla foi pressionada neste frame
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            FlipSprite("left");//chama a função e vira pra esquerda
            AttemptAttack(Vector2.left);
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            FlipSprite("right");//chama a função e vira pra direita
            AttemptAttack(Vector2.right);
        }
    }

    // NOVA FUNÇÃO PRA VIRAR O SPRITE NO ATAQUE
    void FlipSprite(string direction)
    {
        // Atualiza a escala baseada no que está agora no objeto
        currentScale = transform.localScale;

        if (direction == "left")
        {
            currentScale.x = -Mathf.Abs(currentScale.x); // Garante que o x seja negativo
        }
        else if (direction == "right")
        {
            currentScale.x = Mathf.Abs(currentScale.x); // Garante que o x seja positivo
        }

        transform.localScale = currentScale; // Aplica a nova escala ao objeto

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
