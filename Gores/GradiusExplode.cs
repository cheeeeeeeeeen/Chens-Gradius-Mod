using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.Gores
{
  public class GradiusExplode : ModGore
  {
    public const int FrameWidth = 64;
    public const int FrameHeight = 62;

    public static Vector2 CenterSpawn(Vector2 center)
    {
      return new Vector2
      {
        X = center.X - (FrameWidth * .5f),
        Y = center.Y - (FrameHeight * .5f)
      };
    }

    public override void OnSpawn(Gore gore)
    {
      gore.light = 2f;
      gore.numFrames = 5;
    }

    public override bool Update(Gore gore)
    {
      if (++gore.frameCounter >= 10)
      {
        gore.frameCounter = 0;
        if (++gore.frame >= gore.numFrames) gore.active = false;
      }

      return false;
    }
  }
}
