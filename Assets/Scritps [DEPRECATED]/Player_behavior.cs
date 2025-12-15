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
     
     provavelmente a ordem de inimigo será em ordem de geração - fila (FIFO - First In First Out)
     
    */

    public GameObject ataqueAcerto;     // Área de acerto do ataque
    public Transform fist;             // De onde saí o soco (ah, jura?)
    private bool punch;                  // Input do soco do ataque
    public float punchVelocity;             // Velocidade do soco
    private bool flipX = false;


    [SerializeField]
    public float Speed;

    [SerializeField]
    private Rigidbody2D playerRb;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();     
    }

    private void FixedUpdate()
    {
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        playerRb.linearVelocity = direction.normalized * Speed;

    }

    private void Update()
    {
        spriteUpdate();

        this.punch = Input.GetButtonDown("Jump");
    }

    private void spriteUpdate()
    {
        if (this.playerRb.linearVelocity.x > 0) this.spriteRenderer.flipX = false; // olhando pra direita
        else if (this.playerRb.linearVelocity.x < 0) this.spriteRenderer.flipX = true; // olhando pra esquerda
    }

    private void desferirSoco()
    {
        if(punch == true)
        {
            GameObject temp = Instantiate(ataqueAcerto);
            temp.transform.position = fist.position;
            temp.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(punchVelocity, 0);
            Destroy(temp.gameObject, 3f);
        }
    }
}
