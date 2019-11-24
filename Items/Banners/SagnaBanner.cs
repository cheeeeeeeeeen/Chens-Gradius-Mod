using ChensGradiusMod.Tiles.Banners;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Banners
{
  public class SagnaBanner : ParentBanner
  {
    protected override int PartnerTile => ModContent.TileType<SagnaBannerTile>();
  }
}