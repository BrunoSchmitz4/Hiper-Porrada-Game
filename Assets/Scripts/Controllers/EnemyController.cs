using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2.0f; //velocidade de movimento do inimigo
    private Transform player;
    private Vector3 currentScale; //variável pra controlar tamanho/lado

    void Start()
    {
        // Acha o player na cena automaticamente
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // Salva o tamanho original pra não deformar
        currentScale = transform.localScale;
    }

    void Update()
    {
        if (player != null)
        {
            // Move em direção ao player
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            // VIRAR O SPRITE (FLIP)
            //Atualiza a referência da escala atual
            currentScale = transform.localScale;

            //Se o player está à direita do inimigo (x maior)
            if (player.position.x > transform.position.x)
            {
                //vira para a direita
                currentScale.x = -Mathf.Abs(currentScale.x); //garante que o x seja positivo
            }
            else
            {
                //vira para a esquerda
                currentScale.x = Mathf.Abs(currentScale.x); //garante que o x seja negativo
            }

            //Aplica a mudança
            transform.localScale = currentScale;
        }
    }
}