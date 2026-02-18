using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10;
    public Vector2 direction;
    protected Rigidbody2D rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        rb.linearVelocity = direction * speed * Time.deltaTime;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
    // Update is called once per frame
}
