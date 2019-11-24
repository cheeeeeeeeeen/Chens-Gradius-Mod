using ChensGradiusMod.Tiles.Banners;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Banners
{
  public class ZalkBanner : ParentBanner
  {
    public override string Texture => "ChensGradiusMod/Sprites/ZalkBanner";

    protected override int PartnerTile => ModContent.TileType<ZalkBannerTile>();
  }
}