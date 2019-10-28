using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Projectiles.Enemies
{
  public class MoaiBubble : ModProjectile
  {
    public const float Spd = 3f;
    public const int Dmg = 100;
    public const float Kb = 20f;

    private readonly int expertMaxLife = 2;
    private readonly int normalMaxLife = 1;
    private int life = 3;

    public override void SetStaticDefaults()
    {
      Main.projFrames[projectile.type] = 3;
    }

    public override void SetDefaults()
    {
      projectile.width = 32;
      projectile.height = 32;
      projectile.friendly = false;
      projectile.hostile = true;
      projectile.timeLeft = 600;
      projectile.light = .5f;
      projectile.ignoreWater = true;
      projectile.tileCollide = true;
    }

    public override bool PreAI()
    {
      if (life > expertMaxLife)
      {
        if (Main.expertMode) life = expertMaxLife;
        else life = normalMaxLife;
      }

      return true;
    }

    public override void AI()
    {
      if (++projectile.frameCounter >= 4)
      {
        projectile.frameCounter = 0;
        if (++projectile.frame >= Main.projFrames[projectile.type]) projectile.frame = 0;
      }

      SustainDamage();
    }

    public override void Kill(int timeLeft)
    {
      Main.PlaySound(SoundID.NPCHit1, projectile.Center);
    }

    public override string Texture => "ChensGradiusMod/Sprites/MoaiBubble";

    public override Color? GetAlpha(Color lightColor) => Color.White;

    private void SustainDamage()
    {
      for (int i = 0; i < Main.maxProjectiles; i++)
      {
        Projectile selectProj = Main.projectile[i];
        if (selectProj.active && selectProj.friendly && GradiusHelper.CanDamage(selectProj) &&
            projectile.Hitbox.Intersects(selectProj.Hitbox))
        {
          if (!selectProj.minion && !Main.projPet[selectProj.type] &&
              (selectProj.maxPenetrate <= life && selectProj.penetrate != -1))
          {
            GradiusHelper.ProjectileDestroy(selectProj);
          }
          else selectProj.maxPenetrate--;

          if (--life <= 0)
          {
            projectile.Kill();
            return;
          }
          else Main.PlaySound(SoundID.NPCHit1, projectile.Center);
        }
      }
    }
  }
}
