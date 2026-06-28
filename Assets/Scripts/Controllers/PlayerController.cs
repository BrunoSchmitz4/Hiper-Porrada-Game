using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject floatingTextPrefab;
    public Transform textPoint;

    //invulnerabilidade
    [Header("Defesas")]
    public bool isInvulnerable = false;
    public float invulnerabilityTime = 0.2f; // 0.2 segundos de invulnerabilidade

    [Header("Combate")]
    public float attackOffset = 1.0f; // Distância que o player vai parar antes de encostar no inimigo

    public float attackRange = 3.0f;
    public int attackDamage = 1;
    public LayerMask enemyLayer;

    public float missCooldown = 1.0f; // Tempo de cooldown
    private bool isOnMissCooldown = false; // Flag de cooldown

    private Vector3 currentScale;

    [SerializeField] private Animator animator;

    void Start() { currentScale = transform.localScale; }

    void Update()
    {
        // Não permite input se o jogo está pausado ou acabou
        //if (GameManager.instance != null && (GameManager.instance.isPaused || GameManager.instance.isGameOver))
        //    return;

        if (Keyboard.current == null) return;

        if (isOnMissCooldown) return;

        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            FlipSprite("left");
            PlayerAttack(Vector2.left);
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            FlipSprite("right");
            PlayerAttack(Vector2.right);
        }
    }

    void FlipSprite(string direction)
    {
        if (animator != null)
        {
            // 1. Limpa qualquer animação stackada antes
            animator.ResetTrigger("Attack");
            // 2. Força a animação a reiniciar do zero (0f) 
            animator.Play("Player_ATTACK", -1, 0f);
        }

        currentScale = transform.localScale;

        if (direction == "left") { currentScale.x = -Mathf.Abs(currentScale.x); }
        else if (direction == "right") { currentScale.x = Mathf.Abs(currentScale.x); }

        transform.localScale = currentScale;
    }

    void PlayPunchSound()
    {
        // Usa Singleton
        //AudioManager.instance?.PlaySFX(AudioManager.instance.hitPunch);
        AudioManager.instance?.PlaySFXAudioMixer(SFX.PlayerHitPunch);
    }

    void PlayerAttack(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, attackRange, enemyLayer);

        Debug.DrawRay(transform.position, direction * attackRange, Color.red, 0.5f);

        if (hit.collider != null && hit.collider.CompareTag("enemy"))
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();

            if (enemy != null)
            {
                Vector3 enemyPos = hit.collider.transform.position;

                // NOVIDADE: Calcula a posição ideal para o Player parar.
                // Mantemos o Y e o Z do Player, e mudamos apenas o X baseado na direção do ataque e no offset.
                Vector3 targetPos = new Vector3(
                    enemyPos.x - (direction.x * attackOffset),
                    transform.position.y,
                    transform.position.z
                );

                // O Player se move para a posição IDEAL em TODOS os acertos, seja kill ou não.
                transform.position = targetPos;

                // Aplica o dano e a invulnerabilidade
                enemy.TakeDamage(attackDamage);
                StartCoroutine(IFramesRoutine());

                // Verifica se morreu para os logs/sons corretos
                if (enemy == null || enemy.GetCurrentHealth() <= 0)
                {
                    AudioManager.instance?.PlaySFXAudioMixer(SFX.PlayerHitPunch);
                    ScoreManager.instance.AddCombo();
                    Debug.Log("Inimigo derrotado!");
                }
                else
                {
                    AudioManager.instance?.PlaySFXAudioMixer(SFX.PlayerHitPunch);
                    ScoreManager.instance.AddCombo();
                    Debug.Log($"Inimigo atingido! Vida restante: {enemy.GetCurrentHealth()}");
                }
            }
            else
            {
                // Fallback do sistema antigo (sem o script Enemy)
                Vector3 enemyPos = hit.collider.transform.position;
                Vector3 targetPos = new Vector3(enemyPos.x - (direction.x * attackOffset), transform.position.y, transform.position.z);

                Destroy(hit.collider.gameObject);
                AudioManager.instance?.PlaySFXAudioMixer(SFX.PlayerHitPunch);
                transform.position = targetPos;
                ScoreManager.instance.AddCombo();
                Debug.Log("Inimigo atingido (sistema antigo)!");
            }
        }
        else
        {
            // Errou o golpe
            StartCoroutine(MissCooldownRoutine());
            ScoreManager.instance.DelCombo();
            AudioManager.instance?.PlaySFXAudioMixer(SFX.PlayerLoseCombo);
            GameObject tempVFX = Instantiate(floatingTextPrefab, textPoint.position, Quaternion.identity);
            Destroy(tempVFX, 0.7f);
            Debug.Log("Errou o golpe!");
        }
    }
    // Essa corrotina liga a invulnerabilidade, espera os milissegundos e desliga
    public System.Collections.IEnumerator IFramesRoutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityTime);
        isInvulnerable = false;
    }

    public System.Collections.IEnumerator MissCooldownRoutine()
    {
        isOnMissCooldown = true;
        yield return new WaitForSeconds(missCooldown);
        isOnMissCooldown = false;
    }
}