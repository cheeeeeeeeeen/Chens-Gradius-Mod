using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.Projectiles.Aliens
{
  public class AlienDamageType : GeneralAlien
  {
    public readonly string damageType;
    public readonly string internalName;

    private Projectile ProjectileObject(int whoAmI) => Main.projectile[whoAmI];

    public AlienDamageType(string mod, string internalName, string damageType) : base(mod)
    {
      this.damageType = damageType;
      this.internalName = internalName;
    }

    public AlienDamageType(string mod, string damageType) : base(mod)
    {
      this.damageType = damageType;
      internalName = "";
    }

    public bool IsMatchingDamageType(int pWhoAmI)
    {
      if (modInstance != null)
      {
        if (internalName == "") return ModProjectileAction(pWhoAmI);
        else return GlobalProjectileAction(pWhoAmI);
      }

      return false;
    }

    private bool ModProjectileAction(int pWhoAmI)
    {
      ModProjectile mProj = ProjectileObject(pWhoAmI).modProjectile;
      if (mProj.mod.Name == modInstance.Name)
      {
        try
        {
          FieldInfo field = mProj.GetType().GetField(damageType, BindingFlags.Public | BindingFlags.Instance);
          return (bool)field.GetValue(mProj);
        }
        catch { return false; }
      }
      else return false;
    }

    private bool GlobalProjectileAction(int pWhoAmI)
    {
      GlobalProjectile gProj = ProjectileObject(pWhoAmI).GetGlobalProjectile(modInstance, internalName);
      try
      {
        FieldInfo field = gProj.GetType().GetField(damageType, BindingFlags.Public | BindingFlags.Instance);
        return (bool)field.GetValue(gProj);
      }
      catch
      {
        string msg = $"{modInstance.Name}'s {internalName} does not " +
                     $"have {damageType} custom damage type variable.";
        modInstance.Logger.Warn($"Failed integration with ChensGradiusMod. {msg}");

        return false;
      }
    }
  }
}