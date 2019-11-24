using ChensGradiusMod.Tiles.Banners;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Banners
{
  public class MoaiBanner : ParentBanner
  {
    protected override int PartnerTile => ModContent.TileType<MoaiBannerTile>();
  }
}