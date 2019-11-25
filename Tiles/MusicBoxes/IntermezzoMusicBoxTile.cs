using ChensGradiusMod.Items.Placeables.MusicBoxes;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ChensGradiusMod.Tiles.MusicBoxes
{
  public class IntermezzoMusicBoxTile : GradiusMusicBoxTile
  {
    protected override Color MinimapColor => new Color(150, 150, 150);

    protected override int ItemType => ModContent.ItemType<IntermezzoMusicBox>();
  }
}