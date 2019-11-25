using ChensGradiusMod.Tiles.MusicBoxes;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Placeables.MusicBoxes
{
  public class IntermezzoMusicBox : GradiusMusicBox
  {
    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Music Box (Intermezzo)");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.createTile = ModContent.TileType<IntermezzoMusicBoxTile>();
    }
  }
}