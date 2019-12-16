using ChensGradiusMod.Tiles.MusicBoxes;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Placeables.MusicBoxes
{
  public class DepartureForSpaceMusicBox : GradiusMusicBox
  {
    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Music Box (Departure For Space)");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.createTile = ModContent.TileType<DepartureForSpaceMusicBoxTile>();
    }

    public override string Texture => "ChensGradiusMod/Sprites/DepartureForSpaceMusicBox";
  }
}