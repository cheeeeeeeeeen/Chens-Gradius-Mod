using AchievementLib.Elements;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace ChensGradiusMod
{
  public static class GradiusAchievement
  {
    private static readonly string texturePath = "ChensGradiusMod/Sprites/";

    public static Texture2D TextureString(string tName, bool locked)
    {
      string lockedOrNot = locked ? "Locked" : "Unlocked";
      string pathName = $"{texturePath}{tName}{lockedOrNot}";
      return ModContent.GetTexture(pathName);
    }
  }

  public class BydoTechnologyAchievement : ModAchievement
  {
    public override void SetDefaults()
    {
      Name = "Bydo Technology";
      Description = "Create an indestructible weapon made of alien flesh.";

      LockedTexture = GradiusAchievement.TextureString(TextureName, locked: true);
      UnlockedTexture = GradiusAchievement.TextureString(TextureName, locked: false);
    }

    protected virtual string TextureName => "PlaceholderAchievement";
  }

  public class WreekWeaponAchievement : BydoTechnologyAchievement
  {
    public override void SetDefaults()
    {
      base.SetDefaults();

      Name = "Wreek Weapon";
      Description = "Create an Option, an invulnerable drone which copies the host's attacks.";
    }

    protected override string TextureName => "PlaceholderAchievement";
  }

  public class FromMythToLegendAchievement : BydoTechnologyAchievement
  {
    public override void SetDefaults()
    {
      base.SetDefaults();

      Name = "From Myth To Legend";
      Description = "Create your legend by destroying a Big Core.";
    }

    protected override string TextureName => "PlaceholderAchievement";
  }
}