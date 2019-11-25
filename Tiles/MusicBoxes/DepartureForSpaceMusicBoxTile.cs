using ChensGradiusMod.Items.Placeables.MusicBoxes;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ChensGradiusMod.Tiles.MusicBoxes
{
  public class DepartureForSpaceMusicBoxTile : GradiusMusicBoxTile
  {
    protected override Color MinimapColor => new Color(100, 100, 100);

    protected override int ItemType => ModContent.ItemType<DepartureForSpaceMusicBox>();
  }
}