using UnityEngine;
using UnityEngine.Timeline;

public class Crawler : Enemy
{
    public float range;
    public LayerMask playerLayer;
    private float defaultSpeed;
    public float bulletSpeed;
    public GameObject bulletPrefab;
    private bool attackStarted = false;
    private float attackTimer = 0f;
    public float waitbeforeAttack;
    private float attackOffset = 1.6f;
    private float nextAttackTime = 0f;
    public float attackCooldown = 2f;
    private Transform playerTransform;

    protected override void Start()
    {
        base.Start();
        direction = -1;
        defaultSpeed = moveSpeed;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) playerTransform = playerObj.transform;

    }
    protected override void Update()
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

        if (!attackStarted && Time.time >= nextAttackTime && PlayerDetected())
        {
            attackStarted = true;
            attackTimer = 0;
            moveSpeed = 0;
        }

        if (attackStarted)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= waitbeforeAttack)
            {
                if (hp >= 250)
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
    }

    private void RangedAttack()
    {
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + attackOffset, 0);
        GameObject bulletObj = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        Vector2 fireDirection = ((Vector2)playerTransform.position - (Vector2)spawnPos).normalized;
        bulletScript.direction = fireDirection;
    }
    private bool PlayerDetected()
    {
        Vector2 dir = Vector2.up * 0.9f;
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position,dir, range, playerLayer);
        Debug.DrawRay(transform.position, dir * range, Color.red);

        return hit.collider !=null;
    }
    
}
