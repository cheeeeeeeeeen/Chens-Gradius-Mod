using Microsoft.Xna.Framework;
using Terraria;

namespace ChensGradiusMod.Projectiles.Options.Aim
{
  public abstract class AimOptionBaseObject : OptionBaseObject
  {
    public override string Texture => "ChensGradiusMod/Sprites/AimSheet";

    protected override int SpawnDuplicateProjectile(Projectile p)
    {
      int pInd = base.SpawnDuplicateProjectile(p);

      if (ModOwner.isAiming)
      {
        float pSpeed = Vector2.Distance(p.position, p.position + p.velocity);
        Main.projectile[pInd].velocity = GradiusHelper.MoveToward(p.Center, Main.MouseWorld, pSpeed);
      }

      return pInd;
    }
  }
}