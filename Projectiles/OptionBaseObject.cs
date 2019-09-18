using System;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.Projectiles
{
  public abstract class OptionBaseObject : ModProjectile
  {
    private readonly string optionTexture = "ChensGradiusMod/Projectiles/OptionBaseObject";

    public override void SetStaticDefaults()
    {
      Main.projFrames[projectile.type] = 1;
      Main.projPet[projectile.type] = true;
    }

    public override void SetDefaults()
    {
      projectile.netImportant = true;
      projectile.width = 24;
      projectile.height = 12;
      projectile.friendly = true;
      projectile.penetrate = -1;
    }

    public override void AI()
    {
      int listSize = ModOwner.optionFlightPath.Count;
      if (ModOwner.optionOne && listSize > 0) projectile.position = ModOwner.optionFlightPath[Math.Min(listSize - 1, FrameDistance)];
      else projectile.Kill();
    }

    public override string Texture => optionTexture;

    public virtual int FrameDistance => 14;

    private GradiusModPlayer ModOwner => Main.player[projectile.owner].GetModPlayer<GradiusModPlayer>();
  }
}
