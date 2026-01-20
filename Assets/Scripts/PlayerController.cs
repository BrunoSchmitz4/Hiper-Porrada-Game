using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float attackRange = 3.0f; // range de ataque
    public LayerMask enemyLayer; // Para o Raycast só bater nos inimigos

    private Vector3 currentScale;//Essa variavel guarda a escala atual do objeto

    AudioManager audioManager;

    [SerializeField] private Animator animator;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

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

    void FlipSprite(string direction)
    {
        // Dispara a animação de ataque
        animator.SetTrigger("Attack");
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

        Debug.DrawRay(transform.position, direction * attackRange, Color.red, 0.5f);

        if (hit.collider != null && hit.collider.CompareTag("enemy"))
        {
            Vector3 enemyPos = hit.collider.transform.position;
            Destroy(hit.collider.gameObject);
            audioManager.PlaySFX(audioManager.hitPunch);
            transform.position = enemyPos;
            ScoreManager.instance.AddCombo();
            Debug.Log("Inimigo atingido!");
            
        }
        else
        {
            ScoreManager.instance.DelCombo();
            audioManager.PlaySFX(audioManager.loseCombo);
            Debug.Log("Errou o golpe!");
        }
    }

}
