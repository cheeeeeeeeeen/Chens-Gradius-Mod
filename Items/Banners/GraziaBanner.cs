using ChensGradiusMod.Tiles.Banners;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Banners
{
    public class GraziaBanner : ParentBanner
    {
        protected override int PartnerTile => ModContent.TileType<GraziaBannerTile>();
    }
}