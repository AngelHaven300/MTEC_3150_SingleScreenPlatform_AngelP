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
    private bool playerWasDetected = false;
    private bool detected;
    private Transform playerTransform;

    protected override void Start()
    {
        base.Start();
        defaultSpeed = moveSpeed;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) playerTransform = playerObj.transform;

    }
    private void Update()
    {

        if (PlayerDetected())
        {
            detected = true;
            playerWasDetected = true;
        }        
        if (detected && playerWasDetected)
        {
            attackStarted = true;
            attackTimer = 0;
            moveSpeed = 0;
        }
        
        if (attackStarted)
        {
            attackTimer += Time.deltaTime;
            playerWasDetected = false; 
            if (attackTimer >= waitbeforeAttack)
            {
                if (hp >= 250) 
                {
                  RangedAttack();  
                }
                                
                attackStarted = false;
                attackTimer = 0;
                moveSpeed = defaultSpeed;
            }
                     
            return;
        }
        else if (!attackStarted)
        {
            moveSpeed = defaultSpeed;
            detected = false;
        }
    }


    private void RangedAttack()
    {
        //GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        //if (playerObj != null) return;
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + attackOffset, 0);
        GameObject bulletObj = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        Vector2 fireDirection = ((Vector2)playerTransform.position - (Vector2)spawnPos).normalized;
        bulletScript.direction = fireDirection;

    }
    private bool PlayerDetected()
    {
        Vector2 dir = Vector2.up;
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position,dir, range, playerLayer);
        Debug.DrawRay(transform.position, dir * range, Color.red);

        return hit.collider !=null;
    }
}
