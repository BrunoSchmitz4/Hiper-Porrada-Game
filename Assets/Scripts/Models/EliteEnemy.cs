using UnityEngine;

public class EliteEnemy : Enemy
{
    protected override void Start()
    {
        speed = 2.5f;
        maxHealth = 2;
        scoreValue = 30;

        base.Start();
    }

    protected override void Update() { base.Update(); }
        
}