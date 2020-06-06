using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Projectiles.Enemies
{
  public abstract class GradiusBaseBullet : ModProjectile
  {
    public override Color? GetAlpha(Color lightColor) => Color.White;

    public override bool? CanHitNPC(NPC target)
    {
      if (GradiusConfig.bacterionBulletDamageMultiplier <= 0) return false;

      return null;
    }

    public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
    {
      damage = RoundOffToWhole(GradiusConfig.bacterionBulletDamageMultiplier * damage);
    }

    private GradiusModConfig GradiusConfig => ModContent.GetInstance<GradiusModConfig>();
  }
}