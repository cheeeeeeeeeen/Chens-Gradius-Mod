using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Projectiles.Options
{
  public abstract class OptionBaseObject : ModProjectile
  {
    private const int KeepAlive = 5;
    private const int MaxBuffer = 300;

    private readonly string optionTexture = "ChensGradiusMod/Sprites/OptionSheet";
    private readonly List<int> playerAlreadyProducedProjectiles = new List<int>();
    private List<int> projectilesToProduce = new List<int>();
    private readonly float[] lightValues = { .1f, .2f, .3f, .4f, .5f, .4f, .3f, .2f, .1f};
    private bool isSpawning = true;

    protected GradiusModPlayer ModOwner => Main.player[projectile.owner].GetModPlayer<GradiusModPlayer>();

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
    }

    public override bool PreAI()
    {
      if (PlayerHasAccessory() &&
          GradiusHelper.OptionsPredecessorRequirement(ModOwner, Position) &&
          ListSize > 0)
      {
        projectile.timeLeft = KeepAlive;
        return true;
      }
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
        if (p.active && FollowsRules(p) && IsNotAYoyo(p) && IsNotProducedYet(i) && !p.hostile && p.friendly &&
            !p.npcProj && GradiusHelper.CanDamage(p) && IsAbleToCrit(p) && !p.minion && !p.trap && IsSameOwner(p))
        {
          projectilesToProduce.Add(i);
        }
      }

      if (++projectile.frameCounter >= 5)
      {
        projectile.frameCounter = 0;
        if (++projectile.frame >= Main.projFrames[projectile.type]) projectile.frame = 0;
        projectile.light = lightValues[projectile.frame];
      }

      projectile.Center = ModOwner.optionFlightPath[Math.Min(ListSize - 1, FrameDistance)];
      if (isSpawning)
      {
        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Options/OptionSpawn"),
                       projectile.Center);
        isSpawning = false;
      }

      foreach (int prog_ind in projectilesToProduce)
      {
        Projectile p = Main.projectile[prog_ind];
        playerAlreadyProducedProjectiles.Add(prog_ind);

        int new_p_ind = Projectile.NewProjectile(ComputeOffset(Main.player[p.owner].Center, p.Center),
                                                 p.velocity, p.type, p.damage, p.knockBack,
                                                 projectile.owner, 0f, 0f);
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

    public override Color? GetAlpha(Color lightColor) => Color.White;

    public virtual int FrameDistance => 0;

    public virtual int Position => 0;

    public virtual bool PlayerHasAccessory() => false;

    private int ListSize => ModOwner.optionFlightPath.Count;

    private Vector2 ComputeOffset(Vector2 playPos, Vector2 projPos)
    {
      Vector2 projectileSpawn = new Vector2(projectile.Center.X, projectile.Center.Y);

      projectileSpawn.X += projPos.X - playPos.X;
      projectileSpawn.Y += projPos.Y - playPos.Y;

      return projectileSpawn;
    }

    private List<int> OptionAlreadyProducedProjectiles => ModOwner.optionAlreadyProducedProjectiles;

    private bool IsAbleToCrit(Projectile p) => p.melee || p.ranged || p.thrown || p.magic;

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

    private bool IsNotAYoyo(Projectile p) => p.aiStyle != 99;

    private bool FollowsRules(Projectile p) => VanillaRules(p) && ModRules(p);

    private bool VanillaRules(Projectile p)
    {
      return p.type != ProjectileID.Bee &&
             p.type != ProjectileID.GiantBee &&
             p.type != ProjectileID.RainbowBack &&
             p.type != ProjectileID.CrystalPulse2 &&
             p.type != ProjectileID.ToxicCloud &&
             p.type != ProjectileID.ToxicCloud2 &&
             p.type != ProjectileID.ToxicCloud3 &&
             p.type != ProjectileID.IceBlock &&
             p.type != ProjectileID.CrystalShard &&
             p.type != ProjectileID.HallowStar &&
             p.type != ProjectileID.RainFriendly &&
             p.type != ProjectileID.BloodRain &&
             p.type != ProjectileID.FlowerPowPetal &&
             p.type != ProjectileID.TinyEater &&
             p.type != ProjectileID.NorthPoleSnowflake &&
             p.type != ProjectileID.FlaironBubble &&
             p.type != ProjectileID.TerrarianBeam &&
             p.type != ProjectileID.VortexBeater && // This is not really a ban, but a fix.
             p.type != ProjectileID.NebulaArcanumSubshot &&
             p.type != ProjectileID.NebulaArcanumExplosionShot &&
             p.type != ProjectileID.NebulaArcanumExplosionShotShard &&
             p.type != ProjectileID.MoonlordArrowTrail &&
             p.type != ProjectileID.DD2PhoenixBow && // This is not really a ban, but a fix.
             p.type != ProjectileID.MonkStaffT3_AltShot &&
             p.type != ProjectileID.MonkStaffT3 &&
             p.type != ProjectileID.MonkStaffT3_Alt &&
             p.type != ProjectileID.Electrosphere &&
             p.type != ProjectileID.Xenopopper &&
             p.type != ProjectileID.Phantasm;
    }

    public bool ModRules(Projectile p)
    {
      bool result = true;

      Mod crystilium = ModLoader.GetMod("CrystiliumMod");
      if (crystilium != null)
      {
        result = result && p.type != crystilium.ProjectileType("Shatter1")
                        && p.type != crystilium.ProjectileType("Shatter2")
                        && p.type != crystilium.ProjectileType("Shatter3");
      }

      return result;
    }
  }
}
