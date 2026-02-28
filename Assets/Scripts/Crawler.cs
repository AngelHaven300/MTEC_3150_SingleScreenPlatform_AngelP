using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class Crawler : Enemy
{
    private Animator animator;
    public float range;
    public LayerMask playerLayer;
    private float defaultSpeed;
    public float bulletSpeed;
    public GameObject bulletPrefab;
    private bool attackStarted = false;
    private float attackTimer = 0f;
    public float waitbeforeAttack;
    private float attackOffset = 1.8f;
    private float nextAttackTime = 0f;
    public float attackCooldown = 2f;
    private Transform playerTransform;
    public int damageAmount = 1;
    
    public Image lastPlayerHeart;
    protected override void Start()
    {
        base.Start();
        animator = GetComponentInChildren<Animator>();
        
        direction = -1;
        defaultSpeed = moveSpeed;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) playerTransform = playerObj.transform;
        
            hp = maxhp;
        
    }
    protected override void Update()
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime, 0, 0);

        if (!attackStarted && Time.time >= nextAttackTime && PlayerDetected())
        {
            attackStarted = true;
            attackTimer = 0;
            moveSpeed = 0;
            animator.SetFloat("speed", moveSpeed);
        }

        if (attackStarted)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= waitbeforeAttack)
            {
                if (hp <= 250)
                {
                    RangedAttack();
                }
                else
                {
                    RangedAttack();
                }
                    attackStarted = false;
                moveSpeed = defaultSpeed;
                nextAttackTime = Time.time + attackCooldown;
                
            }
            return;
        }
        
        moveSpeed = defaultSpeed;
        float fillAmount = (float)hp / (float)maxhp;
        hpBar.fillAmount = fillAmount;
        animator.SetFloat("speed", moveSpeed);
        //killPlayerHealth = lastPlayerHeart.enabled;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null) lastPlayerHeart.enabled = false;

    }

    private void RangedAttack()
    {
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + attackOffset, 0);
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        var bScript = bullet.GetComponent<Bullet>();
        var player = GetComponent<PlayerController>();
        if (player != null)
        {
            Vector2 fireDirection = ((Vector2)playerTransform.position - (Vector2)spawnPos).normalized;
            bScript.direction = fireDirection;
            bScript.DamageAmount = damageAmount;
        }
    }

    private bool PlayerDetected()
    {
        Vector2 dir = Vector2.up * 0.9f;
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position,dir, range, playerLayer);
        Debug.DrawRay(transform.position, dir * range, Color.red);

        return hit.collider !=null;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<PlayerController>();
            if (player != null)
            {              
                player.IncrementHP(-damageAmount);
                Debug.Log("Subtract player Health");
                direction *= -1;
            }
        }
        if (collision.CompareTag("Wall"))
        {
            direction *= -1;
        }
    }
}
