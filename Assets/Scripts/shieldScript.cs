using UnityEngine;

public class shieldScript : MonoBehaviour
{
    private BoxCollider2D rb;
    private int damageAmount = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Bullet>() != null)
        {
            Destroy(collision.gameObject);
        }
        if (collision.GetComponent<Crawler>() != null)
        {
            var Enemy = collision.GetComponent<Crawler>();
            //var DamageEnemy = collision.GetComponent<Crawler>();
            Enemy.direction *= -1;
            Enemy.DecrementHP(damageAmount);

        }
    }
}
