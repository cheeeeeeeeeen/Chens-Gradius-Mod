using ChensGradiusMod.Tiles.Banners;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Banners
{
  public class DagoomBanner : ParentBanner
  {
    protected override int PartnerTile => ModContent.TileType<DagoomBannerTile>();
  }
}