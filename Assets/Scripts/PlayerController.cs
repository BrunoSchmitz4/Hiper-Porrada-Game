using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject floatingTextPrefab;
    public Transform textPoint;

    public float attackRange = 3.0f;
    public LayerMask enemyLayer;

    private Vector3 currentScale;

    [SerializeField] private Animator animator;

    void Start()
    {
        currentScale = transform.localScale;
    }

    void Update()
    {
        // Não permite input se o jogo está pausado ou acabou
        //if (GameManager.instance != null && (GameManager.instance.isPaused || GameManager.instance.isGameOver))
        //    return;

        if (Keyboard.current == null) return;

        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            FlipSprite("left");
            AttemptAttack(Vector2.left);
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            FlipSprite("right");
            AttemptAttack(Vector2.right);
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

    void AttemptAttack(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, attackRange, enemyLayer);
        Debug.DrawRay(transform.position, direction * attackRange, Color.red, 0.5f);

        if (hit.collider != null && hit.collider.CompareTag("enemy"))
        {
            Vector3 enemyPos = hit.collider.transform.position;
            Destroy(hit.collider.gameObject);
            // Usa Singleton
            AudioManager.instance?.PlaySFX(AudioManager.instance.hitPunch);
            transform.position = enemyPos;
            ScoreManager.instance?.AddCombo();
            Debug.Log("Inimigo atingido!");
        }
        else
        {
            ScoreManager.instance?.DelCombo();
            if (floatingTextPrefab != null && textPoint != null)
            {
                GameObject tempVFX = Instantiate(floatingTextPrefab, textPoint.position, Quaternion.identity);
                Destroy(tempVFX, 0.7f);
            }
            Debug.Log("Errou o golpe!");
        }
    }
}