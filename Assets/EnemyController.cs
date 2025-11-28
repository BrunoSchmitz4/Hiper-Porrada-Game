using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2.0f; //velocidade de movimento do inimigo
    private Transform player;

    void Start()
    {
        // Acha o player na cena automaticamente
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null)
        {
            // Move em direção ao player
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }
}