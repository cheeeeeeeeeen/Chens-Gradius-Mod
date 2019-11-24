using ChensGradiusMod.Tiles.Banners;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Banners
{
  public class GarunBanner : ParentBanner
  {
    protected override int PartnerTile => ModContent.TileType<GarunBannerTile>();
  }
}