namespace ChensGradiusMod.Projectiles.Aliens
{
  public class AlienProjectile : GeneralAlien
  {
    public readonly string projectileName = null;
    public readonly int? projectileType = null;

    public AlienProjectile(string mod, string proj) : base(mod)
    {
      projectileName = proj;
    }

    public AlienProjectile(int proj) : base("Terraria")
    {
      projectileType = proj;
    }

    public bool CheckType(int projType)
    {
      if (modName == "Terraria" && projectileType != null)
      {
        return projType == projectileType;
      }
      else if (modInstance != null && projectileName != null)
      {
        return projType == modInstance.ProjectileType(projectileName);
      }
      else return false;
    }
  }
}