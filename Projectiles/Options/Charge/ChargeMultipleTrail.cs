using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.Projectiles.Options.Charge
{
  public class ChargeMultipleTrail : ModProjectile
  {
    private readonly int dustId = 159;
    private readonly int dustRate = 5;
    private int dustTick = 0;

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Charged Multiple");
      Main.projFrames[projectile.type] = 1;
    }

    public override void SetDefaults()
    {
      projectile.netImportant = true;
      projectile.width = 16;
      projectile.height = 16;
      projectile.light = 1f;
      projectile.friendly = true;
      projectile.hostile = false;
      projectile.tileCollide = false;
      projectile.penetrate = -1;
      projectile.usesLocalNPCImmunity = true;
      projectile.localNPCHitCooldown = 10;
      projectile.timeLeft = 15;
    }

    public override string Texture => "ChensGradiusMod/Sprites/ChargeMultipleTrail";

    public override void AI()
    {
      if (++projectile.frameCounter >= 5)
      {
        projectile.frameCounter = 0;
        if (++projectile.frame >= Main.projFrames[projectile.type]) projectile.frame = 0;
      }

      if (++dustTick >= dustRate)
      {
        dustTick = 0;
        Dust.NewDust(projectile.position, projectile.width, projectile.height, dustId);
      }
    }
  }
}
