using UnityEngine;

public class FastEnemy : Enemy
{
    protected override void Start()
    {
        speed = 3.0f;
        maxHealth = 1;
        scoreValue = 20;

        base.Start();
    }
}