using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.Projectiles.Options
{
  public abstract class OptionBaseObject : ModProjectile
  {
    public const int DistanceInterval = 15;

    protected const int KeepAlive = 2;

    private const int MaxBuffer = 300;
    private List<int> playerAlreadyProducedProjectiles = new List<int>();
    private List<int> projectilesToProduce = new List<int>();
    private bool isSpawning = true;

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
      projectile.light = .1f;
      projectile.tileCollide = false;
      projectile.penetrate = -1;
      projectile.minion = true;
    }

    public override bool PreAI()
    {
      if (PlayerHasAccessory() &&
          GradiusHelper.OptionsPredecessorRequirement(ModOwner, Position))
      {
        if (PathListSize <= 0) ModOwner.optionFlightPath.Add(Owner.Center);
        projectile.timeLeft = KeepAlive;
        return true;
      }
      else
      {
        projectile.active = false;
        return false;
      }
    }

    public override void AI()
    {
      if (GradiusHelper.IsSameClientOwner(projectile))
      {
        for (int h = 0; h < playerAlreadyProducedProjectiles.Count; h++)
        {
          Projectile p = Main.projectile[playerAlreadyProducedProjectiles[h]];
          if (!p.active) playerAlreadyProducedProjectiles.RemoveAt(h--);
        }

        if (AttackLimitation())
        {
          for (int i = 0; i < Main.maxProjectiles; i++)
          {
            Projectile p = Main.projectile[i];
            if (p.active && OptionRules.IsAllowed(p.type) && IsNotAYoyo(p) && IsNotProducedYet(i) && !p.hostile && p.friendly &&
                !p.npcProj && GradiusHelper.CanDamage(p) && IsAbleToCrit(p) && !p.minion && !p.trap && IsSameOwner(p))
            {
              projectilesToProduce.Add(i);
            }
          }

          foreach (int prog_ind in projectilesToProduce)
          {
            Projectile p = Main.projectile[prog_ind];
            playerAlreadyProducedProjectiles.Add(prog_ind);

            int new_p_ind = Projectile.NewProjectile(ComputeOffset(Main.player[p.owner].Center, p.Center),
                                                     p.velocity, p.type, p.damage, p.knockBack,
                                                     projectile.owner, 0f, 0f);
            ModOwner.optionAlreadyProducedProjectiles.Add(new_p_ind);
            Main.projectile[new_p_ind].noDropItem = true;
          }
        }
      }

      OptionAnimate();

      projectile.Center = ModOwner.optionFlightPath[Math.Min(PathListSize - 1, FrameDistance)];

      OptionSpawnSoundEffect();
    }

    public override void PostAI()
    {
      if (GradiusHelper.IsSameClientOwner(projectile))
      {
        projectilesToProduce.Clear();
        projectilesToProduce = new List<int>();
        GradiusHelper.FreeListData(ref playerAlreadyProducedProjectiles, MaxBuffer);
      }
    }

    public override string Texture => "ChensGradiusMod/Sprites/OptionSheet";

    public override Color? GetAlpha(Color lightColor) => Color.White;

    public virtual int Position => 0;

    public virtual bool PlayerHasAccessory() => false;

    protected virtual int FrameSpeed => 5;

    protected virtual float[] LightValues { get; } = { .1f, .2f, .3f, .4f, .5f, .4f, .3f, .2f, .1f };

    protected virtual bool AttackLimitation() => true;

    protected Player Owner => Main.player[projectile.owner];

    protected GradiusModPlayer ModOwner => Owner.GetModPlayer<GradiusModPlayer>();

    protected void OptionAnimate()
    {
      if (++projectile.frameCounter >= FrameSpeed)
      {
        projectile.frameCounter = 0;
        if (++projectile.frame >= Main.projFrames[projectile.type]) projectile.frame = 0;
        projectile.light = LightValues[projectile.frame];
      }
    }

    protected void OptionSpawnSoundEffect()
    {
      if (isSpawning)
      {
        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Options/OptionSpawn"),
                       projectile.Center);
        isSpawning = false;
      }
    }

    private int FrameDistance => (DistanceInterval * Position) - 1;

    private int PathListSize => ModOwner.optionFlightPath.Count;

    private Vector2 ComputeOffset(Vector2 playPos, Vector2 projPos)
    {
      Vector2 projectileSpawn = new Vector2(projectile.Center.X, projectile.Center.Y);

      projectileSpawn.X += projPos.X - playPos.X;
      projectileSpawn.Y += projPos.Y - playPos.Y;

      return projectileSpawn;
    }

    private bool IsAbleToCrit(Projectile p) => p.melee || p.ranged || p.thrown || p.magic;

    private bool IsSameOwner(Projectile p) => p.owner == projectile.owner;

    private bool IsNotProducedYet(int ind)
    {
      if (HasProduced(ModOwner.optionAlreadyProducedProjectiles, ind)) return false;
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

    private bool IsNotAYoyo(Projectile p) => p.aiStyle != 99;
  }
}