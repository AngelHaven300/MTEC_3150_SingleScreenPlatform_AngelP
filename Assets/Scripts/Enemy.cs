using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    public int hp;
    public float direction;
    protected Rigidbody2D rb;
    protected float movingTimer = 0;
    protected float timeToWaitBeforeChangingDirection = 5.5f;


    protected virtual void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       
    }
 
    protected virtual void Update()
    {
        if (movingTimer < timeToWaitBeforeChangingDirection)
        {
            movingTimer += Time.deltaTime;

        }
        else
        {
            direction *= -1;
            movingTimer = 0;
        }
        transform.Translate(direction * moveSpeed * Time.deltaTime, 0, 0);
    }
    

    protected virtual void FixedUpdate()
    {
       // rb.linearVelocity = new Vector2(direction * moveSpeed * Time.deltaTime, rb.linearVelocity.y);
    }
    public void DecrementHP(int damage)
    {
        if (hp - damage > 0)
        {
            hp-= damage;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}