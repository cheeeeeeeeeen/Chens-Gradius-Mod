using ChensGradiusMod.Tiles.Banners;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Banners
{
    public class ZalkBanner : ParentBanner
    {
        protected override int PartnerTile => ModContent.TileType<ZalkBannerTile>();
    }
}