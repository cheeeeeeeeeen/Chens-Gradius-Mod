using ChensGradiusMod.Tiles.Banners;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Banners
{
  public class BigCoreCustomBanner : ParentBanner
  {
    protected override int PartnerTile => ModContent.TileType<BigCoreCustomBannerTile>();
  }
}