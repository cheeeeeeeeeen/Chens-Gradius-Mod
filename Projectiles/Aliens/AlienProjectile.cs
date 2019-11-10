namespace ChensGradiusMod.Projectiles.Aliens
{
  public class AlienProjectile : GeneralAlien
  {
    public string projectileName;

    public AlienProjectile(string mod, string proj) : base(mod)
    {
      projectileName = proj;
    }

    public AlienProjectile() : base("")
    {
      projectileName = "";
    }

    public bool CheckType(int projType)
    {
      if (modInstance == null) return false;

      return projType == modInstance.ProjectileType(projectileName);
    }
  }
}