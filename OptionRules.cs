using ChensGradiusMod.Projectiles.Aliens;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod
{
  public static class OptionRules
  {
    private static readonly List<int> VanillaRules = new List<int>();

    private static readonly List<AlienProjectile> ModRules = new List<AlienProjectile>();

    private static readonly List<AlienDamageType> SupportedDamageTypes = new List<AlienDamageType>()
    {
      new AlienDamageType("CalamityMod", "CalamityGlobalProjectile", "rogue")
    };

    public static bool CompleteRuleCheck(Projectile p)
    {
      return p.active && IsNotAYoyo(p) && !p.hostile && p.friendly && !p.npcProj &&
             CanDamage(p) && IsAbleToCrit(p) && !p.minion && !p.trap;
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