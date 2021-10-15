using ChensGradiusMod.Tiles.MusicBoxes;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Items.Placeables.MusicBoxes
{
    public class PositionLightMusicBox : GradiusMusicBox
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Music Box (The Position Light)");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            item.createTile = ModContent.TileType<PositionLightMusicBoxTile>();
            AssignItemDimensions(item, 32, 14, false);
        }

        public override string Texture => "ChensGradiusMod/Sprites/ThePositionLightMusicBox";
    }
}