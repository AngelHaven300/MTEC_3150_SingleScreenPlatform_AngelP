using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Health Settings")]
    public int Health = 5;
    public Image heartTwo;
    public Image heartThree;
    public Image heartFour;
    public Image heartFive;
    public Image shieldBar;
    public int maxHealth;

    [Header("Movement Settings")]
    public LayerMask ground;
    public int fastFallStrength;
    public float movementSpeed = 1.0f;
    public float jumpSpeed = 1.0f;
    private float xMove;
    private float xVelocity;
    private float facingDirection;
    private float facingDown;
    private bool jumpFlag = false;
    private SpriteRenderer shieldSr;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    [HideInInspector] public float slowFall = 1.0f;
    [HideInInspector] public float originalFall = 1.0f;

    [Header("Weapon Settings")]
    public GameObject meleeAttack;
    public GameObject meleeShield;
    public GameObject bulletPrefab;
    public GameObject bulletDPrefab;
    public int bulletDamageAmount = 1;
    public float meleeDuration = 0.25f;
    private float attackOffset = 0.8f;
    private float shieldOffset = 0.8f;
    private float timeElapsedSinceMelee = 0.0f;
    private float elapsedTimeSinceShield;
    private float meleeCooldownTime = 1f;
    private float shootDown;
    private float nextFireTime = 0f;
    private bool meleeTriggered = false;
    [HideInInspector] public bool fullAuto = false;
    [HideInInspector] public float fireRate = 0.1f;

    private Animator anim;

    void Start()
    {
        elapsedTimeSinceShield = meleeCooldownTime;
        rb = GetComponent<Rigidbody2D>();
        shieldSr = meleeShield.GetComponent<SpriteRenderer>();
        facingDirection = 1;
        facingDown = 0;
        originalFall = rb.gravityScale;
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        elapsedTimeSinceShield += Time.deltaTime;
        xMove = Input.GetAxisRaw("Horizontal");
        shootDown = Input.GetAxisRaw("Vertical");
        AmountOfHearts();
     

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            anim.SetTrigger("jump");
            jumpFlag = true;
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (elapsedTimeSinceShield >= meleeCooldownTime)
            {
               MeleeAttack();
                elapsedTimeSinceShield = 0;
            }   
        }
        if (Input.GetMouseButtonDown(0))
        {
            RangedAttack();
        }
        else if (fullAuto)
        {
            if (Input.GetMouseButton(0))
            {
                if (Time.time > nextFireTime)
                {
                    RangedAttack();
                    nextFireTime = Time.time + fireRate;
                }
            }
        }
        if (shootDown < 0)
        {
            facingDown = -1;
        }
        else 
        {
            facingDown = 0;
        } 
        if (xMove != 0)
        {
            anim.SetBool("running", true);
            facingDirection = xMove;
            if (facingDirection > 0)
            {
                sr.flipX = false;
            }
            else
            {
                sr.flipX = true;
            }
        }
        else
        {
            anim.SetBool("running", false);
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
        float fillAmount = Mathf.Clamp01(elapsedTimeSinceShield / meleeCooldownTime);
        shieldBar.fillAmount = fillAmount;
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

        if (facingDown < -0.1f)
        {
            meleeShield.SetActive(true);
            meleeAttack.SetActive(false);
            meleeShield.transform.localPosition = new Vector2(meleeShield.transform.localPosition.x, shieldOffset * facingDown);

            
            meleeShield.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else
        {
            meleeAttack.SetActive(true);
            meleeShield.SetActive(false);
            meleeAttack.transform.localPosition = new Vector2(shieldOffset * facingDirection, meleeAttack.transform.localPosition.y);

            if (facingDirection > 0) { meleeAttack.transform.rotation = Quaternion.identity; }
            else if (facingDirection < 0) { meleeAttack.transform.rotation = Quaternion.Euler(0, -180, 0); }
        }
    }

    private void RangedAttack()
    {
        anim.SetTrigger("attack");
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
    private void OnLanding()
    {
        anim.SetTrigger("landed");
    }
    
    private bool IsGrounded()
    {
        float radius = GetComponent<Collider2D>().bounds.extents.x;
        float dist = GetComponent<Collider2D>().bounds.extents.y;

        return Physics2D.CircleCast(transform.position, radius, Vector2.down, dist, ground);
      
    }
    private void WhenOnPlatform()
    {
        if (IsGrounded())
        {
        
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

    public void AmountOfHearts()
    {
        if (Health == 5)
        {
           
            heartTwo.enabled = true;
            heartThree.enabled = true;
            heartFour.enabled = true;
            heartFive.enabled = true;
        } 
        else if (Health == 4)
        {
            
            heartTwo.enabled = true;
            heartThree.enabled = true;
            heartFour.enabled = true;
            heartFive.enabled = false;
        }
        else if (Health == 3)
        {
            
            heartTwo.enabled = true;
            heartThree.enabled = true;
            heartFour.enabled = false;
            heartFive.enabled = false;
        }
        else if (Health == 2)
        {
            
            heartTwo.enabled = true;
            heartThree.enabled = false;
            heartFour.enabled = false;
            heartFive.enabled = false;
        }
        else if (Health == 1)
        {
            
            heartTwo.enabled = false;
            heartThree.enabled = false;
            heartFour.enabled = false;
            heartFive.enabled = false;
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
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            OnLanding();
        }
        if ( collision.gameObject.CompareTag("platform"))
        {
            OnLanding();
        }
    }
}
