using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.Projectiles.Forces
{
  public class NeedleForce : ForceBase
  {
    private const float StatReduction = .75f;
    private readonly float angleIncrease = 6f;
    private readonly int[] detachedCooldown = { 3, 3 };
    private float shootAngle = 0f;
    private int angleDirection = 1;

    public override string Texture => "ChensGradiusMod/Sprites/NeedleForceSheet";

    public override void PerformAttack()
    {
      float vX, vY;

      if (mode != (int)States.Attached)
      {
        vX = (float)Math.Cos(MathHelper.ToRadians(shootAngle));
        vY = (float)-Math.Sin(MathHelper.ToRadians(shootAngle));
        shootAngle += angleIncrease * angleDirection;
      }
      else
      {
        vX = 1f * projectile.spriteDirection;
        vY = 0f;
      }

      Projectile.NewProjectile(projectile.Center, new Vector2(vX, vY) * ForceLightBullet.Spd,
                                   mod.ProjectileType<ForceLightBullet>(),
                                   projectile.damage, projectile.knockBack, Owner.whoAmI);
      Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Forces/LightBulletShoot"),
                         projectile.Center);
    }

    public override void SpecialDetachActions()
    {
      shootAngle = 0f;
      GradiusHelper.FlipAngleDirection(ref shootAngle, projectile.spriteDirection);
    }

    protected override bool ForceCheck() => ModOwner.needleForce;

    protected override void Engage()
    {
      if (InBattle)
      {
        if (++InBattleTick < InBattleExpire)
        {
          int[] usedCooldown = mode != (int)States.Attached ? detachedCooldown : AttackCooldowns;
          if (++AttackTick >= usedCooldown[AttackIndex])
          {
            PerformAttack();
            AttackTick = 0;
            if (++AttackIndex >= usedCooldown.Length) AttackIndex = 0;
          }
        }
        else
        {
          InBattle = false;
          SpecialDetachActions();
        }
      }
    }

    protected override void UpdateDamage()
    {
      base.UpdateDamage();

      projectile.damage = Math.Max(Dmg, GradiusHelper.RoundOffToWhole(projectile.damage * StatReduction));
      projectile.knockBack *= StatReduction;
    }

    protected override bool Reattach()
    {
      if(base.Reattach())
      {
        angleDirection = -angleDirection;
        return true;
      }

      return false;
    }

    protected override int InBattleExpire { get; } = 270;

    protected override int[] AttackCooldowns { get; } = { 4, 17 };
  }
}
