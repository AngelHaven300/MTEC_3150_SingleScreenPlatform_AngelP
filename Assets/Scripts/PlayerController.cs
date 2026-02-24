using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int Health = 5;
    public float movementSpeed = 1.0f;
    private float xMove;
    private float xVelocity;
    public float jumpSpeed = 1.0f;
    public LayerMask ground;
    private bool jumpFlag = false;
    [HideInInspector] public float slowFall = 1.0f;
    [HideInInspector] public float originalFall = 1.0f;
    private float facingDown;
    private float facingDirection;
    private float attackOffset = 0.8f;

    private float shieldOffset = 1f;
    public GameObject meleeAttack;
    public GameObject meleeShield;
    public float meleeDuration = 0.25f;
    private float timeElapsedSinceMelee = 0.0f;
    private bool meleeTriggered = false;
    
    
    private float shootDown;
    public GameObject bulletPrefab;
    public GameObject bulletDPrefab;
    [HideInInspector] public bool fullAuto = false;
    [HideInInspector] public float fireRate = 0.1f;   
    private float nextFireTime = 0f;
    public int bulletDamageAmount = 1;
    //public int damageAmount = 1;
    

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        facingDirection = 1;
        facingDown = 0;
        originalFall = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        xMove = Input.GetAxisRaw("Horizontal");
        shootDown = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            jumpFlag = true;
        }
        if (Input.GetMouseButtonDown(0))
        {
            MeleeAttack();
        }
        if (Input.GetMouseButtonDown(1))
        {
            RangedAttack();
        }
        else if (fullAuto)
        {
            if (Input.GetMouseButton(1))
            {
                if (Time.time > nextFireTime)
                {
                    RangedAttack();
                    nextFireTime = Time.time + fireRate;
                }
            }
        }
        if (shootDown == -1)
        {
            facingDown = shootDown;
        }
        else if (shootDown != -1)
        {
            facingDown = shootDown;
        } 
        if (xMove != 0)
        {
            facingDirection = xMove;
        }
        if (meleeTriggered)
        {
            if (timeElapsedSinceMelee < meleeDuration)
            {
                timeElapsedSinceMelee += Time.deltaTime;
            }
            else
            {
                meleeAttack.SetActive(false);
                meleeShield.SetActive(false);
                timeElapsedSinceMelee = 0;
                meleeTriggered = false;
            }
        }
        WhenOnPlatform();
    }

    private void FixedUpdate()
    {
        xVelocity = xMove * movementSpeed * Time.deltaTime;
        rb.linearVelocity = new Vector3(xVelocity, rb.linearVelocity.y, 0);

        if (rb.linearVelocityY < 0)
        {
            GetComponent<Rigidbody2D>().gravityScale = slowFall;
        }
        else
        {
            GetComponent<Rigidbody2D>().gravityScale = originalFall;
        }
        if (jumpFlag)
        {
            
            rb.linearVelocityY = jumpSpeed; 
            jumpFlag = false;
        }
    }
    
    private void MeleeAttack()
    {
        meleeTriggered = true;

        if (facingDown == -1)
        {
            meleeShield.SetActive(true);
            meleeShield.transform.localPosition = new Vector3(meleeShield.transform.localPosition.x, shieldOffset * facingDown, 0);
        }
        else
        {
            meleeAttack.SetActive(true);
            meleeAttack.transform.localPosition = new Vector3(shieldOffset * facingDirection, meleeAttack.transform.localPosition.y, 0); 

        }
    }
    private void RangedAttack()
    {        

        if (facingDown == -1)
        {
            Vector3 posD = new Vector3(transform.position.x, transform.position.y + (attackOffset * facingDown), 0);
            GameObject bulletD = Instantiate(bulletDPrefab, posD, Quaternion.identity);
            bulletD.GetComponent<Bullet>().direction = new Vector2(0, facingDown);
            var bScript = bulletD.GetComponent<Bullet>();
            bScript.DamageAmount = bulletDamageAmount;
        }
        else
        {
            Vector3 pos = new Vector3(transform.position.x + (attackOffset * facingDirection), transform.position.y, 0);
            GameObject bullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
            bullet.GetComponent<Bullet>().direction = new Vector2(facingDirection, 0);
            var bScript = bullet.GetComponent<Bullet>();
            bScript.DamageAmount = bulletDamageAmount;
        }
        
    }
    
    private bool IsGrounded()
    {
        float radius = GetComponent<Collider2D>().bounds.extents.x;
        float dist = GetComponent<Collider2D>().bounds.extents.y;

        return Physics2D.CircleCast(transform.position, radius, Vector2.down, dist, ground);
        //if its not a platform parent it to null
        //
    }
    private void WhenOnPlatform()
    {
        if (IsGrounded())
        {
            //Debug.Log("a");
            float radius = GetComponent<Collider2D>().bounds.extents.x;
            float dist = GetComponent<Collider2D>().bounds.extents.y;
            RaycastHit2D hit;
            hit = Physics2D.CircleCast(transform.position, radius, Vector2.down, dist, ground);
            if (hit.transform.gameObject.CompareTag("platform"))
            {
                transform.SetParent(hit.transform);
            }
            else
            {
                transform.SetParent(null);
            }
        }
        else
        {
            transform.SetParent(null);
        }
        
    }
    public void IncrementHP(int amount)
    {
        if (Health + amount > 0)
        {
            Health += amount;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided");
        if (collision.GetComponent<PowerUp>() != null)
        {
            collision.GetComponent<PowerUp>().ApplyEffect();

            Debug.Log("EffectApplied");
        }
    }
}
