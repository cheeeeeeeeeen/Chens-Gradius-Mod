using Microsoft.Xna.Framework;
using Terraria;

namespace ChensGradiusMod.Projectiles.Options.Aim
{
  public abstract class AimOptionBaseObject : OptionBaseObject
  {
    public override string Texture => "ChensGradiusMod/Sprites/AimSheet";

    protected override int SpawnDuplicateProjectile(Projectile p)
    {
      int pInd;

      if (ModOwner.isAiming)
      {
        Vector2 pPosition = ComputeOffset(Main.player[p.owner].Center, p.Center);
        Vector2 offsetVelocity = ComputeVelocityOffset(p, pPosition);
        pInd = Projectile.NewProjectile(pPosition, offsetVelocity, p.type, p.damage,
                                        p.knockBack, projectile.owner, 0f, 0f);
      }
      else pInd = base.SpawnDuplicateProjectile(p);

      return pInd;
    }

    private Vector2 ComputeVelocityOffset(Projectile p, Vector2 offsetPos)
    {
      float pSpd = Vector2.Distance(Vector2.Zero, p.velocity);
      float dAng = GradiusHelper.GetBearing(Main.player[p.owner].Center, Main.MouseWorld, false);
      float pAng = GradiusHelper.GetBearing(Vector2.Zero, p.velocity, false);
      float offAng = MathHelper.ToRadians(pAng - dAng);
      float aimDAng = MathHelper.ToRadians(GradiusHelper.GetBearing(offsetPos, Main.MouseWorld, false));

      return GradiusHelper.MoveToward(offsetPos, offsetPos + (aimDAng + offAng).ToRotationVector2(), pSpd);
    }
  }
}