using Terraria.ModLoader;

namespace ChensGradiusMod.Projectiles
{
  public class AlienProjectile
  {
    public string modName;
    public string projectileName;

    private readonly Mod modInstance;

    public AlienProjectile(string mod, string proj)
    {
      modName = mod;
      projectileName = proj;
      modInstance = ModLoader.GetMod(modName);
    }

    public AlienProjectile()
    {
      modName = "";
      projectileName = "";
      modInstance = ModLoader.GetMod(modName);
    }

    public bool CheckType(int projType)
    {
      if (modInstance == null) return false;

      return projType == modInstance.ProjectileType(projectileName);
    }
  }
}