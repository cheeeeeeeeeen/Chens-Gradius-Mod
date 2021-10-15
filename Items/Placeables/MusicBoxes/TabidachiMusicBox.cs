using ChensGradiusMod.Tiles.MusicBoxes;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Items.Placeables.MusicBoxes
{
    public class TabidachiMusicBox : GradiusMusicBox
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Music Box (Tabidachi)");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            item.createTile = ModContent.TileType<TabidachiMusicBoxTile>();
            AssignItemDimensions(item, 32, 12, false);
        }

        public override string Texture => "ChensGradiusMod/Sprites/TabidachiMusicBox";
    }
}