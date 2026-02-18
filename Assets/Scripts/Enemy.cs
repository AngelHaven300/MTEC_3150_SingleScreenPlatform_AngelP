using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    public int hp;
    public float direction;
    protected Rigidbody2D rb;


    protected virtual void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       
    }

  
    protected virtual void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(direction * moveSpeed * Time.deltaTime, rb.linearVelocity.y);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            direction *= -1;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            direction *= -1;
        }
    }
}
