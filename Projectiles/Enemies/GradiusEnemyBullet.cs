using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.Projectiles.Enemies
{
  public class GradiusEnemyBullet : ModProjectile
  {
    public const float Spd = 5f;
    public const int Dmg = 70;
    public const float Kb = 0f;

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Bacterion Bullet");
      Main.projFrames[projectile.type] = 2;
    }

    public override void SetDefaults()
    {
      projectile.width = 12;
      projectile.height = 12;
      projectile.friendly = false;
      projectile.hostile = true;
      projectile.timeLeft = 1800;
      projectile.light = .25f;
      projectile.ignoreWater = true;
      projectile.tileCollide = true;
    }

    public override void AI()
    {
      if (++projectile.frameCounter >= 4)
      {
        projectile.frameCounter = 0;
        if (++projectile.frame >= Main.projFrames[projectile.type]) projectile.frame = 0;
      }
    }

    public override string Texture => "ChensGradiusMod/Sprites/EnemyBullet";

    public override Color? GetAlpha(Color lightColor) => Color.White;
  }
}
