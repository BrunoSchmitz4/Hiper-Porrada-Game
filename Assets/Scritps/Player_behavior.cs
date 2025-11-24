using UnityEngine;

public class Player_behavior : MonoBehaviour
{

    // Player no centro
    /*
     Inimigos se aproximam da posição do player;
     player não se move mas ataca na direção do inimigo mais próximo;
     player atinge inimigo se estiver no range;
     Player se movimenta na direção do ataque (definir range da investida);
     Ataque do player é hitkill
     Player substitui posição atrás do inimigo atingido (bah guri)
     Nova posição = posição do inimigo - 5 / + 5 (esquerda/direita)
     Inimigo é atravessável
     Ataque do inimigo é hitkill (se pá)
     
     
    */
    public float Speed;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();     
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        rb.linearVelocity = direction * Speed;
    }
}
