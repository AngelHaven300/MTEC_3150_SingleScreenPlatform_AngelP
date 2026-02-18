using UnityEngine;

public class PU_RunNGun : PowerUp
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void ApplyEffect()
    {
        player.movementSpeed += 100;
        player.fireRate *= 0.5f;
        player.fullAuto = true;
        
        base.ApplyEffect();
    }
    protected override void NegateEffect()
    {
        base.NegateEffect();
        player.movementSpeed -= 100;
        player.fireRate /= 0.5f;
        player.fullAuto = false;
    }
}
