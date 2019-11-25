using ChensGradiusMod.Tiles.Banners;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Banners
{
  public class BigCoreCustomBanner : ParentBanner
  {
    public override void SetDefaults()
    {
      base.SetDefaults();

      item.rare = 2;
    }

    protected override int PartnerTile => ModContent.TileType<BigCoreCustomBannerTile>();
  }
}