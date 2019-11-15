using ChensGradiusMod.Projectiles.Aliens;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace ChensGradiusMod
{
  public static class OptionRules
  {
    private static readonly List<int> VanillaRules = new List<int>()
    {
      ProjectileID.CrystalVileShardHead, ProjectileID.CrystalVileShardShaft, ProjectileID.VilethornBase, ProjectileID.VilethornTip,
      ProjectileID.Bee, ProjectileID.GiantBee, ProjectileID.RainbowBack, ProjectileID.CrystalPulse2, ProjectileID.ToxicCloud,
      ProjectileID.ToxicCloud2, ProjectileID.ToxicCloud3, ProjectileID.IceBlock, ProjectileID.CrystalShard, ProjectileID.HallowStar,
      ProjectileID.RainFriendly, ProjectileID.BloodRain, ProjectileID.FlowerPowPetal, ProjectileID.TinyEater,
      ProjectileID.NorthPoleSnowflake, ProjectileID.FlaironBubble, ProjectileID.TerrarianBeam, ProjectileID.VortexBeater,
      ProjectileID.NebulaArcanumSubshot, ProjectileID.NebulaArcanumExplosionShot, ProjectileID.NebulaArcanumExplosionShotShard,
      ProjectileID.MoonlordArrowTrail, ProjectileID.DD2PhoenixBow, ProjectileID.MonkStaffT3_AltShot, ProjectileID.MonkStaffT3,
      ProjectileID.MonkStaffT3_Alt, ProjectileID.Electrosphere, ProjectileID.Xenopopper, ProjectileID.MolotovFire,
      ProjectileID.MolotovFire2, ProjectileID.MolotovFire3, ProjectileID.NettleBurstLeft, ProjectileID.NettleBurstEnd,
      ProjectileID.StyngerShrapnel, ProjectileID.SeedlerThorn, ProjectileID.DD2BetsyArrow, ProjectileID.PhantasmArrow,
      ProjectileID.Phantasm, ProjectileID.VortexLightning, ProjectileID.LightDisc
    };

    private static readonly List<AlienProjectile> ModRules = new List<AlienProjectile>()
    {
      new AlienProjectile("CrystiliumMod", "DiamondExplosion"), new AlienProjectile("CrystiliumMod", "DiamondBomb"),
      new AlienProjectile("CrystiliumMod", "Shatter1"), new AlienProjectile("CrystiliumMod", "Shatter2"),
      new AlienProjectile("CrystiliumMod", "Shatter3"),

      new AlienProjectile("CalamityMod", "Judgement"), new AlienProjectile("CalamityMod", "Whiterain"),
      new AlienProjectile("CalamityMod", "WhiteOrb"), new AlienProjectile("CalamityMod", "PlagueExplosionFriendly"),
      new AlienProjectile("CalamityMod", "BeamingBolt2"), new AlienProjectile("CalamityMod", "InfernalBlade2"),
      new AlienProjectile("CalamityMod", "FlameBeamTip"), new AlienProjectile("CalamityMod", "FlameBeamTip2"),
      new AlienProjectile("CalamityMod", "BlueBubble"), new AlienProjectile("CalamityMod", "Shockblast"),
      new AlienProjectile("CalamityMod", "ClamorRifleProjSplit"), new AlienProjectile("CalamityMod", "GleamingBolt2"),
      new AlienProjectile("CalamityMod", "SandyWaifuShark"), new AlienProjectile("CalamityMod", "ChargedBlast2"),
      new AlienProjectile("CalamityMod", "FuckYou"), new AlienProjectile("CalamityMod", "FossilShard"),
      new AlienProjectile("CalamityMod", "SHPExplosion"), new AlienProjectile("CalamityMod", "OMGWTH"),
      new AlienProjectile("CalamityMod", "BrimstoneBeam2"), new AlienProjectile("CalamityMod", "BrimstoneBeam3"),
      new AlienProjectile("CalamityMod", "BrimstoneBeam4"), new AlienProjectile("CalamityMod", "BrimstoneBeam5"),
      new AlienProjectile("CalamityMod", "WaterStream2"), new AlienProjectile("CalamityMod", "Shaderain"),
      new AlienProjectile("CalamityMod", "ChickenExplosion"), new AlienProjectile("CalamityMod", "AquashardSplit"),
      new AlienProjectile("CalamityMod", "ManaBoltSmall"), new AlienProjectile("CalamityMod", "ManaBoltSmall2"),
      new AlienProjectile("CalamityMod", "FungiOrb2"), new AlienProjectile("CalamityMod", "SerpentineHead"),
      new AlienProjectile("CalamityMod", "SerpentineBody"), new AlienProjectile("CalamityMod", "SerpentineTail"),
      new AlienProjectile("CalamityMod", "Flash"), new AlienProjectile("CalamityMod", "TerraBulletSplit"),
      new AlienProjectile("CalamityMod", "TerraArrow2"), new AlienProjectile("CalamityMod", "VanquisherArrow2"),
      new AlienProjectile("CalamityMod", "TinyCrystal"), new AlienProjectile("CalamityMod", "Needler"),
      new AlienProjectile("CalamityMod", "SpatialSpear2"), new AlienProjectile("CalamityMod", "SpatialSpear4"),
      new AlienProjectile("CalamityMod", "SpatialSpear3"), new AlienProjectile("CalamityMod", "Celestus2"),
      new AlienProjectile("CalamityMod", "Brimblade2"),

      new AlienProjectile("Bluemagic", "PuriumArrowTrail"),

      new AlienProjectile("ThoriumMod", "TitaniumStaffPro2"), new AlienProjectile("ThoriumMod", "NightStar2"),
      new AlienProjectile("ThoriumMod", "NightStarTrue2"), new AlienProjectile("ThoriumMod", "ShusSandstorm"),
      new AlienProjectile("ThoriumMod", "Shroomy"), new AlienProjectile("ThoriumMod", "ChlorophyteCloud"),
      new AlienProjectile("ThoriumMod", "AdamantiteStaffPro2"), new AlienProjectile("ThoriumMod", "EarthenSpawner"),
      new AlienProjectile("ThoriumMod", "EarthenCascade"), new AlienProjectile("ThoriumMod", "EarthenSpawnerR"),
      new AlienProjectile("ThoriumMod", "BlackHole"), new AlienProjectile("ThoriumMod", "LegionOrnamentShard"),
      new AlienProjectile("ThoriumMod", "ClockWorkBombPro1"), new AlienProjectile("ThoriumMod", "ClockWorkBombPro2"),
      new AlienProjectile("ThoriumMod", "ClockWorkBombPro3"), new AlienProjectile("ThoriumMod", "StalagmitePro"),
      new AlienProjectile("ThoriumMod", "StalagmiteSpawnerR"),  new AlienProjectile("ThoriumMod", "StalagmiteSpike"),
      new AlienProjectile("ThoriumMod", "ReactionFire"), new AlienProjectile("ThoriumMod", "HotPotFlame"),
      new AlienProjectile("ThoriumMod", "NightStar2"), new AlienProjectile("ThoriumMod", "BoomPlasma"),
      new AlienProjectile("ThoriumMod", "BoomNitrogen"), new AlienProjectile("ThoriumMod", "BoomCombustion"),
      new AlienProjectile("ThoriumMod", "BoomAphrodisiac"), new AlienProjectile("ThoriumMod", "BoomCorrosive"),
      new AlienProjectile("ThoriumMod", "DemonExplosion"), new AlienProjectile("ThoriumMod", "CrystalBalloonPro2"),
      new AlienProjectile("ThoriumMod", "CorrupterBalloonPro2"), new AlienProjectile("ThoriumMod", "FesteringBalloonPro2"),
      new AlienProjectile("ThoriumMod", "NightStaffPro2"), new AlienProjectile("ThoriumMod", "JellyPro"),
      new AlienProjectile("ThoriumMod", "PalladiumBoltShatter"), new AlienProjectile("ThoriumMod", "BuriedArrowFireBoom"),
      new AlienProjectile("ThoriumMod", "BuriedMagicPopPro"), new AlienProjectile("ThoriumMod", "MorelPoof"),
      new AlienProjectile("ThoriumMod", "ChumShark"), new AlienProjectile("ThoriumMod", "FungalPopperPro2"),
      new AlienProjectile("ThoriumMod", "SpikeBombBoom"), new AlienProjectile("ThoriumMod", "InfernoStaffPro2"),
      new AlienProjectile("ThoriumMod", "HighTidePro2"), new AlienProjectile("ThoriumMod", "SeedBombPoof"),
      new AlienProjectile("ThoriumMod", "SpineBreaker2"), new AlienProjectile("ThoriumMod", "SpineBreaker1Dummy"),
      new AlienProjectile("ThoriumMod", "WebGunPro2"), new AlienProjectile("ThoriumMod", "MeteoriteClusterBombPro2"),
      new AlienProjectile("ThoriumMod", "JunglesWrathPro2"), new AlienProjectile("ThoriumMod", "GorganGaze"),
      new AlienProjectile("ThoriumMod", "Spud2"), new AlienProjectile("ThoriumMod", "OmniBoom"),
      new AlienProjectile("ThoriumMod", "OmniBurst"), new AlienProjectile("ThoriumMod", "OmniBurstDamage"),
      new AlienProjectile("ThoriumMod", "LaunchJumperPro2"), new AlienProjectile("ThoriumMod", "SuperPlasmaCannonPro2"),
      new AlienProjectile("ThoriumMod", "SuperPlasmaCannonPro3"), new AlienProjectile("ThoriumMod", "SuperPlasmaCannonPro4"),
      new AlienProjectile("ThoriumMod", "SuperPlasmaCannonPro00"), new AlienProjectile("ThoriumMod", "ShadowPurgeCaltropPro2"),
      new AlienProjectile("ThoriumMod", "OmniArrow3"), new AlienProjectile("ThoriumMod", "AncientFireExplosion"),
      new AlienProjectile("ThoriumMod", "BudBombPro2"), new AlienProjectile("ThoriumMod", "BudBombPro3"),
      new AlienProjectile("ThoriumMod", "PharaohsSlabPro2"),

      new AlienProjectile("SpiritMod", "FieryAura"), new AlienProjectile("SpiritMod", "DuskAura"),
      new AlienProjectile("SpiritMod", "FireBolt"), new AlienProjectile("SpiritMod", "GeodeStaveProjectile"),
      new AlienProjectile("SpiritMod", "AquaBolt"), new AlienProjectile("SpiritMod", "AquaFlareProj"),
      new AlienProjectile("SpiritMod", "QuicksilverBolt1"), new AlienProjectile("SpiritMod", "BlueEmber"),
      new AlienProjectile("SpiritMod", "ChaosB"), new AlienProjectile("SpiritMod", "CogW"),
      new AlienProjectile("SpiritMod", "GrassPortal"), new AlienProjectile("SpiritMod", "AdamantiteStaffProj2"),
      new AlienProjectile("SpiritMod", "BloodVessel"), new AlienProjectile("SpiritMod", "Blood3"),
      new AlienProjectile("SpiritMod", "Fire"), new AlienProjectile("SpiritMod", "PhoenixMinion"),
      new AlienProjectile("SpiritMod", "PrimeLaser"), new AlienProjectile("SpiritMod", "PrimeVice"),
      new AlienProjectile("SpiritMod", "SkellyP"), new AlienProjectile("SpiritMod", "PrimeOther"),
      new AlienProjectile("SpiritMod", "PrimeSaw"), new AlienProjectile("SpiritMod", "OrichHoming"),
      new AlienProjectile("SpiritMod", "SpiritLinger"), new AlienProjectile("SpiritMod", "TitaniumStaffProj2"),
      new AlienProjectile("SpiritMod", "IchorBomb"), new AlienProjectile("SpiritMod", "SpiritBoom"),
      new AlienProjectile("SpiritMod", "GhostJellyBombProj"), new AlienProjectile("SpiritMod", "Blaze"),
      new AlienProjectile("SpiritMod", "SoulRune"), new AlienProjectile("SpiritMod", "IchorP"),
      new AlienProjectile("SpiritMod", "Fae"), new AlienProjectile("SpiritMod", "SadBeam"),
      new AlienProjectile("SpiritMod", "StarTrail1"),  new AlienProjectile("SpiritMod", "GraniteShard1"),
      new AlienProjectile("SpiritMod", "CoilMine"), new AlienProjectile("SpiritMod", "AbyssalSludge"),
      new AlienProjectile("SpiritMod", "WitherShard3"), new AlienProjectile("SpiritMod", "HarpyFeather"),
      new AlienProjectile("SpiritMod", "AmberSlasher"), new AlienProjectile("SpiritMod", "QuicksilverBolt")
    };

    private static readonly List<AlienDamageType> SupportedDamageTypes = new List<AlienDamageType>()
    {
      new AlienDamageType("CalamityMod", "CalamityGlobalProjectile", "rogue")
    };

    public static bool CompleteRuleCheck(Projectile p)
    {
      return p.active && IsAllowed(p.type) && IsNotAYoyo(p) && !p.hostile && p.friendly &&
             !p.npcProj && GradiusHelper.CanDamage(p) && IsAbleToCrit(p) && !p.minion && !p.trap;
    }

    public static bool? ImportOptionRule(string modName, string projName)
    {
      if (!ModRules.Exists(ap => modName == ap.modName && projName == ap.projectileName))
      {
        AlienProjectile alienProjectile = new AlienProjectile(modName, projName);
        ModRules.Add(alienProjectile);
        return true;
      }

      return false;
    }

    public static bool? ImportOptionRule(int pType)
    {
      if (!VanillaCheck(pType))
      {
        VanillaRules.Add(pType);
        return true;
      }

      return false;
    }

    public static bool ImportDamageType(string modName, string internalName, string damageType)
    {
      if (!SupportedDamageTypes.Exists(sdt => modName == sdt.modName
                                              && internalName == sdt.internalName
                                              && damageType == sdt.damageType))
      {
        AlienDamageType alienDamageType = new AlienDamageType(modName, internalName, damageType);
        SupportedDamageTypes.Add(alienDamageType);
        return true;
      }

      return false;
    }

    public static bool IsBanned(int pType) => VanillaCheck(pType) || ModCheck(pType);

    public static bool IsAllowed(int pType) => !IsBanned(pType);

    private static bool VanillaCheck(int pType) => VanillaRules.Contains(pType);

    private static bool ModCheck(int pType)
    {
      foreach (AlienProjectile ap in ModRules) if (ap.CheckType(pType)) return true;

      return false;
    }

    private static bool IsNotAYoyo(Projectile p) => p.aiStyle != 99;

    private static bool IsAbleToCrit(Projectile p) => p.melee || p.ranged || p.thrown || p.magic
                                                      || IsDamageTypeSupported(p.whoAmI);

    private static bool IsDamageTypeSupported(int pWhoAmI)
    {
      foreach (AlienDamageType aDamageType in SupportedDamageTypes)
      {
        if (aDamageType.IsMatchingDamageType(pWhoAmI)) return true;
      }

      return false;
    }
  }
}