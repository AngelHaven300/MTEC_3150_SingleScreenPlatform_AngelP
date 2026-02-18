using UnityEngine;

public class PU_SlowFall : PowerUp
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void ApplyEffect()
    {
        player.jumpSpeed += 4;
        player.slowFall = 0.10f;
        base.ApplyEffect();

    }
    protected override void NegateEffect()
    {
        base.NegateEffect();
        player.jumpSpeed -= 4;
        player.slowFall = 1.0f;
    }
}
