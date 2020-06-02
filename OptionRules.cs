using ChensGradiusMod.Projectiles.Aliens;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod
{
  public static class OptionRules
  {
    private static readonly List<AlienProjectile> BannedTypes = new List<AlienProjectile>();

    private static readonly List<AlienDamageType> SupportedDamageTypes = new List<AlienDamageType>()
    {
      new AlienDamageType("CalamityMod", "CalamityGlobalProjectile", "rogue")
    };

    private static readonly List<AlienProjectile> AllowedTypes = new List<AlienProjectile>()
    {
      new AlienProjectile(ItemID.StarCannon, ProjectileID.FallingStar),
      new AlienProjectile(ItemID.OnyxBlaster, ProjectileID.BlackBolt)
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