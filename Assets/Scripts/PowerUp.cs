using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public SpriteRenderer sr;
    public CircleCollider2D cc;
    public Color powerUpColor;
    protected PlayerController player;
    private bool effectsApplied = false;
    public float effectDuration;
    private float timeElapsedSinceEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        sr = GetComponent<SpriteRenderer>();
        cc = GetComponent<CircleCollider2D>();
        sr.color = powerUpColor;
    }
    public virtual void ApplyEffect()
    {
        //Destroy(gameObject);
      cc.enabled = false;
      sr.enabled = false;
      effectsApplied = true;
    }
    private void Update()
    {
        if (effectsApplied)
        {
            if (timeElapsedSinceEffect < effectDuration)
            {
                timeElapsedSinceEffect += Time.deltaTime;
            }
            else
            {
                timeElapsedSinceEffect = 0;
                NegateEffect();
                effectsApplied = false;
                Destroy(gameObject);

            }
        }
    }
    protected virtual void NegateEffect()
    {

    }
}
    // Update is called once per frame
  
