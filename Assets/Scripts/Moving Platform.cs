using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float direction;
    public float movingSpeed;
    protected Rigidbody2D rb;
    private float movingTimer = 0;
    private float timeToWaitBeforeChangingDirection = 8;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        movingTimer += Time.deltaTime;
        if (movingTimer >= timeToWaitBeforeChangingDirection)
        {
            direction *= -1;
            movingTimer = 0;
        }
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(direction * movingSpeed * Time.deltaTime, rb.linearVelocity.y);

    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    { 
        if (collision.gameObject.CompareTag("Wall"))
        {
            movingTimer = 0;
            direction *= -1;
        }
    }
}
