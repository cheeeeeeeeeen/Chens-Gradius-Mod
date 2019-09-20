using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.Projectiles
{
  public abstract class OptionBaseObject : ModProjectile
  {
    private const int MaxBuffer = 300;

    private readonly string optionTexture = "ChensGradiusMod/Sprites/OptionSheet";
    private readonly List<int> playerAlreadyProducedProjectiles = new List<int>();
    private List<int> projectilesToProduce = new List<int>();

    public GradiusModPlayer ModOwner => Main.player[projectile.owner].GetModPlayer<GradiusModPlayer>();

    public override void SetStaticDefaults()
    {
      Main.projFrames[projectile.type] = 9;
      Main.projPet[projectile.type] = true;
    }

    public override void SetDefaults()
    {
      projectile.netImportant = true;
      projectile.width = 28;
      projectile.height = 20;
      projectile.light = .5f;
    }

    public override bool PreAI()
    {
      if (ModOwner.optionOne &&
          GradiusHelper.OptionsPredecessorRequirement(ModOwner, Position) &&
          ListSize > 0) return true;
      else
      {
        projectile.Kill();
        return false;
      }
    }

    public override void AI()
    {
      for (int h = 0; h < playerAlreadyProducedProjectiles.Count; h++)
      {
        Projectile p = Main.projectile[playerAlreadyProducedProjectiles[h]];
        if (!p.active) playerAlreadyProducedProjectiles.RemoveAt(h--);
      }

      for (int i = 0; i < Main.maxProjectiles; i++)
      {
        Projectile p = Main.projectile[i];
        if (p.active && IsNotProducedYet(i) && !p.hostile && p.friendly && !p.npcProj &&
            CanDamage(p) && IsAbleToCrit(p) && !p.melee && !p.minion && !p.trap && IsSameOwner(p))
        {
          projectilesToProduce.Add(i);
        }
      }

      if (++projectile.frameCounter >= 5)
      {
        projectile.frameCounter = 0;
        if (++projectile.frame >= 9) projectile.frame = 0;
      }

      projectile.position = ModOwner.optionFlightPath[Math.Min(ListSize - 1, FrameDistance)];

      foreach (int prog_ind in projectilesToProduce)
      {
        Projectile p = Main.projectile[prog_ind];
        playerAlreadyProducedProjectiles.Add(prog_ind);

        int new_p_ind = Projectile.NewProjectile(projectile.Center, p.velocity, p.type, p.damage, p.knockBack, projectile.owner, 0f, 0f);
        OptionAlreadyProducedProjectiles.Add(new_p_ind);
        Main.projectile[new_p_ind].noDropItem = true;
        // Main.projectile[new_p_ind].usesIDStaticNPCImmunity = true;
        // Main.projectile[new_p_ind].idStaticNPCHitCooldown = 0;
      }
    }

    public override void PostAI()
    {
      projectilesToProduce.Clear();
      projectilesToProduce = new List<int>();
      GradiusHelper.FreeListData(playerAlreadyProducedProjectiles, MaxBuffer);
    }

    public override string Texture => optionTexture;

    public virtual int FrameDistance => 14;

    public virtual int Position => 1;

    private int ListSize => ModOwner.optionFlightPath.Count;

    private List<int> OptionAlreadyProducedProjectiles => ModOwner.optionAlreadyProducedProjectiles;

    private bool IsAbleToCrit(Projectile p) => p.ranged || p.thrown || p.magic;

    private bool IsSameOwner(Projectile p) => p.owner == projectile.owner;

    private bool IsNotProducedYet(int ind)
    {
      if (HasProduced(OptionAlreadyProducedProjectiles, ind)) return false;
      if (HasProduced(playerAlreadyProducedProjectiles, ind)) return false;

      return true;
    }

    private bool HasProduced(List<int> list, int ind)
    {
      foreach (int alreadyProducedInd in list)
      {
        if (alreadyProducedInd == ind) return true;
      }

      return false;
    }

    private bool CanDamage(Projectile p) => p.damage > 0;
  }
}
