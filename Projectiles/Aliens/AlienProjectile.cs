namespace ChensGradiusMod.Projectiles.Aliens
{
  public class AlienProjectile : GeneralAlien
  {
    public readonly string projectileName;

    public AlienProjectile(string mod, string proj) : base(mod)
    {
      projectileName = proj;
    }

    public bool CheckType(int projType)
    {
      if (modInstance == null) return false;

      return projType == modInstance.ProjectileType(projectileName);
    }
  }
}