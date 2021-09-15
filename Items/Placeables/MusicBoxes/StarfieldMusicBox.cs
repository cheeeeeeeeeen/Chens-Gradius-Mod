using ChensGradiusMod.Tiles.MusicBoxes;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Placeables.MusicBoxes
{
  public class StarfieldMusicBox : GradiusMusicBox
  {
    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Music Box (Starfield)");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.createTile = ModContent.TileType<StarfieldMusicBoxTile>();
    }
  }
}