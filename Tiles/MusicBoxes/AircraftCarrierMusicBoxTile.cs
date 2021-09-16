using ChensGradiusMod.Items.Placeables.MusicBoxes;
using Terraria.ModLoader;

namespace ChensGradiusMod.Tiles.MusicBoxes
{
  public class AircraftCarrierMusicBoxTile : GradiusMusicBoxTile
  {
    protected override int ItemType => ModContent.ItemType<AircraftCarrierMusicBox>();
  }
}