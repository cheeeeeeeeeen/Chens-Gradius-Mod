using ChensGradiusMod.Tiles.MusicBoxes;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Placeables.MusicBoxes
{
    public class SensationMusicBox : GradiusMusicBox
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Music Box (Sensation)");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            item.createTile = ModContent.TileType<SensationMusicBoxTile>();
        }
    }
}