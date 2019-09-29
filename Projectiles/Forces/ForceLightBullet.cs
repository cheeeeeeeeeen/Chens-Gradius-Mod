using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.Projectiles.Forces
{
  public class ForceLightBullet : ModProjectile
  {
    public const float Spd = 20f;

    public override void SetDefaults()
    {
      projectile.width = 16;
      projectile.height = 8;
      projectile.friendly = true;
      projectile.hostile = false;
      projectile.timeLeft = 300;
      projectile.light = 0.25f;
      projectile.ignoreWater = true;
      projectile.tileCollide = true;
    }

    public override void AI()
    {
      projectile.rotation = projectile.velocity.ToRotation();
    }

    public override string Texture => "ChensGradiusMod/Sprites/ForceLightBullet";

    public override Color? GetAlpha(Color lightColor) => Color.White;
  }
}