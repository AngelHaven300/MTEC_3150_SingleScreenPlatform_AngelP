using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10;
    public Vector2 direction;
    protected Rigidbody2D rb;
    protected Transform target;
    public int DamageAmount = 1;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {       
        rb = GetComponent<Rigidbody2D>();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        
    }

    protected virtual void FixedUpdate()
    {
        rb.linearVelocity = direction * speed * Time.deltaTime;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) 
        {
            Enemy enemyHit = collision.gameObject.GetComponent<Enemy>();
            enemyHit.DecrementHP(DamageAmount);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            player.IncrementHP(-DamageAmount);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Wall") | collision.gameObject.CompareTag("ground"))
        {
            Destroy(gameObject);
        }
        
     
    }
    // Update is called once per frame
}
