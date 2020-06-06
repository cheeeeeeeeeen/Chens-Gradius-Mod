using ChensGradiusMod.Projectiles.Aliens;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod
{
  public static class OptionRules
  {
    private static readonly List<AlienProjectile> BannedTypes = new List<AlienProjectile>()
    {
      new AlienProjectile(ItemID.RainbowGun, ProjectileID.RainbowFront),
      new AlienProjectile(ItemID.RainbowGun, ProjectileID.RainbowBack),
      new AlienProjectile(ItemID.Vilethorn, ProjectileID.VilethornBase),
      new AlienProjectile(ItemID.Vilethorn, ProjectileID.VilethornTip),
      new AlienProjectile(ItemID.DD2BetsyBow, ProjectileID.DD2BetsyArrow),
      new AlienProjectile(ItemID.MushroomSpear, ProjectileID.Mushroom)
    };

    private static readonly List<AlienDamageType> SupportedDamageTypes = new List<AlienDamageType>()
    {
      new AlienDamageType("CalamityMod", "CalamityGlobalProjectile", "rogue")
    };

    private static readonly List<AlienProjectile> AllowedTypes = new List<AlienProjectile>()
    {
      new AlienProjectile(ItemID.OnyxBlaster, ProjectileID.BlackBolt),
      new AlienProjectile(ItemID.FlareGun, ProjectileID.Flare),
      new AlienProjectile(ItemID.FlareGun, ProjectileID.BlueFlare),
      new AlienProjectile(ItemID.Blowpipe, ProjectileID.Seed),
      new AlienProjectile(ItemID.Marrow, ProjectileID.BoneArrow),
      new AlienProjectile(ItemID.MoltenFury, ProjectileID.FireArrow),
      new AlienProjectile(ItemID.BoneGlove, ProjectileID.BoneGloveProj),
      new AlienProjectile(ItemID.Sandgun, ProjectileID.SandBallGun),
      new AlienProjectile(ItemID.Sandgun, ProjectileID.EbonsandBallGun),
      new AlienProjectile(ItemID.Sandgun, ProjectileID.PearlSandBallGun),
      new AlienProjectile(ItemID.Sandgun, ProjectileID.CrimsandBallGun),
      new AlienProjectile(ItemID.LaserMachinegun, ProjectileID.LaserMachinegunLaser),
      new AlienProjectile(ItemID.ChlorophyteSaber, ProjectileID.SporeCloud),
      new AlienProjectile(ItemID.ChlorophytePartisan, ProjectileID.SporeCloud),
      new AlienProjectile(ItemID.VortexBeater, ProjectileID.VortexBeaterRocket),
      new AlienProjectile(ItemID.IceBow, ProjectileID.FrostArrow),
      new AlienProjectile(ItemID.ChargedBlasterCannon, ProjectileID.ChargedBlasterOrb),
      new AlienProjectile(ItemID.MonkStaffT2, ProjectileID.MonkStaffT2Ghast),
      new AlienProjectile(ItemID.MonkStaffT3, ProjectileID.MonkStaffT3_AltShot),
      new AlienProjectile(ItemID.ChlorophyteClaymore, ProjectileID.ChlorophyteOrb),
      new AlienProjectile(ItemID.PulseBow, ProjectileID.PulseBolt),
      new AlienProjectile(ItemID.FireworksLauncher, ProjectileID.RocketFireworkRed),
      new AlienProjectile(ItemID.FireworksLauncher, ProjectileID.RocketFireworkGreen),
      new AlienProjectile(ItemID.FireworksLauncher, ProjectileID.RocketFireworkBlue),
      new AlienProjectile(ItemID.FireworksLauncher, ProjectileID.RocketFireworkYellow)
    };

    public static bool StandardFilter(Projectile p)
    {
      return RequiredFilter(p) && !p.hostile && p.friendly && !p.npcProj && CanDamage(p)
             && IsAbleToCrit(p) && !p.minion && !p.trap;
    }

    public static bool RequiredFilter(Projectile p)
    {
      return p.active && IsNotAYoyo(p);
    }

    public static bool ImportBanOptionRule(string modName, string weapName, string projName)
    {
      if (!BannedTypes.Exists(ap => modName == ap.modName && weapName == ap.weaponName
                                    && projName == ap.projectileName))
      {
        BannedTypes.Add(new AlienProjectile(modName, weapName, projName));
        return true;
      }

      return false;
    }

    public static bool ImportBanOptionRule(int wType, int pType)
    {
      if (!BannedTypes.Exists(ap => ap.modName == "Terraria" && wType == ap.weaponType
                                    && pType == ap.projectileType))
      {
        BannedTypes.Add(new AlienProjectile(wType, pType));
        return true;
      }

      return false;
    }

    public static bool ImportAllowOptionRule(string modName, string weapName, string projName)
    {
      if (!AllowedTypes.Exists(ap => modName == ap.modName && weapName == ap.weaponName
                                    && projName == ap.projectileName))
      {
        AllowedTypes.Add(new AlienProjectile(modName, weapName, projName));
        return true;
      }

      return false;
    }

    public static bool ImportAllowOptionRule(int wType, int pType)
    {
      if (!AllowedTypes.Exists(ap => ap.modName == "Terraria" && wType == ap.weaponType
                                    && pType == ap.projectileType))
      {
        AllowedTypes.Add(new AlienProjectile(wType, pType));
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

    public static bool ImportDamageType(string modName, string damageType)
    {
      if (!SupportedDamageTypes.Exists(sdt => modName == sdt.modName
                                              && damageType == sdt.damageType))
      {
        AlienDamageType alienDamageType = new AlienDamageType(modName, damageType);
        SupportedDamageTypes.Add(alienDamageType);
        return true;
      }

      return false;
    }

    public static bool IsBanned(Item w, Projectile p) => IsBanned(w.type, p.type);

    public static bool IsBanned(int wType, int pType)
    {
      foreach (AlienProjectile ap in BannedTypes)
      {
        if (ap.CheckType(wType, pType)) return true;
      }

      return false;
    }

    public static bool IsAllowed(Item w, Projectile p) => IsAllowed(w.type, p.type);

    public static bool IsAllowed(int wType, int pType)
    {
      foreach (AlienProjectile ap in AllowedTypes)
      {
        if (ap.CheckType(wType, pType)) return true;
      }

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