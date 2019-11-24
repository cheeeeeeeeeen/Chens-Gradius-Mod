using ChensGradiusMod.Tiles.Banners;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Banners
{
  public class DuckerBanner : ParentBanner
  {
    protected override int PartnerTile => ModContent.TileType<DuckerBannerTile>();
  }
}