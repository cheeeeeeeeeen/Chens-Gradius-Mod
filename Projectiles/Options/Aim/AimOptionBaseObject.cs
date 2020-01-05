using Microsoft.Xna.Framework;
using Terraria;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Projectiles.Options.Aim
{
  public abstract class AimOptionBaseObject : OptionBaseObject
  {
    public static Vector2 ComputeVelocityOffset(Projectile p, Vector2 offsetPos, Vector2 toward)
    {
      Vector2 retVal = Vector2.Zero;

      if (p.velocity != Vector2.Zero)
      {
        float pSpd = Vector2.Distance(Vector2.Zero, p.velocity);
        float dAng = GetBearing(Main.player[p.owner].Center, Main.MouseWorld, false);
        float pAng = GetBearing(Vector2.Zero, p.velocity, false);
        float offAng = MathHelper.ToRadians(pAng - dAng);
        Vector2 offDiff = p.Center - Main.player[p.owner].Center;
        float aimDAng = MathHelper.ToRadians(GetBearing(offsetPos, toward - offDiff, false));
        retVal = MoveToward(offsetPos, offsetPos + (aimDAng + offAng).ToRotationVector2(), pSpd);
      }

      return retVal;
    }

    public override string Texture => "ChensGradiusMod/Sprites/AimSheet";

    protected override int SpawnDuplicateProjectile(Projectile p)
    {
      int pInd;

      if (ModOwner.isAiming)
      {
        Vector2 pPosition = ComputeOffset(Main.player[p.owner].Center, p.Center);
        Vector2 offsetVelocity = ComputeVelocityOffset(p, pPosition, Main.MouseWorld);
        pInd = Projectile.NewProjectile(pPosition, offsetVelocity, p.type, p.damage,
                                        p.knockBack, projectile.owner, 0f, 0f);
      }
      else pInd = base.SpawnDuplicateProjectile(p);

      return pInd;
    }
  }
}