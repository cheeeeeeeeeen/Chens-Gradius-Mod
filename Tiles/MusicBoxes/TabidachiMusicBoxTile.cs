using ChensGradiusMod.Items.Placeables.MusicBoxes;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ChensGradiusMod.Tiles.MusicBoxes
{
  public class TabidachiMusicBoxTile : GradiusMusicBoxTile
  {
    protected override Color MinimapColor => new Color(200, 200, 200);

    protected override string MusicName => "Tabidachi";

    protected override int ItemType => ModContent.ItemType<TabidachiMusicBox>();
  }
}
