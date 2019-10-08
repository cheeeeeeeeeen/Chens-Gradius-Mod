using ChensGradiusMod.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ChensGradiusMod.Tiles.MusicBoxes
{
  public class PositionLightMusicBoxTile : GradiusMusicBox
  {
    protected override Color MinimapColor => new Color(200, 200, 200);

    protected override string MusicName => "The Position Light";

    protected override int ItemType => ModContent.ItemType<PositionLightMusicBox>();
  }
}
