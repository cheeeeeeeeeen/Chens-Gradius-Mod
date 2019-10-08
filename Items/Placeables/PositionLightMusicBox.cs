using ChensGradiusMod.Tiles.MusicBoxes;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Placeables
{
  public class PositionLightMusicBox : ModItem
  {
    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Music Box (The Position Light)");
    }

    public override void SetDefaults()
    {
      item.useStyle = 1;
      item.useTurn = true;
      item.useAnimation = 15;
      item.useTime = 10;
      item.autoReuse = true;
      item.consumable = true;
      item.createTile = ModContent.TileType<PositionLightMusicBoxTile>();
      item.width = 32;
      item.height = 20;
      item.rare = 8;
      item.value = 100000;
      item.accessory = true;
    }

    public override string Texture => "ChensGradiusMod/Sprites/PlaceholderMusicBox";
  }
}