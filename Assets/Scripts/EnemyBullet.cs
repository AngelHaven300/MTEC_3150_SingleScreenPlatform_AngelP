using UnityEngine;

public class EnemyBullet : Bullet
{
    protected override void FixedUpdate()
    {

        if (target != null)
        {
            direction = ((Vector2)target.position - rb.position).normalized;



        }
        rb.linearVelocity = direction * speed;
    }
    
}
