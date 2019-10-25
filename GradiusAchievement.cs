using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace ChensGradiusMod
{
  public static class GradiusAchievement
  {
    private const string texturePath = "ChensGradiusMod/Sprites/";

    public static Texture2D TextureString(string tName, bool locked)
    {
      string lockedOrNot = locked ? "Locked" : "Unlocked";
      string pathName = $"{texturePath}{tName}{lockedOrNot}";
      return ModContent.GetTexture(pathName);
    }
  }
}