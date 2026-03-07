using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject floatingTextPrefab;
    public Transform textPoint;

    public float attackRange = 3.0f;
    public int attackDamage = 1;
    public LayerMask enemyLayer;

    private Vector3 currentScale;

    [SerializeField] private Animator animator;

    void Start() { currentScale = transform.localScale; }

    void Update()
    {
        // Não permite input se o jogo está pausado ou acabou
        //if (GameManager.instance != null && (GameManager.instance.isPaused || GameManager.instance.isGameOver))
        //    return;

        if (Keyboard.current == null) return;

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
            animator.SetTrigger("Attack");
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
            // Tenta pegar o componente Enemy (classe base)
            Enemy enemy = hit.collider.GetComponent<Enemy>();

            if (enemy != null)
            {
                Vector3 enemyPos = hit.collider.transform.position;

                enemy.TakeDamage(attackDamage);

                // Se o inimigo morreu, adiciona combo
                if (enemy == null || enemy.GetCurrentHealth() <= 0)
                {
                    AudioManager.instance?.PlaySFXAudioMixer(SFX.PlayerHitPunch);
                    transform.position = enemyPos;
                    ScoreManager.instance.AddCombo();
                    Debug.Log("Inimigo derrotado!");
                }
                else
                {
                    // Inimigo ainda está vivo (inimigos com mais vida)
                    AudioManager.instance?.PlaySFXAudioMixer(SFX.PlayerHitPunch);
                    ScoreManager.instance.AddCombo();
                    Debug.Log($"Inimigo atingido! Vida restante: {enemy.GetCurrentHealth()}");
                }
            }
            else
            {
                // Fallback para o comportamento antigo (caso ainda existam inimigos sem o script Enemy)
                Destroy(hit.collider.gameObject);
                AudioManager.instance?.PlaySFXAudioMixer(SFX.PlayerHitPunch);
                transform.position = hit.collider.transform.position;
                ScoreManager.instance.AddCombo();
                Debug.Log("Inimigo atingido (sistema antigo)!");
            }
        }
        else
        {
            // Errou o golpe
            ScoreManager.instance.DelCombo();
            AudioManager.instance?.PlaySFXAudioMixer(SFX.PlayerLoseCombo);
            GameObject tempVFX = Instantiate(floatingTextPrefab, textPoint.position, Quaternion.identity);
            Destroy(tempVFX, 0.7f);
            Debug.Log("Errou o golpe!");
        }
    }
}