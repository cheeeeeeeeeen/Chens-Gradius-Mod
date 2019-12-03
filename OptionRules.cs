using ChensGradiusMod.Projectiles.Aliens;
using System.Collections.Generic;
using Terraria;
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

    public static bool StandardFilter(Projectile p)
    {
      return p.active && !p.hostile && p.friendly && !p.npcProj && CanDamage(p)
             && IsAbleToCrit(p) && !p.minion && !p.trap && IsNotAYoyo(p);
    }

    public static bool ImportOptionRule(string modName, string weapName, string projName)
    {
      if (!BannedTypes.Exists(ap => modName == ap.modName && weapName == ap.weaponName
                                    && projName == ap.projectileName))
      {
        BannedTypes.Add(new AlienProjectile(modName, weapName, projName));
        return true;
      }

      return false;
    }

    public static bool ImportOptionRule(int wType, int pType)
    {
      if (!BannedTypes.Exists(ap => ap.modName == "Terraria" && wType == ap.weaponType
                                    && pType == ap.projectileType))
      {
        BannedTypes.Add(new AlienProjectile(wType, pType));
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

    public static bool IsAllowed(Item w, Projectile p) => !IsBanned(w, p);

    public static bool IsAllowed(int wType, int pType) => !IsBanned(wType, pType);

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