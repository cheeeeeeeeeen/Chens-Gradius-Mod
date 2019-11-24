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

    public override string Texture => "ChensGradiusMod/Sprites/BigCoreCustomBanner";

    protected override int PartnerTile => ModContent.TileType<BigCoreCustomBannerTile>();
  }
}