using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Exemplo de como deve ficar onde o player toma dano
        PlayerController controller = GetComponent<PlayerController>();

        // Só toma dano se NÃO estiver invulnerável
        if (!controller.isInvulnerable)
        {
            // Código que tira vida do jogador aqui...
        }
    }
}
