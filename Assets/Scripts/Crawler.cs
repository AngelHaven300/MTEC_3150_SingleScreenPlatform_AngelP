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


    protected override void Start()
    {
        base.Start();
        defaultSpeed = moveSpeed;
        
    }
    private void Update()
    {
        PlayerDetected();

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
        if (!attackStarted)
        {
            moveSpeed = defaultSpeed;
            detected = false;
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
                attackTimer = 0;
                moveSpeed = defaultSpeed;
            }
            playerWasDetected = false;          
            return;
        }
        
    }


    private void RangedAttack()
    {
        Vector3 pos = new Vector3(transform.position.x , transform.position.y + attackOffset, 0);
        GameObject bullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
        bullet.GetComponent<Bullet>().direction = Vector2.up;
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
