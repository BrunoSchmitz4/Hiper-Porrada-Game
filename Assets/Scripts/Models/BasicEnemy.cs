using UnityEngine;

public class BasicEnemy : Enemy
{
    protected override void Start()
    {
        // Configurações padrão do inimigo básico
        speed = 1.5f;
        maxHealth = 1;
        scoreValue = 10;

        base.Start();
    }
}