namespace ChensGradiusMod.Projectiles.Aliens
{
  public class AlienWeapon : GeneralAlien
  {
    public readonly string weaponName = null;
    public readonly int? weaponType = null;

    public AlienWeapon(string mod, string weap) : base(mod)
    {
      weaponName = weap;
    }

    public AlienWeapon(int weap) : base("Terraria")
    {
      weaponType = weap;
    }

    public bool CheckType(int weapType)
    {
      if (modName == "Terraria" && weaponType != null)
      {
        return weapType == weaponType;
      }
      else if (modInstance != null && weaponName != null)
      {
        return weapType == modInstance.ItemType(weaponName);
      }
      else return false;
    }
  }
}