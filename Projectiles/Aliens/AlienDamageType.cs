using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.Projectiles.Aliens
{
  public class AlienDamageType : GeneralAlien
  {
    public string damageType;
    public string internalName;
    public bool? isGlobal;

    private Projectile ProjectileObject(int whoAmI) => Main.projectile[whoAmI];

    public AlienDamageType(string mod, string baseClass, string internalName, string damageType) : base(mod)
    {
      this.damageType = damageType;
      this.internalName = internalName;
      if (baseClass == "GlobalProjectile") isGlobal = true;
      else if (baseClass == "ModProjectile") isGlobal = false;
      else isGlobal = null;
    }

    public AlienDamageType() : base("")
    {
      damageType = "";
      internalName = "";
      isGlobal = null;
    }

    public bool IsMatchingDamageType(int pWhoAmI)
    {
      if (modInstance != null)
      {
        if ((bool)isGlobal)
        {
          GlobalProjectile gProj = ProjectileObject(pWhoAmI).GetGlobalProjectile(modInstance, internalName);
          FieldInfo field = gProj.GetType().GetField(damageType, BindingFlags.Public | BindingFlags.Instance);
          return (bool)field.GetValue(gProj);
        }
        else if ((bool)!isGlobal)
        {
          ModProjectile mProj = ProjectileObject(pWhoAmI).modProjectile.GetType()
        }
      }

      return false;
    }
  }
}