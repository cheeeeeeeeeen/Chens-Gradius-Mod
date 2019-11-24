using ChensGradiusMod.Tiles.Banners;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Banners
{
  public class RushBanner : ParentBanner
  {
    protected override int PartnerTile => ModContent.TileType<RushBannerTile>();
  }
}