using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.Projectiles.Aliens
{
  public class AlienDamageType : GeneralAlien
  {
    public string damageType;
    public string internalName;

    private Projectile ProjectileObject(int whoAmI) => Main.projectile[whoAmI];

    public AlienDamageType(string mod, string internalName, string damageType) : base(mod)
    {
      this.damageType = damageType;
      this.internalName = internalName;
    }

    public AlienDamageType() : base("")
    {
      damageType = "";
      internalName = "";
    }

    public bool IsMatchingDamageType(int pWhoAmI)
    {
      if (modInstance != null)
      {
        GlobalProjectile gProj = ProjectileObject(pWhoAmI).GetGlobalProjectile(modInstance, internalName);
        FieldInfo field = gProj.GetType().GetField(damageType, BindingFlags.Public | BindingFlags.Instance);
        return (bool)field.GetValue(gProj);
      }

      return false;
    }
  }
}