using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ChensGradiusMod.Projectiles.Enemies
{
  public class CoreLaser : ModProjectile
  {
    public const float Spd = 12f;
    public const int Dmg = 100;
    public const float Kb = 50f;

    public override void SetStaticDefaults() => DisplayName.SetDefault("Core Laser");

    public override void SetDefaults()
    {
      projectile.width = 32;
      projectile.height = 8;
      projectile.friendly = false;
      projectile.hostile = true;
      projectile.timeLeft = 600;
      projectile.light = .35f;
      projectile.ignoreWater = true;
      projectile.tileCollide = false;
    }

    public override string Texture => "ChensGradiusMod/Sprites/CoreLaser";

    public override Color? GetAlpha(Color lightColor) => Color.White;
  }
}
