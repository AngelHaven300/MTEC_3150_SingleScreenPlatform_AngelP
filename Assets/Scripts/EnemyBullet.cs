using UnityEngine;

public class EnemyBullet : Bullet
{
    
    //in update transform.pos + vector 2. move towards (transform.pos, player.trans.pos, speed * time.deltatime
    protected override void FixedUpdate()
    {

        if (target != null)
        {
            direction = ((Vector2)target.position - rb.position).normalized;

            
            //transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        rb.linearVelocity = transform.forward * speed;
    }
    //transform.position = Vector2.MoveTowards(transform.position, player.transform.position, bullet.speed* Time.deltaTime);
    
}
