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
    private const int KeepAlive = 2;
    private const int MaxBuffer = 300;

    private List<int> playerAlreadyProducedProjectiles = new List<int>();
    private List<int> projectilesToProduce = new List<int>();
    private readonly float[] lightValues = { .1f, .2f, .3f, .4f, .5f, .4f, .3f, .2f, .1f };
    private bool isSpawning = true;

    public static int distanceInterval = 15;

    protected Player Owner => Main.player[projectile.owner];

    protected GradiusModPlayer ModOwner => Owner.GetModPlayer<GradiusModPlayer>();

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

        for (int i = 0; i < Main.maxProjectiles; i++)
        {
          Projectile p = Main.projectile[i];
          if (p.active && FollowsRules(p) && IsNotAYoyo(p) && IsNotProducedYet(i) && !p.hostile && p.friendly &&
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

      if (++projectile.frameCounter >= 5)
      {
        projectile.frameCounter = 0;
        if (++projectile.frame >= Main.projFrames[projectile.type]) projectile.frame = 0;
        projectile.light = lightValues[projectile.frame];
      }

      projectile.Center = ModOwner.optionFlightPath[Math.Min(PathListSize - 1, FrameDistance)];
      if (isSpawning)
      {
        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Options/OptionSpawn"),
                       projectile.Center);
        isSpawning = false;
      }
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

    private int FrameDistance => (distanceInterval * Position) - 1;

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

    private bool FollowsRules(Projectile p) => VanillaRules(p) && ModRules(p);

    private bool VanillaRules(Projectile p)
    {
      return p.type != ProjectileID.CrystalVileShardHead &&
             p.type != ProjectileID.CrystalVileShardShaft &&
             p.type != ProjectileID.VilethornBase &&
             p.type != ProjectileID.VilethornTip &&
             p.type != ProjectileID.Bee &&
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
             p.type != ProjectileID.MolotovFire &&
             p.type != ProjectileID.MolotovFire2 &&
             p.type != ProjectileID.MolotovFire3 &&
             p.type != ProjectileID.PhantasmArrow &&
             p.type != ProjectileID.Phantasm;
    }

    public bool ModRules(Projectile p)
    {
      Mod selectMod;
      bool result = true;

      selectMod = ModLoader.GetMod("CrystiliumMod");
      if (selectMod != null)
      {
        result = result && p.type != selectMod.ProjectileType("DiamondExplosion")
                        && p.type != selectMod.ProjectileType("DiamondBomb")
                        && p.type != selectMod.ProjectileType("Shatter1")
                        && p.type != selectMod.ProjectileType("Shatter2")
                        && p.type != selectMod.ProjectileType("Shatter3");
      }

      selectMod = ModLoader.GetMod("CalamityMod");
      if (selectMod != null)
      {
        result = result && p.type != selectMod.ProjectileType("ClamorRifleProjSplit")
                        && p.type != selectMod.ProjectileType("GleamingBolt2")
                        && p.type != selectMod.ProjectileType("SandyWaifuShark")
                        && p.type != selectMod.ProjectileType("ChargedBlast2")
                        && p.type != selectMod.ProjectileType("FuckYou")
                        && p.type != selectMod.ProjectileType("FossilShard")
                        && p.type != selectMod.ProjectileType("SHPExplosion")
                        && p.type != selectMod.ProjectileType("OMGWTH")
                        && p.type != selectMod.ProjectileType("BrimstoneBeam2")
                        && p.type != selectMod.ProjectileType("BrimstoneBeam3")
                        && p.type != selectMod.ProjectileType("BrimstoneBeam4")
                        && p.type != selectMod.ProjectileType("BrimstoneBeam5")
                        && p.type != selectMod.ProjectileType("WaterStream2")
                        && p.type != selectMod.ProjectileType("Shaderain")
                        && p.type != selectMod.ProjectileType("ChickenExplosion")
                        && p.type != selectMod.ProjectileType("AquashardSplit")
                        && p.type != selectMod.ProjectileType("ManaBoltSmall")
                        && p.type != selectMod.ProjectileType("ManaBoltSmall2")
                        && p.type != selectMod.ProjectileType("FungiOrb2")
                        && p.type != selectMod.ProjectileType("SerpentineHead")
                        && p.type != selectMod.ProjectileType("SerpentineBody")
                        && p.type != selectMod.ProjectileType("SerpentineTail")
                        && p.type != selectMod.ProjectileType("Flash")
                        && p.type != selectMod.ProjectileType("TerraBulletSplit")
                        && p.type != selectMod.ProjectileType("TerraArrow2")
                        && p.type != selectMod.ProjectileType("VanquisherArrow2");
      }

      selectMod = ModLoader.GetMod("Bluemagic");
      if (selectMod != null)
      {
        result = result && p.type != selectMod.ProjectileType("PuriumArrowTrail");
      }

      selectMod = ModLoader.GetMod("ThoriumMod");
      if (selectMod != null)
      {
        result = result && p.type != selectMod.ProjectileType("StalagmitePro")
                        && p.type != selectMod.ProjectileType("StalagmiteSpawnerR")
                        && p.type != selectMod.ProjectileType("StalagmiteSpike")
                        && p.type != selectMod.ProjectileType("ReactionFire")
                        && p.type != selectMod.ProjectileType("HotPotFlame")
                        && p.type != selectMod.ProjectileType("NightStar2")
                        && p.type != selectMod.ProjectileType("BoomPlasma")
                        && p.type != selectMod.ProjectileType("BoomNitrogen")
                        && p.type != selectMod.ProjectileType("BoomCombustion")
                        && p.type != selectMod.ProjectileType("BoomAphrodisiac")
                        && p.type != selectMod.ProjectileType("BoomCorrosive")
                        && p.type != selectMod.ProjectileType("DemonExplosion")
                        && p.type != selectMod.ProjectileType("CrystalBalloonPro2")
                        && p.type != selectMod.ProjectileType("CorrupterBalloonPro2")
                        && p.type != selectMod.ProjectileType("FesteringBalloonPro2")
                        && p.type != selectMod.ProjectileType("NightStaffPro2")
                        && p.type != selectMod.ProjectileType("JellyPro")
                        && p.type != selectMod.ProjectileType("PalladiumBoltShatter")
                        && p.type != selectMod.ProjectileType("BuriedArrowFireBoom")
                        && p.type != selectMod.ProjectileType("BuriedMagicPopPro")
                        && p.type != selectMod.ProjectileType("MorelPoof")
                        && p.type != selectMod.ProjectileType("ChumShark")
                        && p.type != selectMod.ProjectileType("FungalPopperPro2")
                        && p.type != selectMod.ProjectileType("SpikeBombBoom")
                        && p.type != selectMod.ProjectileType("InfernoStaffPro2")
                        && p.type != selectMod.ProjectileType("HighTidePro2")
                        && p.type != selectMod.ProjectileType("SeedBombPoof")
                        && p.type != selectMod.ProjectileType("SpineBreaker2")
                        && p.type != selectMod.ProjectileType("SpineBreaker1Dummy")
                        && p.type != selectMod.ProjectileType("WebGunPro2")
                        && p.type != selectMod.ProjectileType("MeteoriteClusterBombPro2")
                        && p.type != selectMod.ProjectileType("JunglesWrathPro2")
                        && p.type != selectMod.ProjectileType("GorganGaze")
                        && p.type != selectMod.ProjectileType("Spud2")
                        && p.type != selectMod.ProjectileType("OmniBoom")
                        && p.type != selectMod.ProjectileType("OmniBurst")
                        && p.type != selectMod.ProjectileType("OmniBurstDamage");
      }

      selectMod = ModLoader.GetMod("SpiritMod");
      if (selectMod != null)
      {
        result = result && p.type != selectMod.ProjectileType("SoulRune")
                        && p.type != selectMod.ProjectileType("IchorP")
                        && p.type != selectMod.ProjectileType("Fae")
                        && p.type != selectMod.ProjectileType("SadBeam")
                        && p.type != selectMod.ProjectileType("StarTrail1")
                        && p.type != selectMod.ProjectileType("GraniteShard1")
                        && p.type != selectMod.ProjectileType("CoilMine")
                        && p.type != selectMod.ProjectileType("AbyssalSludge")
                        && p.type != selectMod.ProjectileType("WitherShard3")
                        && p.type != selectMod.ProjectileType("HarpyFeather");
      }

      return result;
    }
  }
}
